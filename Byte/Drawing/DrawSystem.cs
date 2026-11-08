using Byte.Player;
using Byte.Rooms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Byte.Drawing
{
    public sealed class DrawSystem
    {
        private static readonly RasterizerState ClipRasterizer = new() { ScissorTestEnable = true };
        private readonly GraphicsDevice device;
        private readonly SpriteBatch painter;
        private Matrix spriteTransform = Matrix.Identity;

        public DrawSystem(GraphicsDevice device, SpriteBatch painter)
        {
            this.device = device;
            this.painter = painter;
        }

        public void SetSpriteTransform(Matrix transform) => spriteTransform = transform;

        public void Begin(Vector2 translation, Rectangle? clip = null)
        {
            device.ScissorRectangle = ToBackBuffer(clip ?? GameConstants.WINDOW_SIZE);
            painter.Begin(
                sortMode: SpriteSortMode.Deferred,
                blendState: BlendState.NonPremultiplied,
                samplerState: SamplerState.PointClamp,
                rasterizerState: ClipRasterizer,
                transformMatrix: Matrix.CreateTranslation(translation.X, translation.Y, 0f) * spriteTransform);
        }

        public void EndBatch() => painter.End();

        public void DrawRoom(IRoom room)
        {
            BackgroundManager.Instance.Draw(room.Background, painter);
            foreach (var block in room.Blocks) block.Draw(painter);
            foreach (var item in room.Items) item.Draw(painter);
            foreach (var enemy in room.Enemies) enemy.Draw(painter);
            foreach (var door in room.Doors) door.Draw(painter);
        }

        public void DrawLink(Link link)
        {
            ArgumentNullException.ThrowIfNull(link);
            link.state.Draw(link);
        }

        public void DrawLink(Link link, Vector2 worldPosition, Rectangle? clip = null)
        {
            ArgumentNullException.ThrowIfNull(link);
            Begin(worldPosition - link.Position, clip);
            DrawLink(link);
            EndBatch();
        }

        private Rectangle ToBackBuffer(Rectangle area)
        {
            Vector2 topLeft = Vector2.Transform(new Vector2(area.Left, area.Top), spriteTransform);
            Vector2 bottomRight = Vector2.Transform(new Vector2(area.Right, area.Bottom), spriteTransform);
            Rectangle pixels = new Rectangle((int)topLeft.X, (int)topLeft.Y,
                (int)Math.Ceiling(bottomRight.X - topLeft.X), (int)Math.Ceiling(bottomRight.Y - topLeft.Y));
            return Rectangle.Intersect(pixels, device.Viewport.Bounds);
        }
    }
}
