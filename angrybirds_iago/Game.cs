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

        private Player CurrentPlayer;
        private Map NewMap;
        private Map CurrentMap;

        #endregion

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
                            CurrentPlayer = player;
                        }
                    }

                    if (DoesPlayerExist)
                    {
                        Console.WriteLine("Welcome back " + CurrentPlayer.Name + "!\n");
                        PrintMaps();
                        ChooseMap();
                        AddScore();
                        //Angrybirds();
                    }
                    else
                    {
                        Console.WriteLine("Welcome " + CurrentPlayer.Name + "!\n");
                        Console.WriteLine("Adding you to the database, please hold on.\n");
                        AddPlayer(InputPlayerName);
                        PrintMaps();
                        ChooseMap();
                        AddScore();
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

        public Player CurrentSelectedPlayer()
        {
            using (var angryBirdsDb = new ABContext())
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
                }
            }

            return CurrentPlayer;
        }

        public Map CurrentSelectedMap()
        {
            using (var angryBirdsDb = new ABContext())
            {
                if (angryBirdsDb.Database.Exists())
                {
                    foreach (var m in angryBirdsDb.Maps)
                    {
                        if (CurrentMapName == m.MapName)
                        {
                            CurrentMap = m;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Database does not exist.");
                    Console.WriteLine("Returning to Menu.\n");
                }
            }
            return CurrentMap;
        }

        public void AddScore()
        {
            playerObjectNotEmpty = false;
            MapObjectNotEmpty = false;

            #region Score Test
            Console.WriteLine("Enter a number");
            testintinput = int.Parse(Console.ReadLine());
            #endregion

            using (var angryBirdsDb = new ABContext())
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
                        angryBirdsDb.Scores.Add(new Score(CurrentMap, CurrentPlayer, testintinput));
                        angryBirdsDb.SaveChanges();
                    }

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

            Console.WriteLine("Enter the number of the map you wish to play: ");
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
                            CurrentMapName = m.MapName;
                            CurrentMap = m;
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
            CurrentMapName = Console.ReadLine();
        }
    }
}
