using System;
using Microsoft.Xna.Framework;

namespace PrototypeRPG.Traits
{
	public class Positionable : ITrait
	{
		Vector2 position;

		public Vector2 Position
		{
			get { return position; }
			set
			{
				var renderer = self.TraitOrDefault<Renderable>();
				if (renderer != null)
					renderer.UpdateBoundingBox((int)value.X, (int)value.Y);

				position = value;
			}
		}

		Actor self;

		public Positionable(Actor self) { this.self = self; }
	}
}
