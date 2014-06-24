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
		public SpriteBatch SpriteBatch { get; private set; }
		public List<Actor> Actors = new List<Actor>();
		public Player WorldPlayer;
		public Player HumanPlayer;

		public Actor WorldActor;

		public int TotalTickCount { get { return TickCount + TickRenderCount; } }
		public int TickCount { get; private set; }
		public int TickRenderCount { get; private set; }

		ContentManager content;
		Random random = new Random();

		public World(SpriteBatch spriteBatch, ContentManager content)
		{
			SpriteBatch = spriteBatch;
			this.content = content;

			WorldPlayer = new Player();
			WorldActor = new Actor();
			WorldPlayer.PlayerActor = WorldActor;

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
	
		public Actor CreateActor()
		{
			var newActor = new Actor();
			newActor.World = this;
			newActor.Owner = HumanPlayer;

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
			var newActor = new Actor();
			newActor.World = this;
			newActor.Owner = HumanPlayer;

			var health = new Health(100);

			var position = new Positionable(newActor);
			var vx = random.Next(0, 500);
			var vy = random.Next(0, 275);
			position.Position = new Vector2(vx, vy);

			var renderable = new Renderable(content.Load<Texture2D>("logo"));

			var keymovement = new KeyboardMovement();

			newActor.AddTrait(health);
			newActor.AddTrait(position);
			newActor.AddTrait(renderable);
			newActor.AddTrait(keymovement);

			return newActor;
		}

		void SetupWorld()
		{
			HumanPlayer = new Player();
			var pa = HumanPlayer.PlayerActor;
			pa = new Actor();
			pa.World = this;

			HumanPlayer.Diplomacies[WorldPlayer] = Diplomacy.Neutral;

			var keyInput = new KeyboardWorldInteraction(this);
			var mouseInput = new MouseWorldInteraction();

			pa.AddTrait(keyInput);
			pa.AddTrait(mouseInput);

			Actors.Add(pa);

			for (var i = 0; i < 3; i++)
				Actors.Add(CreateTestActor());
		}
	}
}
