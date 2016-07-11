using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Library.Classes.Buttons
{
    class TabButton : Button
    {
        private string _label;
        private SpriteFont _font;
        private Vector2 _labelAnchor;
        public int _index;

        public TabButton(int x, int y, int width, int height, Texture2D tex, Texture2D texSec, Color color, SpriteFont font, string label, int index, EventHandler onHitTarget)
            : base(new Rectangle(x, y, width, height), tex, texSec, color)
        {
            _index = index;
            _label = label;
            _font = font;
            _labelAnchor = new Vector2((width - _font.MeasureString(label).X) * .5f + x, 6 + y);

            base.Clicked += new EventHandler(onHitTarget);
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);

            sb.DrawString(_font, _label, _labelAnchor, StyleSheet.COLORS["GLOBAL_FONT_COLOR"]);
        }
    }
}
