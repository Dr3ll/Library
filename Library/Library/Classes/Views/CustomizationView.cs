using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Library.Classes
{
    class CustomizationView
    {
        static Texture2D _back;
        static public bool _show;

        static Vector2 _anchorHeadline = new Vector2(0,210);

        static Vector2 _anchorLabelGlobal = new Vector2(370, 280);
        static Vector2 _anchorLabelBookView = new Vector2(370, 460);
        static Rectangle _anchorSeperator = new Rectangle(370, 460, 560, 1);
        static Texture2D _seperator;

        static Vector2 _anchorBlockGlobalMenus = new Vector2(925, 280);
        static Vector2 _anchorBookViewMenus = new Vector2(925, 480);

        static float _wrap = 50;
        static float _leftpan = 40;

        static SpriteFont _font = StyleSheet.FONTS["GLOBAL_FONT"];

        static Border _border;

        static DropMenuColor _primeColor;
        static DropMenuColor _secColor;
        static DropButton _globSize;

        static DropMenuFloats _bookSize;
        static DropMenuColor _bookColor;
        static DropMenuFont _bookFont;
        static DropMenuColor _bookBack;

        static Rectangle _mouseBounds;
        static bool _maskMouse;

        static EventHandler _choiceCatcher;

        static List<DropMenuBase> drops;

        public static void LoadContent(ContentManager cm)
        {
            _back = cm.Load<Texture2D>("Layout/customBack");

            _seperator = cm.Load<Texture2D>("Layout/page");

            _anchorHeadline.X = 1280f * .5f - _font.MeasureString("Einstellungen").X * StyleSheet.SCALES["SETTINGS_HEAD_SCALE"] * .5f;

            _border = new Border(new Point(340, 190), new Point(940, 190), new Point(940, 763), new Point(340, 763), 5, _seperator, StyleSheet.COLORS["SECONDARY_COLOR"]);

            _bookFont = new DropMenuFont("book", _anchorBookViewMenus + new Vector2(-_leftpan - 240, _wrap * 1 + 10));
            _bookSize = new DropMenuFloats("bookSize", _anchorBookViewMenus + new Vector2(-_leftpan - 240, 10));
            _primeColor = new DropMenuColor("primary", _anchorBlockGlobalMenus + new Vector2(-_leftpan - 240, _wrap - 20));
            _secColor = new DropMenuColor("secondary", _anchorBlockGlobalMenus + new Vector2(-_leftpan - 240, _wrap * 2 + 10));
            _bookColor = new DropMenuColor("bookFont", _anchorBookViewMenus + new Vector2(-_leftpan - 240, _wrap * 2 + 10));
            _bookBack = new DropMenuColor("bookBack", _anchorBookViewMenus + new Vector2(-_leftpan - 240, _wrap * 3 + 40));

            _bookSize.LoadContent(cm);
            _bookFont.LoadContent(cm);
            _primeColor.LoadContent(cm);
            _bookBack.LoadContent(cm);
            _bookColor.LoadContent(cm);
            _secColor.LoadContent(cm);

            _mouseBounds = new Rectangle(340, 190, 750, 570);

            _bookFont.Choice += new EventHandler(OnFontChoice);
            _bookSize.Choice += new EventHandler(OnSizeChoice);
            _primeColor.Choice += new EventHandler(OnColorChoice);
            _bookBack.Choice += new EventHandler(OnColorChoice);
            _bookColor.Choice += new EventHandler(OnColorChoice);
            _secColor.Choice += new EventHandler(OnColorChoice);

            _primeColor.Mask(1);

            _secColor.Top = new Color(255, 255, 255);
            _secColor._indexTop = 1;
            _secColor.Mask(0);

            _bookColor.Top = new Color(0, 0, 0);
            _bookColor._indexTop = 0;
            _bookColor.Mask(1);

            _bookBack.Top = new Color(255, 255, 255);
            _bookBack._indexTop = 1;
            _bookBack.Mask(0);

            drops = new List<DropMenuBase>();
            drops.Add(_bookFont);
            drops.Add(_bookSize);
            drops.Add(_primeColor);
            drops.Add(_bookBack);
            drops.Add(_bookColor);
            drops.Add(_secColor);


            foreach (DropMenuBase d in drops)
            {
                foreach (DropMenuBase m in drops)
                {
                    if(d != m)
                        d.SubscribeDrop(m);

                }
            }

        }

        private static void OnColorChoice(object sender, EventArgs args)
        {
            string label = ((DropMenuColor)sender)._label;

            switch (label)
            {
                case "primary":
                    StyleSheet.COLORS.Set("PRIMARY_COLOR", ((DropMenuColor)sender).Top);
                    //StyleSheet.COLORS.Set("FORMULAR_COLOR", CalcFormColor(((DropMenuColor)sender).Top));
                    _secColor.Mask(((DropMenuColor)sender)._indexTop);
                    break;
                case "secondary":
                    StyleSheet.COLORS.Set("SECONDARY_COLOR", ((DropMenuColor)sender).Top);
                    StyleSheet.COLORS.Set("GLOBAL_FONT_COLOR", LightenUp(((DropMenuColor)sender).Top));
                    //StyleSheet.COLORS.Set("FORMULAR_COLOR", CalcFormColor(((DropMenuColor)sender).Top));
                    _primeColor.Mask(((DropMenuColor)sender)._indexTop);
                    break;
                case "bookBack":
                    StyleSheet.COLORS.Set("BOOK_BACK_COLOR", ((DropMenuColor)sender).Top);
                    _bookColor.Mask(((DropMenuColor)sender)._indexTop);
                    break;
                case "bookFont":
                    StyleSheet.COLORS.Set("BOOK_FONT_COLOR", ((DropMenuColor)sender).Top);
                    _bookBack.Mask(((DropMenuColor)sender)._indexTop);
                    break;
            }

            foreach (DropMenuBase d in drops)
            {
                d.Unmask();
            }

            Game1._readState.Refresh();
        }

        private static Color CalcFormColor(Color color)
        {
            Color a = StyleSheet.COLORS["PRIMARY_COLOR"];
            Color b = StyleSheet.COLORS["SECONDARY_COLOR"];

            float scaling = 0.20161293225f;

            Vector3 preColor = new Vector3(Math.Abs((a.R - b.R) * scaling + Math.Min(a.R, b.R)),
                                           Math.Abs((a.G - b.G) * scaling + Math.Min(a.G, b.G)),
                                           Math.Abs((a.B - b.B) * scaling + Math.Min(a.B, b.B)));

            Color ret = new Color(preColor.X <= 255 ? preColor.X : 255,
                                  preColor.Y <= 255 ? preColor.Y : 255,
                                  preColor.Z <= 255 ? preColor.Z : 255);

            return ret;
        } 

        private static Color LightenUp(Color color)
        {
            Color ret = new Color(color.R <= 180 ? color.R + 35 : 255,
                                  color.G <= 180 ? color.G + 35 : 255,
                                  color.B <= 180 ? color.B + 35 : 255);

            return ret;
        }

        private static void OnFontChoice(object sender, EventArgs args)
        {
            SpriteFont font = ((DropMenuFont)sender).Top;
            
            StyleSheet.FONTS.Set("BOOK_FONT", font);

            foreach (DropMenuBase d in drops)
            {
                d.Unmask();
            }
        }

        private static void OnSizeChoice(object sender, EventArgs args)
        {
            float value = ((DropMenuFloats)sender).Top;
            string label = ((DropMenuFloats)sender)._label;

            StyleSheet.SCALES.Set("BOOK_FONT_SCALE", value);

            float f = StyleSheet.SCALES["BOOK_FONT_SCALE"];

            foreach (DropMenuBase d in drops)
            {
                d.Unmask();
            }
        }

        public static void ShowCustom()
        {
            _show = true;
            _maskMouse = true;

        }

        public static void HideDetails()
        {
            LibState._clearMouse = false;
            LibState._inputMask = true;

            _show = false;
        }

        public static void Update()
        {
            if (Mouse.GetState().LeftButton == ButtonState.Released)
                _maskMouse = false;


            if (_show)
            {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed && !_maskMouse)
                {
                    Point check = new Point(Mouse.GetState().X, Mouse.GetState().Y);

                    if (!_mouseBounds.Contains(check))
                    {
                        HideDetails();
                    }
                }

                
                _bookFont.Update();
                _bookSize.Update();
                _primeColor.Update();
                _secColor.Update();
                _bookColor.Update();
                _bookBack.Update();
            
            }




        }

        public static void Draw(SpriteBatch sb)
        {
            if (_show)
            {
                sb.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

                Vector2 add = Vector2.Zero;
                add.X += _leftpan;

                sb.Draw(_back, Vector2.Zero, StyleSheet.COLORS["PRIMARY_COLOR"]);


                sb.DrawString(_font, "Einstellungen", _anchorHeadline, StyleSheet.COLORS["GLOBAL_FONT_COLOR"], 0, Vector2.Zero, StyleSheet.SCALES["SETTINGS_HEAD_SCALE"], SpriteEffects.None, .1f);


                sb.DrawString(_font, "Allgemein", _anchorLabelGlobal, StyleSheet.COLORS["GLOBAL_FONT_COLOR"], 0, Vector2.Zero, StyleSheet.SCALES["SETTINGS_CAT_SCALE"], SpriteEffects.None, .1f);
                add.Y += 40;
                sb.DrawString(_font, "Primärfarbe", _anchorLabelGlobal + add, StyleSheet.COLORS["GLOBAL_FONT_COLOR"], 0, Vector2.Zero, StyleSheet.SCALES["SETTINGS_LABEL_SCALE"], SpriteEffects.None, .1f);
                add.Y += _wrap + 30;
                sb.DrawString(_font, "Sekundärfarbe", _anchorLabelGlobal + add, StyleSheet.COLORS["GLOBAL_FONT_COLOR"], 0, Vector2.Zero, StyleSheet.SCALES["SETTINGS_LABEL_SCALE"], SpriteEffects.None, .1f);


                add = Vector2.Zero;
                add.X += _leftpan;
                sb.DrawString(_font, "Leseansicht", _anchorLabelBookView, StyleSheet.COLORS["GLOBAL_FONT_COLOR"], 0, Vector2.Zero, StyleSheet.SCALES["SETTINGS_CAT_SCALE"], SpriteEffects.None, .1f);
                add.Y += 40;
                sb.DrawString(_font, "Schriftgröße", _anchorLabelBookView + add, StyleSheet.COLORS["GLOBAL_FONT_COLOR"], 0, Vector2.Zero, StyleSheet.SCALES["SETTINGS_LABEL_SCALE"], SpriteEffects.None, .1f);
                add.Y += _wrap;
                sb.DrawString(_font, "Schriftart", _anchorLabelBookView + add, StyleSheet.COLORS["GLOBAL_FONT_COLOR"], 0, Vector2.Zero, StyleSheet.SCALES["SETTINGS_LABEL_SCALE"], SpriteEffects.None, .1f);
                add.Y += _wrap;
                sb.DrawString(_font, "Schriftfarbe", _anchorLabelBookView + add, StyleSheet.COLORS["GLOBAL_FONT_COLOR"], 0, Vector2.Zero, StyleSheet.SCALES["SETTINGS_LABEL_SCALE"], SpriteEffects.None, .1f);
                add.Y += _wrap + 30;
                sb.DrawString(_font, "Untergrundfarbe", _anchorLabelBookView + add, StyleSheet.COLORS["GLOBAL_FONT_COLOR"], 0, Vector2.Zero, StyleSheet.SCALES["SETTINGS_LABEL_SCALE"], SpriteEffects.None, .1f);

                sb.Draw(_seperator, _anchorSeperator, StyleSheet.COLORS["GLOBAL_FONT_COLOR"]);

                _border.Draw(sb);


                _primeColor.Draw(sb);
                _secColor.Draw(sb);
                _bookColor.Draw(sb);
                _bookBack.Draw(sb);

                sb.End();
                sb.Begin();

                _bookSize.Draw(sb);
                _bookFont.Draw(sb);


                _primeColor.DrawDrop(sb);
                _secColor.DrawDrop(sb);
                _bookColor.DrawDrop(sb);
                _bookBack.DrawDrop(sb);

                sb.End();
            }

        }
    }
}
