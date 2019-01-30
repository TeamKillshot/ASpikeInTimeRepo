using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine
{
	public class Tile
	{
		int tileWidth;
		int tileHeight;
		int id;
		int x;
		int y;
        Rectangle tileRect;
		TRef refs;
		bool passable;
		string tileName;

		public int ID
		{
			get { return id; }
			set { id = value; }
		}
		public bool Passable
		{
			get { return passable; }
			set { passable = value; }
		}
		public int TileWidth
		{
			get { return tileWidth; }
			set { tileWidth = value; }
		}
		public int TileHeight
		{
			get { return tileHeight; }
			set { tileHeight = value; }
		}
		public string TileName
		{
			get { return tileName; }
			set { tileName = value; }
		}
		public int X
		{
			get { return x; }
			set { x = value; }
		}
		public int Y
		{
			get { return y; }
			set { y = value; }
		}
		public TRef TRefs
		{
			get { return refs; }
			set { refs = value; }
		}
        public Rectangle getTile()
        {
            tileRect = new Rectangle(X, Y, tileWidth, tileHeight);
            return tileRect;
        }

	}
}
