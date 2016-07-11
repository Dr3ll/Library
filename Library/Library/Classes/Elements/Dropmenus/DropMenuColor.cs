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
    class DropMenuColor : DropMenuBase
    {
        Vector2 _pos;
        Vector2 _topPosDropped;
        Vector2 _topPosClosed;
        Vector2 _bottomDropPos;
        Vector2 _bottomClosedPos;
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

        static Texture2D _dropMenuBackSmall;
        static Texture2D _dropMenuTopSmall;
        static Texture2D _dropMenuBotSmall;
        static Texture2D _dropMenuBackSmallSec;
        static Texture2D _dropMenuTopSmallSec;
        static Texture2D _dropMenuBotSmallSec;

        static Texture2D _pad;
        static Texture2D _padH;
        static Texture2D _padM;

        bool _dropped;
        public string _label;

        

        public Color Top;
        public int _indexTop;

        int gHeight = 32;

        List<DropButtonFont<Color>> _buttons;

        EventHandler _catcher;

        public event EventHandler Choice;
        

        Point _posBlub = new Point(500, 300);


        public DropMenuColor(string label, Vector2 pos)
        {
            _pos = pos;
            _label = label;
            _dropMasked = false;

            _masked = true;

            _catcher = new EventHandler(OnClick);

            _closedRec = new Rectangle((int)_pos.X, (int)_pos.Y + 19, 68, 29);
            _dropRec = new Rectangle(_posBlub.X, _posBlub.Y + 19, 280, 6 * gHeight - 6);

            _showRec = _closedRec;

            _buttons = new List<DropButtonFont<Color>>();

        }

        public void LoadContent(ContentManager cm)
        {
            _dropMenuBack = cm.Load<Texture2D>("Layout/dropBackColor");
            _dropMenuTop = cm.Load<Texture2D>("Layout/dropTopColor");
            _dropMenuBot = cm.Load<Texture2D>("Layout/dropBotColor");
            _dropMenuBackSec = cm.Load<Texture2D>("Layout/dropBackColor_sec");
            _dropMenuTopSec = cm.Load<Texture2D>("Layout/dropTopColor_sec");
            _dropMenuBotSec = cm.Load<Texture2D>("Layout/dropBotColor_sec");

            _dropMenuBackSmall = cm.Load<Texture2D>("Layout/dropBackColorSmall");
            _dropMenuTopSmall = cm.Load<Texture2D>("Layout/dropTopColorSmall");
            _dropMenuBotSmall = cm.Load<Texture2D>("Layout/dropBotColorSmall");
            _dropMenuBackSmallSec = cm.Load<Texture2D>("Layout/dropBackColorSmall_sec");
            _dropMenuTopSmallSec = cm.Load<Texture2D>("Layout/dropTopColorSmall_sec");
            _dropMenuBotSmallSec = cm.Load<Texture2D>("Layout/dropBotColorSmall_sec");

            _pad = cm.Load<Texture2D>("Layout/colorPad");
            _padH = cm.Load<Texture2D>("Layout/colorPadHighlight");
            _padM = cm.Load<Texture2D>("Layout/colorPadMask");

            List<Color> colors = new List<Color>();
   
            colors.Add(new Color(7, 7, 7));
            colors.Add(new Color(255, 255, 255));
            colors.Add(new Color(157, 158, 140));
            colors.Add(new Color(187, 163, 127));
            colors.Add(new Color(58, 30, 29));
            colors.Add(new Color(151, 167, 221));
            colors.Add(new Color(3, 111, 137));
            colors.Add(new Color(1, 26, 56));
            colors.Add(new Color(86, 75, 141));
            colors.Add(new Color(244, 178, 182));
            colors.Add(new Color(127, 51, 116));
            colors.Add(new Color(197, 5, 106));
            colors.Add(new Color(206, 0, 0));
            colors.Add(new Color(232, 61, 97));
            colors.Add(new Color(243, 103, 80));
            colors.Add(new Color(249, 148, 66));
            colors.Add(new Color(122, 132, 61));
            colors.Add(new Color(141, 180, 123));
            colors.Add(new Color(202, 197, 97));
            colors.Add(new Color(240, 194, 59));


            DropButtonFont<Color> cycle = null;

            Vector2 move = new Vector2(-43, -45);
            
            int count = 0;
            foreach (Color c in colors)
            {
                move.X += 53;
                if (count % 5 == 0)
                {
                    move.Y += 53;
                    move.X = 10;
                }


                cycle = new DropButtonFont<Color>(count.ToString(), StyleSheet.Arial, _pad, _padH, _padM, c, c, new Vector2(_posBlub.X, _posBlub.Y) + move, count);
                _buttons.Add(cycle);
                cycle.Clicked += _catcher;
                count++;
            }

            Top = _buttons[0].Value;
            _indexTop = 0;
            _buttons[0]._high = true;

            _buttons.Sort();

            _topPosClosed = new Vector2(_pos.X, _pos.Y);
            _topPosDropped = new Vector2(_posBlub.X, _posBlub.Y);
            _bottomDropPos = new Vector2(_posBlub.X, _posBlub.Y + 6 * gHeight + 13);
            _bottomClosedPos = new Vector2(_pos.X, _pos.Y + gHeight + 14);
        }

        public void Update()
        {
            if (!_dropMasked)
            {
                if (_dropped)
                {
                    _showRec = _dropRec;
                    _showTopPos = _topPosDropped;

                    if (Mouse.GetState().LeftButton == ButtonState.Released)
                        _masked = false;

                    if (!_masked)
                    {
                        foreach (DropButtonFont<Color> b in _buttons)
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
                        foreach (DropButtonFont<Color> b in _buttons)
                        {
                            if (b._index == _indexTop)
                                b.UpdateSingle(_pos + new Vector2(9, 7));
                        }

                        _showRec = _closedRec;
                        _showTopPos = _topPosClosed;
                    }
                }

                _buttons.Sort();
            }

            ArrangeButtons(_indexTop);
        }

        private void OnClick(object sender, EventArgs e)
        {
            int index = int.Parse((string)sender);

            if (_dropped)
            {
                ArrangeButtons(index);
                Choice(this, EventArgs.Empty);
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
            _showRec = _closedRec;

            sb.Draw(_dropMenuTopSmall, _topPosClosed, null, StyleSheet.COLORS["PRIMARY_COLOR"], 0, Vector2.Zero, 1, SpriteEffects.None, .1f);
            sb.Draw(_dropMenuBackSmall, _showRec, null, StyleSheet.COLORS["PRIMARY_COLOR"], 0, Vector2.Zero, SpriteEffects.None, .1f);
            sb.Draw(_dropMenuTopSmallSec, _topPosClosed, null, StyleSheet.COLORS["SECONDARY_COLOR"], 0, Vector2.Zero, 1, SpriteEffects.None, .2f);
            sb.Draw(_dropMenuBackSmallSec, _showRec, null, StyleSheet.COLORS["SECONDARY_COLOR"], 0, Vector2.Zero, SpriteEffects.None, .2f);

            sb.Draw(_dropMenuBotSmall, _bottomClosedPos, null, StyleSheet.COLORS["PRIMARY_COLOR"], 0, Vector2.Zero, 1, SpriteEffects.None, .1f);
            sb.Draw(_dropMenuBotSmallSec, _bottomClosedPos, null, StyleSheet.COLORS["SECONDARY_COLOR"], 0, Vector2.Zero, 1, SpriteEffects.None, .2f);

            foreach (DropButtonFont<Color> b in _buttons)
            {
                if (b._index == _indexTop)
                    b.DrawSingle(sb, _pos);
            }
        }

        public void DrawDrop(SpriteBatch sb)
        {
            if (_dropped)
            {
                _showRec = _dropRec;

                sb.Draw(_dropMenuTop, _topPosDropped, null, StyleSheet.COLORS["PRIMARY_COLOR"], 0, Vector2.Zero, 1, SpriteEffects.None, .5f);
                sb.Draw(_dropMenuBack, _showRec, null, StyleSheet.COLORS["PRIMARY_COLOR"], 0, Vector2.Zero, SpriteEffects.None, .5f);
                sb.Draw(_dropMenuTopSec, _topPosDropped, null, StyleSheet.COLORS["SECONDARY_COLOR"], 0, Vector2.Zero, 1, SpriteEffects.None, .6f);
                sb.Draw(_dropMenuBackSec, _showRec, null, StyleSheet.COLORS["SECONDARY_COLOR"], 0, Vector2.Zero, SpriteEffects.None, .6f);

                sb.Draw(_dropMenuBot, _bottomDropPos, null, StyleSheet.COLORS["PRIMARY_COLOR"], 0, Vector2.Zero, 1, SpriteEffects.None, .5f);
                sb.Draw(_dropMenuBotSec, _bottomDropPos, null, StyleSheet.COLORS["SECONDARY_COLOR"], 0, Vector2.Zero, 1, SpriteEffects.None, .6f);

                int count = 0;

                if (true)
                {
                    for (int i = 0; i < 5; ++i)
                    {
                        _buttons[i].Draw(sb, Vector2.Zero);
                    }
                    for (int i = 5; i < 10; ++i)
                    {
                        _buttons[i].Draw(sb, Vector2.Zero);
                    }
                    for (int i = 10; i < 15; ++i)
                    {
                        _buttons[i].Draw(sb, Vector2.Zero);
                    }
                    for (int i = 15; i < 20; ++i)
                    {
                        _buttons[i].Draw(sb, Vector2.Zero);
                    }
                }
            }
        }

        public void Mask(int index)
        {
            foreach (DropButtonFont<Color> b in _buttons)
            {
                if(b._index != index)
                    b._mask = false;
                else
                    b._mask = true;
            }
        }

        private void ArrangeButtons(int top)
        {
            List<DropButtonFont<Color>> newList = new List<DropButtonFont<Color>>();
            List<DropButtonFont<Color>> buffer = new List<DropButtonFont<Color>>(_buttons);

            foreach (DropButtonFont<Color> b in _buttons)
            {
                b._high = false;
            }

            foreach (DropButtonFont<Color> b in _buttons)
            {
                if (b._index.Equals(top))
                {
                    buffer.Remove(b);
                    newList.Add(b);
                    Top = b.Value;
                    _indexTop = b._index;
                    b._high = true;
                    newList.AddRange(buffer);
                    break;
                }
            }

            

            int gHeight = 32;
            int i = 0;


            _buttons = newList;
        }
    }
}
