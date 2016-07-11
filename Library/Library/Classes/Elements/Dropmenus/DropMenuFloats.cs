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
    class DropMenuFloats : DropMenuBase
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

        public string _label;

        bool _dropped;

        public float Top;

        int gHeight = 32;

        List<DropButtonFont<float>> _buttons;

        EventHandler _catcher;

        public event EventHandler Choice;


        public DropMenuFloats(string label, Vector2 pos)
        {
            _pos = pos;
            _label = label;

            _catcher = new EventHandler(OnClick);

            _closedRec = new Rectangle((int)_pos.X, (int)_pos.Y + 19, 202, 1);
            _showRec = _closedRec;

            _buttons = new List<DropButtonFont<float>>();

            
            _closedRec = new Rectangle((int)_pos.X, (int)_pos.Y + 19, 202, 1);
            _showRec = _dropRec;

            DropButtonFont<float> cycle = new DropButtonFont<float>("10", StyleSheet.FONTS["GLOBAL_FONT"], .5f, pos, 1);
            _buttons.Add(cycle);
            cycle.Clicked += _catcher;

            cycle = new DropButtonFont<float>("12", StyleSheet.FONTS["GLOBAL_FONT"], .6f, pos, 2);
            _buttons.Add(cycle);
            cycle.Clicked += _catcher;

            cycle = new DropButtonFont<float>("14", StyleSheet.FONTS["GLOBAL_FONT"], .7f, pos, 3);
            _buttons.Add(cycle);
            cycle.Clicked += _catcher;

            cycle = new DropButtonFont<float>("16", StyleSheet.FONTS["GLOBAL_FONT"], .8f, pos, 4);
            _buttons.Add(cycle);
            cycle.Clicked += _catcher;

            Top = cycle.Value;
            ArrangeButtons("16");

            cycle = new DropButtonFont<float>("18", StyleSheet.FONTS["GLOBAL_FONT"], .9f, pos, 5);
            _buttons.Add(cycle);
            cycle.Clicked += _catcher;

            cycle = new DropButtonFont<float>("20", StyleSheet.FONTS["GLOBAL_FONT"], 1.0f, pos, 6);
            _buttons.Add(cycle);
            cycle.Clicked += _catcher;

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

            ArrangeButtons("16");
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
                    foreach (DropButtonFont<float> b in _buttons)
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
                    case "10":
                        ArrangeButtons("10");
                        Choice(this, EventArgs.Empty);
                        break;
                    case "12":
                        ArrangeButtons("12");
                        Choice(this, EventArgs.Empty);
                        break;
                    case "14":
                        ArrangeButtons("14");
                        Choice(this, EventArgs.Empty);
                        break;
                    case "16":
                        ArrangeButtons("16");
                        Choice(this, EventArgs.Empty);
                        break;
                    case "18":
                        ArrangeButtons("18");
                        Choice(this, EventArgs.Empty);
                        break;
                    case "20":
                        ArrangeButtons("20");
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
            sb.Draw(_dropMenuTop, _showTopPos, null, StyleSheet.COLORS["PRIMARY_COLOR"], 0, Vector2.Zero, 1, SpriteEffects.None, .18f);
            sb.Draw(_dropMenuBack, _showRec, null, StyleSheet.COLORS["PRIMARY_COLOR"], 0, Vector2.Zero, SpriteEffects.None, .18f);
            sb.Draw(_dropMenuBot, new Vector2(_pos.X, _showRec.Y + _showRec.Height), null, StyleSheet.COLORS["PRIMARY_COLOR"], 0, Vector2.Zero, 1, SpriteEffects.None, .18f);

            sb.Draw(_dropMenuTopSec, _showTopPos, null, StyleSheet.COLORS["SECONDARY_COLOR"], 0, Vector2.Zero, 1, SpriteEffects.None, .19f);
            sb.Draw(_dropMenuBackSec, _showRec, null, StyleSheet.COLORS["SECONDARY_COLOR"], 0, Vector2.Zero, SpriteEffects.None, .19f);
            sb.Draw(_dropMenuBotSec, new Vector2(_pos.X, _showRec.Y + _showRec.Height), null, StyleSheet.COLORS["SECONDARY_COLOR"], 0, Vector2.Zero, 1, SpriteEffects.None, .19f);

            if (!_dropped)
            {
                _buttons[0].Draw(sb);
            }
            else
            {
                foreach (DropButtonFont<float> b in _buttons)
                {
                    b.Draw(sb);
                }
            }
        }

        private void ArrangeButtons(string top)
        {
            List<DropButtonFont<float>> newList = new List<DropButtonFont<float>>();
            List<DropButtonFont<float>> buffer = new List<DropButtonFont<float>>(_buttons);

            foreach (DropButtonFont<float> b in _buttons)
            {
                if (b._label.Equals(top))
                {
                    buffer.Remove(b);
                    newList.Add(b);
                    Top = b.Value;
                    buffer.Sort();
                    newList.AddRange(buffer);
                    break;
                }
            }


            int gHeight = 32;
            int i = 0;
            foreach (DropButtonFont<float> b in newList)
            {
                b.SetPos(new Vector2(_pos.X, _pos.Y - i * gHeight));

                ++i;
            }


            _buttons = newList;
        }
    }
}
