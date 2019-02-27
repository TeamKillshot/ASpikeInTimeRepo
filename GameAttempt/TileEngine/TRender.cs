
using GameAttempt.Components;
using Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine;

namespace GameAttempt
{
	
	public class TRender : DrawableGameComponent
	{
		#region Properties
		TManager tileManager;
		Texture2D tSheet;
        Texture2D bgText;
        ServiceManager serviceManager;
        PlayerComponent Player;
        public SpriteEffects effect;
		//Camera camera;

		Vector2 ViewportCentre
		{
			get
			{
				return new Vector2(GraphicsDevice.Viewport.Width / 2,
				GraphicsDevice.Viewport.Height / 2);
			}
		}

		public List<Collider> collisons = new List<Collider>();	
		List<TRef> tRefs = new List<TRef>();

		public int tsWidth;						// gets the width of tSheet
		public int tsHeight;                       // gets teh height of tSheet
	
		public int tsRows = 15;					// how many sprites in a column
		public int tsColumns = 11;                  // how many Sprites in a Row
		
		public int scale = 2;

		public int[,] tileMap = new int[,]
		{
			{   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  },
			{   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  },
			{   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  },
			{   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  },
			{   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  },
			{   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  },
			{   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  },
			{   0,  0,  0,  0,  0,  1,  1,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  1,  1,  1,  0,  0,  0,  0,  },
			{   0,  0,  0,  0,  1,  2,  2,  1,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  1,  2,  2,  2,  1,  0,  0,  0,  },
			{   1,  1,  1,  1,  2,  2,  2,  2,  1,  1,  0,  0,  0,  0,  0,  1,  1,  1,  1,  1,  1,  1,  1,  1,  2,  2,  2,  2,  2,  1,  1,  1,  },
			{   2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  1,  0,  0,  0,  1,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  },
			{   2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  1,  1,  1,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  },
			{   2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  },
			{   2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  },
			{   2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  },
			{   2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  },
			{   2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  },

		};

		
		#endregion 

		public TRender(Game game) : base(game)
		{
           
			game.Components.Add(this);
			
			tileManager = new TManager();
			tSheet = Game.Content.Load<Texture2D>
										  ("Sprites/TileSheet3");    // get TileSheet
            bgText = Game.Content.Load<Texture2D>("Backgrounds/DinoParkBackgroundFinal");
			// create a new tile from the TileSheet in list (locX, locY, IndexNum)
			tRefs.Add(new TRef(0, 15, 0));   // blank space
			tRefs.Add(new TRef(0, 0, 1));   // Ground with grass
			tRefs.Add(new TRef(0, 1, 2));   // Ground 

			string[] tNames = { "Empty", "Ground1", "Ground2"}; // names of tiles
			
			string[] impassableTiles = { "Ground1", "Ground2" };

			tsWidth = tSheet.Width / tsColumns;					// gets Width of tiles
			tsHeight = tSheet.Height / tsRows;                  // gets Height of tiles
			

			// creates Layer of Ground
			tileManager.addLayer("Background", tNames, 
								 tileMap, tRefs, tsWidth, tsHeight);
			
			// sets Ground as Active Layer
			tileManager.ActiveLayer = tileManager.GetLayer("Background");
			

			// Creates a set of impassable tiles
			tileManager.ActiveLayer.makeImpassable(impassableTiles);

			// sets the current tile
			tileManager.CurrentTile = tileManager.ActiveLayer.Tiles[0, 0];

			//Sets Collison tiles
			SetupCollison();
		}

		public override void Initialize()
		{
            //serviceManager = new ServiceManager(Game);
            //Player = new PlayerComponent(Game);
            base.Initialize();
		}

		public void SetupCollison()
		{
			foreach (Tile t in tileManager.ActiveLayer.Impassable)
			{
				collisons.Add(new Collider(Game.Content.Load<Texture2D>("Sprites/Collison"),
							  new Vector2(t.X * t.TileWidth/2, t.Y * t.TileHeight/2), 
							  new Vector2(t.TileWidth/2, t.TileHeight/2)));
			}

		}

        public bool BottomCollision()
        {
            Player = Game.Services.GetService<PlayerComponent>();

            foreach (Collider c in collisons)
            {
                if (Player.Bounds.Bottom > c.collider.Top)
                {
                    return true;
                }
            }
            return false;
        }

        //public bool SideCollision()
        //{
        //    Player = Game.Services.GetService<PlayerComponent>();

        //    foreach (Collider c in collisons)
        //    {
        //        if (Player.Bounds.Left >= c.collider.Right || Player.Bounds.Right <= c.collider.Left)
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}

        public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
            SpriteBatch spriteBatch = Game.Services.GetService<SpriteBatch>();
            Camera Cam = Game.Services.GetService<Camera>();

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Cam.CurrentCamTranslation);
            spriteBatch.Draw(bgText, new Rectangle(Cam.View.X, Cam.View.Y, tsWidth * tileMap.GetLength(0), 24 * tileMap.GetLength(1)), Color.White);

			foreach (Tile t in tileManager.ActiveLayer.Tiles)
			{
				Vector2 position = new Vector2(t.X * t.TileWidth/2,
											   t.Y * t.TileHeight/2);

				spriteBatch.Draw(tSheet, new Rectangle(position.ToPoint(),
													   new Point(t.TileWidth/2,
													   t.TileHeight/2)),


										 new Rectangle((t.TRefs.TLocX * t.TileWidth),
													   (t.TRefs.TLocY * t.TileHeight),
													   t.TileWidth,
													   t.TileHeight),
										 Color.White);

			}
			foreach (var item in collisons)
				item.draw(spriteBatch);


			spriteBatch.End();
			base.Draw(gameTime);
		}
		
	}
}
