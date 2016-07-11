using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Library.Classes
{
    class Border
    {
        private Rectangle _top;
        private Rectangle _right;
        private Rectangle _bottom;
        private Rectangle _left;

        private Texture2D _tex;
        private Color _color;

        public Border(Point topLeft, Point topRight, Point bottomRight, Point bottomLeft, int thickness, Texture2D tex, Color color)
        {
            _tex = tex;
            _color = color;

            _top = new Rectangle(topLeft.X, topLeft.Y, topRight.X - topLeft.X, thickness);
            _bottom = new Rectangle(bottomLeft.X, bottomLeft.Y - thickness, bottomRight.X - bottomLeft.X, thickness);
            _right = new Rectangle(topRight.X - thickness, topRight.Y, thickness, bottomRight.Y - topRight.Y);
            _left = new Rectangle(topLeft.X, topLeft.Y, thickness, bottomLeft.Y - topLeft.Y);
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(_tex, _top, null, StyleSheet.COLORS["SECONDARY_COLOR"], 0, Vector2.Zero, SpriteEffects.None, .1f);
            sb.Draw(_tex, _bottom, null, StyleSheet.COLORS["SECONDARY_COLOR"], 0, Vector2.Zero, SpriteEffects.None, .1f);
            sb.Draw(_tex, _right, null, StyleSheet.COLORS["SECONDARY_COLOR"], 0, Vector2.Zero, SpriteEffects.None, .1f);
            sb.Draw(_tex, _left, null, StyleSheet.COLORS["SECONDARY_COLOR"], 0, Vector2.Zero, SpriteEffects.None, .1f);
        }

    }
}
