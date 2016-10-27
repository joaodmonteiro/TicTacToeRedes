using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;

namespace Core.Stores
{
    public sealed class PlayerStore
    { 
        /// <summary>
        /// I do not know the exact purpose of this class for now. 
        /// It is a singleton to the player class, but I might
        /// have lost track of what I was thinking.
        /// Might get back to it later.
        /// </summary>
        private static PlayerStore _instance;
        public Player Player { get; set; }

        private PlayerStore()
        {
            Player = new Player();
        }

        public static PlayerStore Instance => _instance ?? (_instance = new PlayerStore());

        // TODO: Implement data model dispose method.
        // TODO: We should be able to clear all data saved in this
        // TODO: singleton with this method. Might be important later.
        public void Dispose()
        {
            // eg: player = null; playlist.cler(); etc...
        }
    }
}
