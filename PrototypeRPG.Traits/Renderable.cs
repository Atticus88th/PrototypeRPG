using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PrototypeRPG.Traits
{
	public class Renderable : ITrait, ITickRender
	{
		public Vector2 RenderLocation = Vector2.Zero;

		public Rectangle BoundingBox;

		Texture2D texture;

		public Renderable(Texture2D texture)
		{
			this.texture = texture;
			BoundingBox = texture.Bounds;
		}

		public void TickRender(Actor self, SpriteBatch spriteBatch)
		{
			if (texture == null)
				return;

			spriteBatch.Draw(texture, RenderLocation, Color.White);
		}
	}
}
