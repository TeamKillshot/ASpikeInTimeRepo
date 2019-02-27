using GameAttempt;
using GameAttempt.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Managers
{
    public class ServiceManager : GameComponent
    {
        Camera camera;
        PlayerComponent player;
        TRender tiles;
        SpriteBatch spriteBatch;

        public ServiceManager(Game game) : base(game)
        {
            int MapDebug = 1;
            spriteBatch = new SpriteBatch(game.GraphicsDevice);
            tiles = new TRender(game);
            if (MapDebug == 1)
            {
                camera = new Camera(Vector2.Zero, 
                         new Vector2(tiles.tileMap.GetLength(1) * tiles.tsWidth * 2, 
                         tiles.tileMap.GetLength(0)) * tiles.tsHeight,
                         tiles.GraphicsDevice.Viewport);
            }
            else
            {
                camera = new Camera(Vector2.Zero,
                    new Vector2(tiles.tileMap.GetLength(1) * tiles.tsWidth,
                     tiles.tileMap.GetLength(0) * tiles.tsHeight),
                        tiles.GraphicsDevice.Viewport);
            }
            
            player = new PlayerComponent(game);

            AddToServices();
        }

        public void AddToServices()
        {
            Game.Services.AddService<Camera>(camera);           
            Game.Services.AddService<SpriteBatch>(spriteBatch);
            Game.Services.AddService<PlayerComponent>(player);
            Game.Services.AddService<TRender>(tiles);
        }

        public Camera GetCameraService()
        {
            return Game.Services.GetService<Camera>();
        }

        public PlayerComponent GetPlayerService()
        {
            return Game.Services.GetService<PlayerComponent>();
        }

        public TRender GetTRenderService()
        {
            return Game.Services.GetService<TRender>();
        }

        public SpriteBatch GetSpritebatchServices()
        {
            return Game.Services.GetService<SpriteBatch>();
        }
    }
}
