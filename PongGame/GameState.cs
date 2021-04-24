/**********************************************************************
    Author            : Rilegis
    Repository        : PongGame
    Project           : PongGame
    File name         : GameState.cs
    Date created      : 23/04/2021
    Purpose           : This class contains the game state.

    Revision History  :
    Date        Author      Ref     Revision 
    23/04/2021  Rilegis     1       First code commit.
    24/04/2021  Rilegis     2       Added 'GameState.IsPaused' and 'Player.IsWinner' properties.
**********************************************************************/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PongGame
{
    public class GameState : Game
    {
        public Ball Ball = new Ball();
        public Player Player1 = new Player();
        public Player Player2 = new Player();

        public Vector2 CourtSize = new Vector2(1536, 768);

        public bool IsPaused { get; set; } = true;
    }

    public class Ball
    {
        public Texture2D Texture;
        public Vector2 Position;
        public Vector2 Speed = new Vector2(400f, 400f); // Initialize to default value
    }

    public class Player
    {
        public Texture2D Texture;
        public Vector2 Position;
        public float Speed { get; set; } = 700f; // Initialize to default value
        public int Score { get; set; } = 0; // Initialize to default value
        public bool IsWinner { get; set; } = false; // Initialize to default value
    }
}
