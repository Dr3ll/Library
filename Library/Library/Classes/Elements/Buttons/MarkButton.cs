using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Library.Classes
{
    class MarkButton : Button
    {
        public static Texture2D DOT;

        public bool Active = false;
        Mark _mark;

        public void Resize(int w, int h)
        {
            _hitbox.Width = w;
            _hitbox.Height = h;
        }

        public MarkButton(int width, int height, Mark mark)
            : base(new Rectangle(0, 0, width, height))
        {
            _mark = mark;


        }


        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            sb.Draw(DOT, _hitbox, _mark.Color);
        }

        protected override void OnHit()
        {
            if (Active)
                ReadState._scrolling = -(_mark.Upper.Y - 210);
        }

    }
}
