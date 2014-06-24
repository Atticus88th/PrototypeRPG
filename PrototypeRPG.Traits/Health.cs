using System;

namespace PrototypeRPG.Traits
{
	public class Health : ITrait, IMouseSelectable
	{
		public int MaxHP { get; private set; }
		public int CurrentHP { get; private set; }

		public Health(int maxHP)
		{
			MaxHP = maxHP;
			CurrentHP = maxHP;
		}

		public void OnSelect(Actor self)
		{
			if (self.IsDead)
				return;

			InflictDamage(self, MaxHP);
		}

		public void InflictDamage(Actor self, int damage)
		{
			if (self.IsDead)
				return;

			CurrentHP -= damage;
			Console.WriteLine("Actor{0}'s health: {1}", self.ActorID, CurrentHP);
		}
	}
}
