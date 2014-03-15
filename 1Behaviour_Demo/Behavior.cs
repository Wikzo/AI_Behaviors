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

    class BehaviorWander : Behavior
    {
        // similar to BehaviorConstant. However, after a set interval it finds a new random direction to move in

        //private static Random random = new Random();

        private int changeInterval;
        private int tick; // accumulates updates
        private Vector2 direction;

        public BehaviorWander(float weight, int changeInterval) : base(weight)
        {
            this.changeInterval = changeInterval;
        }

        public override void Update(Actor actor)
        {
            if (tick == 0) // when time is up, find new direction
                this.direction = Actor.GetRandomDirection();

            tick++;
            tick %= this.changeInterval; // tick goes from 0 to changeInterval

            actor.Direction += this.direction * this.Weight;
        }
    }

    class BehavoirSeek : Behavior
    {
        private Actor target;

        public BehavoirSeek(float weight, Actor target) : base(weight)
        {
            this.target = target;
        }

        public override void Update(Actor actor)
        {
            Vector2 targetDirection = target.Position - actor.Position;
            targetDirection.Normalize(); // unit vector - http://www.fundza.com/vectors/normalize/
            
            actor.Direction += targetDirection * this.Weight;
        }
    }
}
