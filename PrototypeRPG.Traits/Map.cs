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

		public void LoadTilesFromArrays()
		{
			var loaded = new int[8][];
			loaded[0] = new int[8] { 0, 1, 1, 1, 1, 1, 1, 1 };
			loaded[1] = new int[8] { 8, 9, 9, 9, 9, 9, 9, 9 };
			loaded[2] = new int[8] { 16, 17, 18, 19, 20, 20, 22, 23 };
			loaded[3] = new int[8] { 24, 17, 26, 27, 28, 28, 30, 31 };
			loaded[4] = new int[8] { 32, 17, 34, 35, 36, 37, 38, 39 };
			loaded[5] = new int[8] { 40, 17, 42, 43, 44, 45, 43, 47 };
			loaded[6] = new int[8] { 48, 17, 50, 51, 52, 53, 54, 55 };
			loaded[7] = new int[8] { 56, 17, 58, 59, 60, 61, 62, 63 };

			for (var aY = 0; aY < loaded.Length; aY++)
				for (var aX = 0; aX < loaded[aY].Length; aX++)
					tiles2D[aX, aY] = new Tile(loaded[aY][aX]);
		}

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
