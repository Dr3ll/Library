using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Library.Classes.Buttons
{
    class RatingButton : Button
    {
        public int _index;
        private Texture2D _aTex;
        private Texture2D _inTex;
        private Color _color;
        private Color _inColor;
        private bool _active = false;

        public RatingButton(Vector2 pos, int width, int height, Texture2D activeTex, Texture2D inTex, Color color, EventHandler hitTarget, int index)
            : base(new Rectangle((int)pos.X, (int)pos.Y, width, height), inTex, inTex, color, false)
        {
            //_inColor = new Color(color.R + 50, color.G + 50, color.B + 50);
            _color = color;

            _index = index;

            _aTex = activeTex;
            _inTex = inTex;

            base._showColor = _color;

            base.Clicked += hitTarget;
        }

        public void Toggle(bool newState)
        {
            _active = newState;

            _showColor = _color;

            //if (_active)
            //{
            //    _showTex = _aTex;
            //}
            //else 
            //{
            //    _showTex = _inTex;
            //}
        }

        public override void Draw(SpriteBatch sb)
        {
            if (_active)
            {
                sb.Draw(_aTex, _pos, StyleSheet.COLORS["SECONDARY_COLOR"]);
            }
            else
            {
                sb.Draw(_aTex, _pos, StyleSheet.COLORS["PRIMARY_COLOR"]);
                sb.Draw(_inTex, _pos, StyleSheet.COLORS["SECONDARY_COLOR"]);
            }
            
            
        }
    }
}
