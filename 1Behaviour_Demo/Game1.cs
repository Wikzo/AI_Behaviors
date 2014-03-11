#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

#endregion

namespace _1Behaviour_Demo
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static int ScreenWidth = 800;
        public static int ScreenHeight = 600;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = ScreenWidth;
            graphics.PreferredBackBufferHeight = ScreenHeight;

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Texture2D arrowTexture = Content.Load<Texture2D>("arrow");

            Actor leader = new Actor(new Color(64, 255, 64), arrowTexture);
            leader.Speed = 1;
            leader.Direction = Actor.GetRandomDirection();
            leader.Position = Actor.GetRandomPosition(ScreenWidth, ScreenHeight);
            leader.BehaviorList.Add(new BehaviorConstant(0.1f, new Vector2(1f, 0)));
            leader.BehaviorList.Add(new BehaviorGamePad(0.5f));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
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

            foreach (var actor in Actor.Actors)
            {
                actor.Update();

                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    actor.Position += new Vector2(-1, 0);

                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    actor.Position += new Vector2(1, 0);

                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                    actor.Position += new Vector2(0, -1);

                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                    actor.Position += new Vector2(0, 1);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(96, 96, 111)); // blue-ish gray

            spriteBatch.Begin();

            foreach (Actor actor in Actor.Actors)
            {
                actor.Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
