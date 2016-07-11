using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Library.Classes
{
    interface ScrollboxItem
    {
        void Draw(SpriteBatch sb, Vector2 scrolling);
        int Height();
    }
}
