using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MostLikely.Models
{
    public class StartGameModel
    {
        public string PlayerName { get; set; }
        public string RoomID { get; set; }
        public string Question { get; set; }
    }
}