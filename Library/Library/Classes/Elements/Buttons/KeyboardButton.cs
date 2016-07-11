using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Library.Classes
{
    class KeyboardButton : Button
    {
        public string _char;
        public string _charCaps;

        public KeyboardButton(string c, string cc, Rectangle rec)
            : base(rec)
        {
            _char = c;
            _charCaps = cc;
        }

        protected override void OnHit()
        {
            base.CallClicked(this, EventArgs.Empty);
            
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            sb.DrawString(DetailView._dropFont, _char, base._pos + new Vector2(10, 8), base._showColor);
        }
    }
}
