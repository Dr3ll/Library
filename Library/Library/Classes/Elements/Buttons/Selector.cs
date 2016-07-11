using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Library.Classes
{
    class Selector : Button
    {
        Color _color;
        string _label;
        Texture2D _toggle;

        public bool Toggled;

        public Selector(int yPos, Texture2D tex, Color color, string label, Texture2D toggle)
            : base(new Rectangle(0, yPos, 63, 70), tex, tex, new Color(255, 255, 255))
        {
            _label = label;

            _toggle = toggle;

            _color = color;
        }

        public override void Draw(SpriteBatch sb)
        {
            base._showColor = _color;

            sb.Draw(_texture, _pos, _showColor);
            sb.Draw(_textureSec, _pos, _showColor);

            if(Toggled)
                sb.Draw(_toggle, base._pos, StyleSheet.COLORS["SECONDARY_COLOR"]);
        }

        public void SetColorDeselect(Color color)
        {
            base._showColor = _color;
        }

        protected override void OnHit()
        {
            base.CallClicked(_label ,EventArgs.Empty);
        }
    }
}
