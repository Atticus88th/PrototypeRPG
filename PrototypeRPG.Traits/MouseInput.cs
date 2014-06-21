using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PrototypeRPG.Traits
{
	public class MouseInput : ITrait, ITick
	{
		MouseState oldState;
		MouseState currentState;
		Point mousePosition { get { return new Point(currentState.X, currentState.Y); } }

		bool leftButton { get { return currentState.LeftButton == ButtonState.Pressed; } }
		bool rightButton { get { return currentState.RightButton == ButtonState.Pressed; } }

		public MouseInput() { }

		public void Tick(Actor self)
		{
			currentState = Mouse.GetState();

			var renderer = self.TraitOrDefault<Renderable>();
			if (renderer == null)
				return;

			var boundingBox = renderer.BoundingBox;

			if (leftButton && boundingBox.Contains(mousePosition))
			{
				Console.WriteLine("Trying to notify OnSelect()");

				var wow = self.TraitsImplementing<IMouseSelectable>();
				Console.WriteLine(wow.Count()); // Always 0. :(

				// TODO: fix actor.TraitsImplementing not finding any traits
				foreach (var notify in self.TraitsImplementing<IMouseSelectable>())
				{
					Console.WriteLine("IMouseSelectable found for {0}.", notify.GetType().Name);
					notify.OnSelect(self);
				}
			}

			oldState = currentState;
		}
	}
}