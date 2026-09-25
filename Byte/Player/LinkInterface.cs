using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Byte.Sprite;

namespace Byte.Player
{

    public interface LinkInterface
    {
        public float Health { get; set; }
        public float MaximumHealth { get; set; }

        public Vector2 Position { get; set; }

        public Vector2 Direction { get; set; }

        public ISprite currentSprite { get; set; }

        public SpriteBatch spriteBatch { get; set; }


        public void Update(GameTime gameTime);

        public void TakeDamage();

        public void Move(GameTime gameTime);

        public void Attack();

        public void UseItem();

        public void GetNewState(IPlayerState state);



    }
}
