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
    public enum GooseStates { Wondering, Chasing, Following, Still }

    public class GooseStateMachine : StateMachine<GooseStates>
    {
        private Goose _goose;
        public GooseStateMachine(Goose goose) : base(GooseStates.Wondering)
        {
            _goose = goose;
            this.Init(GooseStates.Wondering);
        }

        void StillInit(GooseStates previous)
        {
            _goose.transform.direction = Vector2.Zero;

            TaskScheduler.AddTask(() => { if(State == GooseStates.Still) Init(GooseStates.Wondering); }, 2, 2, _goose.id);
        }

        void StillState(GameTime gameTime)
        {
            
        }

        Vector2 targetPosition, targetDir;
        TaskScheduler.Task wonderingDirTask;
        double changeDirTime = 5;
        void WonderingInit(GooseStates previous)
        {
            var physics = _goose.GetComponent<APhysics>();
            if (physics!= null)
                physics.maxSpeed = _goose.baseSpeed;

            _goose.transform.direction = Vector2.Zero;

            Action wonderingDirFunc = () =>
            {

                // Find a random direction vector that is the direction away from the player rotated
                // by at most 90 or -90 degrees, making sure that the resulting vector is still facing
                // away from the player.
                var randAngle = GameManager.random.NextDouble() * 2 * Math.PI;
                targetDir = new Vector2((float)Math.Sin(randAngle), (float)Math.Cos(randAngle));
                targetDir.Normalize();

                Debug.Log($"{_goose.GetType().FullName} Wondering State direction angle is {randAngle}");

                targetPosition = _goose.transform.position + targetDir * 20;
            };

            wonderingDirTask = TaskScheduler.AddTask(wonderingDirFunc, changeDirTime, -1, _goose.id);

            //
            // Choose a direction away from the player at first.
            //

            var p = GameObjectManager.FindObjectWithTag("player");
            if (p != null)
            {
                var awayDir = _goose.transform.position - p.transform.position;

                // Find a random direction vector that is the direction away from the player rotated
                // by at most 90 or -90 degrees, making sure that the resulting vector is still facing
                // away from the player.
                var randAngle = GameManager.random.NextDouble() * Math.PI - Math.PI / 2;
                targetDir = new Vector2(
                    awayDir.X * (float)Math.Cos(randAngle) - awayDir.Y * (float)Math.Sin(randAngle),
                    awayDir.X * (float)Math.Sin(randAngle) + awayDir.Y * (float)Math.Cos(randAngle));
                targetDir.Normalize();

                Debug.Log($"{_goose.GetType().FullName} Wondering State direction angle is {randAngle}");

                targetPosition = _goose.transform.position + targetDir * 10;
            }

        }

        void WonderingState(GameTime gameTime)
        {
            var player = GameObjectManager.FindObjectWithTag("player");
            if (player != null && Vector2.Dot(player.GetComponent<APhysics>().facingDirection, player.transform.position - _goose.transform.position) >= 0 && GameManager.random.NextDouble() < 0.005)
            {
                Init(GooseStates.Following);
                TaskScheduler.RemoveTask(wonderingDirTask);
            }

            _goose.transform.direction = targetPosition - _goose.transform.position;

            if (Vector2.Dot(_goose.transform.direction, targetDir) < 0)
            {
                _goose.transform.direction = Vector2.Zero;
            }
        }

        void ChasingInit(GooseStates previous)
        {
            var physics = _goose.GetComponent<APhysics>();
            if (physics != null)
                physics.maxSpeed = _goose.baseSpeed * 2;
        }

        void ChasingState(GameTime gameTime)
        {
            var player = GameObjectManager.FindObjectWithTag("player");

            if (player == null)
                return;

            _goose.transform.direction = player.transform.position - _goose.transform.position;

            //if (Vector2.Dot(player.GetComponent<APhysics>().facingDirection, player.transform.position - _goose.transform.position) < 0)
            //{
            //    Init(GooseStates.Wondering);
            //}

            if (GameManager.random.NextDouble() < 0.05)
            {
                ParticleManager.AddParticle(new TextParticle(_goose.transform.position + new Vector2(-8, -8), "HONK", 7));
            }
        }

        void FollowingInit(GooseStates previous)
        {
            var physics = _goose.GetComponent<APhysics>();
            if (physics != null)
                physics.maxSpeed = _goose.baseSpeed * 2;
        }

        void FollowingState(GameTime gameTime)
        {
            var player = GameObjectManager.FindObjectWithTag("player");

            if (player == null)
                return;

            _goose.transform.direction = player.transform.position - _goose.transform.position;

            if ((player.transform.position - _goose.transform.position).LengthSquared() < 500)
            {
                Init(GooseStates.Still);
            }

            if (GameManager.random.NextDouble() < 0.05)
            {
                ParticleManager.AddParticle(new TextParticle(_goose.transform.position + new Vector2(-8, -8), "HONK", 7));
            }
        }

        public void CleanUp()
        {
            TaskScheduler.RemoveTask(wonderingDirTask);
        }
    }

    public class Goose : Enemy
    {
        private GooseStateMachine gooseStateMachine;

        public float baseSpeed { get; private set; } = 10;

        public Goose(Vector2 position, int spriteIndex) : base(position, new Box(position, new Vector2(8, 8)), spriteIndex)
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
            AddComponent(new TopDownPhysics(15, 5, 0.9999f));

            doesDamage = false;

            AddComponent(new FillBar(new Vector2(0, -2), 8, 0, lifeTime));

            var newTags = tags;
            newTags.Remove("enemy");
            tags = newTags;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            gooseStateMachine.StateDo(gameTime);

            if (!doesDamage && gooseStateMachine.State == GooseStates.Chasing)
                doesDamage = true;
        }

        public override void Draw()
        {
            if (gooseStateMachine.State == GooseStates.Chasing)
            {
                GameManager.Pico8.Memory.drawState.Pal(7, 8);
            }

            base.Draw();

            if (gooseStateMachine.State == GooseStates.Chasing)
            {
                GameManager.Pico8.Memory.drawState.Pal(7, 7);
            }
        }

        public override void OnCollision(GameObject other)
        {
            base.OnCollision(other);

            if (timePiecesSpawned != null)
            {
                foreach(var tp in timePiecesSpawned )
                {
                    tp.objectFollowing = this;
                }
            }

            if (other.tags.Contains("player_attack"))
            {
                if (gooseStateMachine.State != GooseStates.Chasing)
                {
                    gooseStateMachine.Init(GooseStates.Still);
                    TaskScheduler.AddTask(() => gooseStateMachine.Init(GooseStates.Chasing), 1, 1, this.id);
                }
            }
        }

        public override void CleanUp()
        {
            base.CleanUp();

            gooseStateMachine.CleanUp();
        }
    }
}
