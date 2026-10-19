using Byte.Collidable;
using Byte.Controller;
using Byte.Sprite;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Byte.Player
{
    public class Link(Vector2 position, ISprite newSprite, SpriteBatch newSpriteBatch, Texture2D sheet, IController mouseControl, IController kbControl) : LinkInterface, Byte.Collidable.ICollidable
    {
        public CollisionProperties Properties { get; } =
        new CollisionProperties
        {
            Type = CollidableTypes.Player
        };
        public const int SpriteScale = 6;
        public Rectangle BoundingBox => new Rectangle(
            (int)Position.X - 8 * SpriteScale, (int)Position.Y - 13 * SpriteScale,
            16 * SpriteScale, 16 * SpriteScale);
        public SwordHitbox Sword { get; } = new SwordHitbox();
        public bool isDamaged = false; // Checks if link has taken damage
        private float damageTimer = 0f;
        private const float DamageDuration = 0.5f;
        public int HitsTaken { get; private set; }
        public bool isGameStart = true; // Checks if game was just started

        // use to see if movement/other needs to be stopped - cant move when attacking
        public bool IsAttacking { get; set; } = false;

        // The starting reference point for most animations and sprite drawing
        public static readonly Vector2 LinkOrigin = new Vector2(8, 13);



        // current health
        public float Health { get; set; } = 3;

        // maximum health
        public float MaximumHealth { get; set; } = 20;

        // Current position of the character
        public Vector2 Position { get; set; } = position;
        // Misleading name - direction sprite should be moving
        public float MovementSpeed { get; set; } = 200f;

        public Vector2 Velocity { get; set; } = Vector2.Zero;

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
        public ISprite? currentSprite { get; set; } = newSprite;

        // the sprite batch used for drawing
        public SpriteBatch spriteBatch { get; set; } = newSpriteBatch;

        // controllers for input
        private MouseController mouseControls { get; set; } = (MouseController)mouseControl;
        private KeyboardController kbControls { get; set; } = (KeyboardController)kbControl;

        // How quickly Link slows down (higher = more friction)
        public float Deceleration { get; set; } = 1600f;

        // Minimum velocity before Link fully stops
        private const float StopThreshold = 5f;

        // The list of keys pressed
        private Keys[] keys = new Keys[1];




        public void Update(GameTime gameTime)
        {
            // Check which keys are pressed - if they affect idle state update if not set to idle
            keys = kbControls.currentState.GetPressedKeys();
            if (shouldBeIdle(keys) && !IsAttacking)
            {
                GetNewState(new PlayerIdleState());
                ApplyVelocityDecay(gameTime);
            }
           
            if (IsAttacking)
            {
                Velocity = Vector2.Zero;
            }
            state.Update(this, gameTime);
            float seconds =
                (float)gameTime.ElapsedGameTime.TotalSeconds;

            Position += Velocity * seconds;

            // Keep the sprite aligned with Link's position
            if (currentSprite is MovingAnimatedSprite animatedSprite)
                animatedSprite.SetPos(Position);
            else if (currentSprite is StaticSprite staticSprite)
                staticSprite.SetPos(Position);
            if (isDamaged)
            {
                damageTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (currentSprite != null)
                {
                    currentSprite.Color = (int)(damageTimer / 0.1f) % 2 == 0
                        ? Color.Red : Color.White;
                }

                if (damageTimer <= 0)
                {
                    isDamaged = false;

                    if (currentSprite != null)
                    {
                        currentSprite.Color = Color.White;
                    }
                }
            }
        }

        // Show damage texture
        public void TakeDamage()
        {
            if (isDamaged)
                return;

            isDamaged = true;
            damageTimer = DamageDuration;
            // This framework scene has no death state yet; keep collisions testable at zero HP.
            Health = Math.Max(0, Health - 1);
            HitsTaken++;
            if (currentSprite != null)
                currentSprite.Color = Color.Red;


        }

        // Collision correction happens after animation updates. Keep drawing in sync.
        public void CorrectPosition(Vector2 position)
        {
            Position = position;
            if (currentSprite is MovingAnimatedSprite sprite)
                sprite.SetPos(position);
            else if (currentSprite is StaticSprite staticSprite)
                staticSprite.SetPos(position);

            

        }


        // update player position
        public void Move(Vector2 direction)
        {

            Velocity = MovementSpeed * direction;
        }

        // apply the attack animation and disable movment until attack is finished
        public void Attack()
        {
            if (!IsAttacking)
                Sword.BeginAttack();
            this.IsAttacking = true;
            Velocity = new Vector2(0, 0);
            if (this.state is not PlayerAttackState)
            {
                this.GetNewState(new PlayerAttackState());
            }
        }

        // display the item currently being used
        public void UseItem()
        {
            this.IsAttacking = true;
            Velocity = new Vector2(0, 0);
            if (this.state is not PlayerUseItemState)
            {
                this.GetNewState(new PlayerUseItemState());
            }
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
            Array.Exists(keys, key => key == Keys.Up) ||
            Array.Exists(keys, key => key == Keys.Down) ||
            Array.Exists(keys, key => key == Keys.Left) ||
            Array.Exists(keys, key => key == Keys.Right) ||
            Array.Exists(keys, key => key == Keys.Z) ||
            Array.Exists(keys, key => key == Keys.N);


            return !state;
        }







        public void ApplyVelocityDecay(GameTime gameTime)
        {
            float deltaTime =
                (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Reduce velocity toward zero
            Velocity = new Vector2(
                MoveTowardsZero(Velocity.X, Deceleration * deltaTime),
                MoveTowardsZero(Velocity.Y, Deceleration * deltaTime)
            );

            // Stop tiny residual movement
            if (Velocity.Length() < StopThreshold)
            {
                Velocity = Vector2.Zero;
            }
        }

        private float MoveTowardsZero(float value, float amount)
        {
            if (value > 0)
                return Math.Max(0, value - amount);

            if (value < 0)
                return Math.Min(0, value + amount);

            return 0;
        }
    }
}