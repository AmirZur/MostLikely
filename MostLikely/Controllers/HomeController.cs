using MostLikely.Models;
using MostLikely.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace MostLikely.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult NewRoom(NewRoomModel newRoom)
        {
            return View();
        }

        public ActionResult JoinRoom(NewRoomModel newRoom)
        {
            ViewBag.RoomID = newRoom.ID;
            ViewBag.Nickname = newRoom.Nickname;
            GameManagerModel gameModel = GameManagerModel.GetInstance();
            ViewBag.Players = gameModel.GetGame(newRoom.ID).PlayerNickNames.Values;
            return View("NewRoom");
        }

        public ActionResult StartGame(StartGameModel startGame)
        {
            ViewBag.Player = startGame.PlayerName;
            ViewBag.RoomID = startGame.RoomID;
            ViewBag.QuestionNumber = 1;
            ViewBag.Question = startGame.Question;
            GameManagerModel gameModel = GameManagerModel.GetInstance();
            try
            {
                ViewBag.Players = gameModel.GetGame(startGame.RoomID).PlayerNickNames.Values;
            }
            catch (Exception) { }
            return View();
        }

        public ActionResult EndGame(EndGameModel endGame)
        {
            ViewBag.Player = endGame.Nickname;
            ViewBag.RoomID = endGame.RoomId;
            GameModel game = GameManagerModel.GetInstance().GetGame(endGame.RoomId);
            PlayerModel thisPlayer = game.GetPlayer(endGame.Nickname);
            PlayerModel winner = game.GetWinner();
            if (thisPlayer.NickName.Equals(winner.NickName))
            {
                ViewBag.MostLikely = "You won!";
            }
            else
            {
                ViewBag.MostLikely = "The winner is " + winner.NickName + "!";
            }
            ViewBag.MostPoints = "with " + winner.Points;
            return View();
        }
    }
}
