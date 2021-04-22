/**********************************************************************
    Author            : Rilegis
    Repository        : PongGame
    Project           : PongGame
    File name         : Program.cs
    Date created      : 22/04/2021
    Purpose           : This is the entry point for the application.

    Revision History  :
    Date        Author      Ref     Revision 
    23/04/2021  Rilegis     1       First code commit.
**********************************************************************/

using System;

namespace PongGame
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new PongGame())
                game.Run();
        }
    }
}
