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
    class Header
    {
        Texture2D _back;
        Texture2D _backSec;
        Texture2D _buttonTex;
        Texture2D _buttonTexSec;
        Texture2D _all;
        Texture2D _author;
        Texture2D _favs;
        Texture2D _genre;
        Texture2D _shop;

        Button _customButton;

        List<MenuButton> _buttons;
        ExitButton _exit;
        const int SCREENWIDTH = 1280;
        const int HHEIGHT = 150;
        float _scale = 40;
        float _yVal = 20;
        SpriteBatch _sb;
        int _active = 3;
        Game1 _game;

        public Header(SpriteBatch sb, Game1 game)
        {
            _game = game;
            _sb = sb;
            _buttons = new List<MenuButton>();
        }

        public void LoadContent(ContentManager cm)
        {
            _buttonTex = cm.Load<Texture2D>("Layout/button_n");
            _buttonTexSec = cm.Load<Texture2D>("Layout/button_n_sec");
            _back = cm.Load<Texture2D>("Layout/h_back");
            _backSec = cm.Load<Texture2D>("Layout/h_back_sec");


            _exit = new ExitButton(cm.Load<Texture2D>("Layout/exit"), cm.Load<Texture2D>("Layout/exit_sec"), new Vector2(1230, 40), _scale);

            _customButton = new Button(new Rectangle(12, 6, 73, 68), cm.Load<Texture2D>("Layout/custom"), cm.Load<Texture2D>("Layout/custom_sec"), new Color(255, 255, 255), false);
            _customButton.Clicked += new EventHandler(HandleCustomClicked);


            float posStep = SCREENWIDTH / 7;
            float posIter = 0;
            posIter += 1.5f*posStep;

            for (int i = 0; i < 5; ++i)
            {
                _buttons.Add(new MenuButton(_buttonTex, _buttonTexSec,
                    (i == 0) ? cm.Load<Texture2D>("Layout/all") :
                    (i == 1) ? cm.Load<Texture2D>("Layout/author") :
                    (i == 2) ? cm.Load<Texture2D>("Layout/favs") :
                    (i == 3) ? cm.Load<Texture2D>("Layout/genre") :
                    cm.Load<Texture2D>("Layout/shop")
                    , cm.Load<Texture2D>("Layout/button_big"), cm.Load<Texture2D>("Layout/button_big_sec"), new Vector2(posIter, _yVal), _scale, i + 1));


                posIter += posStep;
            }
        }

        public void Update()
        {
            Vector2 mPos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

            Arrange();
            _buttons.Reverse();

            bool hit = false;
            bool pressed = Mouse.GetState().LeftButton == ButtonState.Pressed;
            foreach (MenuButton b in _buttons)
            {
                if (!hit)
                {
                    hit = b.CheckHit(mPos);
                    if (hit && pressed && !LibState._scrollStarted)
                    {
                        b.CallFunction();
                        _game.SetLibState();

                        _active = b._index;
                        LibState._scrolling = Vector2.Zero;
                    }
                }
                else
                    b.Mask();
            }

            _buttons.Reverse();

            if (_exit.CheckHit(mPos) && pressed)
                _exit.CallFunction();

            _customButton.Update();
        }

        private void HandleCustomClicked(object sender, EventArgs args)
        {

            CustomizationView.ShowCustom();

        }

        private void Arrange()
        {
            List<MenuButton> temp = new List<MenuButton>();

            int c = _buttons.Count;
            int iAc = 0;
            while (temp.Count < c)
            {
                if (_buttons.Count > 4)
                {
                    temp.Add(FetchActive(out iAc));
                }
                temp.Add(FetchNext(iAc));
            }

            temp.Reverse();
            _buttons = temp;

            _buttons[0].Active = false;
            _buttons[1].Active = false;
            _buttons[2].Active = false;
            _buttons[3].Active = false;
            _buttons[4].Active = true;

        }

        private MenuButton FetchActive(out int index)
        {
            List<MenuButton> temp = new List<MenuButton>(_buttons);
            MenuButton res = null;

            foreach (MenuButton b in temp)
            {
                if (b._index.Equals(_active))
                {
                    res = b;
                    _buttons.Remove(b);
                }
            }

            index = res._index;

            return res;
        }

        private MenuButton FetchNext(int iAc)
        {
            List<MenuButton> temp = new List<MenuButton>(_buttons);
            MenuButton res = null;
            int min = int.MaxValue;

            foreach (MenuButton b in temp)
            {
                if (Math.Abs(iAc - b._index) < min )
                {
                    res = b;
                    min = Math.Abs(iAc - b._index);
                }
            }

            if (res == null)
                throw new Exception();

            _buttons.Remove(res);

            return res;
        }

        public void Draw()
        {
            _sb.Begin();

            _sb.Draw(_back, Vector2.Zero, StyleSheet.COLORS["PRIMARY_COLOR"]);
            _sb.Draw(_backSec, Vector2.Zero, StyleSheet.COLORS["SECONDARY_COLOR"]);

            foreach (MenuButton b in _buttons)
            {
                b.Draw(_sb);
            }

            _exit.Draw(_sb);

            _customButton.Draw(_sb);

            _sb.End();
        }
    }
}
