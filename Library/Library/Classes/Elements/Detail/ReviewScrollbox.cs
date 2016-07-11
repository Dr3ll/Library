using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Library.Classes
{
    class ReviewScrollbox
    {
        private static Vector2 _anchor;
        private Entry _entry;

        private Scrollbox _scrollBox;

        public ReviewScrollbox(Entry entry)
        {
            _entry = entry;
            _anchor = new Vector2(415, 360);

            _scrollBox = new Scrollbox(_anchor, 800, 390);

            foreach (Review r in _entry._reviews)
            {
                _scrollBox.Add(r);
            }
        }

        public void Update()
        {
            _scrollBox.Update();

            foreach (Review r in _entry._reviews)
            {
                r.Update();
            }
        }

        public void Draw(SpriteBatch sb)
        {
            _scrollBox.Draw(sb);
        }
    }
}
