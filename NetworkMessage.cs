using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Core.Models
{
    public class NetworkMessage
    {
        /// <summary>
        /// NetworkMessage is the class that we created for
        /// having a "standard" message going from the client
        /// to the server and back.
        /// It is still not finished as I have already identified
        /// some missing information here.
        /// We basically have a simple message, a connection state
        /// and the player that sent the message.
        /// </summary>
        [JsonProperty("Message")]
        public string Message { get; set; }
        [JsonProperty("Connected")]
        public bool Connected { get; set; }
        [JsonProperty("Player")]
        public Player Player { get; set; }
        public NetworkInstruction NetworkInstruction { get; set; }
        
    }
}
