using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace PrototypeRPG.Traits
{
	public class KeyboardWorldInteraction : ITrait, ITick
	{
		Keys CreateActor = Keys.P;
		Keys ReloadTiles = Keys.R;
		KeyboardState oldState;
		KeyboardState currentState;

		World world;

		public KeyboardWorldInteraction(World world)
		{
			this.world = world;
		}

		public Keys[] PollForInput()
		{
			currentState = Keyboard.GetState();

			if (currentState == oldState)
				return null;

			oldState = currentState;

			return currentState.GetPressedKeys();
		}

		public void Tick(Actor self)
		{
			var keys = PollForInput();
			if (keys == null)
				return;

			if (keys.Contains(CreateActor))
			{
				var newActor = world.CreateTestActor();
				world.Actors.Add(newActor);
				return;
			}

			if (keys.Contains(ReloadTiles))
			{
				world.Map.CreateMapTiles();
				return;
			}
		}
	}
}
