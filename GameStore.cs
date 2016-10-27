using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;

namespace Core.Stores
{
    public sealed class GameStore
    {
        /// <summary>
        /// This is a singleton allowing us to access the Game datamodel.
        /// With this, we can make sure that we do not loose information
        /// because of the garbage collector, and we also make sure that
        /// there is not more than one instances of the same object
        /// </summary>
        private static GameStore _instance;
        public Game Game { get; set; }

        private GameStore()
        {
            Game = new Game();
        }

        public static GameStore Instance => _instance ?? (_instance = new GameStore());
        /*public static GameStore Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameStore();
                }
                return _instance;
            }
        }*/

        // TODO: Implement data model dispose method.
        // TODO: We should be able to clear all data saved in this
        // TODO: singleton with this method. Might be important later.
        public void Dispose()
        {
            
        }
    }
}
