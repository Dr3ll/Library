using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Library.Classes
{
    class FormButton : Button
    {
        public string _content;
        private Rectangle _back;
        private Texture2D _tex;
        private Color _backColor;
        private Color _labelColor;
        private string _label;
        private SpriteFont _font;
        private Vector2 _labelAnchor;
        private Vector2 _displacement;

        public FormButton(Vector2 pos, int width, int height, string label, Texture2D tex, SpriteFont font, Color backColor, Color labelColor)
            : base(new Rectangle((int)pos.X, (int)pos.Y, width, height), null, null, backColor)
        {
            _back = new Rectangle((int)pos.X, (int)pos.Y, width, height);
            _tex = tex;
            _backColor = backColor;
            _labelColor = labelColor;
            _label = label;
            _font = font;
            _displacement = Vector2.Zero;

            _labelAnchor = new Vector2(3, 3) + pos;
        }

        public FormButton(Vector2 pos, int width, int height, string label, Texture2D tex, SpriteFont font, Color backColor, Color labelColor, bool bla)
            : base(new Rectangle((int)pos.X, (int)pos.Y, width, height), null, null, backColor)
        {
            _back = new Rectangle((int)pos.X, (int)pos.Y, width, height);
            _tex = tex;
            _backColor = backColor;
            _labelColor = labelColor;
            _label = label;
            _font = font;
            _displacement = new Vector2((width - _font.MeasureString(label).X) * .5f, (height - _font.MeasureString(label).Y) * .5f);

            _labelAnchor = pos + _displacement;
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(_tex, _back, StyleSheet.COLORS["FORMULAR_COLOR"]);

            sb.DrawString(_font, _label, _labelAnchor, StyleSheet.COLORS["GLOBAL_FONT_COLOR"]);
        }

    }
}
