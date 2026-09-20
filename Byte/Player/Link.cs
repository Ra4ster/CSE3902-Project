using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Sprint0.Controller;
using Sprint0.Sprite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Byte.Player
{
    public class Link(Vector2 position, ISprite newSprite, SpriteBatch newSpriteBatch,Texture2D sheet, IController mouseControl, IController kbControl) : LinkInterface
    {
        public Vector2 MovementSpeed { get; set; } = Vector2.Zero;
        public float Health { get; set; } = 3;
        public float MaximumHealth { get; set; } = 20;

        public Vector2 Position { get; set; } = position;

        public Vector2 Direction { get; set; } = CardinalDirections.South;

        public Texture2D SpriteSheet { get; set; } = sheet;

        public TimeSpan TimeDelay { get; set; } = TimeSpan.FromMilliseconds(100);

        public IPlayerState state { get; set; } = new PlayerIdleState();

        public Rectangle[] sourceRects { get; set; } = new Rectangle[1];

        public ISprite currentSprite { get; set; } = newSprite;

        public SpriteBatch spriteBatch { get; set; } = newSpriteBatch;

        private MouseController mouseControls { get; set; } = (MouseController)mouseControl;
        private KeyboardController kbControls { get; set; } = (KeyboardController)kbControl;

        private Keys[] keys = new Keys[1];


        public void Update(GameTime gameTime)
        {
            keys = kbControls.currentState.GetPressedKeys();
            if (shouldBeIdle(keys)){
                GetNewState(new PlayerIdleState());
            }
            state.Update(this,gameTime);
        }

        public void TakeDamage()
        {

        }

        public void Move(GameTime gameTime)
        {
            float seconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position += MovementSpeed * seconds;
        }

        public void Attack()
        {

        }

        public void UseItem()
        {
            
        }

        public void GetNewState(IPlayerState state)
        {
            if (this.state.GetType != state.GetType)
            {


                this.state = state;
                this.currentSprite = null;
            }
        }

        public bool shouldBeIdle(Keys[] keys)
        {
            bool state = 
            Array.Exists(keys, key => key == Keys.W) ||
            Array.Exists(keys, key => key == Keys.A) ||
            Array.Exists(keys, key => key == Keys.S) ||
            Array.Exists(keys, key => key == Keys.D) ||
            Array.Exists(keys, key => key == Keys.Z)||
            Array.Exists(keys, key => key == Keys.I) ||
            Array.Exists(keys, key => key == Keys.U) ||
            Array.Exists(keys, key => key == Keys.N);


            return !state;
        }

        public Rectangle[] IdleFrames =
        {
            new Rectangle(1, 11, 16, 16), // Idle South
            new Rectangle(35, 11, 16, 16), // Idle East  - flip for West
            new Rectangle(69, 11, 16,16) // idle North,
        };

        
    }
}
