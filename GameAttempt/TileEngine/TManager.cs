using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine
{
	public class TManager
	{
		List<TLayer> layers = new List<TLayer>();
		TLayer activeLayer;
		Tile currentTile;

		public TLayer ActiveLayer
		{
			get { return activeLayer; }
			set { activeLayer = value; }
		}
		public Tile CurrentTile
		{
			get { return currentTile; }
			set { currentTile = value; }
		}

		public TLayer MakeLayer(string layerName, 
								string[] tileNames, 
								int[,] tMap,
								List<TRef> refs,
								int tileWidth, 
								int tileHeight)
		{
			int tileMapHeight = tMap.GetLength(0);	// variable for y component
			int tileMapWidth = tMap.GetLength(1);   // variable for x component
			TLayer layer = new TLayer();
			layer.Tiles = new Tile[tileMapHeight, tileMapWidth];
			layer.TRefs = refs;
			layer.LayerName = layerName;
			layer.TileWidth = tileWidth;
			layer.TileHeight = tileHeight;

			for (int x = 0; x < tileMapWidth; x++)
				for (int y = 0; y < tileMapHeight; y++)
				{
					layer.Tiles[y, x] = new Tile
					{
						X = x,
						Y = y,
						ID = tMap[y, x],
						TileName = tileNames[tMap[y, x]],
						TileWidth = layer.TileWidth,
						TileHeight = layer.TileHeight,
						TRefs = layer.TRefs[tMap[y, x]]

					};
				}

			return layer;
			
		}

		public void addLayer(string layerName,
							 string[] tileName,
							 int[,] tMap,
							 List<TRef> refs,
							 int tileWidth,
							 int tileHeight)
		{
			layers.Add(MakeLayer(layerName, tileName, tMap, refs, tileWidth, tileHeight));
		}

		public TLayer GetLayer(string Name)
		{
			foreach (TLayer layer in layers)
			{
				if (layer.LayerName == Name)
					return layer;
				
			}

			return null;
		}

		
		
	}
}
