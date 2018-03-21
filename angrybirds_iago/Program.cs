using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace angrybirds_iago
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.GobalExit = false;
            game.StartMenu();
        }
    }
}
