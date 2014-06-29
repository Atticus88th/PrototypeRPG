using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PrototypeRPG.Traits
{
	public class Renderable : ITrait, ITickRender
	{
		public Rectangle Boundingbox;
		public Rectangle SourceRect;
		public Texture2D Texture { get; private set; }

		public Renderable(Texture2D texture, Rectangle boundingBox, Rectangle sourceRect)
		{
			Texture = texture;
			SourceRect = sourceRect;

			// This is set to the bounds relative to the Window's 0,0
			// TODO: Set this to current location + bounds
			Boundingbox = boundingBox;
		}

		public void UpdateBoundingBox(int x, int y)
		{
			Boundingbox = new Rectangle(x, y, Boundingbox.Width, Boundingbox.Height);
		}

		public void TickRender(Actor self, Rectangle destRect, Rectangle sourceRect, SpriteBatch spriteBatch)
		{
			if (self.IsDead)
				return;

			if (Texture == null)
				throw new ArgumentNullException("No texture provided for Actor{0} to render!".F(self.ActorID));

			var position = self.TraitOrDefault<Positionable>();
			if (position == null)
				throw new ArgumentNullException("No position trait for Actor{0}!".F(self.ActorID));

			spriteBatch.Draw(Texture, destRect, sourceRect, Color.White);
		}
	}
}
