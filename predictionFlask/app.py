from enum import Enum
import os
import pickle
import random
import uuid
from flask import Flask, jsonify, g
import pandas as pd
import numpy as np
import torch
import torch.nn as nn
import datetime
import json
import requests


app = Flask(__name__)


@app.route('/test_ip')
def get_ip_address():
    url = 'https://api.thesports.com/v1/ip/demo'
    response = requests.get(url)
    data = json.loads(response.text)
    return data


class OpportunityType(Enum):
    goals_over = 1
    goals_under = 2


class SimpleNN(nn.Module):
    def __init__(self, input_size):
        super(SimpleNN, self).__init__()
        self.fc1 = nn.Linear(input_size, 128)
        self.relu1 = nn.ReLU()
        self.fc2 = nn.Linear(128, 64)
        self.relu2 = nn.ReLU()
        self.fc3 = nn.Linear(64, 16)
        self.relu3 = nn.ReLU()
        self.fc4 = nn.Linear(16, 1)

    def forward(self, x):
        x = self.fc1(x)
        x = self.relu1(x)
        x = self.fc2(x)
        x = self.relu2(x)
        x = self.fc3(x)
        x = self.relu3(x)
        x = self.fc4(x)
        return x

# def load_model_and_scaler(model_path, scaler_path):
#     with open(model_path, 'rb') as f:
#         model = pickle.load(f)
#     with open(scaler_path, 'rb') as f:
#         scaler = pickle.load(f)
#     return model, scaler


class CustomUnpickler(pickle.Unpickler):
    def find_class(self, module, name):
        if name == 'SimpleNN':
            # from settings import Manager
            return SimpleNN
        return super().find_class(module, name)


def calculate_match_minutes(row):
    if row['status'] == '2':
        return ((row['timestamp'] - row['kickoff_time']).total_seconds() / 60) + 1
    elif row['status'] == '4':
        return ((row['timestamp'] - row['kickoff_time']).total_seconds() / 60) + 46
    elif row['status'] == '5':
        return ((row['timestamp'] - row['kickoff_time']).total_seconds() / 60) + 46
    else:
        return None


class RealTimeData:
    def __init__(self):
        self.match_id = None
        self.status = 0
        self.kickoff_time = 0
        self.home_score = 0
        self.away_score = 0
        self.home_halftime = 0
        self.away_halftime = 0
        self.home_yellow_card = 0
        self.away_yellow_card = 0
        self.home_corners = 0
        self.away_corners = 0
        self.home_penalty_score = 0
        self.away_penalty_score = 0
        self.stat_home_23 = 0
        self.stat_away_23 = 0
        self.stat_home_3 = 0
        self.stat_away_3 = 0
        self.stat_home_25 = 0
        self.stat_away_25 = 0
        self.stat_home_8 = 0
        self.stat_away_8 = 0
        self.stat_home_22 = 0
        self.stat_away_22 = 0
        self.stat_home_2 = 0
        self.stat_away_2 = 0
        self.stat_home_24 = 0
        self.stat_away_24 = 0
        self.stat_home_21 = 0
        self.stat_away_21 = 0
        self.stat_home_4 = 0
        self.stat_away_4 = 0
        self.timestamp = 0
        self.away_team_id = None
        self.home_team_id = None

    def __str__(self):
        return str({
            "match_id": str(self.match_id),
            "status": str(self.status),
            "kickoff_time": str(self.kickoff_time),
            "home_score": str(self.home_score),
            "away_score": str(self.away_score),
            "home_halftime": str(self.home_halftime),
            "away_halftime": str(self.away_halftime),
            "home_yellow_card": str(self.home_yellow_card),
            "away_yellow_card": str(self.away_yellow_card),
            "home_corners": str(self.home_corners),
            "away_corners": str(self.away_corners),
            "home_penalty_score": str(self.home_penalty_score),
            "away_penalty_score": str(self.away_penalty_score),
            "stat_home_23": str(self.stat_home_23),
            "stat_away_23": str(self.stat_away_23),
            "stat_home_3": str(self.stat_home_3),
            "stat_away_3": str(self.stat_away_3),
            "stat_home_25": str(self.stat_home_25),
            "stat_away_25": str(self.stat_away_25),
            "stat_home_8": str(self.stat_home_8),
            "stat_away_8": str(self.stat_away_8),
            "stat_home_22": str(self.stat_home_22),
            "stat_away_22": str(self.stat_away_22),
            "stat_home_2": str(self.stat_home_2),
            "stat_away_2": str(self.stat_away_2),
            "stat_home_24": str(self.stat_home_24),
            "stat_away_24": str(self.stat_away_24),
            "stat_home_21": str(self.stat_home_21),
            "stat_away_21": str(self.stat_away_21),
            "stat_home_4": str(self.stat_home_4),
            "stat_away_4": str(self.stat_away_4),
            "timestamp": str(self.timestamp),
            "away_team_id": str(self.away_team_id),
            "home_team_id": str(self.home_team_id),
        })


class Opportunity:
    def __init__(self, opp_id: str, opp_name: str, match_id: str, prediction: float, odds: float, opportunity_type: int) -> None:
        self.Id = opp_id
        self.Name = opp_name
        self.MatchId: str = match_id
        self.Prediction: float = prediction
        self.Odds: float = odds
        self.OpportunityType: int = opportunity_type

    def __str__(self):
        return (
            f"PredictionResponse(\n"
            f"    opp_id: '{self.Id}',\n"
            f"    name: '{self.Name}',\n"
            f"    match_id: '{self.MatchId}',\n"
            f"    prediction: {self.Prediction}\n"
            f"    odds: {self.Odds}\n"
            f"    opportunity_type: {self.OpportunityType}\n"
            f")"
        )

    def to_json(self):
        return json.dumps({
            "opp_id": self.opp_id,
            "opp_name": self.opp_name,
            "match_id": self.match_id,
            "prediction": self.prediction
        }, indent=4)


class OpportunityResponse:
    def __init__(self, match_id):
        self.MatchId = match_id
        self.opportunities = []


class MyEncoder(json.JSONEncoder):
    def default(self, obj):
        if isinstance(obj, Enum):
            return obj.value
        return json.JSONEncoder.default(self, obj)


@app.route('/opportunities')
def get_model():
    try:
        user = 'ecentrix'
        secret = 'c9db4c7ba3b10baa47207889e206f95b'
        url = f'https://api.thesports.com/v1/football/match/detail_live?user={user}&secret={secret}'
        response = requests.get(url)
        data = json.loads(response.text)
        results = data.get('results', None)
        if (results == None):
            return f'unable to fetch data from sports api. response received is {data}'
        res = []
        for dt in results:
            real_time_data = RealTimeData()
            real_time_data.match_id = dt.get('id')
            score = dt.get("score")
            real_time_data.status = score[1]
            real_time_data.kickoff_time = score[4]
            real_time_data.home_score = score[2][0]
            real_time_data.away_score = score[3][0]
            real_time_data.home_halftime = score[2][1]
            real_time_data.away_halftime = score[3][1]
            real_time_data.home_yellow_card = score[2][3]
            real_time_data.away_yellow_card = score[3][3]
            real_time_data.home_corners = score[2][4]
            real_time_data.away_corners = score[3][4]
            real_time_data.home_penalty_score = score[2][6]
            real_time_data.away_penalty_score = score[3][6]
            stats = dt.get('stats')
            for stat in stats:
                type_stat = stat.get('type')
                home_stat = stat.get('home')
                away_stat = stat.get('away')
                if type_stat == 23:
                    real_time_data.stat_home_23 = home_stat
                    real_time_data.stat_away_23 = away_stat
                elif type_stat == 3:
                    real_time_data.stat_home_3 = home_stat
                    real_time_data.stat_away_3 = away_stat
                elif type_stat == 25:
                    real_time_data.stat_home_25 = home_stat
                    real_time_data.stat_away_25 = away_stat
                elif type_stat == 8:
                    real_time_data.stat_home_8 = home_stat
                    real_time_data.stat_away_8 = away_stat
                elif type_stat == 22:
                    real_time_data.stat_home_22 = home_stat
                    real_time_data.stat_away_22 = away_stat
                elif type_stat == 2:
                    real_time_data.stat_home_2 = home_stat
                    real_time_data.stat_away_2 = away_stat
                elif type_stat == 24:
                    real_time_data.stat_home_24 = home_stat
                    real_time_data.stat_away_24 = away_stat
                elif type_stat == 21:
                    real_time_data.stat_home_21 = home_stat
                    real_time_data.stat_away_21 = away_stat
                elif type_stat == 4:
                    real_time_data.stat_home_4 = home_stat
                    real_time_data.stat_away_4 = away_stat
            real_time_data.timestamp = datetime.datetime.now(
                datetime.timezone.utc).timestamp()
            real_time_data.home_team_id = None
            real_time_data.away_team_id = None
            res.append(real_time_data)
        result = predict_all(res)
        return jsonify({"Opportunities": result})
    except Exception as e:
        return jsonify({"Error": str(e)})


def predict_all(all_data: list):
    try:
        res = []
        for dt in all_data:
            json_data = dt.__dict__
            model_path = 'model.pkl'
            scaler_path = 'scaler.pkl'
            # model, scaler = load_model_and_scaler(model_path, scaler_path)
            model = CustomUnpickler(open('model.pkl', 'rb')).load()
            # with open(model_path, 'rb') as f:
            #     model = pickle.load(f)
            with open(scaler_path, 'rb') as f:
                scaler = pickle.load(f)
            data = pd.DataFrame(json_data, index=[0])
            data['status'] = data['status'].astype(str)
            data['timestamp'] = pd.to_datetime(data['timestamp'], unit='s')
            data['kickoff_time'] = pd.to_datetime(
                data['kickoff_time'], unit='s')
            data['match_min'] = data.apply(calculate_match_minutes, axis=1)
            data.fillna(0, inplace=True)
            # Ensure features match the original feature names used in training
            original_feature_names = scaler.feature_names_in_
            missing_columns = set(original_feature_names) - set(data.columns)
            for col in missing_columns:
                data[col] = 0
            X_data = data[original_feature_names].select_dtypes(include=[
                                                                np.number])
            X_data = scaler.transform(X_data)
            test_inputs = torch.tensor(X_data, dtype=torch.float32)
            with torch.no_grad():
                test_outputs = model(test_inputs)
            predictions = test_outputs.numpy()
            over_threshold = dt.home_score + dt.away_score + \
                0.5  # Predicting more than 2.5 goals
            under_threshold = dt.home_score + dt.away_score + \
                0.5  # Predicting fewer than 1 goal
            correct_over = (predictions > over_threshold).sum() / \
                len(predictions)
            correct_under = (predictions < under_threshold).sum() / \
                len(predictions)
            odds_over = round(
                (round(random.uniform(0.05, 4.50)/0.05)*0.05)/0.05 * 0.05, 2)
            odds_under = round(
                (round(random.uniform(0.05, 4.50)/0.05)*0.05)/0.05 * 0.05, 2)
            opp_over_id = uuid.uuid4()
            match_id = dt.match_id
            pred_over_prob = correct_over * 100
            pred_over = Opportunity(
                opp_over_id, f'Over {over_threshold} goals', match_id, pred_over_prob, odds_over, OpportunityType.goals_over.value)
            opp_under_id = uuid.uuid4()
            pred_under_prob = correct_under * 100
            pred_under = Opportunity(
                opp_under_id, f'Under {under_threshold} goals', match_id, pred_under_prob, odds_under, OpportunityType.goals_under.value)
            opr = OpportunityResponse(match_id)
            opr.opportunities.append(pred_over.__dict__)
            opr.opportunities.append(pred_under.__dict__)
            res.append(opr.__dict__)
        return res
    except Exception as e:
        print(f"error is {str(e)}")


if __name__ == '__main__':
    app.run(debug=True)
