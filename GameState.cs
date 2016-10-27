using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public enum GameState
    {
        /// <summary>
        /// Gamestate is responsible for controlling the current game
        /// state. Some states might be missing.
        /// </summary>
        ConnectionOpen,
        ConnectionClosed,
        GameInitializing,
        GameStarted,
        GameEnded
    }
}
