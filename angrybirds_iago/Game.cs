using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace angrybirds_iago
{
    class Game
    {
        private string mapNameInput;

        public bool GobalExit { get; set; }
        public string TempPlayerName { get; private set; }
        public bool DoesPlayerExist { get; private set; }

        public void Start()
        {
            while (!GobalExit)
            {
                Console.WriteLine("1. Start\n2. Add a map\n3. See a Score\n\n0. Exit\nDelete. Clear Console\n");

                switch (Console.ReadKey().Key)
                {
                    case ConsoleKey.D1:
                        Console.WriteLine();
                        Console.WriteLine("\nWelcome, please enter your Name:");
                        TypeName();
                        Console.WriteLine("Checking database, one moment.\n");
                        CheckPlayer();
                        break;

                    case ConsoleKey.D2:
                        Console.WriteLine(". Add a new map");
                        AddMap();
                        break;

                    case ConsoleKey.D3:
                        Console.WriteLine("N/a");
                        /*Console.WriteLine(". See your Score.");
                        Console.WriteLine("\nEnter your Name:");
                        TypeName();
                        SeeAScore();*/
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
                        if (player.Name == TempPlayerName)
                        {
                            DoesPlayerExist = true;
                        }
                    }

                    if (DoesPlayerExist)
                    {
                        Console.WriteLine("Welcome back " + TempPlayerName + "!\n");
                        //Angrybirds();
                    }
                    else
                    {
                        Console.WriteLine("Welcome " + TempPlayerName + "!\n");
                        Console.WriteLine("Adding you to the database, please hold on.\n");
                        AddPlayer(TempPlayerName);
                        //Angrybirds();
                    }
                }
                else
                {
                    Console.WriteLine("Creating database, hold on.");
                    AddPlayer(TempPlayerName);
                    Console.WriteLine("Welcome " + TempPlayerName + "!\n");
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
                    angryBirdsDb.Players.Add(new Player { Name = inputName });
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

                    angryBirdsDb.Maps.Add(new Map(3, mapNameInput));
                    angryBirdsDb.SaveChanges();

                    Console.WriteLine("Map {0} added!", mapNameInput);
                }
                else
                {
                    Console.WriteLine("Database does not exist.");
                    Console.WriteLine("Returning to Menu.\n");
                }
            }
        }

        public void TypeName()
        {
            TempPlayerName = Console.ReadLine();
        }
    }
}
