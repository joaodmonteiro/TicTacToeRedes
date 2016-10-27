using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core.Models;
using Core.Stores;
using Newtonsoft.Json;
using Server.Configs;

namespace Server.Controllers
{
    public class GameController
    {
        public char[,] Positions;
        //public char[,] AvailablePositions;
        public char a = '1';
        private Player player1, player2;

        public GameController()
        {
            //Random random = new Random();
            //GameStore.Instance.Game.TargetNumber = random.Next(0, 101);
            GameStore.Instance.Game.PlayerList.Last().Turn = true;
            GameStore.Instance.Game.GameState = GameState.GameStarted;

            Positions = new char[3, 3];

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Positions[i, j] = a;
                    a++;
                }
            }

            //AvailablePositions = Positions;
        }


        public void NextTurn()
        {
            if (GameStore.Instance.Game.PlayerList.FindIndex(p => p.Turn) + 1 ==
                GameStore.Instance.Game.PlayerList.Count)
            {
                GameStore.Instance.Game.PlayerList.Find(p => p.Turn).Turn = false;
                GameStore.Instance.Game.PlayerList.First().Turn = true;
            }
            else
            {
                GameStore.Instance.Game.PlayerList[GameStore.Instance.Game.PlayerList.FindIndex(p => p.Turn) + 1].Turn = true;
                GameStore.Instance.Game.PlayerList.Find(p => p.Turn).Turn = false;
            }
        }

        public void AskPlayerToPlay()
        {
            string message = "";

            for (int b = 0; b < 3; b++)
            {
                for(int c = 0; c < 3; c++)
                {
                    message += Positions[b, c];
                    message += "  ";
                    if (c == 2)
                    {
                        message += "\n";
                    }
                }
            }

            message += "\nMake a move! Pick a number";

            NetworkMessage networkMessageToSend = new NetworkMessage()
            {
                Message = message,
                NetworkInstruction = NetworkInstruction.MakeMove
            };

            string networkMessageToSenndJsonString = JsonConvert.SerializeObject(networkMessageToSend);

            GameStore.Instance.Game.PlayerList.Find(p => p.Turn).BinaryWriter.Write(networkMessageToSenndJsonString);
        }

        public void ReceiveAnswer()
        {
            // We know that the server will send a JSON string
            // so we prepare the statement for it

            int currentPlayer = GameStore.Instance.Game.PlayerList.FindIndex(p => p.Turn) + 1; //1 if player 1, 2 if player 2

            int k;

            string answer;

            do
            {
                Console.WriteLine("ANTES");
                answer = GameStore.Instance.Game.PlayerList.Find(p => p.Turn).BinaryReader.ReadString();
                Console.WriteLine("DEPOIS");
                Console.WriteLine(answer);

            } while (!Int32.TryParse(answer, out k) || k < 1 || k > 9 || Positions[(k - 1) / 3, (k - 1) % 3] == 'O' || Positions[(k - 1) / 3, (k - 1) % 3] == 'X');

            Console.WriteLine("SAI DO CICLOOOO!!");

            //while (Int32.TryParse(answer, out k))
            //{
            //    while (k < 1 || k > 9 || Positions[(k - 1) / 3, (k - 1) % 3] == 'O' || Positions[(k - 1) / 3, (k - 1) % 3] == 'X')
            //    {
            //        answer = GameStore.Instance.Game.PlayerList.Find(p => p.Turn).BinaryReader.ReadString();
            //        Thread.Sleep(100);
            //        Int32.TryParse(answer, out k);
            //    }
            //    break;
            //}

            
            //Console.WriteLine((answer-1) / 3);
            //Console.WriteLine((answer-1) % 3);
            //string hint = string.Empty;

            NetworkInstruction targetNetworkInstruction = NetworkInstruction.Wait;

            //if (answer == Positions[(answer-1)/3, (answer-1)%3])
            //{
                Console.WriteLine((k - 1) / 3);
                Console.WriteLine((k - 1) % 3);

                if (currentPlayer == 1)
                {
                    Positions[(k-1)/3, (k-1)%3] = 'X';
                }
                else
                {
                    Positions[(k-1) / 3, (k-1) % 3] = 'O';
                }
            //}

            

            //for (int i = 0; i < 3; i++)
            //{
            //    for (int j = 0; j < 3; j++)
            //    {
            //        if (answer == Positions[i, j])
            //        {
            //            if (player1 == null)
            //            {
            //                player1 = GameStore.Instance.Game.PlayerList.Find(p => p.Turn);

            //                Positions[i, j] = 'X';
            //            }
            //            else if (player2 == null)
            //            {
            //                player2 = GameStore.Instance.Game.PlayerList.Find(p => p.Turn);

            //                Positions[i, j] = 'O';
            //            }
            //            else if (player1.Id == GameStore.Instance.Game.PlayerList.Find(p => p.Turn).Id)
            //            {
            //                Positions[i, j] = 'X';
            //            }
            //            else if (player2.Id == GameStore.Instance.Game.PlayerList.Find(p => p.Turn).Id)
            //            {
            //                Positions[i, j] = 'O';
            //            }
            //        }
            //        else 
            //        {
            //            if (i == 2 && j == 2)
            //            {
            //                //AskPlayerToPlay();
            //                answer = GameStore.Instance.Game.PlayerList.Find(p => p.Turn).BinaryReader.Read();
            //                Thread.Sleep(100);
            //            }
            //        } 
            //    }
            //}






            //if (answer == GameStore.Instance.Game.TargetNumber)
            //{
            //    hint = "O jogador acertou no número correto!";
            //    GameStore.Instance.Game.GameState = GameState.GameEnded;
            //    targetNetworkInstruction = NetworkInstruction.GameEnded;
            //}
            //else if (answer < GameStore.Instance.Game.TargetNumber)
            //{
            //    hint = "ERRADO! O número é superior ao valor introduzido!";
            //}
            //else if (answer > GameStore.Instance.Game.TargetNumber)
            //{
            //    hint = "ERRADO! O número é inferior ao valor introduzido!";
            //}

            //string message = "";

            //for (int i = 0; i < 3; i++)
            //{
            //    for (int j = 0; j < 3; j++)
            //    {
            //        message += Positions[i, j];
            //        message += "  ";
            //        if (j == 2)
            //        {
            //            message += "\n";
            //        }
            //    }
            //}

            //NetworkMessage networkMessageToSend = new NetworkMessage()
            //{
            //    Message = message,
            //    NetworkInstruction = NetworkInstruction.Wait
            //};



            NetworkMessage networkMessageToSend = new NetworkMessage()
            {
                Message = "O jogador " + GameStore.Instance.Game.PlayerList.Find(p => p.Turn).PlayerName + " tentou " + answer + "\n",

                NetworkInstruction = targetNetworkInstruction
            };

            string networkMessageToSenndJsonString = JsonConvert.SerializeObject(networkMessageToSend);

            foreach (Player player in GameStore.Instance.Game.PlayerList)
            {
                player.BinaryWriter.Write(networkMessageToSenndJsonString);
            }

            //TODO: Log player moves
        }
    }
}
