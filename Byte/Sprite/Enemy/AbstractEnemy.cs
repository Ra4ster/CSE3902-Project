using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Sprite;

/// <summary>
/// An abstract enemy; an instance of a moving, animated sprite with a vengeance...
/// </summary>
public abstract class AbstractEnemy
{
	protected int health { get; set; }

	protected Vector2 position;
	protected Vector2 velocity;

	protected ISprite enemySprite;
	protected abstract void Move(GameTime gameTime);

	public virtual void Draw(SpriteBatch spriteBatch)
	{
		enemySprite.Draw(spriteBatch);
	}

	public virtual void Update(GameTime gameTime)
	{
		Move(gameTime);

		float seconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
		position += velocity * seconds;

		if (enemySprite is MovingAnimatedSprite movingSprite)
			movingSprite.SetPos(position);

		enemySprite.Update(gameTime);
	}

	// Uses inherited constructor
	protected AbstractEnemy(Texture2D texture, ref Vector2 position, ref Vector2 velocity, Color color,
	Rectangle[] sourceRectangles, float frameDuration, float scale = 1.0f)
	{
		this.position = position;
		this.velocity = velocity;
		enemySprite = new MovingAnimatedSprite(texture, ref position, ref velocity, color, sourceRectangles, frameDuration, scale);
	}
}