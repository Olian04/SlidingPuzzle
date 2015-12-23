using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SlidingPuzzle.src;
using System;
using System.Collections.Generic;

namespace SlidingPuzzle
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public static bool won = true;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Rectangle cursorRect;
        MouseState ms, oldms;

        float scaler = 2;

        Tile[,] grid = new Tile[3,3];
        Tile[,] correctGrid = new Tile[3,3];
        List<Tile> baseTiles;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = (int)(720 * scaler);
            graphics.PreferredBackBufferWidth = (int)(1080 * scaler);
            graphics.ApplyChanges();
            Window.Position = new Point(300, 100);
            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            cursorRect = new Rectangle(0,0,1,1);

            Tile.Init(Content);

            InitLevel();

            base.Initialize();
        }

        private void InitLevel() {
            baseTiles = new List<Tile>();
            for (int i = 0; i<9; i++) {
                baseTiles.Add(new Tile(i));
                baseTiles[baseTiles.Count-1].setSize(new Point(300));
            }

            int X = 0;
            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < 3; i++, X++)
                {
                    correctGrid[i, j] = baseTiles[X];
                    correctGrid[i, j].setPos(new Point(i * (correctGrid[i, j].getRect().Width + 10) + (int)(graphics.PreferredBackBufferWidth / 3.5f), j * (correctGrid[i, j].getRect().Height + 10) + (int)(graphics.PreferredBackBufferHeight / 5f)));

                    grid[i, j] = baseTiles[X];
                    grid[i, j].setPos(new Point(i * (grid[i, j].getRect().Width + 10) + (int)(graphics.PreferredBackBufferWidth / 3.5f), j * (grid[i, j].getRect().Height + 10) + (int)(graphics.PreferredBackBufferHeight / 5f)));
                }
            }
        }


        private void blendBaseTiles()
        {
            int x = baseTiles.Count;
            Random rand = new Random();
            for (int i = 0; i < x; i++)
            {
                int a = rand.Next(0, x), b = rand.Next(0, x);
                Tile holder = baseTiles[a];
                baseTiles[a] = baseTiles[b];
                baseTiles[b] = holder;
            }
        }

        private void LoadLevel() {
            blendBaseTiles();
            int X = 0;
            for (int j = 0; j < 3; j++)
            {
                for (int i = 0; i < 3; i++, X++)
                {
                    grid[i, j] = baseTiles[X];
                    grid[i, j].setPos(new Point(i * (grid[i, j].getRect().Width + 10) + (int)(graphics.PreferredBackBufferWidth / 3.5f), j * (grid[i, j].getRect().Height + 10) + (int)(graphics.PreferredBackBufferHeight / 5f)));
                }
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            ms = Mouse.GetState();
            cursorRect.Location = ms.Position;

            if (grid[0,0] == correctGrid[0,0]) {
                won = true;
                for (int j = 0; j < 3; j++)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (grid[i, j] != correctGrid[i, j])
                            won = false;
                    }
                }
            }
            if (won) {
                if (ms.LeftButton == ButtonState.Pressed && oldms.LeftButton == ButtonState.Released) {
                    won = false;
                    LoadLevel();
                }
                updatePos();
            }
            else {
                bool endLoop = false;
                for (int i = 0; i < 3; i++)
                {
                    if (endLoop)
                        break;
                    for (int j = 0; j < 3; j++)
                    {
                        if (endLoop)
                            break;
                        if (!grid[i, j].isBlank())
                            if (cursorRect.Intersects(grid[i, j].getRect()))
                                if (ms.LeftButton == ButtonState.Pressed && oldms.LeftButton == ButtonState.Released)
                                {
                                    if (i + 1 < 3 && grid[i + 1, j].isBlank())
                                    { //Right
                                        Tile holder = grid[i, j];
                                        grid[i, j] = grid[i + 1, j];
                                        grid[i + 1, j] = holder;
                                        endLoop = true;
                                        updatePos();
                                    }
                                    else if (i - 1 >= 0 && grid[i - 1, j].isBlank())
                                    {
                                        Tile holder = grid[i, j];
                                        grid[i, j] = grid[i - 1, j];
                                        grid[i - 1, j] = holder;
                                        endLoop = true;
                                        updatePos();
                                    }
                                    else if (j + 1 < 3 && grid[i, j + 1].isBlank()) //Up
                                    {
                                        Tile holder = grid[i, j];
                                        grid[i, j] = grid[i, j + 1];
                                        grid[i, j + 1] = holder;
                                        endLoop = true;
                                        updatePos();
                                    }
                                    else if (j - 1 >= 0 && grid[i, j - 1].isBlank())
                                    {
                                        Tile holder = grid[i, j];
                                        grid[i, j] = grid[i, j - 1];
                                        grid[i, j - 1] = holder;
                                        endLoop = true;
                                        updatePos();
                                    }
                                }
                    }
                }
            }

            oldms = ms;
            base.Update(gameTime);
        }

        private void updatePos() {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    grid[i, j].setPos(new Point(i * (grid[i, j].getRect().Width + 10) + (int)(graphics.PreferredBackBufferWidth / 3.5f), j * (grid[i, j].getRect().Height + 10) + (int)(graphics.PreferredBackBufferHeight / 5f)));
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    grid[i, j].Draw(spriteBatch);
                }
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
