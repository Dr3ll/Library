using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Library.Classes
{
    class BlindButton : Button
    {
        public bool Toggled;
        private Color _color;

        string _label;

        public BlindButton(int x, int y, int w, int h, string label, Texture2D tex, Color color)
            : base(new Rectangle(x,y,w,h), tex, tex, new Color(255, 255, 255))
        {
            _label = label;
            _color = color;
        }

        public void Draw(SpriteBatch sb, Color color)
        {
            _showColor = color;
            //_showColor = add(color, _color);

            sb.Draw(_texture, _pos, Color.White);
            sb.Draw(_textureSec, _pos, Color.White);
        }

        protected override void OnHit()
        {
            base.CallClicked(_label, EventArgs.Empty);
        }

        private Color add(Color rightHand, Color leftHand)
        {
            return new Color(rightHand.R + leftHand.R, rightHand.G + leftHand.G, rightHand.B + leftHand.B); ;
        }
    }
}
