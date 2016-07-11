using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.IO;
using System.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Library.Classes
{
    public enum SortMode
    {
        Alphabetic,
        Autor,
        Favs,
        Genre
    }

    public class LibState : State
    {
        public static List<Entry> _entries;
        List<Vector2> _positions;
        SpriteBatch _sb;
        public static Vector2 _scrolling;
        MouseState _oldMouse;
        public static SortMode _sortMode = SortMode.Favs;
        public static bool _scrollStarted;
        List<Box> _genres = new List<Box>();
        List<Box> _groups = new List<Box>();
        Box _lastRead;
        Box _noOrder;

        static public bool _inputMask = true;

        Vector2 _mouseInitiate;

        int _active = 0;
        public static bool _clearMouse;
        static Texture2D _sepTex;

        int _scrollingBoundary = 0;

        public static SpriteFont _font;

        public LibState(Game1 game, SpriteBatch sb)
            : base(game)
        {
            _entries= new List<Entry>();
            _positions = new List<Vector2>();
            _sb = sb;
            _scrolling = Vector2.Zero;
            _scrollStarted = false;
            LibState._clearMouse = true;
            _lastRead = new Box("Zuletzt gelesen");
            _noOrder = new Box("Ungeordnet");

            _mouseInitiate = Vector2.Zero;
        }

        public void LoadContent(ContentManager cm)
        {
            StyleSheet.LoadStyleSheet(cm);

            string path = cm.RootDirectory + "/Data";

            IEnumerable<string> files = Directory.EnumerateFiles(path, "*.xml", SearchOption.AllDirectories);

            _font = cm.Load<SpriteFont>("titleFont");
            _sepTex = cm.Load<Texture2D>("Layout/seperator");


            foreach (string file in files)
            {
                DataSet data = new DataSet();
                data.ReadXml(file);

                DataRow meta = data.Tables["Meta"].Rows[0];

                string title = meta["Title"].ToString();
                string author = meta["Author"].ToString();
                string genre = meta["Genre"].ToString();

                DataRow peek = data.Tables["EBook"].Rows[0];
                string text = peek["Peek"].ToString();
                DataRow content = data.Tables["EBook"].Rows[0];
                string blub = content["Content"].ToString();
                
                DataRow reviews = data.Tables["Reviews"].Rows[0];
                int nreviews = int.Parse(reviews["Total"].ToString());
                int average = int.Parse(reviews["Average"].ToString());
                int[] distribution = new int[5];
                distribution[0] = int.Parse(reviews["Five"].ToString());
                distribution[1] = int.Parse(reviews["Four"].ToString());
                distribution[2] = int.Parse(reviews["Three"].ToString());
                distribution[3] = int.Parse(reviews["Two"].ToString());
                distribution[4] = int.Parse(reviews["One"].ToString());

                DataTable reviewBox = data.Tables["Review"];
                DataRow curReview = null;

                List<Review> reviewData = new List<Review>();

                for (int i = 0; i < reviewBox.Rows.Count; ++i)
                {
                    curReview = reviewBox.Rows[i];

                    string rTitle = curReview["Title"].ToString();
                    string rAuthor = curReview["Author"].ToString();
                    string rDate = curReview["Date"].ToString();
                    string rText = curReview["Text"].ToString();
                    int rRating = int.Parse(curReview["Rating"].ToString());

                    reviewData.Add(new Review(rTitle, rText, rRating, rDate, rAuthor));
                }


                _entries.Add(new Entry(
                    cm.Load<Texture2D>("Data/Covers/" + title),
                    null,
                    Vector2.Zero,
                    title,
                    author,
                    text,
                    blub,
                    genre,
                    nreviews,
                    average,
                    distribution,
                    reviewData)
                    );
            }

            int leftb = 100;
            int spacer = 30;
            int width = 200;
            int height = 300;
            int topstart = 170;

            #region Division
            foreach (Entry e in _entries)
            {
                bool exists = false;
                foreach (Box b in _genres)
                {
                    if (b.label.Equals(e._genre))
                    {
                        b.entries.Add(e);
                        exists = true;
                    }
                }

                if (!exists)
                {
                    Box newBox = new Box(e._genre);
                    newBox.entries.Add(e);
                    _genres.Add(newBox);
                }
            }
            #endregion

            

            _genres.Sort();

            for(int i=0; i<_entries.Count * 2; ++i)
            {
                _positions.Add(new Vector2(
                    leftb + i%5 * spacer + i%5 * width,
                    topstart + (int)(i / 5) * (spacer * .6f) + (int)(i / 5) * height));
            }
        }

        public override void Draw()
        {
            _sb.Begin();

            if (_sortMode == SortMode.Genre)
            {
                foreach (Box b in _genres)
                {
                    b.sep.Draw(_sb);
                    foreach (Entry e in b.entries)
                    {
                        e.Draw(_sb);
                    }
                }
            }
            else if (_sortMode == SortMode.Favs)
            {
                bool empty = true;
                foreach (Box b in _groups)
                {
                    b.sep.Draw(_sb);
                    foreach (Entry e in b.entries)
                    {
                        e.Draw(_sb);

                        empty = false;
                    }
                }

                if(empty)
                    _sb.DrawString(_font, "Es sind keine Favoriten gesetzt.", new Vector2(640 - _font.MeasureString("Es sind keine Favoriten gesetzt.").X * .5f, 225), StyleSheet.COLORS["GLOBAL_FONT_COLOR"]);
            }
            else
            {
                foreach (Entry e in _entries)
                {
                    e.Draw(_sb);
                }
            }

            _sb.End();
        }

        public override void Update()
        {
            if (_inputMask)
            {
                if (Mouse.GetState().LeftButton == ButtonState.Released)
                    _inputMask = false;
            }

            if (!_inputMask)
                HandleScrolling();

            ArrangeEntries();

            if (_clearMouse)
                CheckButtons();

            if (Keyboard.GetState().IsKeyDown(Keys.C))
                _oldMouse = _oldMouse;

            if (Mouse.GetState().LeftButton == ButtonState.Released)
                LibState._clearMouse = true;

            _oldMouse = Mouse.GetState();
        }

        public void AddToGroup(Entry entry, string group)
        {
            foreach(Box g in _groups)
            {
                if (g.entries.Contains(entry))
                    g.entries.Remove(entry);
                else if (g.label.Equals(group))
                    g.entries.Add(entry);
            }
        }

        private void ArrangeEntries()
        {
            switch(_sortMode)
            {
                case SortMode.Alphabetic:
                    _entries.Sort();
                    StanagPositioning();
                    break;
                case SortMode.Autor:
                    _entries.Sort();
                    StanagPositioning();
                    break;
                case SortMode.Genre:
                    GenreSort();
                    break;
                case SortMode.Favs:
                    FavSort();
                    break;
            }
        }

        private void StanagPositioning()
        {
            int i = 0;
            foreach (Entry e in _entries)
            {
                e.SetPos(_positions[i] + _scrolling);

                ++i;
            }

            int rows = _entries.Count % 5 - 2;
            _scrollingBoundary = rows * -300 - 30;
        }

        private void GenreSort()
        {
            int i = 0;
            int j = 0;
            float seperator = 30;

            
            foreach (Box b in _genres)
            {
                b.SetPosition(_positions[j] + new Vector2(-60, i * seperator));

                ++i;

                foreach (Entry e in b.entries)
                {
                    e.SetPos(_positions[j] + _scrolling + new Vector2(0, i * seperator));

                    ++j;
                }

                if (j % 5 != 0)
                    j += 5 - j % 5;
            }

            _scrollingBoundary = -(int)((int)(j / 5) * 100 + i * seperator);
        }

        private void FavSort()
        {
            List<Box> newGroups = new List<Box>();
            //_groups.Add(new Box("Zuletzt gelesen"));

            _noOrder.entries.Clear();

            foreach (Entry e in _entries)
            {
                if (e._group.Equals("default"))
                {
                    if (e._fav)
                        _noOrder.entries.Add(e);
                    else
                        e.SetPos(new Vector2(-500));
                    continue;
                }

                bool exists = false;
                foreach (Box b in newGroups)
                {
                    if (b.label.Equals(e._group))
                    {
                        b.entries.Add(e);
                        exists = true;
                        break;
                    }
                }

                if (!exists)
                {
                    Box newBox = new Box(e._group);
                    newBox.entries.Add(e);
                    newGroups.Add(newBox);
                }
            }

            _groups = newGroups;
            _groups.Sort();
            _groups.Add(_noOrder);
            _groups.Reverse();
            _groups.Add(_lastRead);
            _groups.Reverse();

            int i = 0;
            int j = 0;
            float seperator = 30;

            foreach (Box b in _groups)
            {
                if (b.entries.Count == 0)
                    continue;

                b.SetPosition(_positions[j] + new Vector2(-60, i * seperator));

                ++i;

                foreach (Entry e in b.entries)
                {
                    e.SetPos(_positions[j] + _scrolling + new Vector2(0, i * seperator));

                    ++j;
                }

                if (j % 5 != 0)
                    j += 5 - j % 5;
            }

            _scrollingBoundary = -(int)((int)(j / 5) * 100 + i * seperator);
        }

        private void CheckButtons()
        {
            Vector2 mPos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

            if (Mouse.GetState().LeftButton == ButtonState.Pressed && _oldMouse.LeftButton == ButtonState.Released)
                _mouseInitiate = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

            bool hit = false;
            Vector2 nowPos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y) - _mouseInitiate;
            bool pressed = Mouse.GetState().LeftButton == ButtonState.Released && _oldMouse.LeftButton == ButtonState.Pressed &&
                nowPos.Length() < 10;

            foreach (Entry b in _entries)
            {
                if (!hit)
                {
                    hit = b.CheckHit(mPos);
                    if (hit && pressed)
                    {
                        b.CallFunction();
                        //LibState._scrolling = Vector2.Zero;
                    }
                }
                else
                    b.Mask();
            }

        }

        private void HandleScrolling()
        {
            MouseState mouse = Mouse.GetState();

            if (Mouse.GetState().Y > 160)
            {
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    _scrollStarted = true;

                    Vector2 move = new Vector2(_oldMouse.X, _oldMouse.Y) - new Vector2(mouse.X, mouse.Y);

                    _scrolling.Y -= move.Y;
                }
            }

            if (_scrollStarted && Mouse.GetState().LeftButton == ButtonState.Released)
                _scrollStarted = false;

            if (_scrolling.Y > 0)
                _scrolling = Vector2.Zero;

            int rows = _entries.Count % 5 - 2;
            if (_scrolling.Y < _scrollingBoundary)
                _scrolling.Y = _scrollingBoundary;
        }

        public List<string> GetGroups()
        {
            List<string> groups = new List<string>();

            foreach (Entry e in _entries)
            {
                bool found = false;
                if (e._group.Equals("default"))
                    continue;

                foreach (string g in groups)
                {
                    if (g.Equals(e._group))
                    {
                        found = true;
                        break;
                    }
                }
                if(!found)
                    groups.Add(e._group);
            }


            return groups;
        }

        public void AddLastRead(Entry e)
        {
            if (_lastRead.entries.Contains(e))
            {
                _lastRead.entries.Remove(e);

                _lastRead.entries.Reverse();
                _lastRead.entries.Add(e);
                _lastRead.entries.Reverse();
            }
            else
            {
                if (_lastRead.entries.Count >= 5)
                    _lastRead.entries.Remove(_lastRead.entries[4]);

                _lastRead.entries.Reverse();
                _lastRead.entries.Add(e);
                _lastRead.entries.Reverse();
            }
        }

        private class Box : IComparable<Box>
        {
            public Seperator sep;
            public List<Entry> entries;
            public string label;

            public Box(string _label)
            {
                label = _label;
                entries = new List<Entry>();
                sep = new Seperator(label, Vector2.Zero);
            }

            public void SetPosition(Vector2 pos)
            {
                sep.SetPos(pos);
            }

            public int CompareTo(Box other)
            {
                return this.label.CompareTo(other.label);
            }
        }

        private class Seperator
        {
            string label;
            private Vector2 pos;

            public Seperator(string _label, Vector2 _pos)
            {
                label = _label;
                pos = _pos;
            }

            public void SetPos(Vector2 _pos)
            {
                pos = _pos;
            }

            public void Draw(SpriteBatch sb)
            {
                sb.DrawString(_font, label, pos + _scrolling, StyleSheet.COLORS["GLOBAL_FONT_COLOR"]);
                Vector2 sepPos = pos + new Vector2(_font.MeasureString(label).X + 15, 3) + _scrolling;

                Rectangle sepRec = new Rectangle((int)sepPos.X, (int)sepPos.Y, 1220 - (int)sepPos.X, (int)_sepTex.Height);
                sb.Draw(_sepTex, sepRec, StyleSheet.COLORS["SECONDARY_COLOR"]);
            }
        }
    }
}
