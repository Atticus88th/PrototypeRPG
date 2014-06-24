using System;
using System.Collections.Generic;

namespace PrototypeRPG.Traits
{
	public enum Diplomacy
	{
		Allied,
		Neutral,
		Enemy
	}

	public class Player
	{
		public Actor PlayerActor;

		public Dictionary<Player, Diplomacy> Diplomacies = new Dictionary<Player, Diplomacy>();

		public Player()	{ }
	}
}
