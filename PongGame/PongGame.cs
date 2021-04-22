/**********************************************************************
    Author            : Rilegis
    Repository        : PongGame
    Project           : PongGame
    File name         : PongGame.cs
    Date created      : 22/04/2021
    Purpose           : Main game class; Implements all the core methods
                        for the game.

    Revision History  :
    Date        Author      Ref     Revision 
    23/04/2021  Rilegis     1       First code commit.
**********************************************************************/

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace PongGame
{
    public class PongGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Fonts
        SpriteFont font;

        // Textures
        Texture2D ballTexture;
        Texture2D barTexture;

        // Positions
        Vector2 ballPosition;
        Vector2 player1Position;
        Vector2 player2Position;

        // Speeds
        float ballSpeedX;
        float ballSpeedY;
        float playerSpeed;

        // Player scores
        int player1Score = 0;
        int player2Score = 0;

        // tmp
        GameTime updateTime;


        public PongGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // NOTE: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = 1536;
            _graphics.PreferredBackBufferHeight = 768;
            _graphics.ApplyChanges();
            base.Window.Title = "Pong";

            // Positions
            ballPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2); // Position the ball at the center of the screen
            player1Position = new Vector2(30f, _graphics.PreferredBackBufferHeight / 2); // Position player 1 on the left of the screen
            player2Position = new Vector2(_graphics.PreferredBackBufferWidth - 50f, _graphics.PreferredBackBufferHeight / 2); // Position player 1 on the right of the screen

            // Speeds
            ballSpeedX = 500f; // Set ball's 'X' speed
            ballSpeedY = 500f; // Set ball's 'Y' speed
            playerSpeed = 350f; // Set players bars speed
            
            // Initialize the game
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // NOTE: use this.Content to load your game content here
            ballTexture = Content.Load<Texture2D>("ball_32x32");
            barTexture = Content.Load<Texture2D>("bar_20x150");

            font = Content.Load<SpriteFont>("Score");
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState kState = Keyboard.GetState(); // Get keyboard state

            ballPosition.X += ballSpeedX * (float)gameTime.ElapsedGameTime.TotalSeconds;
            ballPosition.Y += ballSpeedY * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Player 1 controls
            if (kState.IsKeyDown(Keys.W))
                player1Position.Y -= playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (kState.IsKeyDown(Keys.S))
                player1Position.Y += playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            // Player 2 controls
            if (kState.IsKeyDown(Keys.Up))
                player2Position.Y -= playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (kState.IsKeyDown(Keys.Down))
                player2Position.Y += playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;


            // Check if ball is hitting the screen's bounding box
            if ((ballPosition.X > _graphics.PreferredBackBufferWidth - (ballTexture.Width / 2)) || (ballPosition.X < (ballTexture.Width / 2)))
            {
                // TODO: Show winner
                //ballSpeedX = -ballSpeedX;
                ballPosition.X = _graphics.PreferredBackBufferWidth / 2;
                ballPosition.Y = _graphics.PreferredBackBufferHeight / 2;
                player1Score = 0;
                player2Score = 0;
            }
            if (ballPosition.Y > _graphics.PreferredBackBufferHeight - (ballTexture.Height / 2) || (ballPosition.Y < (ballTexture.Height / 2)))
                ballSpeedY = -ballSpeedY;

            // Check if ball hit player 1 bar
            if (((ballPosition.X - ballTexture.Width / 2) <= (player1Position.X + barTexture.Width)) && (ballPosition.Y >= player1Position.Y) && (ballPosition.Y <= (player1Position.Y + barTexture.Height)))
            {
                ballSpeedX = -ballSpeedX;
                ballSpeedY = -ballSpeedY;
                player1Score++;
            }
            // Check if ball hit player 2 bar
            else if (((ballPosition.X + ballTexture.Width / 2) >= player2Position.X) && (ballPosition.Y >= player2Position.Y) && (ballPosition.Y <= (player2Position.Y + barTexture.Height)))
            {
                ballSpeedX = -ballSpeedX;
                ballSpeedY = -ballSpeedY;
                player2Score++;
            }

            // Check if player 1 bar is hitting the screen's bounding box
            if (player1Position.Y >= _graphics.PreferredBackBufferHeight - barTexture.Height)
                player1Position.Y = _graphics.PreferredBackBufferHeight - barTexture.Height;
            else if (player1Position.Y <= 0)
                player1Position.Y = 0;
            // Check if player 2 bar is hitting the screen's bounding box
            if (player2Position.Y >= _graphics.PreferredBackBufferHeight - barTexture.Height)
                player2Position.Y = _graphics.PreferredBackBufferHeight - barTexture.Height;
            else if (player2Position.Y <= 0)
                player2Position.Y = 0;

            // NOTE: Add your update logic here
            updateTime = gameTime;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // NOTE: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw( // Draw the ball
                ballTexture,
                ballPosition,
                null,
                Color.White,
                0f,
                new Vector2(ballTexture.Width / 2, ballTexture.Height / 2),
                Vector2.One,
                SpriteEffects.None,
                0f
            );
            _spriteBatch.Draw(barTexture, player1Position, Color.White); // Draw player 1
            _spriteBatch.Draw(barTexture, player2Position, Color.White); // Draw player 2

            // Draw court divider
            int tmp = 0;
            for (int i = 0; i < 8; i++)
            {
                _spriteBatch.Draw(barTexture, new Rectangle(_graphics.PreferredBackBufferWidth / 2 - 5, 9 + tmp, 10, 50), Color.White);
                tmp += 100;
            }

            // Draw scoreboard
            _spriteBatch.DrawString(font, $"Player 1: {player1Score}\nPlayer 2: {player2Score}", new Vector2(20f, 20f), Color.White);
            _spriteBatch.DrawString(font, $"Frame time: {gameTime.ElapsedGameTime.TotalMilliseconds}ms\nFPS: {Math.Round(1/gameTime.ElapsedGameTime.TotalSeconds, 2)}", new Vector2(20f, _graphics.PreferredBackBufferHeight - 80f), Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
