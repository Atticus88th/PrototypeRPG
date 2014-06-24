using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using PrototypeRPG.Traits;

namespace PrototypeRPG
{
	public class Actor
	{
		public bool IsDead
		{
			get
			{
				var health = this.TraitOrDefault<Health>();
				if (health == null)
					return true;

				return health.CurrentHP <= 0;
			}

			internal set { }
		}
		public int ActorID;
		public World World;

		List<object> traits = new List<object>();

		public Actor(World world) { World = world; }

		public void Kill()
		{
			IsDead = true;
		}

		public void AddTrait(ITrait trait)
		{
			traits.Add(trait);
		}

		public T TraitOrDefault<T>()
		{
			return traits.OfType<T>().FirstOrDefault();
		}

		public T Trait<T>()
		{
			return traits.OfType<T>().First();
		}

		public bool HasTrait<T>()
		{
			return traits.Exists(t => t is T);
		}

		public IEnumerable<T> TraitsImplementing<T>() where T : class
		{
			return traits.Select(t => t as T).Where(t => t != null);
		}
	}
}
