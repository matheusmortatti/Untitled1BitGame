using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using SharedCode.Graphics;
using SharedCode.Misc;
using SharedCode.Physics;
using SharedCode.Particles;

namespace SharedCode
{
    public enum GooseStates { Wondering, Chasing }

    public class GooseStateMachine : StateMachine<GooseStates>
    {
        private Goose _goose;
        public GooseStateMachine(Goose goose) : base(GooseStates.Chasing)
        {
            _goose = goose;
            this.Init(GooseStates.Chasing);
        }

        Vector2 targetPosition, targetDir;
        void WonderingInit(GooseStates previous)
        {
            _goose.transform.direction = Vector2.Zero;

            var player = GameObjectManager.FindObjectWithTag("player");
            if (player != null)
            {
                var awayDir = _goose.transform.position - player.transform.position;

                // Find a random direction vector that is the direction away from the player rotated
                // by at most 90 or -90 degrees, making sure that the resulting vector is still facing
                // away from the player.
                var randAngle = GameManager.random.NextDouble() * Math.PI - Math.PI / 2;
                targetDir = new Vector2(
                    awayDir.X * (float)Math.Cos(randAngle) - awayDir.Y * (float)Math.Sin(randAngle),
                    awayDir.X * (float)Math.Sin(randAngle) + awayDir.Y * (float)Math.Cos(randAngle));
                targetDir.Normalize();

                targetPosition = _goose.transform.position + targetDir * 10;
            }
        }

        void WonderingState(GameTime gameTime)
        {
            var player = GameObjectManager.FindObjectWithTag("player");
            if (player != null && Vector2.Dot(player.GetComponent<APhysics>().facingDirection, player.transform.position - _goose.transform.position) >= 0)
            {
                Init(GooseStates.Chasing);
            }

            _goose.transform.direction = targetPosition - _goose.transform.position;

            if (Vector2.Dot(_goose.transform.direction, targetDir) < 0)
            {
                _goose.transform.direction = Vector2.Zero;
            }
        }

        void ChasingInit(GooseStates previous)
        {

        }

        void ChasingState(GameTime gameTime)
        {
            var player = GameObjectManager.FindObjectWithTag("player");

            if (player == null)
                return;

            _goose.transform.direction = player.transform.position - _goose.transform.position;

            if (Vector2.Dot(player.GetComponent<APhysics>().facingDirection, player.transform.position - _goose.transform.position) < 0)
            {
                //Init(GooseStates.Wondering);
            }

            if (GameManager.random.NextDouble() < 0.05)
            {
                ParticleManager.AddParticle(new TextParticle(_goose.transform.position + new Vector2(-8, -8), "HONK", 7));
            }
        }
    }

    public class Goose : Enemy
    {
        private GooseStateMachine gooseStateMachine;

        public Goose(Vector2 position) : base(position, new Box(position, new Vector2(8, 8)))
        {
            gooseStateMachine = new GooseStateMachine(this);

            List<P8Sprite> spriteListLeft = new List<P8Sprite>();
            spriteListLeft.Add(new P8StrechedSprite(42, 12, 8));
            spriteListLeft.Add(new P8StrechedSprite(92, 16, 12, 8));
            spriteListLeft.Add(new P8StrechedSprite(58, 12, 8));
            spriteListLeft.Add(new P8StrechedSprite(92, 24, 12, 8));

            List<P8Sprite> spriteListRight = new List<P8Sprite>();
            spriteListRight.Add(new P8StrechedSprite(42, 12, 8, 12, 8, true, false));
            spriteListRight.Add(new P8StrechedSprite(92, 16, 12, 8, 12, 8, true, false));
            spriteListRight.Add(new P8StrechedSprite(58, 12, 8, 12, 8, true, false));
            spriteListRight.Add(new P8StrechedSprite(92, 24, 12, 8, 12, 8, true, false));

            var anim = new P8TopDownAnimator(P8TopDownAnimator.AnimationMode.SIDES_ONLY);

            anim.RunLeft = new SpriteAnimation(spriteListLeft, 0.1f);
            anim.IdleLeft = new SpriteAnimation(new List<P8Sprite>() { new P8StrechedSprite(104, 22, 10, 10) }, 0);

            anim.RunRight = new SpriteAnimation(spriteListRight, 0.1f);
            anim.IdleRight = new SpriteAnimation(new List<P8Sprite>() { new P8StrechedSprite(104, 22, 10, 10, 10, 10, true, false) }, 0);

            AddComponent(anim);
            AddComponent(new TopDownPhysics(15, 10));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            gooseStateMachine.StateDo(gameTime);
        }

        public override void Draw()
        {
            base.Draw();
        }
    }
}
