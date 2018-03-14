using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace angrybirds_iago
{
    class Game
    {
        #region Properties

        private string mapNameInput;

        public bool GobalExit { get; set; }
        public bool DoesPlayerExist { get; private set; }

        public string InputPlayerName { get; private set; }
        public string InputMapName { get; private set; }

        public int InputMapNumber { get; private set; }

        private Player CurrentPlayer;
        private Map NewMap;

        #endregion

        public void Start()
        {
            while (!GobalExit)
            {
                Console.WriteLine("1. Start\n2. Add a map\n3. See Scores\n\n0. Exit\nDelete. Clear Console\n");

                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.D1:
                        Console.WriteLine();
                        Console.WriteLine("\nWelcome, please enter your Name:");
                        EnterPlayerName();
                        Console.WriteLine("Checking database, one moment.\n");
                        CheckPlayer();
                        break;

                    case ConsoleKey.D2:
                        Console.WriteLine(". Add a new map");
                        AddMap();
                        break;

                    case ConsoleKey.D3:
                        Console.WriteLine(". See Scores\n");
                        break;

                    case ConsoleKey.D0:
                        GobalExit = true;
                        break;

                    case ConsoleKey.Delete:
                        Console.Clear();
                        break;

                    default:
                        Console.WriteLine("\nWrong command.\n");
                        //Thread.Sleep(100);
                        break;
                }
                while (!GobalExit) { Start(); }
            }
        }

        public void CheckPlayer() // 1
        {
            DoesPlayerExist = false;

            using (var playerDb = new ABContext())
            {
                playerDb.Configuration.LazyLoadingEnabled = false;

                if (playerDb.Database.Exists())
                {
                    foreach (var player in playerDb.Players)
                    {
                        if (player.Name == InputPlayerName)
                        {
                            DoesPlayerExist = true;
                        }
                    }

                    if (DoesPlayerExist)
                    {
                        Console.WriteLine("Welcome back " + InputPlayerName + "!\n");
                        ChooseMap();
                        //Angrybirds();
                    }
                    else
                    {
                        Console.WriteLine("Welcome " + InputPlayerName + "!\n");
                        Console.WriteLine("Adding you to the database, please hold on.\n");
                        AddPlayer(InputPlayerName);
                        ChooseMap();
                        //Angrybirds();
                    }
                }
                else
                {
                    Console.WriteLine("Creating database, hold on.");
                    AddPlayer(InputPlayerName);
                    Console.WriteLine("Welcome " + InputPlayerName + "!\n");
                    ChooseMap();
                    //Angrybirds();
                    Console.WriteLine("TODO : Run game (Mtd:CheckPlayer)");
                }
            }
        }

        public void AddPlayer(string inputName)
        {
            using (var angryBirdsDb = new ABContext())
            {
                if (angryBirdsDb.Database.Exists())
                {
                    CurrentPlayer = new Player { Name = inputName };

                    angryBirdsDb.Players.Add(CurrentPlayer);
                    angryBirdsDb.SaveChanges();
                }
                else
                {
                    Console.WriteLine("Database does not exist.");
                    Console.WriteLine("Returning to Menu.\n");
                }
            }
        }

        public void AddMap()
        {
            using (var angryBirdsDb = new ABContext())
            {
                if (angryBirdsDb.Database.Exists())
                {
                    Console.WriteLine("Enter a custom name for your map: ");
                    mapNameInput = Console.ReadLine();
                    Console.WriteLine();
                    mapNameInput.ToUpper();

                    NewMap = new Map { MapName = mapNameInput };

                    angryBirdsDb.Maps.Add(NewMap);
                    angryBirdsDb.SaveChanges();

                    Console.WriteLine("Map {0} added!\n", NewMap.MapName.ToString());
                }
                else
                {
                    Console.WriteLine("Database does not exist.");
                    Console.WriteLine("Returning to Menu.\n");
                }
            }
        }

        public void AddScore()
        {
            using (var angryBirdsDb = new ABContext())
            {
                if (angryBirdsDb.Database.Exists())
                {


                    angryBirdsDb.SaveChanges();
                }
                else
                {
                    Console.WriteLine("Database does not exist.");
                    Console.WriteLine("Returning to Menu.\n");
                }
            }
        }

        public void ChooseMap()
        {
            PrintMaps();

            Console.WriteLine("Enter the number of the map:");
            InputMapNumber = int.Parse(Console.ReadLine());
            Console.WriteLine();


            using (var angryBirdsDb = new ABContext())
            {
                if (angryBirdsDb.Database.Exists())
                {
                    foreach (var m in angryBirdsDb.Maps)
                    {
                        if (m.MapId == InputMapNumber)
                        {
                            Console.WriteLine("Entering {0}", m.MapName);
                            break;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Database does not exist.");
                    Console.WriteLine("Returning to Menu.\n");
                }
            }
        }

        public void PrintMaps()
        {
            using (var angryBirdsDb = new ABContext())
            {
                if (angryBirdsDb.Database.Exists())
                {
                    foreach (var m in angryBirdsDb.Maps)
                    {
                        Console.WriteLine(m.MapId + ". " + m.MapName);
                    }
                }
                else
                {
                    Console.WriteLine("Database does not exist.");
                    Console.WriteLine("Returning to Menu.\n");
                }
            }
            Console.WriteLine();
        }

        public void EnterPlayerName()
        {
            InputPlayerName = Console.ReadLine();
        }

        public void EnterMapName()
        {
            InputMapName = Console.ReadLine();
        }
    }
}
