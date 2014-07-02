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

		public int MovementSpeed { get; private set; }

		KeyboardState oldState;
		KeyboardState currentState;
		bool newState { get { return currentState != oldState; } }

		public KeyboardMovement(int moveSpeed) { MovementSpeed = moveSpeed; }

		public Keys[] PollForInput()
		{
			currentState = Keyboard.GetState();

			return currentState.GetPressedKeys();
		}

		void DoVisualMovement(Actor self, Vector2 vector2)
		{
			var position = self.TraitOrDefault<Positionable>();

			if (position != null)
				position.Position += vector2;
		}

		void UpdateSprite(Actor self, string animID)
		{
			// TODO: Need a way to guarantee traits
			var r = self.TraitOrDefault<Renderable>();
			if (r == null)
				throw new NullReferenceException("No Renderable for Actor{0}".F(self.ActorID));

			r.Sprite.SetAnimation(animID);
		}

		public void Tick(Actor self)
		{
			var keys = PollForInput();
			if (keys == null)
				return;

			if (keys.Contains(Up))
			{
				DoVisualMovement(self, new Vector2(0, -MovementSpeed));
				UpdateSprite(self, "up");
			}

			if (keys.Contains(Down))
			{
				DoVisualMovement(self, new Vector2(0, MovementSpeed));
				UpdateSprite(self, "down");
			}

			if (keys.Contains(Left))
			{
				DoVisualMovement(self, new Vector2(-MovementSpeed, 0));
				UpdateSprite(self, "left");
			}

			if (keys.Contains(Right))
			{
				DoVisualMovement(self, new Vector2(MovementSpeed, 0));
				UpdateSprite(self, "right");
			}

			oldState = currentState;
		}
	}
}
