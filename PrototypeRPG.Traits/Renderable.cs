using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PrototypeRPG.Traits
{
	public class Renderable : ITrait, ITickRender
	{
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
			if (self.IsDead)
				return;

			if (texture == null)
				throw new ArgumentNullException("No texture provided for Actor{0} to render!".F(self.ActorID));

			var position = self.TraitOrDefault<Positionable>();

			if (position == null)
				throw new ArgumentNullException("No position trait for Actor{0}!".F(self.ActorID));

			spriteBatch.Draw(texture, position.Position, Color.White);
			spriteBatch.Draw(texture, BoundingBox, Color.Red);
		}
	}
}
