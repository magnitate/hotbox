using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hotbox
{
#if WINDOWS || XBOX
    static class Program
    {
        static void Main(string[] args)
        {
            using (GameStateManagementGame game = new GameStateManagementGame())
                game.Run();
        }
    }
#endif
}
