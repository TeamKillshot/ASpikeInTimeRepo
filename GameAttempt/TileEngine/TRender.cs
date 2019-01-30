
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
        ServiceManager serviceManager;
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
	
		public int tsRows = 11;					// how many sprites in a column
		public int tsColumns = 8;                  // how many Sprites in a Row
		
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
			{   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  1,  1,  1,  0,  0,  0,  0,  },
			{   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  1,  2,  2,  2,  1,  0,  0,  0,  },
			{   1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  0,  0,  0,  0,  0,  1,  1,  1,  1,  1,  1,  1,  1,  1,  2,  2,  2,  2,  2,  1,  1,  1,  },
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
										  ("TileSheetOne");    // get TileSheet

			// create a new tile from the TileSheet in list (locX, locY, IndexNum)
			tRefs.Add(new TRef(0, 9, 0));   // blank space
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
            serviceManager = new ServiceManager(Game);
            base.Initialize();
		}

		public void SetupCollison()
		{
			foreach (Tile t in tileManager.ActiveLayer.Impassable)
			{
				collisons.Add(new Collider(Game.Content.Load<Texture2D>("Collison"),
							  new Vector2(t.X * t.TileWidth/2, t.Y * t.TileHeight/2), 
							  new Vector2(t.TileWidth/2, t.TileHeight/2)));
			}

		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
            SpriteBatch spriteBatch = serviceManager.GetSpritebatchServices();
            Camera Cam = serviceManager.GetCameraService();
			
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Cam.CurrentCamTranslation);

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
