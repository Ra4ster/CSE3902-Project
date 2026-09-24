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
        // Contains the starting position for each link
        public static readonly Vector2 startPos = new Vector2(220.0f);
        public bool isGameStart = true; // Checks if game was just started

        // use to see if movement/other needs to be stopped - cant move when attacking
        public bool IsAttacking { get; set; } = false;
        
        // The starting reference point for most animations and sprite drawing
        public static readonly Vector2 LinkOrigin = new Vector2(8, 13);

        // Misleading name - direction sprite should be moving
        public Vector2 MovementSpeed { get; set; } = Vector2.Zero;

        // current health
        public float Health { get; set; } = 3;

        // maximum health
        public float MaximumHealth { get; set; } = 20;

        // Current position of the character
        public Vector2 Position { get; set; } = position;

        // Direction that link is facing
        public Vector2 Direction { get; set; } = CardinalDirections.South;


        // The sprite sheet with link textures
        public Texture2D SpriteSheet { get; set; } = sheet;

        // The time in between each frame of animation
        public TimeSpan TimeDelay { get; set; } = TimeSpan.FromMilliseconds(100);

        // the current state link is in
        public IPlayerState state { get; set; } = new PlayerIdleState();

        // the current frames that need to be drawn
        public Rectangle[] sourceRects { get; set; } = new Rectangle[1];

        // the sprite that is active for link
        public ISprite currentSprite { get; set; } = newSprite;

        // the sprite batch used for drawing
        public SpriteBatch spriteBatch { get; set; } = newSpriteBatch;

        // controllers for input
        private MouseController mouseControls { get; set; } = (MouseController)mouseControl;
        private KeyboardController kbControls { get; set; } = (KeyboardController)kbControl;

        
        // The list of keys pressed
        private Keys[] keys = new Keys[1];


        public void Update(GameTime gameTime)
        {
            // Check which keys are pressed - if they affect idle state update if not set to idle
            keys = kbControls.currentState.GetPressedKeys();
            if (shouldBeIdle(keys) && !IsAttacking){
                GetNewState(new PlayerIdleState());
            }
            state.Update(this,gameTime);
        }

        // Show damage texture
        public void TakeDamage()
        {

        }

        // update player position
        public void Move(GameTime gameTime)
        {
            float seconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position += MovementSpeed * seconds;
        }

        // apply the attack animation and disable movment until attack is finished
        public void Attack()
        {
            this.IsAttacking = true;
            this.MovementSpeed = new Vector2(0,0);
            if (this.state is not PlayerAttackState)
            {
                this.GetNewState(new PlayerAttackState());
            }
        }

        // display the item currently being used
        public void UseItem()
        {
            
        }

        // update state to the new type that is being passed
        public void GetNewState(IPlayerState state)
        {
            // if the state passed is the same as current state dont do anything
            if (this.state.GetType() != state.GetType())
            {


                this.state = state;
                this.currentSprite = null;
            }
        }

        // check if a key that is pressed has a different state attached to it
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
        

        
       

        
    }
}
