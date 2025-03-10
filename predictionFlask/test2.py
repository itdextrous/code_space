# import json
# import pickle
# from flask import Flask, jsonify
# import requests
# from app import get_model
# import torch.nn as nn


# app = Flask(__name__)


# class SimpleNN(nn.Module):
#     def __init__(self, input_size):
#         super(SimpleNN, self).__init__()
#         self.fc1 = nn.Linear(input_size, 128)
#         self.relu1 = nn.ReLU()
#         self.fc2 = nn.Linear(128, 64)
#         self.relu2 = nn.ReLU()
#         self.fc3 = nn.Linear(64, 16)
#         self.relu3 = nn.ReLU()
#         self.fc4 = nn.Linear(16, 1)

#     def forward(self, x):
#         x = self.fc1(x)
#         x = self.relu1(x)
#         x = self.fc2(x)
#         x = self.relu2(x)
#         x = self.fc3(x)
#         x = self.relu3(x)
#         x = self.fc4(x)
#         return x

# model_path = 'model.pkl'
# scaler_path = 'scaler.pkl'
# with open(model_path, 'rb') as f:
#     _model = pickle.load(f)
# with open(scaler_path, 'rb') as f:
#     _scaler = pickle.load(f)

# @app.route('/opportunities')
# def get_all_opportunities():
#     return get_model(_model, _scaler)


# @app.route('/test_ip')
# def get_ip_address():
#     url = 'https://api.thesports.com/v1/ip/demo'
#     response = requests.get(url)
#     data = json.loads(response.text)
#     return data


# if __name__ == '__main__':
#     model_path = 'model.pkl'
#     scaler_path = 'scaler.pkl'
#     with open(model_path, 'rb') as f:
#         _model = pickle.load(f)
#     with open(scaler_path, 'rb') as f:
#         _scaler = pickle.load(f)
#     app.run(debug=True)
