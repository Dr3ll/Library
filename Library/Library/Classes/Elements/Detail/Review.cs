using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Library.Classes
{
    public class Review : ScrollboxItem
    {
        private string _title;
        private string _text;
        private int _rating;
        private string _date;
        private string _author;
        private TextBox _content;
        private Buttons.WrapButton _wrapButton;
        private Vector2 _buttonAnchor;

        private const int MAX_ROWS = 4;

        public int Height()
        {
            return (int)_content.Height + 60;
        }

        public Review(string title, string text, int rating, string date, string author)
        {
            _title = title;
            _text = text;
            _rating = rating;
            _date = date;
            _author = author;

            _buttonAnchor = new Vector2(10, 80);
            _wrapButton = new Buttons.WrapButton((int)_buttonAnchor.X, (int)_buttonAnchor.Y, 50, 50, ReviewStats._wrapButtonWrapped, ReviewStats._wrapButtonRevealed, new Color(255, 255, 255));
        }

        public void Update()
        {
            _wrapButton.Update(ReviewStats._wrapButtonWrapped, ReviewStats._wrapButtonRevealed);
        }

        public void Draw(SpriteBatch sb, Vector2 pos)
        {
            _content = new TextBox(ReviewStats._textFont);
            _content.Width = 700;
            if (_wrapButton.Toggled)
                _content.MaxWraps = MAX_ROWS;
            else
                _content.MaxWraps = 99999;
            _content.Text = _text;

            if (!((pos + _buttonAnchor).Y < 360 || (pos + _buttonAnchor).Y > 728))
            {
                _wrapButton.SetPos(pos + _buttonAnchor);
                _wrapButton.Draw(sb);
            }

            // Text
            _content.Position = pos + new Vector2(40, 30);
            _content.Draw(sb, 360, 630);

            if (!(pos.Y + 30 < 360 || pos.Y > 728 - 30))
            {
                // Author
                sb.DrawString(ReviewStats._textFont, "von ", new Vector2(pos.X + 15, pos.Y + 30), StyleSheet.COLORS["GLOBAL_FONT_COLOR"]);
                sb.DrawString(ReviewStats._textFont, _author, new Vector2(pos.X + 15 + ReviewStats._statsFont.MeasureString("von ").X, pos.Y + 30), StyleSheet.COLORS["GLOBAL_FONT_COLOR"]);
            }

            if ((pos.Y + 20) < 340 || pos.Y > 728)
                return;

            // Title, date
            sb.DrawString(ReviewStats._titleFont, _title, new Vector2(pos.X + 5 * ReviewStats._starFull.Width + 27, pos.Y), StyleSheet.COLORS["GLOBAL_FONT_COLOR"]);
            sb.DrawString(ReviewStats._titleFont, ", ", new Vector2(pos.X + 5 * ReviewStats._starFull.Width + 27 + ReviewStats._titleFont.MeasureString(_title).X, pos.Y), StyleSheet.COLORS["GLOBAL_FONT_COLOR"]);
            sb.DrawString(ReviewStats._titleFont, _date, new Vector2(pos.X + 5 * ReviewStats._starFull.Width + 27 + ReviewStats._titleFont.MeasureString(_title).X + ReviewStats._titleFont.MeasureString(", ").X, pos.Y),
                StyleSheet.COLORS["GLOBAL_FONT_COLOR"]);

            int full = _rating;
            int empty = 5 - full;

            // Draw rating
            for (int i = 0; i < full; ++i)
            {
                sb.Draw(ReviewStats._starFull, new Vector2(pos.X + i * ReviewStats._starFull.Width, pos.Y), StyleSheet.COLORS["SECONDARY_COLOR"]);
            }
            for (int i = 0; i < empty; ++i)
            {
                sb.Draw(ReviewStats._starEmpty, new Vector2(pos.X + full * ReviewStats._starFull.Width + i * ReviewStats._starFull.Width, pos.Y), StyleSheet.COLORS["PRIMARY_COLOR"]);
                sb.Draw(ReviewStats._starEmptySec, new Vector2(pos.X + full * ReviewStats._starFull.Width + i * ReviewStats._starFull.Width, pos.Y), StyleSheet.COLORS["SECONDARY_COLOR"]);
            }


            


        }
    }
}
