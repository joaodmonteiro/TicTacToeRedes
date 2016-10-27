using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core.Models;
using Core.Stores;
using Newtonsoft.Json;
namespace Client.Controllers
{
    /// <summary>
    /// ClientController is responsible for managing all client
    /// behaviours. It starts with the client/server connection
    /// and will parse and invoke different client states and
    /// game behaviours.
    /// </summary>
    public class ClientController
    {
        private PlayerState _playerState;
        private NetworkInstruction _networkInstruction;
        public ClientController()
        {
            _playerState = PlayerState.ReceivePlayerInformation;
            _networkInstruction = NetworkInstruction.Wait;
        }
            
        public void StartClient()
        {
            // Start udp client
            TcpClient tcpClient = new TcpClient();
            tcpClient.Connect(IPAddress.Parse("127.0.0.1"), 7777);
            BinaryWriter binaryWriter = new BinaryWriter(tcpClient.GetStream());
            BinaryReader binaryReader = new BinaryReader(tcpClient.GetStream());

            string receivedNetworkMessageJsonString;
            NetworkMessage receivedNetworkMessage;
            // This while::cycle works like a game cycle.
            // A boolean should take it's place so that the client
            // can exit the cycle when it wants.
            // It is here to make sure that the client can make
            // several communications with the server.
            while (true)
            {
                switch (_playerState)
                {
                    case PlayerState.ReceivePlayerInformation:
                        Console.WriteLine("Input player name:");
                        // Here we initiate a new Player class with 
                        // information from the player input (player name)
                        Player player = new Player
                        {
                            PlayerName = Console.ReadLine()
                        };

                        // Check player name for null
                        if (!string.IsNullOrEmpty(player.PlayerName))
                        {
                            // Create a new instance of a class
                            // called networkMessage and set the current
                            // player in that model so that we can send
                            // this object to the server
                            NetworkMessage networkMessage = new NetworkMessage()
                            {
                                Player = player
                            };

                            // Serialize the NetworkMessage object to a JSON string
                            string networkMessageJsonStrong = JsonConvert.SerializeObject(networkMessage);
                            binaryWriter.Write(networkMessageJsonStrong);

                            // We know that the server will send a JSON string
                            // so we prepare the statement for it
                            receivedNetworkMessageJsonString = binaryReader.ReadString();

                            // Unserialize the JSON string to the object NetworkMessage
                            receivedNetworkMessage = JsonConvert.DeserializeObject<NetworkMessage>(receivedNetworkMessageJsonString);

                            // Temporary and simple validation indicating 
                            // that we received a positive connected state
                            // from the server
                            if (receivedNetworkMessage.Connected)
                            {
                                Console.WriteLine(receivedNetworkMessage.Message);
                                _playerState = PlayerState.GameStarted;
                            }
                            else
                            {
                                Console.WriteLine("Not Connected");
                            }
                        }
                        break;
                    case PlayerState.GameStarted:
                        switch (_networkInstruction)
                        {
                                case NetworkInstruction.Wait:
                                    // We know that the server will send a JSON string
                                    // so we prepare the statement for it
                                    receivedNetworkMessageJsonString = binaryReader.ReadString();

                                    // Unserialize the JSON string to the object NetworkMessage
                                    receivedNetworkMessage = JsonConvert.DeserializeObject<NetworkMessage>(receivedNetworkMessageJsonString);

                                    _networkInstruction = receivedNetworkMessage.NetworkInstruction;
                                    Console.WriteLine(receivedNetworkMessage.Message);
                                break;
                                case NetworkInstruction.MakeMove:
                                    string answer = Console.ReadLine();
                                    binaryWriter.Write(answer);
                                    
                                break;
                                case NetworkInstruction.GameEnded:
                                    _playerState = PlayerState.GameEnded;
                                break;
                        }

                        break;
                    case PlayerState.GameEnded:
                        Thread.Sleep(100);
                        break;
                }
            }
        }
    }
}
