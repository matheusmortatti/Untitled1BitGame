using Microsoft.Xna.Framework;
using SharedCode.Graphics;
using SharedCode.Misc;
using SharedCode.Physics;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedCode
{
    public class Key : GameObject
    {
        public Key(Vector2 position) : base(position, new Box(position, new Vector2(8, 8), true))
        {
            AddComponent(new P8Sprite(105));
            AddComponent(new TopDownPhysics(0, 0));

            InitState("Floating");
        }

        Vector2 initialPosition;
        float amplitude = 5;
        float floatingSpeed = 0.5f;
        private void FloatingStateInit(string prev)
        {
            initialPosition = new Vector2(transform.position.X, transform.position.Y);
        }

        private void FloatingStateUpdate(GameTime gameTime)
        {
            var sin = (float)Math.Sin(gameTime.TotalGameTime.TotalSeconds * 2 * Math.PI * floatingSpeed);
            transform.position = new Vector2(
                initialPosition.X, 
                initialPosition.Y + sin * amplitude
            );
        }

        float followingSpeed = 1f;
        Vector2 colMiddle = new Vector2(4, 4);
        private void FollowingStateUpdate(GameTime gameTime)
        {
            Vector2 targetPosition = this.transform.position;
            var gate = GameObjectManager.FindObjectWithTag("gate");
            if (gate != null && 
                util.CorrespondingMapIndex(gate.transform.position) == util.CorrespondingMapIndex(transform.position))
            {
                targetPosition = (gate.collisionBox == null ? gate.transform.position : gate.collisionBox.middle) - colMiddle;
            }
            else
            {
                var player = GameObjectManager.FindObjectWithTag("player");
                if (player != null)
                {
                    targetPosition = (player.collisionBox == null ? player.transform.position : player.collisionBox.middle) - colMiddle;
                }
            }

            //
            // Go to position
            //

            Vector2 finalPos = new Vector2();

            finalPos.X = Misc.util.Lerp(
                transform.position.X,
                targetPosition.X,
                (transform.position.X - targetPosition.X) * followingSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            finalPos.Y = Misc.util.Lerp(
                transform.position.Y,
                targetPosition.Y,
                (transform.position.Y - targetPosition.Y) * followingSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);

            transform.position = finalPos;
        }

        public override void OnCollisionEnter(GameObject other)
        {
            base.OnCollisionEnter(other);

            if (other.tags.Contains("player") && currentState != "Following")
            {
                InitState("Following");
            }

            if (other is Gate)
            {
                GameObjectManager.AddObject(new Explosion(this.collisionBox.middle));
                done = true;
            }
        }
    }
}
