using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace PrototypeRPG.Traits
{
	public class World
	{
		public const int TickTimestep = 40;

		public SpriteBatch SpriteBatch { get; private set; }
		public List<Actor> Actors = new List<Actor>();
		public Actor PlayerActor;

		public int TotalTickCount { get { return TickCount + TickRenderCount; } }
		public int TickCount { get; private set; }
		public int TickRenderCount { get; private set; }

		ContentManager content;
		Random random = new Random();

		public World(SpriteBatch spriteBatch, ContentManager content)
		{
			SpriteBatch = spriteBatch;
			this.content = content;

			SetupWorld();
		}

		public void Tick()
		{
			TickCount++;

			foreach (var actor in Actors.ToList())
				foreach (var trait in actor.TraitsImplementing<ITick>())
					trait.Tick(actor);
		}

		public void TickRender()
		{
			TickRenderCount++;

			foreach (var actor in Actors.ToList())
				foreach (var trait in actor.TraitsImplementing<ITickRender>())
					trait.TickRender(actor, SpriteBatch);
		}
	
		public Actor CreateActor(params ITrait[] traits)
		{
			var newActor = new Actor(this);

			foreach (var trait in traits)
				newActor.AddTrait(trait);

			Actors.Add(newActor);

			return newActor;
		}

		public Actor GetActorAtLocation(Point point)
		{
			return Actors.Where(a => a.HasTrait<Renderable>() &&
				a.Trait<Renderable>().BoundingBox.Contains(point)).FirstOrDefault();
		}

		public Actor CreateTestActor()
		{
			var newActor = new Actor(this);

			var health = new Health(100);

			var renderable = new Renderable(content.Load<Texture2D>("logo"));
			var vx = random.Next(0, 200);
			var vy = random.Next(0, 200);
			renderable.RenderLocation = new Vector2(vx, vy);

			var keymovement = new KeyboardMovement();

			newActor.AddTrait(health);
			newActor.AddTrait(renderable);
			newActor.AddTrait(keymovement);

			return newActor;
		}

		void SetupWorld()
		{
			PlayerActor = new Actor(this);
			var keyInput = new KeyboardWorldInteraction(this);
			var mouseInput = new MouseWorldInteraction();

			PlayerActor.AddTrait(keyInput);
			PlayerActor.AddTrait(mouseInput);

			Actors.Add(PlayerActor);

			for (var i = 0; i < 3; i++)
				Actors.Add(CreateTestActor());
		}
	}
}
