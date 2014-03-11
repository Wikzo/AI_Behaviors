using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace _1Behaviour_Demo
{
    class Behavior
    {
        protected float Weight;

        public Behavior(float weight)
        {
            this.Weight = weight;
        }

        public virtual void Update(Actor actor) { }
    }

    class BehaviorConstant : Behavior
    {
        private Vector2 direction;

        public BehaviorConstant(float weight, Vector2 direction) : base(weight)
        {
            this.direction = direction;
        }

        public override void Update(Actor actor)
        {
            actor.Direction += this.direction * this.Weight;
        }
    }

    class BehaviorGamePad : Behavior
    {
        public BehaviorGamePad(float weight) : base(weight) { }

        public override void Update(Actor actor)
        {
            GamePadState padState = GamePad.GetState(PlayerIndex.One);

            if (padState.ThumbSticks.Left.Length() > 0)
                actor.Direction += padState.ThumbSticks.Left * new Vector2(1, -1) * this.Weight; // Y needs to be inverted so character moves upwards
        }
    }
}
