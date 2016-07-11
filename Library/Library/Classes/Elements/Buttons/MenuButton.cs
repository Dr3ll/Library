
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Library.Classes
{
    class MenuButton
    {
        Texture2D _tex;
        Texture2D _texSec;
        Texture2D _icon;
        Texture2D _activeTex;
        Texture2D _activeTexSec;

        Vector2 _pos;
        List<Vector2> _hitbox;
        float _scale;
        Color dcolor;

        public bool Active;

        public int _index { private set; get; }

        public event EventHandler Hovered;

        public MenuButton(Texture2D tex, Texture2D texSec, Texture2D icon, Texture2D activeTex, Texture2D activeTexSec, Vector2 pos, float scale, int index)
        {
            _tex = tex;
            _texSec = texSec;
            _pos = pos;
            _activeTex = activeTex;
            _activeTexSec = activeTexSec;
            _scale = scale;
            _index = index;
            _icon = icon;

            dcolor = new Color(255, 255, 255);

            _hitbox = new List<Vector2>();
            List<Vector2> temp = new List<Vector2>();

            temp.Add(new Vector2(-3.5f, -0.75f));
            temp.Add(new Vector2(-2.0f, 3f));
            temp.Add(new Vector2(2.0f, 3f));
            temp.Add(new Vector2(3.5f, -0.75f));
            temp.Add(new Vector2(0f, -3f));

            foreach(Vector2 v in temp)
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
            if(!LibState._scrollStarted)
                dcolor = Color.Red;
        }

        public void Mask()
        {
            dcolor = new Color(255, 255, 255);
        }

        public void CallFunction()
        {
            if (!LibState._scrollStarted)
            {
                if (_index == 1)
                    LibState._sortMode = SortMode.Alphabetic;
                if (_index == 2)
                    LibState._sortMode = SortMode.Autor;
                if (_index == 3)
                    LibState._sortMode = SortMode.Favs;
                if (_index == 4)
                    LibState._sortMode = SortMode.Genre;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            Vector2 drawPos = new Vector2(_pos.X - .5f *_tex.Width, _pos.Y - .5f * _tex.Height);

            if (Active)
            {
                sb.Draw(_activeTex, drawPos, StyleSheet.COLORS["PRIMARY_COLOR"]);
                sb.Draw(_activeTexSec, drawPos, StyleSheet.COLORS["SECONDARY_COLOR"]);
            }
            else
            {
                sb.Draw(_tex, drawPos, StyleSheet.COLORS["PRIMARY_COLOR"]);
                sb.Draw(_texSec, drawPos, StyleSheet.COLORS["SECONDARY_COLOR"]);
            }

            sb.Draw(_icon, drawPos, StyleSheet.COLORS["SECONDARY_COLOR"]);


        }

    }
}
