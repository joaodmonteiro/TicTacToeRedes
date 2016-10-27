using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Game
    {
        /// <summary>
        /// Game data model.
        /// This class is responsible for maintaining the game state
        /// We know the current playerlist, the target game number
        /// We also have track of the movelist and the current game state
        /// </summary>
        public List<Player> PlayerList { get; set; }
        public int ConnectingPlayers { get; set; }
        public int TargetNumber { get; set; }
        public List<Move> MoveList { get; set; }
        public GameState GameState { get; set; }
    }
}
