using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Sprite;

/// <summary>
/// An abstract enemy; an instance of a moving, animated sprite with a vengeance...
/// </summary>
public abstract class AbstractEnemy : MovingAnimatedSprite
{
	protected int health { get; set; }
	protected abstract void Move(GameTime gameTime);
	public override void Update(GameTime gameTime)
	{
		Move(gameTime);
		base.Update(gameTime);
	}

	// Uses inherited constructor
	protected AbstractEnemy(Texture2D texture, ref Vector2 position, ref Vector2 velocity, Color color,
	Rectangle[] sourceRectangles, float frameDuration, float scale = 1.0f)
		: base(texture, ref position, ref velocity, color, sourceRectangles, frameDuration, scale) { }
}