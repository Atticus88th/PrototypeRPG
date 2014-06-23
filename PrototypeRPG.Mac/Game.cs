using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PrototypeRPG.Traits;

namespace PrototypeRPG
{
	public class Game : Microsoft.Xna.Framework.Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		List<Actor> actors = new List<Actor>();

		public Game()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Assets";
			IsMouseVisible = true;
			graphics.IsFullScreen = false;
		}

		Actor CreateTestActor()
		{
			var image = Content.Load<Texture2D>("logo");

			var ret = new Actor();

			var render = new Renderable(image);
			var health = new Health(100);
			var keyboard = new KeyboardInput();

			ret.AddTraits(render, health, keyboard);

			ret.ActorID = actors.Count + 1;

			return ret;
		}

		Actor CreateSecondTestActor()
		{
			var image = Content.Load<Texture2D>("logo");

			var ret = new Actor();

			var render = new Renderable(image);
			var health = new Health(100);
			var mouse = new MouseInput();

			render.RenderLocation = new Vector2(180, 90);

			ret.AddTraits(render, health, mouse);

			ret.ActorID = actors.Count + 1;

			return ret;
		}

		protected override void Initialize()
		{
			base.Initialize();

			var test1 = CreateTestActor();
			var test2 = CreateSecondTestActor();

			actors.Add(test1);
			actors.Add(test2);
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
		}

		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			foreach (var actor in actors)
				foreach (var traits in actor.TraitsImplementing<ITick>())
					traits.Tick(actor);
		}

		protected override void Draw(GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear(Color.Green);
			spriteBatch.Begin();

			foreach (var actor in actors)
				foreach (var tick in actor.TraitsImplementing<ITickRender>())
					tick.TickRender(actor, spriteBatch);

			spriteBatch.End();
			base.Draw(gameTime);
		}
	}
}
