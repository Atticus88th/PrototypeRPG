using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PrototypeRPG.Traits;

namespace PrototypeRPG.Windows
{
	public class Game : Microsoft.Xna.Framework.Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		World world;

		public Game()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "../../../../Assets";
			IsMouseVisible = true;
			graphics.IsFullScreen = false;

			// This needs to be set in each client/project. Perhaps it can be set on World?
			// Tick 40 times per second
			this.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 40.0f);
		}

		protected override void Initialize()
		{
			base.Initialize();
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
			world = new World(spriteBatch, Content, graphics);
		}

		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);

			world.Tick();
		}

		protected override void Draw(GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear(Color.Green);
			spriteBatch.Begin();

			world.TickRender();

			spriteBatch.End();
			base.Draw(gameTime);
		}
	}
}
