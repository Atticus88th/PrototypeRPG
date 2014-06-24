using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PrototypeRPG.Traits
{
	public class Renderable : ITrait, ITickRender
	{
		// TODO: Separate render location from (need to add!)
		//			logical location
		public Vector2 RenderLocation = Vector2.Zero;

		public Rectangle BoundingBox;

		Texture2D texture;

		public Renderable(Texture2D texture)
		{
			this.texture = texture;

			// This is set to the bounds relative to the Window's 0,0
			// TODO: Set this to current location + bounds
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
