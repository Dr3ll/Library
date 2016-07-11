using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Library.Classes
{
    class DropButton : Button
    {
        public string _label;

        public DropButton(string label)
            : base(new Rectangle(0,0, 170, 22))
        {
            _label = label;
        }

        public override void SetPos(Vector2 pos)
        {
            _pos = pos;

            _hitbox.X = (int)pos.X;
            _hitbox.Y = (int)pos.Y + 8;
        }

        protected override void OnHit()
        {
            base.CallClicked(_label, EventArgs.Empty);
            
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            sb.DrawString(DetailView._dropFont, _label, base._pos + new Vector2(10, 8), StyleSheet.COLORS["GLOBAL_FONT_COLOR"]);
        }
    }
}
