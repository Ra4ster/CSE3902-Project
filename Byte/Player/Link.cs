using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Player
{
    public class Link : LinkInterface
    {

        public float Health { get; set; } = 3;
        public float MaximumHealth { get; set; } = 20;

        public Vector2 Position { get; set; } = Vector2.Zero;

        public Vector2 Direction { get; set; } = CardinalDirections.South;

        public required Texture2D SpriteSheet { get; set; }

        public TimeSpan TimeDelay { get; set; } = TimeSpan.FromMilliseconds(100);

        public IPlayerState state { get; set; } = new PlayerIdleState();

        public Rectangle[] sourceRects { get; set; } = new Rectangle[1];


        public void Update()
        {
            state.Update(this);
        }

        public void TakeDamage()
        {

        }

        public void Move(Vector2 direction)
        {

        }

        public void Attack()
        {

        }

        public void UseItem()
        {
            
        }

        public Rectangle[] IdleFrames =
        {
            new Rectangle(1, 11, 16, 16), // Idle South
            new Rectangle(35, 11, 16, 16), // Idle East  - flip for West
            new Rectangle(69, 11, 16,16) // idle North,
        };
    }
}
