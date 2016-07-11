using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Library.Classes.Buttons
{
    class WrapButton : Button
    {
        public bool Toggled;
        private Color _color;
        Texture2D _onTex;
        Texture2D _offTex;

        string _label;

        public WrapButton(int x, int y, int w, int h, Texture2D onTex, Texture2D offTex, Color color)
            : base(new Rectangle(x - 10,y - 20,w,h), onTex, onTex, new Color(255, 255, 255))
        {
            _label = "";
            _color = color;
            _showColor = color;
            _onTex = onTex;
            _offTex = offTex;
            _texture = _offTex;

            Toggled = true;

            base.Clicked += new EventHandler(OnHit);
        }

        public void Draw(SpriteBatch sb)
        {
            if(Toggled)
                sb.Draw(_onTex, _pos, StyleSheet.COLORS["SECONDARY_COLOR"]);
            else
                sb.Draw(_offTex, _pos, StyleSheet.COLORS["SECONDARY_COLOR"]);
        }

        public void Update(Texture2D onTex, Texture2D offTex)
        {
            _onTex = onTex;
            _offTex = offTex;

            base.Update();

            if (Toggled)
                base._showTex= _onTex;
            else
                base._showTex = _offTex;
        }

        protected void OnHit(object sender, EventArgs args)
        {
            if (Toggled)
            {
                Toggled = false;
                base._texture = _onTex;
            }
            else
            {
                Toggled = true;
                base._texture = _offTex;
            }
        }
    }
}
