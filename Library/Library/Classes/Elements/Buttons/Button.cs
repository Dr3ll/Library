using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Library.Classes
{
    class Button
    {
        public event EventHandler Clicked;

        private MouseState _oldMouse;

        protected  Vector2 _pos;
        protected Rectangle _hitbox;
        protected Texture2D _texture;
        protected Texture2D _textureSec;
        protected Color _hoverColor;

        protected Texture2D _showTex;
        protected Color _showColor;
        private bool _hoverActive = false;

        public Button(Rectangle rec, Texture2D tex, Texture2D texSec, Color hColor, bool hover = false)
        {
            _pos = new Vector2(rec.X, rec.Y);
            _hitbox = rec;
            _texture = tex;
            _textureSec = texSec;
            _hoverColor = new Color(255, 255, 255);
            _hoverActive = hover;

            _showTex = _texture;
            _showColor = new Color(255, 255, 255);
        }

        public virtual void SetPos(Vector2 pos)
        {
            _pos = pos;

            _hitbox.X = (int)pos.X;
            _hitbox.Y = (int)pos.Y;
        }

        public Button(Rectangle rec)
            : this(rec, null, null, Color.Red)
        {
        }

        private void CheckHover(int x, int y)
        {
            if (_hitbox.Contains(new Point(x, y)))
            {
                _showColor = _hoverColor;
            }
            else
            {
                _showTex = _texture;
                _showColor = new Color(255, 255, 255);
            }
        }

        private void CheckHit(int x, int y)
        {
            if(_hitbox.Contains(new Point(x, y)))
            {
                OnHit();
            }
        }

        public virtual void Update()
        {
            MouseState _mouse = Mouse.GetState();

            if(_hoverActive)
                CheckHover(_mouse.X, _mouse.Y);

            if (_mouse.LeftButton == ButtonState.Pressed &&
               _oldMouse.LeftButton == ButtonState.Released)
                CheckHit(_mouse.X, _mouse.Y);

            _oldMouse = _mouse;
        }

        protected virtual void OnHit()
        {
            if (Clicked != null)
                Clicked(this, EventArgs.Empty);
        }

        public virtual void Draw(SpriteBatch sb)
        {
            if (_texture != null || _textureSec != null)
            {
                sb.Draw(_texture, _pos, StyleSheet.COLORS["PRIMARY_COLOR"]);
                sb.Draw(_textureSec, _pos, StyleSheet.COLORS["SECONDARY_COLOR"]);
            }
        }

        protected void CallClicked(object obj, EventArgs args)
        {
            if (Clicked != null)
                Clicked(obj, args);
        }
    }
}
