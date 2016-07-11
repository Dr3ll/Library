using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Library.Classes
{
    public class Entry : IComparable<Entry>
    {
        public Texture2D _tex;
        Texture2D _dot;

        public Vector2 _pos;
        Rectangle _hitbox;
        Color dcolor;

        public string _title { private set; get; }
        public string _author { private set; get; }
        public string _text { private set; get; }
        public string _content { private set; get; }
        public string _genre { private set; get; }
        public string _group;
        public int _nreviews;
        public int _averageRating;
        public int[] _revDistribution;
        public List<Review> _reviews;

        public bool _fav;

        public int _index { private set; get; }

        public event EventHandler Hovered;

        const int width = 187;
        const int height = 300;

        private Vector2 _topLeftPosition;
        private readonly static Vector2 _relBoxPosition = new Vector2(0, -10);
        private readonly static Vector2 _relTextPosition = new Vector2(-10, 255);
        private readonly static float _lineSpacing = -3;
        private static float _lineLength = width;    // in pixels
        private readonly static char commandIndicator = '/';
        private readonly static char lineWrap = 'n';
        private SpriteFont _font = LibState._font;

        public int CompareTo(Entry other)
        {
            if (LibState._sortMode == SortMode.Alphabetic)
                return this._title.CompareTo(other._title);
            if (LibState._sortMode == SortMode.Autor)
                return this._author.CompareTo(other._author);

            return 0;
        }

        public List<Mark> _reds;
        public List<Mark> _yellows;
        public List<Mark> _blues;

        public Entry(Texture2D tex, 
                     Texture2D dot, 
                     Vector2 pos, 
                     string title, 
                     string author, 
                     string text, 
                     string content, 
                     string genre,
                     int nreviews,
                     int averageRating,
                     int[] distribution,
                     List<Review> reviews)
        {
            _tex = tex;
            _dot = dot;
            _pos = pos;
            _title = title;
            _author = author;
            _text = text;
            _content = content;
            _genre = genre;
            _group = "default";
            _nreviews = nreviews;
            _averageRating = averageRating;
            _revDistribution = distribution;
            _reviews = reviews;

            _reds = new List<Mark>();
            _yellows = new List<Mark>();
            _blues = new List<Mark>();

            dcolor = new Color(255, 255, 255);

            _topLeftPosition = _pos;

            _hitbox = new Rectangle((int)(pos.X - width / 2), (int)(pos.Y - height / 2), width, height);

            this.Hovered += new EventHandler(OnHovered);
        }

        public void FavIt()
        {
            _fav = true;
        }

        public void UnFavIt()
        {
            _fav = false;
        }

        public bool CheckHit(Vector2 pos)
        {
            if (pos.X > 1280 || pos.X < 0
                || pos.Y > 800 || pos.Y < 0)
            {
                dcolor = new Color(255, 255, 255);
                return false;
            }

            if(!_hitbox.Contains(new Point((int)pos.X, (int)pos.Y)))
            {
                Mask();
                return false;
            }

            if (Hovered != null)
                Hovered(null, EventArgs.Empty);

            return true;
        }

        public void SetPos(Vector2 pos)
        {
            _hitbox = new Rectangle((int)(pos.X), (int)(pos.Y), width, height);

            _pos = pos;
        }

        private void OnHovered(object sender, EventArgs e)
        {
            dcolor = Color.Red;
        }

        public void Mask()
        {
            dcolor = new Color(255, 255, 255);
        }

        public void CallFunction()
        {
            DetailView.ShowDetails(this);
        }

        public void Draw(SpriteBatch sb)
        {
            //Vector2 drawPos = new Vector2(_pos.X - .5f *_tex.Width, _pos.Y - .5f * _tex.Height);

            sb.Draw(_tex, _pos, Color.White);

            DrawTitle(sb);


        }

        private void DrawTitle(SpriteBatch sb)
        {
            string text = _title;
            Vector2 pos = Vector2.Zero;
            List<string> words = new List<string>();
            List<Vector2> wPositions = new List<Vector2>();
            Color drawColor = new Color(255, 255, 255);

            int wraps = 0;
            while (text.Length > 0)
            {
                string next = NextWord(ref text, ref pos);

                // If the next word would exeed line length wrap it
                if (pos.X + _font.MeasureString(next).X > _lineLength)
                {
                    ++wraps;
                    if (wraps == 2)
                    {

                        words.Add("[...]");
                        wPositions.Add(_pos + pos + _topLeftPosition + _relTextPosition);
                        break;
                    }
                    Wrap(ref pos);
                }
                if (pos.X + _font.MeasureString(next).X > _lineLength)
                    throw new Exception("Word is too long for a line to contain it: " + next + " Line length is set to " + _lineLength + ".");

                next = next.Replace('~', ' ');
                words.Add(next);
                wPositions.Add(_pos + pos + _topLeftPosition + _relTextPosition);
                //sb.DrawString(_font, next, _pos + pos + _topLeftPosition + _relTextPosition, drawColor);

                pos += new Vector2(_font.MeasureString(next).X, 0);
            }

            // Alling lines to center
            int line1 = 0;
            int line2 = 0;
            int line1Y = (int)(wPositions[0].Y);
            bool line2spaceBounce = true;

            for (int i = 0; i < words.Count; i++)
            {
                if ((int)(wPositions[i].Y) == line1Y)
                {
                    line1 += (int)(_font.MeasureString(words[i]).X);
                }
                else
                {
                    line2 += (int)(_font.MeasureString(words[i]).X);
                    if (line2spaceBounce && words[i][0] == ' ')
                    {
                        line2 -= (int)(_font.MeasureString(" ").X);
                        line2spaceBounce = false;
                    }
                }
            }
            int move1 = (int)((width - line1) * .5f) - 1;
            int move2 = (int)((width - line2) * .5f) - 1;

            for (int i = 0; i < words.Count; i++)
            {
                if (wPositions[i].Y == line1Y)
                {
                    sb.DrawString(_font, words[i], wPositions[i] + new Vector2(move1 , 0f), drawColor);
                }
                else
                {
                    sb.DrawString(_font, words[i], wPositions[i] + new Vector2(move2, 0f), drawColor);
                }
            }
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
                        if (text[i].Equals(' ') && position.X + _font.MeasureString(text[i].ToString()).X > _lineLength)
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

        private void Wrap(ref Vector2 position)
        {
            position += new Vector2(0, _lineSpacing + _font.MeasureString("Ig").Y);
            position.X = 0;
        }
    }
}
