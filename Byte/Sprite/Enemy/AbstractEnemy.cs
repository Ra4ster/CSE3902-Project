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

	// on-screen size of the largest frame, used to keep the whole sprite inside the window
	private readonly Vector2 size;

	public virtual void Draw(SpriteBatch spriteBatch)
	{
		enemySprite.Draw(spriteBatch);
	}

	public virtual void Update(GameTime gameTime)
	{
		Move(gameTime);

		float seconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
		position = ClampToWindow(position + velocity * seconds);

		if (enemySprite is MovingAnimatedSprite movingSprite)
			movingSprite.SetPos(position);

		enemySprite.Update(gameTime);
	}

	// Uses inherited constructor
	protected AbstractEnemy(Texture2D texture, ref Vector2 position, ref Vector2 velocity, Color color,
	Rectangle[] sourceRectangles, float frameDuration, float scale = 1.0f)
	{
		foreach (Rectangle frame in sourceRectangles)
			size = Vector2.Max(size, new Vector2(frame.Width, frame.Height) * scale);

		this.position = ClampToWindow(position);
		this.velocity = velocity;
		enemySprite = new MovingAnimatedSprite(texture, ref position, ref velocity, color, sourceRectangles, frameDuration, scale);
	}

	/// <summary>
	/// Moves a point so the sprite drawn there stays fully inside GameConstants.WINDOW_SIZE.
	/// </summary>
	protected Vector2 ClampToWindow(Vector2 point)
	{
		Rectangle window = GameConstants.WINDOW_SIZE;
		Vector2 min = new Vector2(window.Left, window.Top);
		Vector2 max = Vector2.Max(min, new Vector2(window.Right, window.Bottom) - size);
		return Vector2.Clamp(point, min, max);
	}
}