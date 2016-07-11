using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Library.Classes
{
    class FavButton : Button
    {
        Texture2D _texEmpty;
        Texture2D _texFull;
        bool _hit;

        public FavButton(Texture2D texEmpty, Texture2D texFull, bool hit)
            : base(new Rectangle(1090, 675, 50, 48), !hit ? texEmpty : texFull, texEmpty, Color.Red)
        {
            _texEmpty = texEmpty;
            _texFull = texFull;
            _hit = hit;
        }

        protected override void OnHit()
        {
            base.OnHit();

            _hit = !_hit;

            base._texture = !_hit ? _texEmpty : _texFull;

        }

        public override void Draw(SpriteBatch sb)
        {
            if (!_hit)
            {
                sb.Draw(_texFull, base._pos, StyleSheet.COLORS["PRIMARY_COLOR"]);
                sb.Draw(_texEmpty, base._pos, StyleSheet.COLORS["SECONDARY_COLOR"]);
            }
            else
            {
                sb.Draw(_texFull, base._pos, StyleSheet.COLORS["SECONDARY_COLOR"]);
            }

        }
    }
}
