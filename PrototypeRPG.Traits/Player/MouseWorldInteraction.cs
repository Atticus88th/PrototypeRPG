using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PrototypeRPG.Traits
{
	public class MouseWorldInteraction : ITrait, ITick
	{
		MouseState oldState;
		MouseState currentState;
		Point position { get { return new Point(currentState.X, currentState.Y); } }

		bool leftClick { get { return currentState.LeftButton == ButtonState.Pressed; } }
		bool rightClick { get { return currentState.RightButton == ButtonState.Pressed; } }
		bool newState { get { return currentState != oldState; } }

		public MouseWorldInteraction() { }

		public void Tick(Actor self)
		{
			currentState = Mouse.GetState();

			// TODO: This allows "dragging", force single click (with possible time delay?)
			if (!newState)
				return;

			var clicked = self.World.GetActorAtLocation(position);

			if (leftClick)
			{
				if (clicked == null)
					return;

				foreach (var notify in clicked.TraitsImplementing<IMouseInteraction>())
					notify.OnLeftClick(clicked);
			}

			if (rightClick)
			{
				if (clicked == null)
					return;

				foreach (var notify in clicked.TraitsImplementing<IMouseInteraction>())
					notify.OnRightClick(clicked);
			}

			oldState = currentState;
		}
	}
}
