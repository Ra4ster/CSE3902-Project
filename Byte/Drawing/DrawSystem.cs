
using System.Collections.ObjectModel;
using Byte.Drawing;
using Byte.Rooms;
using Microsoft.Xna.Framework.Graphics;

namespace Byte
{
    public class DrawSystem
    {
        public required Collection<IDrawable> drawables;
        public required string background;
        public required SpriteBatch painter;
        public DrawSystem(SpriteBatch painter, IDrawable[] drawables, string background)
        {
            this.drawables = new Collection<IDrawable>(drawables.ToList());
            this.background = background;
            this.painter = painter;
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

        public void Draw()
        {
            BackgroundManager.Instance.Draw(background, painter);

            foreach (IDrawable drawable in drawables)
                drawable.Draw(painter);
        }
    }
}