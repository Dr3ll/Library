using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Library.Classes
{
    class DetailView
    {
        static Entry _entry;
        static ReviewStats _reviewStats;
        static ReviewScrollbox _reviewScrollBox;
        static Texture2D _back;
        static Vector2 _picAnchor;
        public static bool Active;
        static Rectangle _bounds;
        static Rectangle _picBounds;
        static bool _maskMouse;
        static Game1 _game;

        static Vector2 _Atitle;
        static Vector2 _Aauthor;
        static Vector2 _Atext;

        static TextBox _text;
        static TextBox _title;
        static TextBox _author;
        
        static Texture2D _close;
        static Texture2D _closeSec;
        static Texture2D _dropMenuBack;
        static Texture2D _dropMenuTop;
        static Texture2D _dropMenuBot;
        static Texture2D _dropMenuBackSec;
        static Texture2D _dropMenuTopSec;
        static Texture2D _dropMenuBotSec;
        public static SpriteFont _dropFont;

        static DropMenu _dropMenu;
        static Buttons.TabButton _synopsisTab;
        static Buttons.TabButton _reviewsTab;
        static Buttons.TabButton _writeTab;

        static FavButton _favButton;
        static Texture2D _favEmpty;
        static Texture2D _favFull;

        static Texture2D _open;
        static Texture2D _barFilling;
        static Texture2D _keyboardTex;
        static Texture2D _keyboardTexCaps;
        static SpriteFont _keyboardFont;
        static MyKeyboardState _keyboardState;
        static Texture2D _tabTex;
        static Texture2D _tabTexSec;
        static SpriteFont _tabFont;
        static Border _border;

        static Formular _formular;

        static EventHandler _tabCatcher;

        static int _activeTab = 0;

        

        public static void LoadContent(ContentManager cm, Game1 game)
        {
            _game = game;
            _back = cm.Load<Texture2D>("Layout/detailBack");
            _picAnchor = new Vector2(400, 190);
            _bounds = new Rectangle(233, 205, 940, 528);
            _text = new TextBox(LibState._font);
            _title = new TextBox(LibState._font);
            _author = new TextBox(LibState._font);

            _Atitle = new Vector2(420, 250);
            _Aauthor = new Vector2(420, 280);
            _Atext = new Vector2(440, 310);

            _favFull = cm.Load<Texture2D>("Layout/favEmpty");
            _favEmpty = cm.Load<Texture2D>("Layout/favEmpty_sec");
            _close = cm.Load<Texture2D>("Layout/close");
            _closeSec = cm.Load<Texture2D>("Layout/close_sec");
            _open = cm.Load<Texture2D>("Layout/open");
            _barFilling = cm.Load<Texture2D>("Layout/page");
            _tabTex = cm.Load<Texture2D>("Layout/tab");
            _tabTexSec = cm.Load<Texture2D>("Layout/tab_sec");

            _dropMenuBack = cm.Load<Texture2D>("Layout/dropBack");
            _dropMenuTop = cm.Load<Texture2D>("Layout/dropTop");
            _dropMenuBot = cm.Load<Texture2D>("Layout/dropBot");
            _dropMenuBackSec = cm.Load<Texture2D>("Layout/dropBack_sec");
            _dropMenuTopSec = cm.Load<Texture2D>("Layout/dropTop_sec");
            _dropMenuBotSec = cm.Load<Texture2D>("Layout/dropBot_sec");
            _dropFont = cm.Load<SpriteFont>("dropFont");

            _keyboardTex = cm.Load<Texture2D>("Layout/keyboard");
            _keyboardTexCaps = cm.Load<Texture2D>("Layout/keyboardCaps");
            _keyboardFont = cm.Load<SpriteFont>("keyboardFont");
            _tabFont = cm.Load<SpriteFont>("dropFont");

            _text.Position = _Atext;
            _title.Position = _Atitle;
            _author.Position = _Aauthor;

            _tabCatcher = new EventHandler(OnTabHit);

            _formular = new Formular(cm.Load<Texture2D>("Layout/page"), _favFull, _favEmpty, _tabFont);

            _synopsisTab = new Buttons.TabButton(400, 197, 207, 38, _tabTex, _tabTexSec, new Color(255, 255, 255), _tabFont, "Synopsis", 0, _tabCatcher);
            _reviewsTab = new Buttons.TabButton(550, 197, 207, 38, _tabTex, _tabTexSec, new Color(255, 255, 255), _tabFont, "Rezensionen", 1, _tabCatcher);
            _writeTab = new Buttons.TabButton(700, 197, 207, 38, _tabTex, _tabTexSec, new Color(255, 255, 255), _tabFont, "Rez. verfassen", 2, _tabCatcher);

            int top = 230;
            int right = 1210;
            int bottom = 760;
            int left = 230;

            _border = new Border(new Point(left, top), new Point(right, top), new Point(right, bottom), new Point(left, bottom), 5, cm.Load<Texture2D>("Layout/page"), new Color(192, 192, 192));

            _text.Width = 650;
            _title.Width = 650;
            _author.Width = 650;

            ReviewStats.LoadContent(cm);
        }

        public static void OnTabHit(object sender, EventArgs args)
        {
            _activeTab = ((Buttons.TabButton)sender)._index;
        }

        public static void ShowDetails(Entry entry)
        {
            _entry = entry;
            _reviewStats = new ReviewStats(entry);
            _reviewScrollBox = new ReviewScrollbox(entry);
            _text.Text = _entry._text;
            _title.Text = _entry._title;
            _author.Text = _entry._author;

            _activeTab = 0;

            _favButton = new FavButton(_favEmpty, _favFull, _entry._fav);

            _favButton.Clicked += new EventHandler(OnFavClicked);

            _dropMenu = new DropMenu();

            
        }

        public static void HideDetails()
        {
            LibState._clearMouse = false;
            LibState._inputMask = true;
            _entry = null;
            _reviewStats = null;
        }

        public static void Update()
        {
            if (_keyboardState == null)
            {
                if (Mouse.GetState().LeftButton == ButtonState.Released)
                    _maskMouse = false;

                if (_entry != null)
                {
                    if (!_entry._fav)
                        _entry._group = "default";

                    Active = true;

                    if (Mouse.GetState().LeftButton == ButtonState.Pressed && !_maskMouse)
                    {
                        Point mouse = new Point(Mouse.GetState().X, Mouse.GetState().Y);

                        if (_picBounds.Contains(mouse))
                        {
                            _game.ShowReaderState(_entry);
                            Game1._libState.AddLastRead(_entry);
                            HideDetails();
                        }

                        Vector2 check = new Vector2(1130 - mouse.X, 210 - mouse.Y);

                        if ((!_picBounds.Contains(mouse) && !_bounds.Contains(mouse)) || check.Length() < 30)
                        {
                            HideDetails();
                            return;
                        }

                    }

                    _reviewsTab.Update();
                    _synopsisTab.Update();
                    _writeTab.Update();

                    switch (_activeTab)
                    {
                        case 0: // synopsis tab
                            {
                                if (_entry != null && _entry._fav)
                                    _dropMenu.Update();
                                _favButton.Update();
                                break;
                            }
                        case 1: // view reviews tab
                            {
                                _reviewScrollBox.Update();

                                break;
                            }
                        case 2: // write review tab
                            {
                                _formular.Update();



                                break;                                
                            }
                    }


                }
                else
                    Active = false;
            }
            else
            {
                _keyboardState.Update();
            }
        }

        public static void Draw(SpriteBatch sb)
        {
            sb.Begin();

            if (_entry != null)
            {
                switch (_activeTab)
                {
                    case 0: // synopsis tab
                        {
                            sb.Draw(_back, Vector2.Zero, StyleSheet.COLORS["PRIMARY_COLOR"]);

                            _text.FontColor = StyleSheet.COLORS["GLOBAL_FONT_COLOR"];
                            _title.FontColor = StyleSheet.COLORS["GLOBAL_FONT_COLOR"];
                            _author.FontColor = StyleSheet.COLORS["GLOBAL_FONT_COLOR"];

                            _text.Draw(sb);
                            _title.Draw(sb);
                            _author.Draw(sb);

                            _writeTab.Draw(sb);
                            _reviewsTab.Draw(sb);

                            _border.Draw(sb);

                            _synopsisTab.Draw(sb);

                            if (_entry._fav)
                                _dropMenu.Draw(sb);

                            _favButton.Draw(sb);
                            break;
                        }
                    case 1: // view reviews tab
                        {
                            sb.Draw(_back, Vector2.Zero, StyleSheet.COLORS["PRIMARY_COLOR"]);

                            _reviewScrollBox.Draw(sb);

                            _reviewStats.Draw(sb);

                            Rectangle bar = new Rectangle(100, 351, 1101, 1);
                            sb.Draw(_barFilling, bar, StyleSheet.COLORS["SECONDARY_COLOR"]);

                            _writeTab.Draw(sb);
                            _synopsisTab.Draw(sb);

                            _border.Draw(sb);

                            _reviewsTab.Draw(sb);

                            break;
                        }
                    case 2: // write review tab
                        {
                            sb.End();
                            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

                            sb.Draw(_back, Vector2.Zero, StyleSheet.COLORS["PRIMARY_COLOR"]);

                            _synopsisTab.Draw(sb);
                            _reviewsTab.Draw(sb);

                            _border.Draw(sb);

                            _writeTab.Draw(sb);


                            _formular.Draw(sb);

                            sb.End();
                            sb.Begin();

                            break;
                        }
                }

                // Draw book cover
                float height = 580;
                float width = (580 / _entry._tex.Height) * _entry._tex.Width + 40;
                Rectangle drawRec = new Rectangle((int)(_picAnchor.X - width), (int)_picAnchor.Y, (int)width, (int)height);

                // Draw corner icon on lower right corner
                _picBounds = drawRec;

                sb.Draw(_entry._tex, drawRec, new Color(255, 255, 255));
                sb.Draw(_open, new Vector2(drawRec.X + drawRec.Width - _open.Width, drawRec.Y + drawRec.Height - _open.Height), new Color(255, 255, 255));

                sb.Draw(_close, new Vector2(1173, 210), StyleSheet.COLORS["PRIMARY_COLOR"]);
                sb.Draw(_closeSec, new Vector2(1173, 210), StyleSheet.COLORS["SECONDARY_COLOR"]);
            }

            if (_keyboardState != null)
            {
                _keyboardState.Draw(sb);
            }


            sb.End();
        }

        static private void OnFavClicked(object sender, EventArgs e)
        {
            _entry._fav = !_entry._fav;
        }

        private class DropMenu
        {
            Vector2 _pos;
            Vector2 _topPosDropped;
            Vector2 _topPosClosed;
            Vector2 _showTopPos;

            Rectangle _closedRec;
            Rectangle _dropRec;
            Rectangle _showRec;

            bool _masked;

            static DropButton _addNewButton;
            static DropButton _stanagButton;

            bool _dropped;

            int gHeight = 32;

            List<DropButton> _buttons;

            EventHandler _catcher;

            public DropMenu()
            {
                _pos = new Vector2(925, 685);

                _catcher = new EventHandler(OnClick);

                _addNewButton = new DropButton("+ Neue Gruppe");
                _addNewButton.Clicked += _catcher;
                _stanagButton = new DropButton("~ Standardgruppe");
                _stanagButton.Clicked += _catcher;

                _closedRec = new Rectangle((int)_pos.X, (int)_pos.Y + 19, 202, 1);
                _showRec = _closedRec;


                CheckGroups();

                ArrangeButtons();
            }

            public void Update()
            {
                if (_dropped)
                {
                    foreach (DropButton b in _buttons)
                    {
                        b.Update();
                    }

                    _showRec = _dropRec;
                    _showTopPos = _topPosDropped;

                    if (Mouse.GetState().LeftButton == ButtonState.Released)
                        _masked = false;

                    if (!_masked)
                    if (!_dropRec.Contains(Mouse.GetState().X, Mouse.GetState().Y) &&
                       Mouse.GetState().LeftButton == ButtonState.Pressed)
                        _dropped = false;
                }
                else
                {
                    _buttons[0].Update();
                    _showRec = _closedRec;
                    _showTopPos = _topPosClosed;
                }
            }

            private void CheckGroups()
            {
                List<string> groups = Game1._libState.GetGroups();
                _buttons = new List<DropButton>();
 
                int i = 0;
                foreach (string g in groups)
                {
                    ++i;

                    DropButton newButton = new DropButton(g);
                    newButton.SetPos(new Vector2(_pos.X, _pos.Y - i * gHeight + 3));

                    newButton.Clicked += _catcher;

                    _buttons.Add(newButton);
                }

                groups.Sort();

                _addNewButton.SetPos(new Vector2(_pos.X, _pos.Y));
                _stanagButton.SetPos(new Vector2(_pos.X, _pos.Y));

                _buttons.Add(_addNewButton);
                _buttons.Add(_stanagButton);

                _topPosClosed = _pos;
                _topPosDropped = new Vector2(_pos.X, _pos.Y - (_buttons.Count -1) * gHeight);
                _showTopPos = _topPosClosed;

                _dropRec = new Rectangle((int)_pos.X, (int)_pos.Y + 19 - (_buttons.Count - 1) * gHeight, 202, (_buttons.Count - 1) * gHeight);
            }

            private void ArrangeButtons()
            {
                List<DropButton> newList = new List<DropButton>();
                List<DropButton> buffer = new List<DropButton>(_buttons);

                buffer.Remove(_addNewButton);
                buffer.Remove(_stanagButton);

                if (_entry._group.Equals("default"))
                {
                    newList.Add(_stanagButton);
                    newList.Add(_addNewButton);
                    newList.AddRange(buffer);
                }
                else
                {
                    foreach (DropButton b in _buttons)
                    {
                        if (b._label.Equals(_entry._group))
                        {
                            buffer.Remove(b);
                            newList.Add(b);
                            newList.Add(_addNewButton);
                            newList.Add(_stanagButton);
                            _stanagButton._label = "~ Standardgruppe";
                            newList.AddRange(buffer);
                            break;
                        }
                    }
                }

                int gHeight = 32;
                int i = 0;
                foreach (DropButton b in newList)
                {
                    b.SetPos(new Vector2(_pos.X, _pos.Y - i * gHeight));

                    ++i;
                }

                if (newList[0]._label.Equals("~ Standardgruppe"))
                    newList[0]._label = "Gruppe zuweisen";

                _buttons = newList;
            }

            public void Draw(SpriteBatch sb)
            {
                sb.Draw(_dropMenuTop, _showTopPos, StyleSheet.COLORS["PRIMARY_COLOR"]);
                sb.Draw(_dropMenuBack, _showRec, StyleSheet.COLORS["PRIMARY_COLOR"]);
                sb.Draw(_dropMenuBot, new Vector2(_pos.X, _showRec.Y + _showRec.Height), StyleSheet.COLORS["PRIMARY_COLOR"]);
                sb.Draw(_dropMenuTopSec, _showTopPos, StyleSheet.COLORS["SECONDARY_COLOR"]);
                sb.Draw(_dropMenuBackSec, _showRec, StyleSheet.COLORS["SECONDARY_COLOR"]);
                sb.Draw(_dropMenuBotSec, new Vector2(_pos.X, _showRec.Y + _showRec.Height), StyleSheet.COLORS["SECONDARY_COLOR"]);

                if (!_dropped)
                {
                    _buttons[0].Draw(sb);
                }
                else
                {
                    foreach (DropButton b in _buttons)
                    {
                        b.Draw(sb);
                    }
                }
            }

            private void OnClick(object sender, EventArgs e)
            {
                string label = (string)sender;

                if (_dropped)
                {
                    if (label.Equals("+ Neue Gruppe"))
                    {
                        _keyboardState = new MyKeyboardState(_keyboardTex, _keyboardTexCaps, _keyboardFont);

                        _keyboardState.Return += new EventHandler(OnReturn);
                    }

                    if (label.Equals("Gruppe zuweisen") || label.Equals("~ Standardgruppe"))
                        _entry._group = "default";
                    else
                        _entry._group = label;
                }

                ArrangeButtons();

                if (!_dropped)
                    _masked = true;

                _dropped = !_dropped;
            }

            private void OnReturn(object sender, EventArgs e)
            {
                _entry._group = (string)sender;
                LibState._entries = LibState._entries;
                _keyboardState = null;

                CheckGroups();
                ArrangeButtons();

                
            }
        }
    }
}
