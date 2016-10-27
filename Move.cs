using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Move
    {
        /// <summary>
        /// Move class is responsible for having the information
        /// regarding a player move. We track the player that made
        /// the move, it's time (making it possible to know how
        /// much time the player took to play)
        /// And we also know what number the player tried to guess
        /// We could possibly show some interesting statistics
        /// at the end of the game with this information.
        /// </summary>
        public int Id { get; set; }
        public Player TargetPlayer { get; set; }
        public DateTime MoveDateTime { get; set; }
        public int PlayedNumber { get; set; }
    }
}
