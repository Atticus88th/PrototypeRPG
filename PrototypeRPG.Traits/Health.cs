using System;

namespace PrototypeRPG.Traits
{
	public class Health : ITrait, ITick, IMouseSelectable // IMouseSelectable is temporary here
	{
		public int MaxHP { get; private set; }
		public int CurrentHP { get; private set; }

		public Health(int maxHP)
		{
			MaxHP = maxHP;
			CurrentHP = maxHP;
			Console.WriteLine("Created a Health trait with a MaxHP of {0}.", MaxHP);
		}

		public void OnSelect(Actor self)
		{
			InflictDamage(self, 3);
		}

		public void InflictDamage(Actor self, int damage)
		{
			Console.WriteLine("Actor{0}'s health: {1}", self.ActorID, CurrentHP);

			if (self.IsDead)
			{
				Console.WriteLine("Returning!");
				return;
			}

			CurrentHP -= damage;
			Console.WriteLine("Actor{0}'s health: {1}", self.ActorID, CurrentHP);
		}

		public void Tick(Actor self) { }
	}
}

