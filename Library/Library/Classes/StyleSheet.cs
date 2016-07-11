using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Library.Classes
{
    class StyleSheet
    {
        public static SpriteFont Arial = null;
        public static SpriteFont Cambria = null;
        public static SpriteFont ComicSans = null;
        public static SpriteFont LucidaConsole = null;
        public static SpriteFont NewsGothic = null;
        public static SpriteFont Rockwell = null;
        public static SpriteFont TimesNewRoman = null;

        public static Dictionary<float> SCALES;
        public static Dictionary<Color> COLORS;
        public static Dictionary<SpriteFont> FONTS;

        public static void LoadStyleSheet(ContentManager cm)
        {
            try
            {
                Arial = cm.Load<SpriteFont>("Fonts/Arial");
            }
            catch (Exception e) { }
            try
            {
                Cambria = cm.Load<SpriteFont>("Fonts/Cambria");
            }
            catch (Exception e) { }
            try
            {
                ComicSans = cm.Load<SpriteFont>("Fonts/ComicSans");
            }
            catch (Exception e) { }
            try
            {
                LucidaConsole = cm.Load<SpriteFont>("Fonts/LucidaConsole");
            }
            catch (Exception e) { }
            try
            {
                NewsGothic = cm.Load<SpriteFont>("Fonts/NewsGothic");
            }
            catch (Exception e) { }
            try
            {
                Rockwell = cm.Load<SpriteFont>("Fonts/Rockwell");
            }
            catch (Exception e) { }
            try
            {
                TimesNewRoman = cm.Load<SpriteFont>("Fonts/TimesNewRoman");
            }
            catch (Exception e) { }


            SCALES = new Dictionary<float>();
            COLORS = new Dictionary<Color>();
            FONTS = new Dictionary<SpriteFont>();

            SCALES.Register("SYNOPSIS_SCALE", .62f);
            SCALES.Register("TITLE_SCALE", .62f);
            SCALES.Register("AUTHOR_SCALE", .62f);
            SCALES.Register("GROUP_SCALE", .62f);
            SCALES.Register("STAR_SCALE", .62f);
            SCALES.Register("TAB_SCALE", .62f);
            SCALES.Register("TITLES_SCALE", .62f);
            SCALES.Register("TAB_FONT_SCALE", .62f);
            SCALES.Register("BOOK_FONT_SCALE", .62f);
            SCALES.Register("SETTINGS_HEAD_SCALE", 1.6f);
            SCALES.Register("SETTINGS_CAT_SCALE", .76f);
            SCALES.Register("SETTINGS_LABEL_SCALE", .62f);
            SCALES.Register("CSTM_FONT_SCALE", .76f);

            FONTS.Register("GLOBAL_FONT", Arial);
            FONTS.Register("BOOK_FONT", Arial);
            
            COLORS.Register("BOOK_FONT_COLOR", new Color(0, 0, 0));
            COLORS.Register("BOOK_BACK_COLOR", new Color(255, 255, 255));
            COLORS.Register("PRIMARY_COLOR", new Color(7, 7, 7));
            COLORS.Register("SECONDARY_COLOR", new Color(255, 255, 255));
            COLORS.Register("GLOBAL_FONT_COLOR", new Color(255, 255, 255));
            COLORS.Register("FORMULAR_COLOR", new Color(.2f, .2f, .2f, .4f));
        }
    }
}
