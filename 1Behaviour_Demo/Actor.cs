using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _1Behaviour_Demo
{
    class Actor
    {
        public static List<Actor> Actors = new List<Actor>();
        private static Random random = new Random();

        public Color Color;
        public Texture2D Texture;
        public Vector2 Position;
        public Vector2 Direction; // unit vector, point in a direction to move in conjunction with Speed
        public float Speed;
        public Vector2 Origin;
        public List<Behavior> BehaviorList = new List<Behavior>();

        public static Vector2 GetRandomPosition(int rangeX, int rangeY)
        {
            return new Vector2(random.Next(rangeX), random.Next(rangeY));
        }

        public static Vector2 GetRandomDirection()
        {
            // random value between 0 to 2 pi circle
            double rotation = random.NextDouble() * MathHelper.TwoPi;

            // convert from radians to vector2
            return new Vector2((float)Math.Cos(rotation), (float)Math.Sin(rotation));
        }

        public Actor(Color color, Texture2D texture)
        {
            Actors.Add(this);

            this.Color = color;
            this.Texture = texture;
            this.Origin = new Vector2(texture.Width / 2, texture.Height / 2);

            this.Position = new Vector2(Game1.ScreenWidth / 2,  Game1.ScreenHeight / 2);
            this.Direction = new Vector2(0, -1);
        }

        public void Update()
        {
            foreach (Behavior behavior in BehaviorList)
                behavior.Update(this);
        
            // normalize direction
            if (this.Direction.Length() > 0f)
                this.Direction.Normalize();

            this.Position += this.Direction * this.Speed;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // convert vector2 to radians
            float rotation = (float) Math.Atan2(this.Direction.Y, this.Direction.X);// +MathHelper.PiOver2; // add 90 degree offset to orientate sprite correctly
            
            spriteBatch.Draw(this.Texture, this.Position, null, this.Color, rotation, this.Origin, 1f,
                             SpriteEffects.None, 0f);

            // screen wrapping physically
            if (Position.X < -Texture.Width) Position.X += Game1.ScreenWidth;
            if (Position.X > Game1.ScreenWidth) Position.X -= Game1.ScreenWidth;

            if (Position.Y < -Texture.Height) Position.Y += Game1.ScreenHeight;
            if (Position.Y > Game1.ScreenHeight) Position.Y -= Game1.ScreenHeight;

            // SCREEN WRAPPING VISUAL START ------------------------
            // check horizontal screen wrapping
            if (this.Position.X < 0)
            {
                spriteBatch.Draw(this.Texture, new Vector2(this.Position.X + Game1.ScreenWidth, this.Position.Y), 
                    null, this.Color, rotation, this.Origin, 1f,
                             SpriteEffects.None, 0f);
            }
            else if (this.Position.X + this.Texture.Width > Game1.ScreenWidth)
            {
                spriteBatch.Draw(this.Texture, new Vector2(this.Position.X - Game1.ScreenWidth, this.Position.Y),
                    null, this.Color, rotation, this.Origin, 1f,
                             SpriteEffects.None, 0f);
            }

            // check vertical screen wrapping
            if (this.Position.Y < 0)
            {

                spriteBatch.Draw(this.Texture, new Vector2(this.Position.X, this.Position.Y + Game1.ScreenHeight),
                    null, this.Color, rotation, this.Origin, 1f,
                             SpriteEffects.None, 0f);
            }
            else if (this.Position.Y + this.Texture.Height > Game1.ScreenHeight)
            {
                spriteBatch.Draw(this.Texture, new Vector2(this.Position.X, this.Position.Y - Game1.ScreenHeight),
                    null, this.Color, rotation, this.Origin, 1f,
                             SpriteEffects.None, 0f);
            }

            // SCREEN WRAPPING VISUAL END ------------------------
        }
    }
}
