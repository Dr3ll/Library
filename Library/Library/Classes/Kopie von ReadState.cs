using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace Library.Classes
{
    class ReadSxtdgfghate : State
    {
        Entry _entry;
        SpriteBatch _sb;
        Rectangle _page;
        Texture2D _pageTex;
        Effect _markerEffect;

        VertexPositionColor[] vertices1;
        VertexPositionColor[] vertices2;

        List<Vector2>[] _marks;
        List<float[]> _buffer;

        Vector2 _active;

        int _markerCD = 0;
        int _selection = 0;

        int _markRad = 10;
        int _markRadSQ = 100;

        bool _oldMouse;


        public ReadState(Game1 game, SpriteBatch sb)
            : base(game)
        {
            _sb = sb;
            _page = new Rectangle(80, 180, 1120, 620);

            _marks = new List<Vector2>[3];
            _marks[0] = new List<Vector2>();
            _marks[1] = new List<Vector2>();
            _marks[2] = new List<Vector2>();

            _buffer = new List<float[]>();
            _active = Vector2.Zero;

            //Vector2 test1s = new Vector2(300, 300);
            //Vector2 test1d = new Vector2(300, 350);

            //Vector2 test2s = new Vector2(350, 400);
            //Vector2 test2d = new Vector2(500, 350);

            //Vector2 test3s = new Vector2(650, 500);
            //Vector2 test3d = new Vector2(200, 350);

            //_starts[0].Add(test1s);
            //_dirs[0].Add(test1d);
            //_starts[0].Add(test2s);
            //_dirs[0].Add(test2d);
            //_starts[0].Add(test2s);
            //_dirs[0].Add(test2d);


            vertices1 = new VertexPositionColor[3];
            vertices2 = new VertexPositionColor[3];

            vertices1[0].Position = new Vector3(-2.5f, -3.8f, 0f);
            vertices1[0].Color = Color.Red;
            vertices1[1].Position = new Vector3(-1.25f, -.3f, 0f);
            vertices1[1].Color = Color.Green;
            vertices1[2].Position = new Vector3(1.25f, -0.3f, 0f);
            vertices1[2].Color = Color.Blue;

            vertices2[0].Position = new Vector3(-2.53f, -3.79f, 0f);
            vertices2[0].Color = Color.Red;
            vertices2[1].Position = new Vector3(1.25f, -0.3f, 0f);
            vertices2[1].Color = Color.Green;
            vertices2[2].Position = new Vector3(2.59f, -3.85f, 0f);
            vertices2[2].Color = Color.Yellow;
        }

        private void ProduceMarks()
        {
            if (!_active.Equals(Vector2.Zero))
            {
                //Vector2 dir = _active - new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

                //Vector2 mark = new Vector2(
                //    (Mouse.GetState().Y - _active.Y) / (Mouse.GetState().X - _active.X),
                //    _active.Y - (_active.Y - Mouse.GetState().Y) / (_active.X - Mouse.GetState().X) * _active.X
                //    );


            }
            else
            {
                _active = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            }
        }

        private void CheckMarks()
        {
            

        }

        private void SwapMarkBuffer()
        {
            //_marks[_selection - 1].AddRange(_buffer);

            _buffer.Clear();

            _markerCD = 0;

            float m = (Mouse.GetState().Y - _active.Y) / (Mouse.GetState().X - _active.X);
            float n = _active.Y - (_active.Y - Mouse.GetState().Y) / (_active.X - Mouse.GetState().X) * _active.X;

            Vector2 mark = new Vector2(
                -(m / n),
                1 / n
                );

            _marks[_selection - 1].Add(mark);

            _active = Vector2.Zero;
        }

        public override void Update()
        {
            _selection = 1;

            if (Mouse.GetState().LeftButton == ButtonState.Pressed &&
                _selection != 0 &&
                _page.Contains(Mouse.GetState().X, Mouse.GetState().Y) && 
                 _markerCD > 0)
            {
                ProduceMarks();
                _markerCD = 0;
            }

            if (_markerCD <= 300)
            {
                _markerCD += 15;
            }

            if (Mouse.GetState().LeftButton == ButtonState.Released &&
                _oldMouse)
            {
                SwapMarkBuffer();
            }


            _oldMouse = Mouse.GetState().LeftButton == ButtonState.Pressed;

            #region Debug

            float inc = .005f;

            if (Keyboard.GetState().IsKeyDown(Keys.D1))
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                    vertices2[0].Position.Y += inc;
                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                    vertices2[0].Position.Y -= inc;
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    vertices2[0].Position.X += inc;
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    vertices2[0].Position.X -= inc;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D2))
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                    vertices2[1].Position.Y += inc;
                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                    vertices2[1].Position.Y -= inc;
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    vertices2[1].Position.X += inc;
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    vertices2[1].Position.X -= inc;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D3))
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                    vertices2[2].Position.Y += inc;
                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                    vertices2[2].Position.Y -= inc;
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    vertices2[2].Position.X += inc;
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    vertices2[2].Position.X -= inc;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D4))
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                    vertices2[3].Position.Y += inc;
                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                    vertices2[3].Position.Y -= inc;
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    vertices2[3].Position.X += inc;
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    vertices2[3].Position.X -= inc;
            }

            #endregion
        }

        public void ShowReader(Entry entry)
        {
            _entry = entry;
        }

        public void LoadContent(ContentManager cm)
        {
            _pageTex = cm.Load<Texture2D>("Layout/page");

            _markerEffect = cm.Load<Effect>("marker");
            _markerEffect.CurrentTechnique = _markerEffect.Techniques["Technique1"];
            _markerEffect.Parameters["xViewProjection"].SetValue(
                Matrix.CreateLookAt(new Vector3(0, 1, 2), new Vector3(0, 0, 1), new Vector3(0, 1, 0)) *
                Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, Game1.graphics.GraphicsDevice.Viewport.AspectRatio, 1.0f, 200.0f)
                );
        }

        public override void Draw()
        {
            //float[][] d = _marks[0].ToArray();

            _sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            _markerEffect.CurrentTechnique.Passes[0].Apply();
            _markerEffect.Parameters["marks"].SetValue
                (_marks[0].ToArray());
            _markerEffect.Parameters["max"].SetValue
                (_marks[0].Count);


            //_sb.Draw(_pageTex, _page, Color.White);

            Game1.graphics.GraphicsDevice
            .DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices1, 0, 1, VertexPositionColor.VertexDeclaration);
            Game1.graphics.GraphicsDevice
            .DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices2, 0, 1, VertexPositionColor.VertexDeclaration);

            _sb.End();
        }
    }
}
