using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Library.Classes.Buttons;

namespace Library.Classes
{
    class Formular
    {
        private Vector2 _anchor;
        private Vector2 _titleAnchor;
        private Vector2 _dateAnchor;
        private Vector2 _nameAnchor;
        private Vector2 _textAnchor;
        private Vector2 _ratingAnchor;
        private Vector2 _sendAnchor;

        private FormButton _titleField;
        private FormButton _textField;
        private FormButton _nameField;
        private FormButton _sendField;

        private SpriteFont _font;

        private RatingButton _one;
        private RatingButton _two;
        private RatingButton _three;
        private RatingButton _four;
        private RatingButton _five;

        private EventHandler _starTarget;

        private int _rating;

        private Color _textColor;

        public Formular(Texture2D buttonTex, Texture2D starFull, Texture2D starEmpty, SpriteFont font)
        {
            _anchor = new Vector2();

            int left = 455;

            _rating = 0;

            _titleAnchor = new Vector2(left, 340);
            _dateAnchor = new Vector2(1150, 393);
            _nameAnchor = new Vector2(left, 380);
            _textAnchor = new Vector2(left, 420);
            _ratingAnchor = new Vector2(680, 270);
            _sendAnchor = new Vector2(710, 695);

            _font = font;

            _textColor = StyleSheet.COLORS["GLOBAL_FONT_COLOR"];

            _titleField = new FormButton(_titleAnchor, 300, 30, "Benutzername", buttonTex, _font, StyleSheet.COLORS["FORMULAR_COLOR"], _textColor);
            _nameField = new FormButton(_nameAnchor, 300, 30, "Überschrift", buttonTex, _font, StyleSheet.COLORS["FORMULAR_COLOR"], _textColor);
            _textField = new FormButton(_textAnchor, 695, 260, "", buttonTex, _font, StyleSheet.COLORS["FORMULAR_COLOR"], _textColor);
            _sendField = new FormButton(_sendAnchor, 140, 45, "Absenden", buttonTex, _font, StyleSheet.COLORS["FORMULAR_COLOR"], new Color(170, 170, 170), true);

            int starwidth = 50;
            int starheight = 50;

            _starTarget = new EventHandler(StarHit);

            _one = new RatingButton(_ratingAnchor, starwidth, starheight, starFull, starEmpty, new Color(180, 180, 180), _starTarget, 0);
            _two = new RatingButton(_ratingAnchor + new Vector2(starwidth, 0), starwidth, starheight, starFull, starEmpty, new Color(150, 150, 150), _starTarget, 1);
            _three = new RatingButton(_ratingAnchor + new Vector2(starwidth * 2, 0), starwidth, starheight, starFull, starEmpty, new Color(150, 150, 150), _starTarget, 2);
            _four = new RatingButton(_ratingAnchor + new Vector2(starwidth * 3, 0), starwidth, starheight, starFull, starEmpty, new Color(150, 150, 150), _starTarget, 3);
            _five = new RatingButton(_ratingAnchor + new Vector2(starwidth * 4, 0), starwidth, starheight, starFull, starEmpty, new Color(150, 150, 150), _starTarget, 4);
        }

        private void StarHit(object sender, EventArgs args)
        {
            switch (((RatingButton)sender)._index)
            {
                case 0:
                    _one.Toggle(true);
                    _two.Toggle(false);
                    _three.Toggle(false);
                    _four.Toggle(false);
                    _five.Toggle(false);
                    _rating = 1;
                    break;
                case 1:
                    _one.Toggle(true);
                    _two.Toggle(true);
                    _three.Toggle(false);
                    _four.Toggle(false);
                    _five.Toggle(false);
                    _rating = 2;
                    break;
                case 2:
                    _one.Toggle(true);
                    _two.Toggle(true);
                    _three.Toggle(true);
                    _four.Toggle(false);
                    _five.Toggle(false);
                    _rating = 3;
                    break;
                case 3:
                    _one.Toggle(true);
                    _two.Toggle(true);
                    _three.Toggle(true);
                    _four.Toggle(true);
                    _five.Toggle(false);
                    _rating = 4;
                    break;
                case 4:
                    _one.Toggle(true);
                    _two.Toggle(true);
                    _three.Toggle(true);
                    _four.Toggle(true);
                    _five.Toggle(true);
                    _rating = 5;
                    break;
                default:
                    _one.Toggle(true);
                    _two.Toggle(true);
                    _three.Toggle(true);
                    _four.Toggle(true);
                    _five.Toggle(true);
                    _rating = 5;
                    break;
            }

        }

        public void Update()
        {
            _one.Update();
            _two.Update();
            _three.Update();
            _four.Update();
            _five.Update();
        }

        public void Draw(SpriteBatch sb)
        {


            _titleField.Draw(sb);
            _nameField.Draw(sb);
            _textField.Draw(sb);
            _sendField.Draw(sb);

            string day = DateTime.Today.Day.ToString();
            int month = DateTime.Today.Month;
            string monthWord = "";

            if (day.Length < 2)
            {
                day = "0" + day;
            }

            #region Month switch
            switch (month)
            {
                case 1:
                    monthWord = "´Januar";
                    break;
                case 2:
                    monthWord = "Februar";
                    break;
                case 3:
                    monthWord = "März";
                    break;
                case 4:
                    monthWord = "April";
                    break;
                case 5:
                    monthWord = "Mai";
                    break;
                case 6:
                    monthWord = "Juni";
                    break;
                case 7:
                    monthWord = "Juli";
                    break;
                case 8:
                    monthWord = "August";
                    break;
                case 9:
                    monthWord = "September";
                    break;
                case 10:
                    monthWord = "Oktober";
                    break;
                case 11:
                    monthWord = "November";
                    break;
                case 12:
                    monthWord = "Dezember";
                    break;
                default:
                    monthWord = "Monat";
                    break;
            }
            #endregion

            string date = day + ". " + monthWord + " " + DateTime.Today.Year.ToString();

            sb.DrawString(_font, date, _dateAnchor - new Vector2(_font.MeasureString(date).X, 0), StyleSheet.COLORS["GLOBAL_FONT_COLOR"]);

            _one.Draw(sb);
            _two.Draw(sb);
            _three.Draw(sb);
            _four.Draw(sb);
            _five.Draw(sb);



        }

    }
}
