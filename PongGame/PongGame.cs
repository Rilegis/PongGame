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
    23/04/2021  Rilegis     2       Renamed 'Score' SpriteFont to 'Font'.
    23/04/2021  Rilegis     3       Fixed ball bouncing direction.
    23/04/2021  Rilegis     4       Implemented game state class.
    24/04/2021  Rilegis     5       Implemented game paused state and added winner message.
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

        // Initialize game state
        private GameState _gameState = new GameState();

        // Textures
        private Texture2D _ballTexture;
        private Texture2D _rectangleTexture;

        // Fonts
        private SpriteFont _font;


        public PongGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // NOTE: Add your initialization logic here
            _graphics.PreferredBackBufferWidth = (int)_gameState.CourtSize.X;
            _graphics.PreferredBackBufferHeight = (int)_gameState.CourtSize.Y;
            _graphics.ApplyChanges();
            base.Window.Title = "Pong";

            // Positions
            _gameState.Ball.Position = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2); // Position the ball at the center of the screen
            _gameState.Player1.Position = new Vector2(0f, _graphics.PreferredBackBufferHeight / 2); // Position player 1 on the left of the screen
            _gameState.Player2.Position = new Vector2(_graphics.PreferredBackBufferWidth - 20, _graphics.PreferredBackBufferHeight / 2); // Position player 1 on the right of the screen

            // Initialize the game
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // NOTE: use this.Content to load your game content here
            _ballTexture = Content.Load<Texture2D>("ball_32x32");
            _rectangleTexture = Content.Load<Texture2D>("bar_20x150");
            _font = Content.Load<SpriteFont>("Font");

            _gameState.Ball.Texture = _ballTexture;
            _gameState.Player1.Texture = _rectangleTexture;
            _gameState.Player2.Texture = _rectangleTexture;
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState kState = Keyboard.GetState(); // Get keyboard state

            // NOTE: Add your update logic here
            if (_gameState.IsPaused.Equals(true))
            {
                if (kState.IsKeyDown(Keys.Space))
                {
                    _gameState.IsPaused = false;
                    _gameState.Player1.IsWinner = false;
                    _gameState.Player2.IsWinner = false;
                }
            }
            else
            {
                _gameState.Ball.Position.X += _gameState.Ball.Speed.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
                _gameState.Ball.Position.Y += _gameState.Ball.Speed.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;

                // Player 1 controls
                if (kState.IsKeyDown(Keys.W))
                    _gameState.Player1.Position.Y -= _gameState.Player1.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (kState.IsKeyDown(Keys.S))
                    _gameState.Player1.Position.Y += _gameState.Player1.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                // Player 2 controls
                if (kState.IsKeyDown(Keys.Up))
                    _gameState.Player2.Position.Y -= _gameState.Player2.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (kState.IsKeyDown(Keys.Down))
                    _gameState.Player2.Position.Y += _gameState.Player2.Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;


                // Check if ball is hitting the screen's bounding box
                if (_gameState.Ball.Position.X >= _graphics.PreferredBackBufferWidth - (_gameState.Ball.Texture.Width / 2))
                {
                    _gameState.Ball.Position.X = _graphics.PreferredBackBufferWidth / 2;
                    _gameState.Ball.Position.Y = _graphics.PreferredBackBufferHeight / 2;

                    _gameState.Player1.Score++;

                    _gameState.IsPaused = true;
                }
                else if (_gameState.Ball.Position.X < (_gameState.Ball.Texture.Width / 2))
                {
                    _gameState.Ball.Position.X = _graphics.PreferredBackBufferWidth / 2;
                    _gameState.Ball.Position.Y = _graphics.PreferredBackBufferHeight / 2;

                    _gameState.Player2.Score++;

                    _gameState.IsPaused = true;
                }

                if (_gameState.Ball.Position.Y > _graphics.PreferredBackBufferHeight - (_gameState.Ball.Texture.Height / 2) || (_gameState.Ball.Position.Y < (_gameState.Ball.Texture.Height / 2)))
                    _gameState.Ball.Speed.Y = -_gameState.Ball.Speed.Y;

                // Check if ball hit player 1 bar
                if (((_gameState.Ball.Position.X - _gameState.Ball.Texture.Width / 2) <= (_gameState.Player1.Position.X + _gameState.Player1.Texture.Width)) && (_gameState.Ball.Position.Y >= _gameState.Player1.Position.Y) && (_gameState.Ball.Position.Y <= (_gameState.Player1.Position.Y + _gameState.Player1.Texture.Height)))
                    _gameState.Ball.Speed.X = -_gameState.Ball.Speed.X;
                // Check if ball hit player 2 bar
                else if (((_gameState.Ball.Position.X + _gameState.Ball.Texture.Width / 2) >= _gameState.Player2.Position.X) && (_gameState.Ball.Position.Y >= _gameState.Player2.Position.Y) && (_gameState.Ball.Position.Y <= (_gameState.Player2.Position.Y + _gameState.Player2.Texture.Height)))
                    _gameState.Ball.Speed.X = -_gameState.Ball.Speed.X;

                // Check if player 1 bar is hitting the screen's bounding box
                if (_gameState.Player1.Position.Y >= _graphics.PreferredBackBufferHeight - _gameState.Player1.Texture.Height)
                    _gameState.Player1.Position.Y = _graphics.PreferredBackBufferHeight - _gameState.Player1.Texture.Height;
                else if (_gameState.Player1.Position.Y <= 0)
                    _gameState.Player1.Position.Y = 0;
                // Check if player 2 bar is hitting the screen's bounding box
                if (_gameState.Player2.Position.Y >= _graphics.PreferredBackBufferHeight - _gameState.Player2.Texture.Height)
                    _gameState.Player2.Position.Y = _graphics.PreferredBackBufferHeight - _gameState.Player2.Texture.Height;
                else if (_gameState.Player2.Position.Y <= 0)
                    _gameState.Player2.Position.Y = 0;

                // Check if a winner is present
                if (_gameState.Player1.Score.Equals(10))
                {
                    _gameState.IsPaused = true;
                    _gameState.Player1.Score = 0;
                    _gameState.Player1.IsWinner = true;
                }
                else if (_gameState.Player2.Score.Equals(10))
                {
                    _gameState.IsPaused = true;
                    _gameState.Player2.Score = 0;
                    _gameState.Player2.IsWinner = true;
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // NOTE: Add your drawing code here
            _spriteBatch.Begin();
            _spriteBatch.Draw( // Draw the ball
                _gameState.Ball.Texture,
                _gameState.Ball.Position,
                null,
                Color.White,
                0f,
                new Vector2(_gameState.Ball.Texture.Width / 2, _gameState.Ball.Texture.Height / 2),
                Vector2.One,
                SpriteEffects.None,
                0f
            );
            _spriteBatch.Draw(_gameState.Player1.Texture, _gameState.Player1.Position, Color.White); // Draw Player1
            _spriteBatch.Draw(_gameState.Player2.Texture, _gameState.Player2.Position, Color.White); // Draw Player2

            // Draw court divider
            int tmp = 0;
            for (int i = 0; i < 8; i++)
            {
                _spriteBatch.Draw(_gameState.Player1.Texture, new Rectangle(_graphics.PreferredBackBufferWidth / 2 - 5, 9 + tmp, 10, 50), Color.White);
                tmp += 100;
            }

            // Draw scoreboard
            _spriteBatch.DrawString(_font, $"Player 1: {_gameState.Player1.Score}", new Vector2(16f, 16f), Color.White);
            _spriteBatch.DrawString(_font, $"Player 2: {_gameState.Player2.Score}", new Vector2((_graphics.PreferredBackBufferWidth / 2) + 21, 16f), Color.White);

            // If game is paused, show message
            if (_gameState.IsPaused)
            {
                _spriteBatch.DrawString(
                    _font,
                    "Press SPACE to start game",
                    new Vector2((_graphics.PreferredBackBufferWidth / 2) - (_font.MeasureString("Press SPACE to start game").X / 2), _graphics.PreferredBackBufferHeight - 595),
                    Color.White
                );
            }

            // If one of the players is winner, show message
            if (_gameState.Player1.IsWinner)
            {
                _spriteBatch.DrawString(
                    _font,
                    "Player1 won!",
                    new Vector2((_graphics.PreferredBackBufferWidth / 2) - (_font.MeasureString("Player1 won!").X / 2), _graphics.PreferredBackBufferHeight - 195),
                    Color.White
                );
            }
            else if(_gameState.Player2.IsWinner)
            {
                _spriteBatch.DrawString(
                    _font,
                    "Player2 won!",
                    new Vector2((_graphics.PreferredBackBufferWidth / 2) - (_font.MeasureString("Player2 won!").X / 2), _graphics.PreferredBackBufferHeight - 195),
                    Color.White
                );
            }


#if DEBUG
            _spriteBatch.DrawString(_font, $"Ball speed: X={_gameState.Ball.Speed.X}, Y={_gameState.Ball.Speed.Y}\nFrame time: {gameTime.ElapsedGameTime.TotalMilliseconds}ms\nFPS: {Math.Round(1 / gameTime.ElapsedGameTime.TotalSeconds, 2)}", new Vector2(20f, _graphics.PreferredBackBufferHeight - 76f), Color.White);
#endif

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
