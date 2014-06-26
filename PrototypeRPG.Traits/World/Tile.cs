using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PrototypeRPG.Traits
{
	public class Tile
	{
		public int TileType { get; private set; }

		public Tile(int type) { TileType = type; }

		public void Render(Texture2D texture, Rectangle destRect, Rectangle sourceRect, SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(texture, destRect, sourceRect, Color.White);
		}
	}
}
