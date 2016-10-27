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
using Server.Configs;

namespace Server.Controllers
{
    public class ServerController
    {
        private GameController _gameController;

        /// <summary>
        /// Class responsible for controlling all communications with
        /// the server.
        /// Maybe we should consider refactoring some of this code
        /// and makeing the gamestate loop integrate the gamecontroller
        /// </summary>
        public ServerController()
        {
            // Initiate an empty playerlist
            GameStore.Instance.Game.PlayerList = new List<Player>();
            // Indicate that the gamestate is ConnectionClosed
            // Meaning that we are currentlly not accepting any connections
            // to the server
            GameStore.Instance.Game.GameState = GameState.ConnectionClosed;
            GameStore.Instance.Game.ConnectingPlayers = 0;
        }

        public void StartServer()
        {
            // Initiate an tcp Listener
            TcpListener tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 7777);
            tcpListener.Start();

            // Indicate that the gamestate is ConnectionOpen
            // Meaning that we now accept client connections
            GameStore.Instance.Game.GameState = GameState.ConnectionOpen;
            Console.WriteLine("Server Connections Oppened");

            // this works like a gameloop making sure that the server 
            // never stops running
            while (true)
            {
                // Find which game state is the game running
                switch (GameStore.Instance.Game.GameState)
                {
                    case GameState.ConnectionClosed:
                        // Connections are closed
                        // TODO: The client should timeout trying
                        // TODO: to connect to this server.
                        // If we do accept a client connection
                        // we should consider changing the name of
                        // connectionclosed to some other name since
                        // we would need a connection to notify the
                        // client that the connections are closed
                        // Thread.Sleep makes sure the thread "sleeps" for 100ms
                        Thread.Sleep(100);
                        break;
                    case GameState.ConnectionOpen:
                        // Connection state is open, players may connect
                        // to the server and inform the server of their information
                        if (GameStore.Instance.Game.ConnectingPlayers < ServerConfig.MaxPlayers - 1)
                        {
                            TcpClient tcpClient = tcpListener.AcceptTcpClient();
                            Thread newThread = new Thread(new ParameterizedThreadStart(ProcessClient));
                            newThread.Start(tcpClient);
                        }
                        else
                        {
                            Thread.Sleep(100);
                        }
                        break;
                    case GameState.GameInitializing:
                        // In this state we should prepare everything for the game
                        // define the number that the player should try to guess
                        // Define who plays first
                        _gameController = new GameController();
                        break;
                    case GameState.GameStarted:
                        // All the game process should be processed here
                        _gameController.NextTurn();
                        _gameController.AskPlayerToPlay();
                        _gameController.ReceiveAnswer();
                        break;
                    case GameState.GameEnded:
                        // Notify all players that the game ended
                        // Show some stats?
                        // clear data models and repeat
                        // Maybe allow game to have a "payback" mode
                        Thread.Sleep(100);
                        break;
                }

            }
        }

        private void ProcessClient(object tcpClientObject)
        {
            GameStore.Instance.Game.ConnectingPlayers++;
            TcpClient tcpClient = tcpClientObject as TcpClient;
            if (tcpClient == null)
            {
                return;
            }

            BinaryWriter binaryWriter = new BinaryWriter(tcpClient.GetStream());
            BinaryReader binaryReader = new BinaryReader(tcpClient.GetStream());


            // We know that the server will send a JSON string
            // so we prepare the statement for it
            string message = binaryReader.ReadString();

            // Unserialize the JSON string to the object NetworkMessage
            NetworkMessage receivedNetworkMessage = JsonConvert.DeserializeObject<NetworkMessage>(message);

            // We create a new player object with information
            // from the client message sush as its' name
            Player player = new Player()
            {
                Id = Guid.NewGuid(),
                PlayerName = receivedNetworkMessage.Player.PlayerName,
                Turn = false,
                TcpClient = tcpClient,
                BinaryReader = binaryReader,
                BinaryWriter = binaryWriter
            };

            // We add the player to the player list
            GameStore.Instance.Game.PlayerList.Add(player);

            // We create a new networkmessage object to
            // send back to the client notifying the client
            // that everything is ok
            NetworkMessage networkMessageToSend = new NetworkMessage()
            {
                Connected = true,
                Message = "TIC TAC TOE \n \n Welcome to the game!\n",
                Player = player,
                NetworkInstruction = NetworkInstruction.Wait
            };

            // Serialize the NetworkMessage object to a JSON string
            string networkMessageToSendJsonString = JsonConvert.SerializeObject(networkMessageToSend);

            binaryWriter.Write(networkMessageToSendJsonString);

            // We check for the current number of players
            // to see if we can start the game
            if (GameStore.Instance.Game.PlayerList.Count == ServerConfig.MaxPlayers)
            {
                GameStore.Instance.Game.GameState = GameState.GameInitializing;
            }

            // Display some information to the server console
            Console.WriteLine("Player " + receivedNetworkMessage.Player.PlayerName + " entered the game");
            Console.WriteLine("Players " + GameStore.Instance.Game.PlayerList.Count + "/" + ServerConfig.MaxPlayers);
        }
    }
}
