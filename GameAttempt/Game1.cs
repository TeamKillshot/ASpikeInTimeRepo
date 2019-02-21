using Components;
using GameAttempt.Components;
using Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using TileEngine;

namespace GameAttempt
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        //SpriteBatch spriteBatch;

        ServiceManager serviceManager;
        Song bkgrnd;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.ApplyChanges();

            new InputManager(this);
            serviceManager = new ServiceManager(this);
            //player = new PlayerComponent(this);
            //tiles = new TRender(this);
        }

        protected override void Initialize()
        {
            //spriteBatch = new SpriteBatch(GraphicsDevice);
            //this.Services.AddService<SpriteBatch>(spriteBatch);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            bkgrnd = Content.Load<Song>("Audio/GreenGrave_mp3");

            if (bkgrnd != null)
            {
                MediaPlayer.Play(bkgrnd);
                MediaPlayer.Volume = .5f;
                MediaPlayer.IsRepeating = true;
            }

            #region Load Menu Textures and Scenes

            //menuTextures = Loader.ContentLoad<Texture2D>(Content, "MenuTextures");

            //play = new Menu(menuTextures["MenuTexture2_Play"], new Vector2(520, 300), Color.White, false);
            //scores = new Menu(menuTextures["MenuTexture3_HighScore"], new Vector2(520, 420), Color.White, false);
            //exit = new Menu(menuTextures["MenuTexture4_Exit"], new Vector2(520, 540), Color.White, false);

            //List<Menu> menuItems = new List<Menu>();

            ////add them to the list
            //menuItems.Add(play);
            //menuItems.Add(scores);
            //menuItems.Add(exit);

            ////create the Menu
            //menuScene = new Scene(menuTextures["MenuTexture1_Background"], menuItems);

            //_current = _activeScreen.MENU;
            //activeScene = menuScene;

            //_current = _activeScreen.PLAY;
            //activeScene = playScene;

            #endregion
        }
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            #region Update Menu

            //Vector2 mousePos = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            //IsMouseVisible = true;

            ////Rectangle over the play button to know when the button is clicked, changes the both screen state enum and the scene variable
            //Rectangle playButtonPos = play.Bounds;
            //if (playButtonPos.Contains(mousePos) && Mouse.GetState().LeftButton == ButtonState.Pressed)
            //{
            //    _current = _activeScreen.PLAY;
            //    activeScene = playScene;
            //}

            #endregion

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            #region Draw Menu
            //switch (_current)
            //{
            //    case _activeScreen.MENU:
            //        spriteBatch.Begin();
            //        activeScene.Draw(spriteBatch, playersList);
            //        spriteBatch.End();
            //        break;

            //    case _activeScreen.PLAY:
            //        spriteBatch.Begin();
            //        activeScene.Draw(spriteBatch, playersList);
            //        player.Draw(spriteBatch, playersList);
            //        spriteBatch.End();
            //        break;
            //}
            #endregion

            base.Draw(gameTime);
        }
    }
}
