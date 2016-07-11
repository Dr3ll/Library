using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Library.Classes
{
    class Scrollbox
    {
        public Rectangle _bounds;

        private Vector2 _scrolling;
        private List<ScrollboxItem> _items;
        private MouseState _oldMouse;
        private bool _scrollStarted;
        private bool _tapBlock;


        public Scrollbox(Vector2 pos, int width, int height)
        {
            _bounds = new Rectangle((int)pos.X, (int)pos.Y, width, height);
            _scrolling = new Vector2(0);

            _items = new List<ScrollboxItem>();

            _scrollStarted = false;
            _tapBlock = true;
        }

        public void Draw(SpriteBatch sb)
        {
            Vector2 tempPos = _scrolling +new Vector2(_bounds.X, _bounds.Y);

            foreach (ScrollboxItem i in _items)
            {
                i.Draw(sb, tempPos);

                tempPos.Y += i.Height();
            }

        }

        public void Update()
        {
            MouseState mouse = Mouse.GetState();

            if (_tapBlock)
            {
                if(mouse.LeftButton == ButtonState.Released)
                {
                    _tapBlock = false;
                }
                else
                    return;
            }

            if (_bounds.Contains(mouse.X, mouse.Y))
            {
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    _scrollStarted = true;

                    Vector2 move = new Vector2(_oldMouse.X, _oldMouse.Y) - new Vector2(mouse.X, mouse.Y);

                    _scrolling.Y -= move.Y;
                }
            }

            if (_scrollStarted && Mouse.GetState().LeftButton == ButtonState.Released)
                _scrollStarted = false;

            if (_scrolling.Y > 0)
                _scrolling = Vector2.Zero;

            int end = -_bounds.Height;
            foreach (ScrollboxItem i in _items)
            {
                end += i.Height();
            }

            end *= -1;

            if (_scrolling.Y < end)
                _scrolling.Y = end;

            _oldMouse = mouse;
        }

        public void Add(ScrollboxItem item)
        {
            _items.Add(item);
        }
    }
}
