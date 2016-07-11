using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Library.Classes;

namespace Library
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public static GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static LibState _libState;
        public static ReadState _readState;
        State _curState;
        Texture2D _back;
        Texture2D _cursor;
        Header _header;
        static Game game;
        public static GameTime _gameTime;


        public Game1()
        {
            Game1.game = this;

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 800;
            graphics.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            _readState = new ReadState(this, spriteBatch);
            _libState = new LibState(this, spriteBatch);
            _libState.LoadContent(Content);
            _readState.LoadContent(Content);
            _curState = _libState;

            DetailView.LoadContent(Content, this);
            CustomizationView.LoadContent(Content);

            _header = new Header(spriteBatch, this);
            _header.LoadContent(Content);
            _cursor = Content.Load<Texture2D>("Layout/cursor");

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            _gameTime = gameTime;

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            if (!CustomizationView._show)
                DetailView.Update();
            
            CustomizationView.Update();
            _header.Update();

            if (!DetailView.Active && !CustomizationView._show)
            {
                _curState.Update();
            }

            base.Update(gameTime);
        }

        public void SetLibState()
        {
            _curState = _libState;
        }

        public void ShowReaderState(Entry entry)
        {
            _curState = _readState;
            _readState.ShowReader(entry);
        }

        public void ShowLibraryState()
        {
            _curState = _libState;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(StyleSheet.COLORS["PRIMARY_COLOR"]);

           
            _curState.Draw();
            DetailView.Draw(spriteBatch);
            CustomizationView.Draw(spriteBatch);

            _header.Draw();

            spriteBatch.Begin();
            
            Vector2 mPos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            spriteBatch.Draw(_cursor, mPos, new Color(255, 255, 255));
            
            spriteBatch.End();
            

            base.Draw(gameTime);
        }

        public static void ExitGame()
        {
            game.Exit();
        }
    }
}
