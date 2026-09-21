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
}