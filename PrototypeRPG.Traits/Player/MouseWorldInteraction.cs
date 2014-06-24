using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PrototypeRPG.Traits
{
	public class MouseWorldInteraction : ITrait, ITick
	{
		MouseState oldState;
		MouseState currentState;
		Point mousePosition { get { return new Point(currentState.X, currentState.Y); } }

		bool leftButton { get { return currentState.LeftButton == ButtonState.Pressed; } }
		bool rightButton { get { return currentState.RightButton == ButtonState.Pressed; } }

		public MouseWorldInteraction() { }

		public void Tick(Actor self)
		{
			currentState = Mouse.GetState();

			var actorAtMouse = self.World.GetActorAtLocation(mousePosition);

			if (leftButton && actorAtMouse != null)
				foreach (var notify in actorAtMouse.TraitsImplementing<IMouseSelectable>())
					notify.OnSelect(actorAtMouse);

			oldState = currentState;
		}
	}
}
