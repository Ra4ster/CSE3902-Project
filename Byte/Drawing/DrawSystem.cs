
using System.Collections.ObjectModel;
using Byte.Rooms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Drawing
{
    public class DrawSystem
    {
        public required Collection<IDrawable> drawables;
        public required string background;
        public required SpriteBatch painter;
        private Matrix spriteTransform = Matrix.Identity;
        private static readonly RasterizerState ClipRasterizer = new() { ScissorTestEnable = true };

        private readonly GraphicsDevice device;

        public DrawSystem(GraphicsDevice device, SpriteBatch painter, IDrawable[] drawables, string background)
        {
            this.drawables = new Collection<IDrawable>(drawables.ToList());
            this.background = background;
            this.painter = painter;
            this.device = device;
        }

        public void AddDrawable(IDrawable drawable)
        {
            drawables.Add(drawable);
        }

        public void DrawRoom(IRoom room, SpriteBatch painter)
        {
            BackgroundManager.Instance.Draw(room.Background, painter);
            foreach (var block in room.Blocks) block.Draw(painter);
            foreach (var item in room.Items) item.Draw(painter);
            foreach (var enemy in room.Enemies) enemy.Draw(painter);
            foreach (var door in room.Doors) door.Draw(painter);
        }

        private Rectangle ToBackBuffer(Rectangle area)
        {
            Vector2 topLeft = Vector2.Transform(new Vector2(area.Left, area.Top), spriteTransform);
            Vector2 bottomRight = Vector2.Transform(new Vector2(area.Right, area.Bottom), spriteTransform);
            Rectangle pixels = new Rectangle((int)topLeft.X, (int)topLeft.Y,
                (int)Math.Ceiling(bottomRight.X - topLeft.X), (int)Math.Ceiling(bottomRight.Y - topLeft.Y));
            return Rectangle.Intersect(pixels, device.Viewport.Bounds);
        }

        public void Begin(SpriteBatch painter, Vector2 translation, Rectangle? clip = null)
        {
            device.ScissorRectangle = ToBackBuffer(clip ?? GameConstants.WINDOW_SIZE);

            painter.Begin(
                sortMode: SpriteSortMode.Deferred,
                blendState: BlendState.NonPremultiplied,
                samplerState: SamplerState.PointClamp, // Disables sampling outside pixel bounds
                rasterizerState: ClipRasterizer,
                transformMatrix: Matrix.CreateTranslation(translation.X, translation.Y, 0f) * spriteTransform // Transforms drawing to a space separate from window bounds for resizing.
            );
        }

        public void Draw()
        {
            BackgroundManager.Instance.Draw(background, painter);

            foreach (IDrawable drawable in drawables)
                drawable.Draw(painter);
        }
    }
}