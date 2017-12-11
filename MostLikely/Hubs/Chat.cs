using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using MostLikely.Models;
using System.Threading.Tasks;
using MostLikely.Services;
using System.Timers;
using System.Diagnostics;

namespace MostLikely
{
    public class Chat : Hub
    {
        public async Task NewRoom(string nickname)
        {
            GameManagerModel gameManager = GameManagerModel.GetInstance();

            Random random = new Random();
            int randomNumber = random.Next(0, 9999);
            string newID = randomNumber.ToString("D4");
            GameModel newGame = new GameModel(newID, 5); //change to new input textbox!
            PlayerModel newPlayer = new PlayerModel(nickname, Context.ConnectionId);
            newGame.AddPlayer(newPlayer);
            gameManager.AddGame(newGame);

            await Groups.Add(Context.ConnectionId, newID);
            
            Clients.Caller.submitNewRoomForm(newID);
            
        }

        public void JoinRoom(string nickname, string roomID)
        {
            GameManagerModel gameManager = GameManagerModel.GetInstance();
            GameModel game = gameManager.GetGame(roomID);
            if (game == null)
            {
                Clients.Caller.throwExeption();
            }
            else
            {
                game.AddPlayer(new PlayerModel(nickname, Context.ConnectionId));
                Clients.Caller.submitJoinRoomForm();
            }
        }

        public async Task RejoinRoom(string roomID, string nickname)
        {
            await Groups.Add(Context.ConnectionId, roomID);
            Clients.OthersInGroup(roomID).send(nickname);
        }

        public void StartGame(string roomID)
        {
            
            string firstQuestion = GameManagerModel.GetInstance().GetGame(roomID).GetQuestion();
            Clients.Group(roomID).startGame(firstQuestion);
        }

        public void SendChoice(string player, string playerChoice, string roomId)
        {
            GameModel game = GameManagerModel.GetInstance().GetGame(roomId);
            PlayerModel result = game.SendAnswer(player, playerChoice);
            if(result != null)
            {
                Clients.Group(roomId).showResult(result);
                Timer transitionTimer = new Timer(10000);
                transitionTimer.Elapsed += (source, e) => HandleTransitionTimer(roomId);
                transitionTimer.Enabled = true;
                transitionTimer.AutoReset = false;
                transitionTimer.Start();
            }
        }

        private void HandleTransitionTimer(string roomId)
        {
            GameModel game = GameManagerModel.GetInstance().GetGame(roomId);
            string newQuestion = game.GetQuestion();
            if(newQuestion == null)
            {
                Clients.Group(roomId).endGame();
            }
            else
            {
                Clients.Group(roomId).newQuestion(newQuestion);
            }
        }

        public void GetScore(string player, string RoomID)
        {
            GameModel game = GameManagerModel.GetInstance().GetGame(RoomID);
            string score = game.GetPlayer(player).Points.ToString();
            Clients.Caller.sendScore(score);
        }
    }
}