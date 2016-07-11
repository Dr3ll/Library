using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Library.Classes
{
    class TextBox
    {
        private readonly float _lineSpacing = -3;
        private readonly char commandIndicator = '/';
        private readonly char lineWrap = 'n';
        private SpriteFont _font;
        private StringBuilder _build;

        private float _scrolling;
        public float Scrolling
        {
            get { return _scrolling; }
            set { _scrolling = value; }
        }
        private string _text;
        public string Text
        {
            get { return _text; }
            set { _text = value; BuildBox(); }
        }
        public Vector2 Position { get; set; }
        public float Width { get; set; }    // in pixels
        public int Wraps { get; set; }
        public float Height
        {
            get { return _font.MeasureString(_build).Y - 10; }
        }
        private int _maxWraps;
        public int MaxWraps
        {
            get { return _maxWraps; }
            set { _maxWraps = value; }
        }
        private float _scale;

        private Color _drawColor;
        public Color FontColor
        {
            get { return _drawColor; }
            set { _drawColor = value; }
        }

        public TextBox(SpriteFont font, float scale = 1.0f)
        {
            _font = font;
            _drawColor = StyleSheet.COLORS["GLOBAL_FONT_COLOR"];
            MaxWraps = 200;

            _scale = scale;

            Scrolling = 0;
        }

        private void SearchAndDestroy(string text)
        {
            while (true)
            {
                try
                {
                    _font.MeasureString(text);

                    break;
                }
                catch (ArgumentException e)
                {
                    string fail = e.Message.ElementAt(15).ToString();

                    text = text.Replace(fail, " ");
                }
            }
            Text = text;

        }

        public void BuildBox()
        {
            string text = Text;
            string line = "";
            _build = new StringBuilder();
            Vector2 pos = Vector2.Zero;

            try
            {
                _font.MeasureString(text);
            }
            catch (Exception e)
            {
                SearchAndDestroy(text);
            }

            text = Text;

            int wraps = 0;
            while (text.Length > 0)
            {
                string next = NextWord(ref text, ref pos);

                // If the next word would exeed line length wrap it
                if (pos.X + (_font.MeasureString(next).X * _scale) > Width)
                {
                    ++wraps;
                    if (wraps == MaxWraps)
                    {
                        _build.Append("[...]");
                        return;
                    }
                    Wrap(ref pos);
                }
                if (pos.X + (_font.MeasureString(next).X * _scale) > Width)
                    throw new Exception("Word is too long for a line to contain it: " + next + " Line length is set to " + Width + ".");

                next = next.Replace('~', ' ');
                _build.Append(next);

                pos += new Vector2(_font.MeasureString(next).X * _scale, 0);
            }
        }

        public void BuildBox(int upperBound, int lowerBound)
        {
            string text = Text;
            string bla = "/n";
            text = text.Replace("\n", bla);

            string line = "";
            _build = new StringBuilder();
            Vector2 pos = Vector2.Zero;

            int boundBuffer = (int)(_font.MeasureString("Ig").Y * 4 * _scale);

            int wraps = 0;
            while (text.Length > 0)
            {
                string next = NextWord(ref text, ref pos);

                // If the next word would exeed line length wrap it
                if (pos.X + (_font.MeasureString(next).X * _scale) > Width)
                {
                    ++wraps;
                    if (wraps == MaxWraps)
                    {
                        if (!((pos.Y + Position.Y) < (upperBound - boundBuffer) || (pos.Y + Position.Y) > (lowerBound + boundBuffer)))
                        {
                            _build.Append("[...]");
                        }
                        return;
                    }
                    Wrap(ref pos);
                }
                if (pos.X + (_font.MeasureString(next).X * _scale) > Width)
                    throw new Exception("Word is too long for a line to contain it: " + next + " Line length is set to " + Width + ".");

                next = next.Replace('~', ' ');

                if ((pos.Y + Position.Y) < (upperBound - boundBuffer) || (pos.Y + Position.Y) > (lowerBound + boundBuffer))
                {
                    _build.Append(CheckForWrap(next, ref wraps));
                }
                else
                {
                    _build.Append(next);
                }

                pos += new Vector2(_font.MeasureString(next).X * _scale, 0);
            }
        }

        // Returns as many wrap symbols as the given text contains, returns empty string otherwise if none is found
        private string CheckForWrap(string text, ref int wraps)
        {
            int count = 0;
            for (int i = 0; i < text.Length; ++i)
            {
                if (text[i] == '\n')
                    count++;
            }

            string res = "";

            for (int i = 0; i < count; ++i)
            {
                res += "\n";
                //++wraps;
            }

            return res;
        }

        private string NextWord(ref string text, ref Vector2 position)
        {
            int i = 0;
            string res = "";

            while (i <= text.Length)
            {
                // Check if the next symbol is a space or terminating a sentence
                if (i == text.Length || text[i].Equals(' ') || text[i].Equals('.') || text[i].Equals('!') || text[i].Equals('?'))
                {
                    // If the read-in just started cut the symbol off
                    if (i == 0)
                    {
                        if (text[i].Equals(' ') && position.X + (_font.MeasureString(text[i].ToString()).X * _scale) > Width)
                        {
                            // In the special case that a SPACE would stand at the beginning of a line, dont show it
                        }
                        else
                            res += text[i];

                        text = text.Remove(0, i + 1);
                    }
                    else
                    {
                        text = text.Remove(0, i);
                    }
                    break;
                }

                // Check for command
                if (text[i].Equals(commandIndicator))
                {
                    // If the read in just started execute command, else return the current word
                    if (i != 0)
                    {
                        text = text.Remove(0, i);
                        break;
                    }

                    // Check for lineBreak
                    if (text[i + 1].Equals(lineWrap))
                    {
                        text = text.Remove(0, i + 2);
                        Wrap(ref position);
                        break;
                    }
                }


                res += text[i];
                ++i;
            }

            return res;
        }

        public void Draw(SpriteBatch sb, int upperBound, int lowerBound)
        {
            if (upperBound != 0 || lowerBound != 0)
            {
                BuildBox( upperBound, lowerBound);
            }
            if(_build != null)
                sb.DrawString(_font, _build, Position + new Vector2(0, Scrolling), _drawColor, 0, Vector2.Zero, _scale, SpriteEffects.None, 0);
        }

        public void Draw(SpriteBatch sb)
        {
            if (_build != null)
                sb.DrawString(_font, _build, Position + new Vector2(0, Scrolling), _drawColor, 0, Vector2.Zero, _scale, SpriteEffects.None, 0);
        }

        private void Wrap(ref Vector2 position)
        {
            position += new Vector2(0, _lineSpacing + (_font.MeasureString("Ig").Y * _scale));
            position.X = 0;

            _build.AppendLine();
        }
    }
}
