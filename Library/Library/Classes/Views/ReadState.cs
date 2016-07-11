using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Library.Classes
{
    public class ReadState : State
    {
        Entry _entry;
        SpriteBatch _sb;
        Rectangle _page;
        Rectangle _mask;
        Texture2D _pageTex;

        Texture2D _leftEndM;
        Texture2D _rightEndM;
        Texture2D _middleM;

        BlindButton _redBlind;
        BlindButton _yellowBlind;
        BlindButton _blueBlind;

        List<Mark>[] _marks;
        Mark _buffer;

        bool _MouseMask;
        bool _marking;

        Vector2 _active;
        int _selection = 0;

        MouseState _oldMouse;

        Vector2 _selectorPanelPos;
        Texture2D _selectorPanel;
        Texture2D _toggleTex;
        Selector _redSelector;
        Selector _yellowSelector;
        Selector _blueSelector;
        Selector _deselect;
        Color _red;
        Color _yellow;
        Color _blue;

        EventHandler _catcher;
        EventHandler _blindCatcher;

        public static float _scrolling;
        float _scrollingBoundary;
        bool _scrollStarted;
        TextBox _content;

        MarkerBar _bar;

        public ReadState(Game1 game, SpriteBatch sb)
            : base(game)
        {
            _catcher = new EventHandler(OnSelection);
            _blindCatcher = new EventHandler(OnBlind);

            _scrolling = 0;

            _sb = sb;
            _page = new Rectangle(180, 180, 920, 620);
            _mask = new Rectangle(150, 0, 1050, 180);

            _marks = new List<Mark>[3];

            _buffer = new Mark();
            _active = Vector2.Zero;

            _bar = new MarkerBar(new Vector2(1110, 180));
        }

        public void LoadContent(ContentManager cm)
        {
            _bar.LoadContent(cm);

            _red = new Color(180, 100, 100);
            _yellow = new Color(255, 204, 90);
            _blue = new Color(90, 168, 218);

            _pageTex = cm.Load<Texture2D>("Layout/page");

            _middleM = cm.Load<Texture2D>("Layout/MarkMiddle");
            _leftEndM = cm.Load<Texture2D>("Layout/MarkLeft");
            _rightEndM = cm.Load<Texture2D>("Layout/MarkRight");

            _toggleTex = cm.Load<Texture2D>("Layout/toggle");

            _selectorPanel = cm.Load<Texture2D>("Layout/SelectorPanel");
            _selectorPanelPos = new Vector2(0, 170);

            _redBlind = new BlindButton(1245, 186, 32, 40, "Red", cm.Load<Texture2D>("Layout/redBlind"), _red);
            _yellowBlind = new BlindButton(1212, 201, 30, 32, "Yellow", cm.Load<Texture2D>("Layout/yellowBlind"), _yellow);
            _blueBlind = new BlindButton( 1182, 217, 23, 30, "Blue", cm.Load<Texture2D>("Layout/blueBlind"), _blue);

            _redBlind.Clicked += _blindCatcher;
            _yellowBlind.Clicked += _blindCatcher;
            _blueBlind.Clicked += _blindCatcher;

            Texture2D selectorTex = cm.Load<Texture2D>("Layout/SelectorTex");
            Texture2D deselectorTex = cm.Load<Texture2D>("Layout/des");
            int start = 32;
            int spacer = 17;
            int h = 70;

            _redSelector = new Selector((int)_selectorPanelPos.Y + start, selectorTex, _red, "Red", _toggleTex);
            _yellowSelector = new Selector((int)_selectorPanelPos.Y + start + spacer + h, selectorTex, _yellow, "Yellow", _toggleTex);
            _blueSelector = new Selector((int)_selectorPanelPos.Y + start + 2 * spacer + 2 * h, selectorTex, _blue, "Blue", _toggleTex);
            _deselect = new Selector((int)_selectorPanelPos.Y + start + 3 * spacer + 3 * h, deselectorTex, new Color(255, 255, 255), "DES", _toggleTex);

            _deselect.Toggled = true;

            _redSelector.Clicked += _catcher;
            _yellowSelector.Clicked += _catcher;
            _blueSelector.Clicked += _catcher;
            _deselect.Clicked += _catcher;
        }

        float GetFlip(Vector2 a, Vector2 b)
        {

            Vector2 temp = b - a;
            temp.Normalize();
            
            return ((a.Y - b.Y) > 0 ? -1 : 1) * (float)Math.Acos((double)Vector2.Dot(temp, new Vector2(1, 0)));
        }

        private void ProduceMarks()
        {
            #region If just released
            if (Mouse.GetState().LeftButton == ButtonState.Released &&
               _oldMouse.LeftButton == ButtonState.Pressed &&
               _selection != 0 &&
               _page.Contains(Mouse.GetState().X, Mouse.GetState().Y) &&
               !_buffer.Equals(Mark.Studd))
            {
                _buffer.PointA.Y -= _scrolling;
                _buffer.PointB.Y -= _scrolling;

                _marks[_selection - 1].Add(_buffer);
                _marks[_selection - 1].Sort();
                UpdateBar();

                _buffer = Mark.Studd;

                _marking = false;
            }
            #endregion

            if (Mouse.GetState().LeftButton == ButtonState.Released &&
                _oldMouse.LeftButton == ButtonState.Pressed &&
                _selection != 0 &&
                !_page.Contains(Mouse.GetState().X, Mouse.GetState().Y))
            {
                _active = Vector2.Zero;

                _marking = false;
            }


            #region If Mouse Pressed
            if (Mouse.GetState().LeftButton == ButtonState.Pressed &&
                _oldMouse.LeftButton == ButtonState.Released &&
               _selection != 0 &&
               _page.Contains(Mouse.GetState().X, Mouse.GetState().Y))
            {
                _active = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
                _marking = true;
            }
            #endregion

            if (_marking)
            {
                Mark next = new Mark();

                next.PointA = _active;
                next.PointB = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
                next.flip = GetFlip(next.PointA, next.PointB);

                _buffer = next;
            }

        }

        public override void Update()
        {
            if (Mouse.GetState().LeftButton == ButtonState.Released)
            {
                _MouseMask = true;
            }

            _redBlind.Update();
            _yellowBlind.Update();
            _blueBlind.Update();

            if(_MouseMask && _selection > 0)
                ProduceMarks();

            _redSelector.Update();
            _yellowSelector.Update();
            _blueSelector.Update();
            _deselect.Update();

            if(_selection == 0)
                HandleScrolling();

            _oldMouse = Mouse.GetState();

            _bar.Update();

            #region Debug


            #endregion

            _entry._reds = _marks[0];
            _entry._yellows = _marks[1];
            _entry._blues = _marks[2];
        }

        public void Refresh()
        {
            ShowReader(_entry);
        }

        public void ShowReader(Entry entry)
        {
            _entry = entry;

            _marks[0] = _entry._reds;
            _marks[1] = _entry._yellows;
            _marks[2] = _entry._blues;

            _bar.Clear();
            UpdateBar();

            _content = new TextBox(StyleSheet.FONTS["BOOK_FONT"], StyleSheet.SCALES["BOOK_FONT_SCALE"]);
            _content.MaxWraps = 5000;
            _content.Width = 810;
            _content.Text = _entry._content;
            _content.Position = new Vector2(236, 200);
            _content.FontColor = StyleSheet.COLORS["BOOK_FONT_COLOR"];
            

            _scrollingBoundary = -(_content.Height - 500);
        }

        private void DrawMark(Mark m, Color color)
        {
            if (m.Upper.Y + _scrolling + 10 < 180)
                return;

            Vector2 a = new Vector2( m.PointA.X, m.PointA.Y + _scrolling);
            Vector2 b = new Vector2(m.PointB.X, m.PointB.Y + _scrolling);

            Rectangle midRec = new Rectangle((int)a.X, (int)a.Y, (int)(Vector2.Distance(a, b)), 23);

            _sb.Draw(_leftEndM, a, null, color, m.flip, new Vector2(_leftEndM.Width * .5f, _leftEndM.Height * .5f), 1, SpriteEffects.None, 1);
            _sb.Draw(_middleM, midRec, null, color, m.flip, new Vector2(0, 11), SpriteEffects.None, 1);
            _sb.Draw(_rightEndM, b, null, color, m.flip, new Vector2(_leftEndM.Width * .5f, _leftEndM.Height * .5f), 1, SpriteEffects.None, 1);
        }

        private void UpdateBar()
        {
            List<Mark> feed = new List<Mark>();

            for(int i=0; i<3; ++i)
            {

                foreach (Mark m in _marks[i])
                {
                    if (i == 0)
                        m.SetColor(_red);
                    if (i == 1)
                        m.SetColor(_yellow);
                    if (i == 2)
                        m.SetColor(_blue);

                    feed.Add(m);
                }
            }

            _bar.UpdateMarks(feed);
        }

        public override void Draw()
        {
            _sb.Begin();

            _sb.Draw(_pageTex, _page, StyleSheet.COLORS["BOOK_BACK_COLOR"]);


            _sb.Draw(_selectorPanel, _selectorPanelPos, StyleSheet.COLORS["SECONDARY_COLOR"]);
            _redSelector.Draw(_sb);
            _yellowSelector.Draw(_sb);
            _blueSelector.Draw(_sb);

            _deselect.SetColorDeselect(StyleSheet.COLORS["SECONDARY_COLOR"]);
            _deselect.Draw(_sb);

            if(_bar.ToggleRed)
            foreach(Mark m in _marks[0])
            { // Red Marks
                DrawMark(m, _red);
            }

            if(_bar.ToggleYellow)
            foreach (Mark m in _marks[1])
            { // Yellow Marks
                DrawMark(m, _yellow);
            }

            if (_bar.ToggleBlue)
            foreach (Mark m in _marks[2])
            { // Blue Marks
                DrawMark(m, _blue);
            }

            _redBlind.Draw(_sb, (_bar.ToggleRed ? new Color(255, 255, 255) : new Color(160, 160, 160)));
            _yellowBlind.Draw(_sb, (_bar.ToggleYellow ? new Color(255, 255, 255) : new Color(150, 150, 150)));
            _blueBlind.Draw(_sb, (_bar.ToggleBlue ? new Color(255, 255, 255) : new Color(150, 150, 150)));

            _bar.Draw(_sb);

            Rectangle bufferRec = new Rectangle((int)_buffer.PointA.X, (int)_buffer.PointA.Y, (int)(Vector2.Distance(_buffer.PointA, _buffer.PointB)), 23);

            Color buffColor = new Color(255, 255, 255);
            if (_selection == 1) buffColor = _red;
            if (_selection == 2) buffColor = _yellow;
            if (_selection == 3) buffColor = _blue;

            _sb.Draw(_leftEndM, _buffer.PointA, null, buffColor, _buffer.flip, new Vector2(_leftEndM.Width * .5f, _leftEndM.Height * .5f), 1, SpriteEffects.None, 1);
            _sb.Draw(_middleM, bufferRec, null, buffColor, _buffer.flip, new Vector2(0, 11), SpriteEffects.None, 1);
            _sb.Draw(_rightEndM, _buffer.PointB, null, buffColor, _buffer.flip, new Vector2(_leftEndM.Width * .5f, _leftEndM.Height * .5f), 1, SpriteEffects.None, 1);

            _content.Scrolling = _scrolling;
            _content.Draw(_sb);


            _sb.Draw(_pageTex, _mask, StyleSheet.COLORS["PRIMARY_COLOR"]);

            _sb.End();
        }

        private void HandleScrolling()
        {
            MouseState mouse = Mouse.GetState();

            if (_page.Contains(Mouse.GetState().X, Mouse.GetState().Y))
            {
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    _scrollStarted = true;

                    Vector2 move = new Vector2(_oldMouse.X, _oldMouse.Y) - new Vector2(mouse.X, mouse.Y);

                    _scrolling -= move.Y;
                }
            }

            if (_scrollStarted && Mouse.GetState().LeftButton == ButtonState.Released)
                _scrollStarted = false;

            if (_scrolling > 0)
                _scrolling = 0;

            if (_scrolling < _scrollingBoundary)
                _scrolling = _scrollingBoundary;
        }

        private void OnSelection(object sender, EventArgs e)
        {
            string label = (string)sender;

            if (label.Equals("DES"))
            {
                _selection = 0;

                _blueSelector.Toggled = false;
                _redSelector.Toggled = false;
                _yellowSelector.Toggled = false;
                _deselect.Toggled = true;
            }
            if (label.Equals("Red")) 
            {
                _selection = 1;

                _blueSelector.Toggled = false;
                _redSelector.Toggled = true;
                _yellowSelector.Toggled = false;
                _deselect.Toggled = false;
            }
            if (label.Equals("Yellow"))
            {
                _selection = 2;

                _blueSelector.Toggled = false;
                _redSelector.Toggled = false;
                _yellowSelector.Toggled = true;
                _deselect.Toggled = false;
            }


            if (label.Equals("Blue"))
            {
                _selection = 3;

                _blueSelector.Toggled = true;
                _redSelector.Toggled = false;
                _yellowSelector.Toggled = false;
                _deselect.Toggled = false;
            }
        }

        private void OnBlind(object label, EventArgs e)
        {
            _bar.Toggle((string)label);
        }
    }

    public class Mark : IComparable<Mark>
    {
        public Vector2 PointA;
        public Vector2 PointB;
        public float flip;
        public Color Color = new Color(255, 255, 255);

        public void SetColor(Color c)
        {
            Color = c;
        }

        public Vector2 Upper
        {
            get { return (PointA.Y < PointB.Y) ? PointA : PointB; }
        }

        public static Mark Studd = new Mark();

        public int CompareTo(Mark other)
        {
            Vector2 upper = (PointA.Y < PointB.Y) ? PointA : PointB;
            Vector2 upperO = (other.PointA.Y < other.PointB.Y) ? other.PointA : other.PointB;

            return upper.Y.CompareTo(upperO.Y);
        }
    }
}
