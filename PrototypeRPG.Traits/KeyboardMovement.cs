using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace PrototypeRPG.Traits
{
	public class KeyboardMovement : ITrait, ITick
	{
		public Keys Up = Keys.W;
		public Keys Down = Keys.S;
		public Keys Left = Keys.A;
		public Keys Right = Keys.D;

		KeyboardState oldState;
		KeyboardState currentState;

		public KeyboardMovement() { }

		public Keys[] PollForInput()
		{
			currentState = Keyboard.GetState();

			if (currentState == oldState)
				return null;

			oldState = currentState;

			return currentState.GetPressedKeys();
		}

		void DoVisualMovement(Actor self, Vector2 vector2)
		{
			var position = self.TraitOrDefault<Positionable>();

			if (position != null)
				position.Position += vector2;
		}

		public void Tick(Actor self)
		{
			var keys = PollForInput();
			if (keys == null)
				return;

			if (keys.Contains(Up))
				DoVisualMovement(self, new Vector2(0, -4));

			if (keys.Contains(Down))
				DoVisualMovement(self, new Vector2(0, 4));

			if (keys.Contains(Left))
				DoVisualMovement(self, new Vector2(-4, 0));

			if (keys.Contains(Right))
				DoVisualMovement(self, new Vector2(4, 0));

			oldState = currentState;
		}
	}
}
