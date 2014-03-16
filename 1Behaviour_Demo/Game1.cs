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
        private SpriteFont font;

        private FPSCounter fpsCounter;

        public static int ScreenWidth = 800;
        public static int ScreenHeight = 600;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = ScreenWidth;
            graphics.PreferredBackBufferHeight = ScreenHeight;

            Content.RootDirectory = "Content";

            //this.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 100.0f); // set update interval to 100 FPS. Default is 60

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

            font = Content.Load<Microsoft.Xna.Framework.Graphics.SpriteFont>("Tahoma");

            fpsCounter = new FPSCounter(this, spriteBatch, font);


            // dummy blocks
            Texture2D block1 = Content.Load<Texture2D>("1_block");
            Actor blok1 = new Actor(Color.White, block1, false);
            blok1.Speed = 5f;
            blok1.Position = Actor.GetRandomPosition(ScreenWidth, ScreenHeight);
            blok1.BehaviorList.Add(new BehaviorConstant(0.1f, new Vector2(1f, 0)));
            //blok1.BehaviorList.Add(new BehaviorGamePad(1));
            //blok1.BehaviorList.Add(new BehaviorWander(0.05f, 60));

            Texture2D arrowTexture = Content.Load<Texture2D>("arrow");

            // set up leader actor
            Actor leader = new Actor(new Color(64, 255, 64), arrowTexture);
            leader.Speed = 4;
            leader.DrawDepth = 0f;
            leader.IsPlayer = true;
            leader.Direction = Actor.GetRandomDirection();
            leader.Position = Actor.GetRandomPosition(ScreenWidth, ScreenHeight);
            //leader.BehaviorList.Add(new BehaviorConstant(0.1f, new Vector2(1f, 0)));
            leader.BehaviorList.Add(new BehaviorGamePad(0.5f));
            leader.BehaviorList.Add(new BehaviorWander(0.05f, 60)); // if framerate is 60 FPS, then change direction once every second

            // set up a 10 drone actors
            BehavoirSeek seek = new BehavoirSeek(0.05f, leader);

            for (int i = 0; i < 10000; i++)
            {
                Actor drone = new Actor(Color.White, arrowTexture);
                drone.Color = Actor.GetRandomColor();
                drone.Speed = Actor.GetRandomSpeed(0.7d, 3.5d);
                drone.Direction = Actor.GetRandomDirection();
                drone.Position = Actor.GetRandomPosition(ScreenWidth, ScreenHeight);
                drone.BehaviorList.Add(seek);
                drone.BehaviorList.Add(new BehaviorWander(0.03f, 15));
            }
            
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

            // manipulate the swarm by pressing B
            if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.B))
            {
                foreach (Actor a in Actor.Actors)

                    if (a.IsPlayer) // dont manipulate the player
                        continue;
                    else
                    {
                        //a.Position = Actor.GetRandomPosition(ScreenWidth, ScreenHeight);
                        a.Position += a.Direction*-3;
                    }

            }

            foreach (var actor in Actor.Actors)
            {
                
                actor.Update();

                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    actor.Position += new Vector2(-2, 0);

                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    actor.Position += new Vector2(2, 0);

                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                    actor.Position += new Vector2(0, -2);

                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                    actor.Position += new Vector2(0, 2);
            }

            if (GamePad.GetState(PlayerIndex.One).Triggers.Right >= 0.2f)
                this.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 100.0f); // 100 FPS
            else if (GamePad.GetState(PlayerIndex.One).Triggers.Left >= 0.2f)
                this.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 10); // 10 FPS
            else
                this.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 60f); // 60 FPS (default)

            fpsCounter.Update(gameTime);


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(96, 96, 111)); // blue-ish gray

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            foreach (Actor actor in Actor.Actors)
            {
                actor.Draw(spriteBatch);
            }

            fpsCounter.Draw(gameTime);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
