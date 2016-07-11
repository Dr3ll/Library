using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Library.Classes
{
    public abstract class State
    {
        Game1 _game;

        public State(Game1 game)
        {
            _game = game;
        }

        public abstract void Update();
        public abstract void Draw();
    }
}
