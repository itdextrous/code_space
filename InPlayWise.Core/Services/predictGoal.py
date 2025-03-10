import pickle
import json

from sklearn.impute import SimpleImputer
from sklearn.model_selection import  GridSearchCV
import numpy as np  # Assuming you have NumPy for creating the input data array
from sklearn.preprocessing import MinMaxScaler
from sklearn.linear_model import LogisticRegression
# predictGoal.py

import pandas as pd



# Load the saved model from the .pkl file
with open('model_logit.pkl', 'rb') as model_file:
    loaded_model = pickle.load(model_file)

no_goal_row={'minute':[90],'goals_home':[0],'goals_away':[0],'goals_diff':[0],'current_total_goals':[0],'after_ht':[0],'added_time_1':[0],'added_time_2':[0],'time_sqrd':[90**2],'scored_this_min':[0]}

print(loaded_model)

def make_prediction(input_data_json):
     input_data = json.loads(input_data_json)
     feature_names = [
          'home_score',
          'home_shots_on_target', 'home_shots_off_target', 'home_attacks',
          'home_dangerous_attacks', 'home_corners', 'home_penalties_awarded', 'away_score',
          'away_shots_on_target', 'away_shots_off_target', 'away_attacks', 'away_dangerous_attacks',
          'away_corners', 'away_penalties_awarded', 'match_minute', 'total_score', 'time_from_last_goal',
          'time_from_last_goal_home', 'time_from_last_goal_away', 'cp_goal_first_10m', 'total_shots_on_off_ratio',
          'home_shots_on_off_ratio', 'away_shots_on_off_ratio', 'total_attacks_ratio', 'home_attacks_ratio',
          'away_attacks_ratio', 'total_corners_ratio', 'home_corners_ratio', 'away_corners_ratio',
          'total_penalties_ratio', 'home_penalties_ratio', 'away_penalties_ratio', 'avg_time_to_goal',
          'home_avg_time_to_goal', 'away_avg_time_to_goal', 'identity_0_0', 'identity_0_1', 'identity_0_2',
          'identity_0_3', 'identity_0_4', 'identity_0_5', 'identity_0_6', 'identity_0_7', 'identity_0_8',
          'identity_1_1',
          'identity_1_2', 'identity_1_3', 'identity_1_4', 'identity_1_5', 'identity_1_6', 'identity_1_7',
          'identity_1_8',
          'identity_0_over8', 'identity_1_over8', 'identity_2_2', 'identity_2_3', 'identity_2_4', 'identity_2_5',
          'identity_2_6', 'identity_2_7', 'identity_2_8', 'identity_2_over8', 'identity_3_3', 'identity_3_4',
          'identity_3_5',
          'identity_3_6', 'identity_3_7', 'identity_3_8', 'identity_3_over8', 'identity_4_4', 'identity_4_5',
          'identity_4_6',
          'identity_4_7', 'identity_4_8', 'identity_4_over8', 'identity_5_5', 'identity_5_6', 'identity_5_7',
          'identity_5_8',
          'identity_5_over8', 'identity_6_6', 'identity_6_7', 'identity_6_8', 'identity_6_over8', 'identity_7_7',
          'identity_7_8', 'identity_7_over8', 'identity_8_8', 'identity_over8_over8',
          'dangerous_attacks_minute_ratio', 'home_dangerous_attacks_minute_ratio',
          'away_dangerous_attacks_minute_ratio', 'dangerous_attacks_ratio', 'home_dangerous_attacks_ratio',
          'away_dangerous_attacks_ratio', 'is_second_half', 'goal_diff', 'time_2', 'time_log', 'time_3',
          'total_attacks_ratio_log', 'total_attacks_ratio_2', 'home_attacks_ratio_log', 'home_attacks_ratio_2',
          'away_attacks_ratio_log', 'away_attacks_ratio_2', 'total_penalties_ratio_log', 'total_penalties_ratio_2',
          'home_penalties_ratio_log', 'home_penalties_ratio_2', 'away_penalties_ratio_log',
          'away_penalties_ratio_2', 'total_corners_ratio_log', 'total_corners_ratio_2', 'home_corners_ratio_log',
          'home_corners_ratio_2', 'away_corners_ratio_log', 'away_corners_ratio_2',
          'dangerous_attacks_minute_ratio_log', 'dangerous_attacks_minute_ratio_2',
          'home_dangerous_attacks_minute_ratio_log', 'home_dangerous_attacks_minute_ratio_2',
          'away_dangerous_attacks_minute_ratio_log', 'away_dangerous_attacks_minute_ratio_2',
          'dangerous_attacks_ratio_log', 'dangerous_attacks_ratio_2', 'home_dangerous_attacks_ratio_log',
          'home_dangerous_attacks_ratio_2', 'goal_diff_log', 'goal_diff_2' ]
     feature_names1 = {
          "minute",
          "goals_home",
          "goals_away",
          "goals_diff",
          "current_total_goals",
          "after_ht",
          "added_time_1",
          "added_time_2",
          "time_sqrd",
          "scored_this_min"
     }





     # Convert the input data into a format that can be used by the model
     # Assuming 'input_data' is a dictionary containing your input data
     # Extract the 'match_info' field
     match_info = input_data['match_info']
     match_info_mapping = {
          '1H': 0,
          '2H': 1,
          # Add more categories as needed
     }

     # Use the mapping to convert match_info to a numerical value (0 or 1)
     # If 'match_info' is not in the mapping, you can handle it as needed (e.g., assign a default value)
     numerical_match_info = match_info_mapping.get(match_info, -1)

     # Update the 'input_data' dictionary with the numerical match_info
     input_data['match_info'] = numerical_match_info

     is_second_half = input_data['is_second_half']
     numerical_is_second_half = 1.0 if is_second_half == 'true' else 0.0
     input_data['is_second_half'] = numerical_is_second_half
     X_new = np.array([input_data[feature] for feature in feature_names]).reshape(1, -1)

     predicted_class = loaded_model.predict(X_new)
     print("predicted Result for (predict_proba)",loaded_model.predict_proba(X_new))

     # Print the predicted class (0 or 1 for binary classification)
     print("Predicted Result:", predicted_class[0])
     return predicted_class[0]



def train_and_save_model(json_data):

    model_filename = 'model_logit.pkl'
    # Parse input JSON data
    data = json.loads(json_data)
    data = pd.DataFrame(data['items'])

    # Replace empty strings with NaN
    data.replace('', np.nan, inplace=True)

    # Select features
    features = [
        'matchStatus', 'matchMinutes', 'extraTime',
        'homeRed', 'awayRed', 'homeYellow', 'awayYellow',
        'homeCorners', 'awayCorners', 'homeShotsOnTarget', 'homeShotsOffTarget',
        'awayShotsOnTarget', 'awayShotsOffTarget',
        'homeDangerousAttacks', 'awayDangerousAttacks',
        'homeAttacks', 'awayAttacks',
        'homePenalties', 'awayPenalties',
        'homePossession', 'awayPossession',
        'homeOwnGoals', 'awayOwnGoals'
    ]
    X = data[features]

    # Define 'home_wins' as the target variable
    data['home_wins'] = (data['homeGoals'] > data['awayGoals'])
    y = data['home_wins']

    # Use SimpleImputer to fill missing values with the mean
    imputer = SimpleImputer(strategy='mean')
    X = imputer.fit_transform(X)

    # Train the logistic regression model
    model = LogisticRegression()
    model.fit(X, y)

    # Save the trained model to a pickle file
    with open(model_filename, 'wb') as file:
        pickle.dump(model, file)