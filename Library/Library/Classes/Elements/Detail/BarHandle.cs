using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Library.Classes
{
    class BarHandle : Button
    {
        Vector2 _topPos;
        Vector2 _anchor;

        public int MarkPos = 0;
        public int InMarkHeight = 0;
        public int AcMarkHeight = 0;

        public BarHandle(Texture2D tex)
            : base(new Rectangle(1190, 0, 48, 155), tex, tex, new Color(255, 255, 255))
        {
            _topPos = new Vector2(1190, 261);

            _anchor = Vector2.Zero;
        }

        public override void Update()
        {
            if (Mouse.GetState().LeftButton == ButtonState.Released)
                _anchor = Vector2.Zero;


            if (!_anchor.Equals(Vector2.Zero))
            {
                int dist = Mouse.GetState().Y - (int)_anchor.Y;
                 
                if (dist > InMarkHeight)
                {
                    MarkPos++;

                    _anchor = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
                }
                if (-dist > InMarkHeight && MarkPos > 0)
                {
                    MarkPos--;

                    _anchor = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
                }
            }


            base.Update();
        }

        public override void Draw(SpriteBatch sb)
        {
            Vector2 drawPos = _topPos + new Vector2(0, MarkPos * InMarkHeight);
            
            base.SetPos(drawPos);

            base.Draw(sb);
        }

        protected override void OnHit()
        {
            _anchor = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
        }

    }
}
