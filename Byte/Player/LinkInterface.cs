using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Player
{
    
    public interface LinkInterface
    {
        public float Health { get; set; }
        public float MaximumHealth { get; set; }

        public Vector2 Position { get; set; }

        public Vector2 Direction { get; set; }


        public void Update();

        public void TakeDamage();

        public void Move(Vector2 direction);

        public void Attack();

        public void UseItem();

        

    }
}
