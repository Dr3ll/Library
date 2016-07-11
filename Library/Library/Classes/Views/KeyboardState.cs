using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Library.Classes
{
    class MyKeyboardState
    {
        List<KeyboardButton> _keys;
        Texture2D _board;
        Texture2D _boardCaps;
        StringBuilder _display;
        bool _caps;
        SpriteFont _font;

        int _chars = 0;

        public event EventHandler Return;

        EventHandler _catcher;

        public MyKeyboardState(Texture2D board, Texture2D boardCaps, SpriteFont font)
        {
            _display = new StringBuilder(20);
            _font = font;
            _chars = 0;
            _board = board;
            _boardCaps = boardCaps;

            _keys = new List<KeyboardButton>();

            _catcher = new EventHandler(OnClicked);

            _keys.Add(new KeyboardButton("a", "A", new Rectangle(66, 483, 107, 64)));
            _keys.Add(new KeyboardButton("b", "B", new Rectangle(664, 567, 107, 64)));
            _keys.Add(new KeyboardButton("c", "C", new Rectangle(426, 567, 107, 64)));
            _keys.Add(new KeyboardButton("d", "D", new Rectangle(306, 483, 107, 64)));
            _keys.Add(new KeyboardButton("e", "E", new Rectangle(245, 398, 107, 64)));
            _keys.Add(new KeyboardButton("f", "F", new Rectangle(426, 483, 107, 64)));
            _keys.Add(new KeyboardButton("g", "G", new Rectangle(544, 483, 107, 64)));
            _keys.Add(new KeyboardButton("h", "H", new Rectangle(664, 483, 107, 64)));
            _keys.Add(new KeyboardButton("i", "I", new Rectangle(843, 398, 107, 64)));
            _keys.Add(new KeyboardButton("j", "J", new Rectangle(782, 483, 107, 64)));
            _keys.Add(new KeyboardButton("k", "K", new Rectangle(900, 483, 107, 64)));
            _keys.Add(new KeyboardButton("l", "L", new Rectangle(1025, 483, 107, 64)));
            _keys.Add(new KeyboardButton("m", "M", new Rectangle(900, 567, 107, 64)));
            _keys.Add(new KeyboardButton("n", "N", new Rectangle(782, 567, 107, 64)));
            _keys.Add(new KeyboardButton("o", "O", new Rectangle(964, 398, 107, 64)));
            _keys.Add(new KeyboardButton("p", "P", new Rectangle(1082, 398, 107, 64)));
            _keys.Add(new KeyboardButton("q", "Q", new Rectangle(6, 398, 107, 64)));
            _keys.Add(new KeyboardButton("r", "R", new Rectangle(365, 398, 107, 64)));
            _keys.Add(new KeyboardButton("s", "S", new Rectangle(185, 483, 107, 64)));
            _keys.Add(new KeyboardButton("t", "T", new Rectangle(483, 398, 107, 64)));
            _keys.Add(new KeyboardButton("u", "U", new Rectangle(724, 398, 107, 64)));
            _keys.Add(new KeyboardButton("v", "V", new Rectangle(544, 567, 107, 64)));
            _keys.Add(new KeyboardButton("w", "W", new Rectangle(126, 398, 107, 64)));
            _keys.Add(new KeyboardButton("x", "X", new Rectangle(306, 567, 107, 64)));
            _keys.Add(new KeyboardButton("y", "Y", new Rectangle(185, 567, 107, 64)));
            _keys.Add(new KeyboardButton("z", "Z", new Rectangle(603, 398, 107, 64)));

            _keys.Add(new KeyboardButton("DEL", "DEL", new Rectangle(1034, 567, 154, 64)));
            _keys.Add(new KeyboardButton(" ", " ", new Rectangle(305, 650, 585, 64)));
            _keys.Add(new KeyboardButton("CAPS", "CAPS", new Rectangle(6, 567, 154, 64)));
            _keys.Add(new KeyboardButton("ENTER", "ENTER", new Rectangle(1023, 650, 168, 64)));



            foreach (KeyboardButton k in _keys)
            {
                k.Clicked += _catcher;
            }

        }

        public void Update()
        {
            foreach(KeyboardButton k in _keys)
            {
                k.Update();
            }
        }

        public void Draw(SpriteBatch sb)
        {


            if(!_caps)
                sb.Draw(_board, new Vector2(0, 0), new Color(255, 255, 255));
            else
                sb.Draw(_boardCaps, new Vector2(0, 0), new Color(255, 255, 255));


            sb.DrawString(_font, "Label der neuen Gruppe:", new Vector2(645 - _font.MeasureString("Label der neuen Gruppe:").X * .5f, 250), StyleSheet.COLORS["GLOBAL_FONT_COLOR"]);

            sb.DrawString(_font, _display, new Vector2(640 - _font.MeasureString(_display).X * .5f, 295), StyleSheet.COLORS["GLOBAL_FONT_COLOR"]);

            if(Game1._gameTime.TotalGameTime.Seconds % 2 == 0)
                sb.DrawString(_font, "|", new Vector2(640 + _font.MeasureString(_display).X * .5f, 294), StyleSheet.COLORS["GLOBAL_FONT_COLOR"]);

        }

        private void OnClicked(object sender, EventArgs e)
        {
            KeyboardButton key = (KeyboardButton)sender;

            if (key._char.Equals("CAPS"))
            {
                _caps = !_caps;
                return;
            }

            if (key._char.Equals("ENTER"))
            {
                if (Return != null)
                    Return(_display.ToString().Trim(), EventArgs.Empty);
                return;
            }

            if (key._char.Equals("DEL"))
            {
                _display.Remove(_display.Length - 1, 1);
                _chars--;
                return;
            }

            if (_chars >= 25)
                return;

            if(!_caps)
                _display.Append(key._char);
            else
                _display.Append(key._charCaps);

            _chars++;
        }
    }
}
