
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
			{   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  1,  1,  1,  0,  0,  0,  0,  },
			{   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  1,  2,  2,  2,  1,  0,  0,  0,  },
			{   9,  9,  9,  9,  1,  1,  1,  1,  1,  1,  0,  0,  0,  0,  0,  1,  1,  1,  1,  1,  1,  1,  1,  1,  2,  2,  2,  2,  2,  1,  1,  1,  },
			{   9,  9,  9,  9,  2,  2,  2,  2,  2,  2,  1,  0,  0,  0,  1,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  },
			{   9,  9,  9,  9,  2,  2,  2,  2,  2,  2,  2,  1,  1,  1,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  },
			{   9,  9,  9,  9,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  },
			{   9,  9,  9,  9,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  },
			{   9,  9,  9,  9,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  },
			{   9,  9,  9,  9,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  },
			{   1,  1,  1,  1,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  },

		};

		
		#endregion 

		public TRender(Game game) : base(game)
		{
           
			game.Components.Add(this);
			
			tileManager = new TManager();
			tSheet = Game.Content.Load<Texture2D>
										  ("Sprites/TileSheet3");    // get TileSheet
            bgText = Game.Content.Load<Texture2D>("Sprites/BackgroundWater");
            // create a new tile from the TileSheet in list (locX, locY, IndexNum)
            // Blank Space
            tRefs.Add(new TRef(0, 15, 0));   // blank space
           
            // Level One tiles
            tRefs.Add(new TRef(0, 0, 1 ));   // 1 Ground with grass			 
            tRefs.Add(new TRef(1, 0, 2 ));   // 2 Left Rounded Ground  
            tRefs.Add(new TRef(2, 0, 3 ));   // 3 Right Rounded Ground 
            tRefs.Add(new TRef(3, 0, 4 ));   // 4 Rounded Ground
            tRefs.Add(new TRef(4, 0, 5 ));   // 5 Right Slanted Edge
            tRefs.Add(new TRef(5, 0, 6 ));   // 6 Left Slanted Edge
            tRefs.Add(new TRef(6, 0, 7 ));   // 7 Left Slanted Ground
            tRefs.Add(new TRef(7, 0, 8 ));   // 8 Right Slanted Ground
            tRefs.Add(new TRef(0, 1, 9 ));   // 9 Ground without grass
            tRefs.Add(new TRef(1, 1, 10 ));   // 10 Left Curved Ground
            tRefs.Add(new TRef(2, 1, 11 ));   // 11 Right Curved Ground
            tRefs.Add(new TRef(3, 1, 12 ));   // 12 Thin Square 
            tRefs.Add(new TRef(4, 1, 13 ));   // 13 Rounded Thin Square  
            tRefs.Add(new TRef(5, 1, 14 ));   // 14 Right Rounded Thin Square  
            tRefs.Add(new TRef(6, 1, 15 ));   // 15 Left Rounded Thin Square

            //// Level Two Tiles
            //tRefs.Add(new TRef(0, 2, 16));   // 16 Ground with grass			 
            //tRefs.Add(new TRef(1, 2, 17));   // 17 Left Rounded Ground  
            //tRefs.Add(new TRef(2, 2, 18));   // 18 Right Rounded Ground 
            //tRefs.Add(new TRef(3, 2, 19));   // 19 Rounded Ground
            //tRefs.Add(new TRef(4, 2, 20));   // 20 Right Slanted Edge
            //tRefs.Add(new TRef(5, 2, 21));   // 21 Left Slanted Edge
            //tRefs.Add(new TRef(6, 2, 22));   // 22 Left Slanted Ground
            //tRefs.Add(new TRef(7, 2, 23));   // 23 Right Slanted Ground
            //tRefs.Add(new TRef(0, 3, 24));   // 24 Ground without grass
            //tRefs.Add(new TRef(1, 3, 25));   // 25 Left Curved Ground
            //tRefs.Add(new TRef(2, 3, 26));   // 26 Right Curved Ground
            //tRefs.Add(new TRef(3, 3, 27));   // 27 Thin Square 
            //tRefs.Add(new TRef(4, 3, 28));   // 28 Rounded Thin Square  
            //tRefs.Add(new TRef(5, 3, 29));   // 29 Right Rounded Thin Square  
            //tRefs.Add(new TRef(6, 3, 30));   // 30 Left Rounded Thin Square


            string[] tNames = { "Empty", "GroundG", "LRGround",
                                "RRGround", "RGround", "RSEdge",
                                "LSEdge", "LSGround", "RSGround",
                                "GroundNG", "LCEdge", "RCEdge",
                                "SPlat", "RPLat", "RRPLat",
                                "LRPlat" }; // names of tiles
			
			string[] impassableTiles = { "GroundG", "LRGround",
                                         "RRGround", "RGround", "RSEdge",
                                         "LSEdge", "LSGround", "RSGround",
                                         "LCEdge", "RCEdge","SPlat",
                                         "RPLat", "RRPLat","LRPlat" };
                                         

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

        public bool SideCollision()
        {
            Player = Game.Services.GetService<PlayerComponent>();

            foreach (Collider c in collisons)
            {
                if (Player.Bounds.Left >= c.collider.Right || Player.Bounds.Right <= c.collider.Left)
                {
                    return true;
                }
            }
            return false;
        }

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
