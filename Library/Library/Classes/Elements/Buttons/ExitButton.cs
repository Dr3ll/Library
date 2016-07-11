
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Library.Classes
{
    class ExitButton
    {
        Texture2D _tex;
        Texture2D _texSec;

        Vector2 _pos;
        List<Vector2> _hitbox;
        float _scale;
        Color dcolor;

        public event EventHandler Hovered;
        public event EventHandler Clicked;

        public ExitButton(Texture2D tex, Texture2D texSec, Vector2 pos, float scale)
        {
            _pos = pos;
            _scale = scale;
            _tex = tex;
            _texSec = texSec;

            dcolor = new Color(255, 255, 255);

            _hitbox = new List<Vector2>();
            List<Vector2> temp = new List<Vector2>();

            float range = 1.0f;
            temp.Add(new Vector2(-range, -range));
            temp.Add(new Vector2(-range, range));
            temp.Add(new Vector2(range, range));
            temp.Add(new Vector2(range, -range));

            foreach (Vector2 v in temp)
            {
                _hitbox.Add(_scale * v + _pos);
            }

            this.Hovered += new EventHandler(OnHovered);
        }

        public bool CheckHit(Vector2 pos)
        {
            //pos = _pos;


            if (pos.X > 1280 || pos.X < 0
                || pos.Y > 800 || pos.Y < 0)
            {
                dcolor = new Color(255, 255, 255);
                return false;
            }

            List<Vector2> temp = new List<Vector2>();
            foreach (Vector2 v in _hitbox)
            {
                temp.Add(v - pos);
            }

            for (int i = 0; i < temp.Count; ++i)
            {
                float c = temp[(i + 1) % temp.Count].X * temp[i].Y -
                    temp[(i + 1) % temp.Count].Y * temp[i].X;

                if (
                    temp[(i + 1) % temp.Count].X * temp[i].Y -
                    temp[(i + 1) % temp.Count].Y * temp[i].X
                    <= 0
                    )
                {
                    Mask();
                    return false;
                }
            }

            if (Hovered != null)
                Hovered(null, EventArgs.Empty);

            return true;
        }

        private void OnHovered(object sender, EventArgs e)
        {
            dcolor = Color.Red;
        }

        public void Mask()
        {
            dcolor = new Color(255, 255, 255);
        }

        public void CallFunction()
        {
            Game1.ExitGame();
        }

        public void Draw(SpriteBatch sb)
        {
            Vector2 drawPos = new Vector2(_pos.X - .5f * _tex.Width, _pos.Y - .5f * _tex.Height);


            sb.Draw(_tex, drawPos, StyleSheet.COLORS["PRIMARY_COLOR"]);
            sb.Draw(_texSec, drawPos, StyleSheet.COLORS["SECONDARY_COLOR"]);


        }

    }
}
