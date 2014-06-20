using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PrototypeRPG.Mac
{
	public class Game : Microsoft.Xna.Framework.Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch sprites;
		Texture2D logo;

		public Game()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Assets";
			IsMouseVisible = true;
			graphics.IsFullScreen = false;
		}

		protected override void Initialize()
		{
			base.Initialize();
		}

		protected override void LoadContent()
		{
			sprites = new SpriteBatch(graphics.GraphicsDevice);
			logo = Content.Load<Texture2D>("logo");
		}

		protected override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}

		protected override void Draw(GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear(Color.Green);
			sprites.Begin();

			sprites.Draw(logo, new Vector2 (130, 200), Color.White);

			sprites.End();
			base.Draw (gameTime);
		}
	}
}
