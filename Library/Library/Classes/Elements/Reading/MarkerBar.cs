using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Library.Classes
{
    class MarkerBar
    {
        Vector2 _position;

        Vector2 _relMarksStart;

        Texture2D _back;
        Texture2D _dot;
        BarHandle _barHandle;
        Color _red;
        Color _yellow;
        Color _blue;

        int _InHeightd = 20;
        int _inAcWidth = 60;
        int _acWidth = 90;
        int _acHeight = 31;

        List<MarkButton> _marks;
        List<Mark> _backup;

        public bool ToggleRed = true;
        public bool ToggleYellow = true;
        public bool ToggleBlue = true;

        int _max = 450;

        int _lastHandlePos = 0;

        public MarkerBar(Vector2 pos)
        {
            _position = pos;
            _relMarksStart = new Vector2(110, 80);

            _red = new Color(180, 0, 0);
            _yellow = new Color(255, 204, 2);
            _blue = new Color(2, 168, 218);

            _marks = new List<MarkButton>();
        }

        public void LoadContent(ContentManager cm)
        {
            _dot = cm.Load<Texture2D>("Layout/page");
            _back = cm.Load<Texture2D>("Layout/barBack");
            _barHandle = new BarHandle(cm.Load<Texture2D>("Layout/barHandle"));

            _barHandle.AcMarkHeight = _acHeight;
            _barHandle.InMarkHeight = _InHeightd;

            _backup = new List<Mark>();

            MarkButton.DOT = _dot;
        }

        public void UpdateMarks(List<Mark> marks)
        {
            marks.Sort();
            _backup = marks;

            ArrangeMarks();

            _barHandle.Update();
        }

        public void Clear()
        {
            _marks.Clear();
            _backup.Clear();

        }

        private void ArrangeMarks()
        {
            if (_backup == null || _backup.Count == 0)
                return;

            _marks.Clear();

            int dismiss = 0;
            foreach (Mark m in _backup)
            {
                if ((m.Color.R == 180 && !ToggleRed) ||
                   (m.Color.R == 255 && !ToggleYellow) ||
                   (m.Color.R == 90 && !ToggleBlue))
                    ++dismiss;
            }

            if (_backup.Count - dismiss > 15)
            {
                OverflowRoutine(dismiss);
            }
            else
            {
                StandardRoutine();
            }
        }

        private void StandardRoutine()
        {
            int i = 0;

            foreach (Mark m in _backup)
            {
                if ((m.Color.R == 180 && !ToggleRed) ||
                   (m.Color.R == 255 && !ToggleYellow) ||
                   (m.Color.R == 90 && !ToggleBlue))
                    continue;

                MarkButton next = new MarkButton(_inAcWidth, _acHeight, m);
                next.SetPos(new Vector2(0, i * _acHeight) + _relMarksStart + _position);

                if (i % 2 == 0)
                {
                    next.Resize(_inAcWidth - 15, _acHeight);
                    next.SetPos(new Vector2(15, i * _acHeight) + _relMarksStart + _position);
                }

                next.Active = true;
                _marks.Add(next);

                ++i;
            }
        }

        private void OverflowRoutine(int dismiss)
        {
            int mod = 0;

            while ((_backup.Count - dismiss) * (_InHeightd - mod) - 5* _acHeight > _max)
            {
                ++mod;

                if (mod + 1 >= _InHeightd)
                    break;
            }

            int i = 0;
            int stack = 0;
            int wHeight = _InHeightd - mod;

            foreach (Mark m in _backup)
            {
                if ((m.Color.R == 180 && !ToggleRed) ||
                   (m.Color.R == 255 && !ToggleYellow) ||
                   (m.Color.R == 90 && !ToggleBlue))
                    continue;

                int chosenHeight = 0;
                MarkButton next;

                if (i - _barHandle.MarkPos < 5 && i >= _barHandle.MarkPos)
                {
                    next = new MarkButton(_inAcWidth - (i % 2 == 0 ? 15 : 0), _acHeight, m);
                    next.SetPos(new Vector2((i % 2 == 0 ? 15 : 0), _barHandle.MarkPos * wHeight + (i - _barHandle.MarkPos) * _acHeight) + _relMarksStart + _position);
                    chosenHeight = _acHeight;
                    next.Active = true;
                }
                else
                {
                    next = new MarkButton(_inAcWidth - (i % 2 == 0 ? 15 : 0), wHeight, m);

                    next.Active = false;
                    if (_barHandle.MarkPos > i)
                    {
                        next.SetPos(new Vector2((i % 2 == 0 ? 15 : 0) , i * wHeight) + _relMarksStart + _position);
                    }
                    else
                    {
                        next.SetPos(new Vector2((i % 2 == 0 ? 15 : 0), (i - 5) * wHeight + 5 * _acHeight) + _relMarksStart + _position);
                    }

                    chosenHeight = wHeight;
                }

                //if (i % 2 == 0)
                //{
                //    next.Resize(_inAcWidth - 15, chosenHeight);

                //    if (_barHandle.MarkPos > i)
                //        next.SetPos(new Vector2(15, i * chosenHeight) + _relMarksStart + _position);
                //    else
                //        next.SetPos(new Vector2(15, (i - 5) * chosenHeight + 5 * chosenHeight) + _relMarksStart + _position);
                //}

                _marks.Add(next);

                stack += chosenHeight;

                ++i;

                if (stack >= _max)
                    break;
            }
            _barHandle.InMarkHeight = wHeight;

        }

        public void Update()
        {
            if (_barHandle.MarkPos < 0)
                _barHandle.MarkPos = 0;

            _barHandle.Update();
            if (_barHandle.MarkPos > _marks.Count - 5)
                _barHandle.MarkPos = _marks.Count - 5;

            if (_lastHandlePos != _barHandle.MarkPos)
                ArrangeMarks();

            _lastHandlePos = _barHandle.MarkPos;

            foreach(MarkButton m in _marks)
            {
                m.Update();
            }

        }

        public void Toggle(string color)
        {
            if (color.Equals("Red"))
                ToggleRed = !ToggleRed;
            if (color.Equals("Yellow"))
                ToggleYellow = !ToggleYellow;
            if (color.Equals("Blue"))
                ToggleBlue = !ToggleBlue;

            ArrangeMarks();
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(_back, _position, StyleSheet.COLORS["SECONDARY_COLOR"]);

            if(_marks.Count > 15)
                _barHandle.Draw(sb);

            foreach (MarkButton m in _marks)
            {
                m.Draw(sb);
            }
        }
    }
}
