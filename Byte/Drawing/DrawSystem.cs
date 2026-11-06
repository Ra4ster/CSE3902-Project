
using System.Collections.ObjectModel;
using Byte.Drawing;
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

        public void Draw()
        {
            foreach (IDrawable drawable in drawables)
                drawable.Draw(painter);

            BackgroundManager.Instance.Draw(background, painter);
        }
    }
}