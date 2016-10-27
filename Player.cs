using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Core.Models
{
    public class Player
    {
        /// <summary>
        /// Player class with information regarding the player itself.
        /// We have the player name, ip address and port.
        /// We could possibly add more information here in the future.
        /// </summary>
        public Guid Id { get; set; }
        public string PlayerName { get; set; }
        public bool Turn { get; set; }
        [JsonIgnore]
        public TcpClient TcpClient { get; set; }
        [JsonIgnore]
        public BinaryWriter BinaryWriter { get; set; }
        [JsonIgnore]
        public BinaryReader BinaryReader { get; set; }
    }
}
