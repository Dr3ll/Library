using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Library.Classes
{
    class DropMenuFont : DropMenuBase
    {
        Vector2 _pos;
        Vector2 _topPosDropped;
        Vector2 _topPosClosed;
        Vector2 _showTopPos;

        Rectangle _closedRec;
        Rectangle _dropRec;
        Rectangle _showRec;

        static Texture2D _dropMenuBack;
        static Texture2D _dropMenuTop;
        static Texture2D _dropMenuBot;
        static Texture2D _dropMenuBackSec;
        static Texture2D _dropMenuTopSec;
        static Texture2D _dropMenuBotSec;

        string _label;

        bool _dropped;

        public SpriteFont Top;

        int gHeight = 32;

        List<DropButtonFont<SpriteFont>> _buttons;

        EventHandler _catcher;

        public event EventHandler Choice;


        public DropMenuFont(string label, Vector2 pos)
        {
            _pos = pos;
            _label = label;

            _catcher = new EventHandler(OnClick);

            _closedRec = new Rectangle((int)_pos.X, (int)_pos.Y + 19, 202, 1);
            _showRec = _closedRec;

            _buttons = new List<DropButtonFont<SpriteFont>>();


            DropButtonFont<SpriteFont> cycle = new DropButtonFont<SpriteFont>("Arial", StyleSheet.Arial, StyleSheet.Arial, pos, 1);
            _buttons.Add(cycle);
            cycle.Clicked += _catcher;


            Top = cycle.Value;

            if (StyleSheet.Arial != null)
            {
                _closedRec = new Rectangle((int)_pos.X, (int)_pos.Y + 19, 202, 1);
                _showRec = _dropRec;
            }

            if (StyleSheet.Cambria != null)
            {
                cycle = new DropButtonFont<SpriteFont>("Cambria", StyleSheet.Cambria, StyleSheet.Cambria, pos, 2);
                _buttons.Add(cycle);
                cycle.Clicked += _catcher;
            }

            if (StyleSheet.ComicSans != null)
            {
                cycle = new DropButtonFont<SpriteFont>("Comic Sans", StyleSheet.ComicSans, StyleSheet.ComicSans, pos, 3);
                _buttons.Add(cycle);
                cycle.Clicked += _catcher;
            }

            if (StyleSheet.LucidaConsole != null)
            {
                cycle = new DropButtonFont<SpriteFont>("Lucida Console", StyleSheet.LucidaConsole, StyleSheet.LucidaConsole, pos, 4);
                _buttons.Add(cycle);
                cycle.Clicked += _catcher;
            }

            if (StyleSheet.NewsGothic != null)
            {
                cycle = new DropButtonFont<SpriteFont>("News Gothic", StyleSheet.NewsGothic, StyleSheet.NewsGothic, pos, 5);
                _buttons.Add(cycle);
                cycle.Clicked += _catcher;
            }

            if (StyleSheet.Rockwell != null)
            {
                cycle = new DropButtonFont<SpriteFont>("Rockwell", StyleSheet.Rockwell, StyleSheet.Rockwell, pos, 6);
                _buttons.Add(cycle);
                cycle.Clicked += _catcher;
            }

            if (StyleSheet.TimesNewRoman != null)
            {
                cycle = new DropButtonFont<SpriteFont>("Times New Roman", StyleSheet.TimesNewRoman, StyleSheet.TimesNewRoman, pos, 7);
                _buttons.Add(cycle);
                cycle.Clicked += _catcher;
            }

            _topPosClosed = _pos;
            _topPosDropped = new Vector2(_pos.X, _pos.Y - (_buttons.Count - 1) * gHeight);
        }

        public void LoadContent(ContentManager cm)
        {
            _dropMenuBack = cm.Load<Texture2D>("Layout/dropBack");
            _dropMenuTop = cm.Load<Texture2D>("Layout/dropTop");
            _dropMenuBot = cm.Load<Texture2D>("Layout/dropBot");

            _dropMenuBackSec = cm.Load<Texture2D>("Layout/dropBack_sec");
            _dropMenuTopSec = cm.Load<Texture2D>("Layout/dropTop_sec");
            _dropMenuBotSec = cm.Load<Texture2D>("Layout/dropBot_sec");

            ArrangeButtons("Arial");
        }

        public void Update()
        {
            if (_dropped)
            {
                _dropRec = new Rectangle((int)_pos.X, (int)_pos.Y + 19 - (_buttons.Count - 1) * gHeight, 202, (_buttons.Count - 1) * gHeight);

                _showRec = _dropRec;
                _showTopPos = _topPosDropped;

                if (Mouse.GetState().LeftButton == ButtonState.Released)
                    _masked = false;

                if (!_masked)
                {
                    foreach (DropButtonFont<SpriteFont> b in _buttons)
                    {
                        b.Update();
                    }

                    if (!_dropRec.Contains(Mouse.GetState().X, Mouse.GetState().Y) &&
                       Mouse.GetState().LeftButton == ButtonState.Pressed)
                    {
                        _dropped = false;
                        Choice(this, EventArgs.Empty);
                    }
                }
            }
            else
            {
                if (Mouse.GetState().LeftButton == ButtonState.Released)
                    _masked = false;

                if (!_masked)
                {
                    _buttons[0].Update();
                    _showRec = _closedRec;
                    _showTopPos = _topPosClosed;
                }
            }
        }

        private void OnClick(object sender, EventArgs e)
        {
            string label = (string)sender;

            if (_dropped)
            {
                switch (label)
                {
                    case "Arial":
                        ArrangeButtons("Arial");
                        Choice(this, EventArgs.Empty);
                        break;
                    case "Cambria":
                        ArrangeButtons("Cambria");
                        Choice(this, EventArgs.Empty);
                        break;
                    case "Comic Sans":
                        ArrangeButtons("Comic Sans");
                        Choice(this, EventArgs.Empty);
                        break;
                    case "Lucida Console":
                        ArrangeButtons("Lucida Console");
                        Choice(this, EventArgs.Empty);
                        break;
                    case "News Gothic":
                        ArrangeButtons("News Gothic");
                        Choice(this, EventArgs.Empty);
                        break;
                    case "Rockwell":
                        ArrangeButtons("Rockwell");
                        Choice(this, EventArgs.Empty);
                        break;
                    case "Times New Roman":
                        ArrangeButtons("Times New Roman");
                        Choice(this, EventArgs.Empty);
                        break;
                    default:
                        break;
                }

            }


            if (!_dropped)
            {
                _masked = true;
                Drop();
            }

            _dropped = !_dropped;
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
                foreach (DropButtonFont<SpriteFont> b in _buttons)
                {
                    b.Draw(sb);
                }
            }
        }

        private void ArrangeButtons(string top)
        {
            List<DropButtonFont<SpriteFont>> newList = new List<DropButtonFont<SpriteFont>>();
            List<DropButtonFont<SpriteFont>> buffer = new List<DropButtonFont<SpriteFont>>(_buttons);

            foreach (DropButtonFont<SpriteFont> b in _buttons)
            {
                if (b._label.Equals(top))
                {
                    buffer.Remove(b);
                    newList.Add(b);
                    Top = b.Value;
                    newList.AddRange(buffer);
                    break;
                }
            }


            int gHeight = 32;
            int i = 0;
            foreach (DropButtonFont<SpriteFont> b in newList)
            {
                b.SetPos(new Vector2(_pos.X, _pos.Y - i * gHeight));

                ++i;
            }


            _buttons = newList;
        }
    }
}
