
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace _1Behaviour_Demo
{
    public class FPSCounter : DrawableGameComponent
    {
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;

        int frameRate = 0;
        int frameCounter = 0;
        TimeSpan elapsedTime = TimeSpan.Zero;


        public FPSCounter(Game game, SpriteBatch Batch, SpriteFont Font)
            : base(game)
        {
            spriteFont = Font;
            spriteBatch = Batch;
        }


        public override void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }
        }


        public override void Draw(GameTime gameTime)
        {
            frameCounter++;

            string fps = string.Format("FPS: {0} GC memory : {1}", frameRate, GC.GetTotalMemory(false));

            spriteBatch.DrawString(spriteFont, fps, new Vector2(0, 0), Color.White);
            spriteBatch.DrawString(spriteFont, fps, new Vector2(1, 1), Color.Black);
        }
    }
}