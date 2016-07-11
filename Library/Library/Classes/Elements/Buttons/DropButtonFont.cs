using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Library.Classes
{
    class DropButtonFont<T> : Button, IComparable<DropButtonFont<T>>
    {
        public string _label;
        SpriteFont _font;
        public T Value;
        public int _index;
        Texture2D _pad;
        Texture2D _padH;
        Texture2D _padM;
        Color _color;
        Color _maskColor;
        public bool _high;
        public bool _mask;

        public DropButtonFont(string label, SpriteFont font, T value, Vector2 pos, int index)
            : base(new Rectangle((int)pos.X,(int)pos.Y, 170, 22))
        {
            _index = index;
            _label = label;
            _font = font;
            Value = value;
        }

        public DropButtonFont(string label, SpriteFont font, Texture2D pad, Texture2D padH, Texture2D padM, T value, Color color, Vector2 pos, int index)
            : base(new Rectangle((int)pos.X, (int)pos.Y, 50, 50))
        {
            _high = false;
            _index = index;
            _label = label;
            _pad = pad;
            _padH = padH;
            _padM = padM;
            Value = value;
            _color = color;

            _maskColor = new Color(.2f, .2f, .2f, 0.9f);

            _font = font;
            _mask = false;
        }

        public int CompareTo(DropButtonFont<T> other)
        {
            return this._index - other._index;
        }

        public override void Update()
        {
            if (!_mask)
                base.Update();
        }

        public void UpdateSingle(Vector2 singlePos)
        {
            Vector2 buffer = this._pos;

            this.SetPos(singlePos);

            if (!_mask)
                base.Update();

            this.SetPos(buffer);
        }

        public override void SetPos(Vector2 pos)
        {
            _pos = pos;

            _hitbox.X = (int)pos.X;
            _hitbox.Y = (int)pos.Y + 8;
        }

        protected override void OnHit()
        {
            base.CallClicked(_label, EventArgs.Empty);
            
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb)
        {
            Vector2 luci = Vector2.Zero;

            if (_label.Equals("Lucida Console"))
                luci = new Vector2(0, 4);

            if (_font != null)
                sb.DrawString(_font, _label, luci + base._pos + new Vector2(18, 5), StyleSheet.COLORS["GLOBAL_FONT_COLOR"], 0, Vector2.Zero, StyleSheet.SCALES["CSTM_FONT_SCALE"], SpriteEffects.None, 0);
            else
                sb.Draw(_pad, base._pos + new Vector2(18, 5), _color);
        }

        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, Vector2 move)
        {
            Vector2 luci = Vector2.Zero;

            if (_label.Equals("Lucida Console"))
                luci = new Vector2(0, 2);

            if(_high)
                sb.Draw(_padH, luci + base._pos + move - new Vector2(1,1), null, new Color(255, 255, 255), 0, Vector2.Zero, 1, SpriteEffects.None, .75f);

            sb.Draw(_pad, luci + base._pos + move, null, _color, 0, Vector2.Zero, 1, SpriteEffects.None, .8f);
            if (_mask)
                sb.Draw(_padM, luci + base._pos + move, null, new Color(255, 255, 255), 0, Vector2.Zero, 1, SpriteEffects.None, .8f);
        }

        public void DrawSingle(Microsoft.Xna.Framework.Graphics.SpriteBatch sb, Vector2 singlePos)
        {
            Vector2 luci = Vector2.Zero;

            if (_label.Equals("Lucida Console"))
                luci = new Vector2(0, 10);

            Vector2 pan = new Vector2(9, 7);

            if (_high)
                sb.Draw(_padH, luci + pan + singlePos - new Vector2(1, 1), null, new Color(255, 255, 255), 0, Vector2.Zero, 1, SpriteEffects.None, .15f);

            sb.Draw(_pad, luci + pan + singlePos, null, _color, 0, Vector2.Zero, 1, SpriteEffects.None, .2f);
            if (_mask)
                sb.Draw(_pad, luci + pan + singlePos, null, _maskColor,  0, Vector2.Zero, 1, SpriteEffects.None, .22f);

        }
    }
}
