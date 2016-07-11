using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Library.Classes
{
    class ReviewStats
    {
        private static Vector2 _anchor;
        private static Vector2 _barAnchor;
        private static Vector2 _totalsAnchor;
        public static Texture2D _starFull;
        public static Texture2D _starEmpty;
        public static Texture2D _starEmptySec;
        private static Texture2D _barFilling;
        public static Texture2D _wrapButtonWrapped;
        public static Texture2D _wrapButtonRevealed;
        private Entry _entry;
        public static SpriteFont _statsFont;
        public static SpriteFont _titleFont;
        public static SpriteFont _textFont;
        private Rectangle _upperMask;
        private Rectangle _lowerMask;

        public ReviewStats(Entry entry)
        {
            _entry = entry;

            _upperMask = new Rectangle(401, 251, 800, 100);
            _lowerMask = new Rectangle(401, 728, 800, 26);
        }

        public static void LoadContent(ContentManager cm)
        {
            _starFull = cm.Load<Texture2D>("Layout/starFull");
            _starEmpty = cm.Load<Texture2D>("Layout/starEmpty");
            _starEmptySec = cm.Load<Texture2D>("Layout/starEmpty_sec");
            _barFilling = cm.Load<Texture2D>("Layout/page");
            _statsFont = cm.Load<SpriteFont>("reviewStatFont");
            _titleFont = cm.Load<SpriteFont>("reviewTitleFont");
            _textFont = cm.Load<SpriteFont>("reviewTextFont");
            _wrapButtonWrapped = cm.Load<Texture2D>("Layout/wrappButtonWrap");
            _wrapButtonRevealed = cm.Load<Texture2D>("Layout/wrappButtonReveal");


            _anchor = new Vector2(402, 251);
            _barAnchor = new Vector2(15, 0);
            _totalsAnchor = new Vector2(15, 40);
        }

        public void Draw(SpriteBatch sb)
        {
            DrawBars(sb);

            DrawTotals(sb);

        }

        private void DrawBars(SpriteBatch sb)
        {
            sb.Draw(_barFilling, _upperMask, StyleSheet.COLORS["PRIMARY_COLOR"]);

            sb.Draw(_barFilling, _lowerMask, StyleSheet.COLORS["PRIMARY_COLOR"]);

            sb.Draw(_starFull, _barAnchor + _anchor, StyleSheet.COLORS["SECONDARY_COLOR"]);

            int totalBarLength = 680;
            int postNumberSpacer = 2;
            int numberSpacer = 30;

            Vector2 numberA = _anchor + _barAnchor;
            numberA.X += 20;
            Rectangle bar = new Rectangle(0, (int)numberA.Y - 1, 0, 30);


            for(int i=0; i<5; ++i)
            {
                int stars = 5 - i;
                numberA.X += numberSpacer - postNumberSpacer - _statsFont.MeasureString(""+ stars +"").X;

                sb.DrawString(_statsFont, stars.ToString(), numberA, StyleSheet.COLORS["GLOBAL_FONT_COLOR"]);
                numberA.X += _statsFont.MeasureString(""+ stars +"").X + postNumberSpacer;

                bar.X = (int)numberA.X;

                float barWidthCalc = (float)(totalBarLength - 5 * numberSpacer - 20) * ((float)_entry._revDistribution[i] / (float)_entry._nreviews);

                numberA.X += barWidthCalc;

                bar.Width = (int)barWidthCalc;

                sb.Draw(_barFilling, bar, StyleSheet.COLORS["SECONDARY_COLOR"]);
            }

        }

        private void DrawTotals(SpriteBatch sb)
        {
            Vector2 drawPos = _totalsAnchor + _anchor;

            // Draw stars for average rating
            int full = _entry._averageRating;
            int empty = 5 - full;

            for (int i = 0; i < full; ++i)
            {
                sb.Draw(_starFull, new Vector2(drawPos.X + i * _starFull.Width, drawPos.Y), StyleSheet.COLORS["SECONDARY_COLOR"]);
            }
            for (int i = 0; i < empty; ++i)
            {
                sb.Draw(_starEmpty, new Vector2(drawPos.X + full * _starFull.Width + i * _starFull.Width, drawPos.Y), StyleSheet.COLORS["SECONDARY_COLOR"]);
            }

            int spacer = 35;
            int starsWidth = 5 * _starFull.Width;

            sb.DrawString(_statsFont, "Durchschnittliche Bewertung", new Vector2(drawPos.X + starsWidth + spacer, drawPos.Y), StyleSheet.COLORS["GLOBAL_FONT_COLOR"]);

            drawPos.Y += 30;

            sb.DrawString(_statsFont, _entry._nreviews.ToString(), new Vector2(drawPos.X + starsWidth - _statsFont.MeasureString(_entry._nreviews.ToString()).X, drawPos.Y), StyleSheet.COLORS["GLOBAL_FONT_COLOR"]);

            sb.DrawString(_statsFont, "Rezensionen", new Vector2(drawPos.X + starsWidth + spacer, drawPos.Y), StyleSheet.COLORS["GLOBAL_FONT_COLOR"]);
        }

    }
}
