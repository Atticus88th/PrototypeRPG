using System;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PrototypeRPG.Traits
{
	public class Map
	{
		public int TileSize;
		public int TilesInMapSquare;
		public World World { get; private set; }
		public Texture2D MapTileset;
		
		Tile[ , ] tiles2D;
		Random random = new Random();

		public Map(World world, int tileCount, int tileSize)
		{
			World = world;
			TilesInMapSquare = tileCount;
			TileSize = tileSize;

			tiles2D = new Tile[TilesInMapSquare, TilesInMapSquare];
		}

		public void DrawTiles()
		{
			for (var x = 0; x < tiles2D.GetLength(0); x++)
				for (var y = 0; y < tiles2D.GetLength(1); y++)
				{
					var tile = tiles2D[x,y];
					var destRect = new Rectangle(x * TileSize, y * TileSize, TileSize, TileSize);
					var sourceRect = World.GetSourceRectangleSquare(MapTileset, tile.TileType, TileSize, 0);

					tile.Render(MapTileset, destRect, sourceRect, World.SpriteBatch);
				}
		}

//		public void LoadTilesFromFile(string filename)
//		{
//			var fullText = File.ReadAllLines(filename);
//
//			var output = new int[fullText.Length][];
//
//			foreach (var fullLine in fullText)
//			{
//				var lineAsIntArray = fullLine.ToIntArray(',');
//				for (var intIndex = 0; intIndex < lineAsIntArray.Length; intIndex++)
//				{
//					Console.WriteLine(lineAsIntArray[intIndex]);
//
//					// No idea what should come next
//				}
//			}
//
//			for (var x = 0; x < tiles2D.GetLength(0); x++)
//			{
//				for (var y = 0; y < tiles2D.GetLength(1); y++)
//				{
//
//				}
//			}
//		}

		// TODO: Create from int[] where each index is a tileType
		public void CreateMapTiles()
		{
			var destRect = new Rectangle();
			var sourceRect = new Rectangle();

			for (var x = 0; x < tiles2D.GetLength(0); x++)
			{
				for (var y = 0; y < tiles2D.GetLength(1); y++)
				{
					var typeNum = random.Next(0, 63);
					sourceRect = World.GetSourceRectangleSquare(MapTileset, typeNum, TileSize, 0);
					destRect = new Rectangle(x * TileSize, y * TileSize, TileSize, TileSize);

					tiles2D[x,y] = new Tile(typeNum);
				}
			}
		}
	}
}
