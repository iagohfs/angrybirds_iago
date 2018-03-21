using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace angrybirds_iago
{
    class Game
    {
        #region Properties

        public bool GobalExit { get; set; }

        private string mapNameInput;
        public string InputPlayerName { get; private set; }
        public string CurrentMapName { get; private set; }

        public bool DoesPlayerExist { get; private set; }
        public bool MapObjectNotEmpty { get; private set; }
        public bool PlayerObjectNotEmpty { get; private set; }
        public bool MapsAvailable { get; private set; }
        public bool MapIsChosen { get; private set; }
        public bool Done { get; private set; }
        public bool Hit { get; private set; }
        public bool AddNewScore { get; private set; }
        public bool UpdateTheScore { get; private set; }

        public int InputMapNumber { get; private set; }
        public int TempScore { get; private set; }
        public int Tries { get; private set; }

        private ABContext angryBirdsDb;
        private Player CurrentPlayer;
        private Map CurrentMap;
        private Map NewMap;
        private Random Rand1;
        private Random Rand2;

        #endregion        

        public Game()
        {
            angryBirdsDb = new ABContext();
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

        public void Angrybirds()
        {
            Console.WriteLine("Which map would you like to play?\n");

            PrintAvailableMaps();

            ChooseMap();

            Done = false;

            Rand1 = new Random();
            Rand2 = new Random();

            Gameplay();
            UpdateScore(TempScore);

            PrintPlayerScore();

            Console.WriteLine("Would you like to continue playing? Y/N");

            if (Console.ReadKey().Key == ConsoleKey.Y)
            {
                ClearLastLine();

                Console.WriteLine();
                Console.WriteLine();
                Angrybirds();
            }
            else if (Console.ReadKey().Key == ConsoleKey.N)
            {
                ClearLastLine();
                Console.WriteLine("Going back to menu.\n");
                Console.WriteLine();
                Console.WriteLine();
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

        public void AddScore(int score)
        {

            var scoreTable = angryBirdsDb;

            if (scoreTable.Database.Exists())
            {
                if (MapObjectNotEmpty && PlayerObjectNotEmpty)
                {

                    scoreTable.Scores.Add(new Score { PlayerScore = score, Map = CurrentSelectedMap(), Player = CurrentSelectedPlayer() });
                    scoreTable.SaveChanges();
                }
            }
            else
            {
                Console.WriteLine("Database does not exist.");
                Console.WriteLine("Returning to Menu.\n");
            }
            Console.WriteLine();
        }

        public void CheckForAvailableMaps()
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
                    }
                }
                else
                {
                    Console.WriteLine("Database does not exist.");
                    Console.WriteLine("Returning to Menu.\n");
                }
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
                        CheckForAvailableMaps();

                        if (MapsAvailable)
                        {
                            Angrybirds();
                        }
                    }
                    else
                    {
                        Console.WriteLine("Welcome " + InputPlayerName + "!\n");
                        Console.WriteLine("Adding you to the database, please hold on.\n");
                        AddPlayer(InputPlayerName);
                        CheckForAvailableMaps();

                        if (MapsAvailable)
                        {
                            Angrybirds();
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Database does not exist, Try again.");
                }
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

                var mapTable = angryBirdsDb;
                {
                    if (mapTable.Database.Exists())
                    {
                        foreach (var m in mapTable.Maps)
                        {
                            if (m.MapId == InputMapNumber)
                            {
                                CurrentMapName = m.MapName;
                                CurrentMap = m;

                                Console.WriteLine("Entering {0}!\n", m.MapName);

                                MapIsChosen = true;
                                break;
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

        public static void ClearLastLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }

        public void EnterPlayerName()
        {
            InputPlayerName = Console.ReadLine();
        }

        public void EnterMapName()
        {
            CurrentMapName = Console.ReadLine();
        }

        public void Gameplay()
        {
            Hit = false;
            TempScore = 3;
            Tries = 0;
            int rand1;
            int rand2;

            while (!Done)
            {
                while (TempScore > 0)
                {
                    Console.WriteLine("Press S to shoot!");

                    if (Console.ReadKey().Key == ConsoleKey.S)
                    {
                        ClearLastLine();
                        Console.Beep(1000, 100);
                        Console.WriteLine("WEEEeeee...");

                        Thread.Sleep(5);
                        rand1 = Rand1.Next(1, 5);

                        Thread.Sleep(5);
                        rand2 = Rand2.Next(1, 7);

                        Thread.Sleep(2000);
                        if (rand1 == rand2)
                        {
                            Console.Beep(2000, 200);
                            Console.Beep(2300, 200);
                            Console.WriteLine("You got a hit!");
                            Thread.Sleep(1000);
                            Hit = true;
                            Done = true;
                            break;
                        }
                        else
                        {
                            TempScore--;
                            Tries++;
                            Console.Beep(600, 300);
                            Console.WriteLine("You missed!");
                            Console.WriteLine($"You have {TempScore} turns left");
                            Thread.Sleep(1000);

                            Hit = false;
                        }

                        if (TempScore == 0)
                        {
                            Done = true;
                        }
                    }
                }
            }            
        }

        public void PrintPlayerScore()
        {
            var scoreDb = angryBirdsDb;
            {
                if (angryBirdsDb.Database.Exists())
                {
                    foreach (var score in scoreDb.Scores)
                    {
                        if (score.Player.PlayerId == CurrentPlayer.PlayerId)
                        {
                            Console.WriteLine($"Player: {score.Player.Name} | Map: {score.Map.MapName} | Score: {score.PlayerScore}");
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

        public void PrintAvailableMaps()
        {
            var playerDb = angryBirdsDb;
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

        public void StartMenu()
        {
            while (!GobalExit)
            {
                Console.WriteLine("1. Start\n2. Add a map\n\n0. Exit\nDelete. Clear Console\n");

                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.D1:
                        Console.WriteLine(". Start");
                        Console.WriteLine();
                        Console.WriteLine("Welcome, please enter your Name:");
                        EnterPlayerName();
                        Console.WriteLine("Checking database, one moment.\n");
                        CheckPlayer();
                        break;

                    case ConsoleKey.D2:
                        Console.WriteLine(". Add a new map");
                        AddMap();
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
                while (!GobalExit) { StartMenu(); }
            }
        }

        public void UpdateScore(int score)
        {
            CurrentSelectedPlayer();
            CurrentSelectedMap();

            PlayerObjectNotEmpty = false;
            MapObjectNotEmpty = false;
            AddNewScore = false;

            var scoreTable = angryBirdsDb;
            {
                if (angryBirdsDb.Database.Exists())
                {
                    if (CurrentMap.MapName == CurrentMapName)
                    {
                        MapObjectNotEmpty = true;
                    }

                    if (CurrentPlayer.Name == InputPlayerName)
                    {
                        PlayerObjectNotEmpty = true;
                    }

                    var result = scoreTable.Scores.SingleOrDefault(s => s.Map.MapId == CurrentMap.MapId && s.Player.PlayerId == CurrentPlayer.PlayerId);

                    if (result != null)
                    {
                        result.PlayerScore = score;
                        scoreTable.SaveChanges();
                    }
                    else
                    {
                        AddScore(score);
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

    }
}