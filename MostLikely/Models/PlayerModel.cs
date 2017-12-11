using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MostLikely.Models
{
    public class PlayerModel
    {
        public PlayerModel(string nickname, string connectionId)
        {
            NickName = nickname;
            ConnectionId = connectionId;
            Points = 0;
        }

        public string NickName { get; private set; }

        public int Points { get; set; }

        public string ConnectionId { get; private set; }

        public int IconId { get; set; }
    }
}