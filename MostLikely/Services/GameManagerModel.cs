using MostLikely.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MostLikely.Services
{
    public class GameManagerModel
    {
        private static GameManagerModel _instance;

        private GameManagerModel()
        {
            GameIDs = new Dictionary<string, GameModel>();
            GameIDs["TEST"] = new GameModel("TEST", 3);
        }

        public Dictionary<string, GameModel> GameIDs { get; set; } 

        public void AddGame(GameModel game)
        {
            GameIDs[game.ID] = game;
        }

        public GameModel GetGame(string id)
        {
            try
            {
                if (GameIDs.ContainsKey(id))
                {
                    return GameIDs[id];
                }
                else
                {
                    return null;
                }
            } catch(Exception)
            {
                return null;
            }            
        }

        public static GameManagerModel GetInstance()
        {
            if(_instance == null)
            {
                _instance = new GameManagerModel();
            }

            return _instance;
        }
    }
}