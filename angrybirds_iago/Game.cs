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
        public string CurrentMapName { get; private set; }

        public int InputMapNumber { get; private set; }
        public bool MapObjectNotEmpty { get; private set; }
        public bool playerObjectNotEmpty { get; private set; }
        public int testintinput { get; private set; }
        public bool MapsAvailable { get; private set; }
        public bool MapIsChosen { get; private set; }

        private Player CurrentPlayer;
        private Map CurrentMap;
        private Map NewMap;

        #endregion
        private ABContext angryBirdsDb;
        public Game()
        {
            angryBirdsDb = new ABContext();
        }

        public void Start()
        {
            while (!GobalExit)
            {
                Console.WriteLine("1. Start\n2. Add a map\n3. add Scores\n\n0. Exit\nDelete. Clear Console\n");

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

        public void CheckPlayer()
        {
            DoesPlayerExist = false;

            var playerDb = angryBirdsDb;
            {
                playerDb.Configuration.LazyLoadingEnabled = false;

                if (playerDb.Database.Exists())
                {
                    foreach (var player in playerDb.Players)
                    {
                        if (player.Name == InputPlayerName)
                        {
                            DoesPlayerExist = true;
                            CurrentPlayer = player;
                        }
                    }

                    if (DoesPlayerExist)
                    {
                        Console.WriteLine("Welcome back " + CurrentPlayer.Name + "!\n");
                        PrintMaps();

                        if (MapsAvailable)
                        {
                            ChooseMap();
                            AddScore();
                        }
                        //Angrybirds();
                    }
                    else
                    {
                        Console.WriteLine("Welcome " + InputPlayerName + "!\n");
                        Console.WriteLine("Adding you to the database, please hold on.\n");
                        AddPlayer(InputPlayerName);
                        PrintMaps();

                        if (MapsAvailable)
                        {
                            ChooseMap();
                            //AddScore();
                        }
                        //Angrybirds();
                    }
                }
                else
                {
                    Console.WriteLine("Database does not exist, Try again.");
                }
            }
        }

        public void AddPlayer(string inputName)
        {
            var playerDb = angryBirdsDb;
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
            var playerDb = angryBirdsDb;
            {
                if (angryBirdsDb.Database.Exists())
                {
                    Console.WriteLine("Enter a custom name for your map: ");
                    mapNameInput = Console.ReadLine();
                    Console.WriteLine();
                    mapNameInput.ToUpper();

                    NewMap = new Map { MapName = mapNameInput, BirdsAvailable = 3 };

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

        public Player CurrentSelectedPlayer()
        {
            var playerDb = angryBirdsDb;
            {
                if (angryBirdsDb.Database.Exists())
                {
                    foreach (var p in angryBirdsDb.Players)
                    {
                        if (InputPlayerName == p.Name)
                        {
                            CurrentPlayer = p;
                        }
                        else
                        {
                            // Add new player
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Database does not exist.");
                    Console.WriteLine("Returning to Menu.\n");
                    CurrentMap = null;
                }
            }

            return CurrentPlayer;
        }

        public Map CurrentSelectedMap()
        {
            var playerDb = angryBirdsDb;
            {
                if (angryBirdsDb.Database.Exists())
                {
                    foreach (var m in angryBirdsDb.Maps)
                    {
                        if (CurrentMapName == m.MapName)
                        {
                            return m;
                            //CurrentMap = m;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Database does not exist.");
                    Console.WriteLine("Returning to Menu.\n");
                    CurrentMap = null;
                }
            }
            return null;
        }

        public void AddScore()
        {
            CurrentSelectedPlayer();
            CurrentSelectedMap();

            playerObjectNotEmpty = false;
            MapObjectNotEmpty = false;

            #region Score Test
            Console.WriteLine("Enter a number");
            testintinput = int.Parse(Console.ReadLine());
            #endregion

            var playerDb = angryBirdsDb;
            {
                if (angryBirdsDb.Database.Exists())
                {
                    if (CurrentMap.MapName == CurrentMapName)
                    {
                        MapObjectNotEmpty = true;
                    }

                    if (CurrentPlayer.Name == InputPlayerName)
                    {
                        playerObjectNotEmpty = true;
                    }

                    if (MapObjectNotEmpty && playerObjectNotEmpty)
                    {
                        angryBirdsDb.Scores.Add(new Score { PlayerScore = testintinput, Map = CurrentSelectedMap(), Player = CurrentSelectedPlayer() });
                        angryBirdsDb.SaveChanges();
                    }

                }
                else
                {
                    Console.WriteLine("Database does not exist.");
                    Console.WriteLine("Returning to Menu.\n");
                }
                Console.WriteLine();
            }
        }

        public void ChooseMap()
        {
            MapIsChosen = false;

            while (!MapIsChosen)
            {
                Console.WriteLine("Enter the number of the map you wish to play: ");
                InputMapNumber = int.Parse(Console.ReadLine());
                Console.WriteLine();

                var playerDb = angryBirdsDb;
                {
                    if (angryBirdsDb.Database.Exists())
                    {
                        foreach (var m in angryBirdsDb.Maps)
                        {
                            if (m.MapId == InputMapNumber)
                            {
                                Console.WriteLine("Entering {0}", m.MapName);
                                CurrentMapName = m.MapName;
                                CurrentMap = m;
                                MapIsChosen = true;
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Please try again");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Database does not exist.");
                        Console.WriteLine("Returning to Menu.\n");
                        break;
                    }
                }
            }
        }

        public void PrintMaps()
        {
            var playerDb = angryBirdsDb;
            {
                if (angryBirdsDb.Database.Exists())
                {
                    int mapscount = angryBirdsDb.Maps.Count();
                    if (mapscount == 0)
                    {
                        Console.WriteLine("There are no maps available,\n" +
                            "Please add a map first and try again.");
                        MapsAvailable = false;
                    }
                    else
                    {
                        MapsAvailable = true;
                        foreach (var m in angryBirdsDb.Maps)
                        {
                            Console.WriteLine(m.MapId + ". " + m.MapName);
                        }
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
            CurrentMapName = Console.ReadLine();
        }
    }
}
