using System;

namespace PrototypeRPG.Traits
{
	public class Health : ITrait
	{
		public int MaxHP { get; private set; }
		public int CurrentHP { get; private set; }

		public Health(int maxHP)
		{
			MaxHP = maxHP;
			CurrentHP = maxHP;
		}

		public void InflictDamage(Actor self, int damage)
		{
			if (self.IsDead)
				return;

			CurrentHP -= damage;
		}
	}
}
