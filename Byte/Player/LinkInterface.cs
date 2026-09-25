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
        // current health
        public float Health { get; set; }

        // maximum health
        public float MaximumHealth { get; set; }

        public Vector2 Position { get; set; }

        // direction link is facing
        public Vector2 Direction { get; set; }

        // current sprite link has
        public ISprite currentSprite { get; set; }

        // sprite batch to draw
        public SpriteBatch spriteBatch { get; set; }

        // update link state
        public void Update(GameTime gameTime);

        // take damage
        public void TakeDamage();

        // move player
        public void Move(GameTime gameTime);

        // cause player to attack
        public void Attack();

        // make player use item
        public void UseItem();


        // update player state
        public void GetNewState(IPlayerState state);



    }
}
