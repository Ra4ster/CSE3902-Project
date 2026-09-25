using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Sprint0.Sprite;

public interface ISprite
{

    public SpriteEffects Effects { get; set; }
    /// <summary>
    /// Draws this to the screen using the sprite batch.
    /// </summary>
    /// <param name="spriteBatch">Sprite batch, REQUIRES: Begin() has been called.</param>
    public void Draw(SpriteBatch spriteBatch);

    /// <summary>
    /// Updates this sprite according to timed events.
    /// </summary>
    /// <param name="gameTime">Time in the game.</param>
    public void Update(GameTime gameTime);

    // Updates position to draw sprite
    public void MoveOrigin(Vector2 origin);

    // Allows each frame to have its own origin to help prevent movment across screen by single sprites
    public void SetFrameOrigin(Vector2[] Origins);

    // Loop and Is finished used for animations that don't loop such as attacking - can be used to stop animations
    public bool Loop { get; set; }
    public bool IsFinished {  get; set; }
}