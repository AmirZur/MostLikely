using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace MostLikely.Models
{
    public class GameModel
    {
        public GameModel(string id, int totalrounds)
        {
            ID = id;
            _totalRounds = totalrounds;
            PlayerConnectionIDs = new Dictionary<string, PlayerModel>();
            PlayerNickNames = new Dictionary<string, PlayerModel>();
            PlayerChoices = new Dictionary<PlayerModel, PlayerModel>();
            QuestionBank = File.ReadAllLines(HttpContext.Current.Server.MapPath("~/App_Data/QuestionBank.txt")).ToList();
        }

        public string ID { get; private set; }

        private int _totalRounds;

        private int _roundNumber = 0;

        private Random _random = new Random();

        public Dictionary<string, PlayerModel> PlayerNickNames { get; set; }

        public Dictionary<string, PlayerModel> PlayerConnectionIDs { get; set; }

        private Dictionary<PlayerModel, PlayerModel> PlayerChoices { get; set; }

        private List<string> QuestionBank;

        public PlayerModel GetPlayer(string id, string property="NickName")
        {
            if (property.Equals("ConnectionID"))
            {
                return PlayerConnectionIDs[id];
            }
            else if (property.Equals("NickName"))
            {
                return PlayerNickNames[id];
            }
            else
            {
                return null;
            }
        }

        public void AddPlayer(PlayerModel player)
        {
            PlayerNickNames.Add(player.NickName, player);
            PlayerConnectionIDs[player.ConnectionId] = player;
            player.IconId = PlayerNickNames.Count;
        }

        public PlayerModel SendAnswer(string player, string playerChoice)
        {
            PlayerModel _player = GetPlayer(player);
            PlayerModel choice = GetPlayer(playerChoice);
            PlayerChoices[_player] = choice;
            if(PlayerChoices.Count < PlayerNickNames.Count)
            {
                return null;
            }
            else
            {
                List<PlayerModel> allChoices = PlayerChoices.Values.ToList<PlayerModel>(); ;
                var orderedChoices =  allChoices.GroupBy(c => c).OrderByDescending(g => g.Count()).Select(g => g.Key);
                PlayerModel majority = orderedChoices.First();
                
                foreach(PlayerModel p in PlayerChoices.Keys)
                {
                    if(p.Equals(PlayerChoices[p]))
                    {
                        if (p.Equals(majority))
                        {
                            p.Points += 2;
                        }
                        else
                        {
                            p.Points -= 1;
                        }
                    }
                    else
                    {
                        if (PlayerChoices[p].Equals(majority))
                        {
                            p.Points += 1;
                        }
                    }
                }
                PlayerChoices.Clear();
                return majority;
            }
        }

        public string GetQuestion()
        {
            if (_roundNumber >= _totalRounds)
            {
                return null;
            }
            
            int randomNumber = _random.Next(0, QuestionBank.Count - 1);
            string question = QuestionBank[randomNumber];
            QuestionBank.RemoveAt(randomNumber);
            _roundNumber += 1;
            return question;
        }

        public PlayerModel GetWinner()
        {
            int max = 0;
            PlayerModel winner = null;
            foreach(PlayerModel player in PlayerNickNames.Values)
            {
                if(player.Points > max)
                {
                    winner = player;
                    max = player.Points;
                }
            }

            return winner;
        }
    }
}