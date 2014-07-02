using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
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
		public readonly Player WorldPlayer;
		public Player HumanPlayer;
		public readonly Actor WorldActor;

		public int TotalTickCount { get { return TickCount + TickRenderCount; } }
		public int TickCount { get; private set; }
		public int TickRenderCount { get; private set; }

		// Be careful with this, it shouldn't exist til after everything here is done
		public Map Map { get; private set; }

		readonly ContentManager content;
		readonly GraphicsDevice graphics;
		Random random = new Random();

		int windowSize;

		public World(SpriteBatch spriteBatch, ContentManager content, GraphicsDeviceManager gdm)
		{
			SpriteBatch = spriteBatch;
			this.content = content;
			this.graphics = gdm.GraphicsDevice;

			Map = new Map(this, 8, 16);
			Map.MapTileset = content.Load<Texture2D>("lttp");

			windowSize = Map.TilesInMapSquare * Map.TileSize;
			Console.WriteLine(windowSize);

			gdm.PreferredBackBufferHeight = windowSize;
			gdm.PreferredBackBufferWidth = windowSize;

			// Workaround for Windows MonoGame implementation
			gdm.GraphicsDevice.PresentationParameters.BackBufferHeight = gdm.PreferredBackBufferHeight;
			gdm.GraphicsDevice.PresentationParameters.BackBufferWidth = gdm.PreferredBackBufferWidth;
			gdm.ApplyChanges();

			WorldPlayer = new Player();
			WorldActor = new Actor();
			WorldPlayer.PlayerActor = WorldActor;
			WorldActor.Owner = WorldPlayer;

			SetupWorld();
		}

		public void Tick()
		{
			TickCount++;

			foreach (var actor in Actors.ToList())
				foreach (var trait in actor.TraitsImplementing<ITick>())
					trait.Tick(actor);
		}

		public void TickRender(GameTime gameTime)
		{
			TickRenderCount++;

			Map.DrawTiles();

			foreach (var actor in Actors.ToList())
			{
				var render = actor.TraitOrDefault<Renderable>();
				if (render == null)
					continue;

				var position = actor.TraitOrDefault<Positionable>();
				if (position == null)
					continue;

				foreach (var trait in actor.TraitsImplementing<ITickRender>())
					trait.TickRender(actor, SpriteBatch, gameTime);
			}
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
				a.Trait<Renderable>().Boundingbox.Contains(point)).FirstOrDefault();
		}

		public Rectangle GetSourceRectangleSquare(Texture2D texture, int tileIndex, int tileSize, int tileBorder)
		{
			var paddedTileSize = tileSize + tileBorder;
			var tilesPerRow = texture.Width / paddedTileSize;
			var x = paddedTileSize * (tileIndex % tilesPerRow);
			var y = paddedTileSize * (tileIndex / tilesPerRow);

			return new Rectangle(x, y, tileSize, tileSize);
		}

		public Rectangle GetSourceRectangle(Texture2D texture, int spriteIndex, int spriteWidth, int spriteHeight, int spriteBorder)
		{
			var paddedSpriteW = spriteWidth + spriteBorder;
			var paddedSpriteH = spriteHeight + spriteBorder;
			var spritesPerRow = texture.Width / paddedSpriteW;
			var x = paddedSpriteW * (spriteIndex % spritesPerRow);
			var y = paddedSpriteH * (spriteIndex / spritesPerRow);

			return new Rectangle(x, y, spriteWidth, spriteHeight);
		}

		public Actor CreateTestActor()
		{
			var newActor = new Actor();
			newActor.World = this;
			newActor.Owner = HumanPlayer;

			var health = new Health(100);

			var position = new Positionable(newActor);
			var rx = random.Next(16, windowSize - 17).TileSubdivision(16);
			var ry = random.Next(24, windowSize - 25).TileSubdivision(16);
			position.Position = new Vector2(rx, ry);

			var texture = content.Load<Texture2D>("link");

			var sourceRect = GetSourceRectangle(texture, 0, 16, 24, 0);
			var bounding = new Rectangle((int)position.Position.X, (int)position.Position.Y, sourceRect.Width, sourceRect.Height);

			// TODO: Un-hardcode all of these animations
			var animationReference = new string[] { "left", "up", "right", "down" };
			var animationFrameCount = new Dictionary<string, int> { { "left", 8 }, { "up", 8 }, { "right", 8 }, { "down", 8 } };

			var animationFPS = new Dictionary<string, int> { { "left", 5 }, { "up", 5 }, { "right", 5 }, { "down", 5 } };
			var animationData = new AnimationData
			{
				AnimationRowCount = 4,
				IsAnimated = true,
				LoopAnimation = true,
				MaxFrameCount = 8,
				Scale = 1f,
				SpriteColor = Color.White,
				Texture = texture,
				AnimationReference = animationReference,
				AnimationFrameCount = animationFrameCount,
				AnimationFPS = animationFPS
			};

			var renderable = new Renderable(newActor, animationData);
			var keyMove = new KeyboardMovement(2);

			newActor.AddTrait(health);
			newActor.AddTrait(position);
			newActor.AddTrait(renderable);
			newActor.AddTrait(keyMove);

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

//			Map.CreateMapTiles();
//			Map.LoadTilesFromFile(Path.Combine(content.RootDirectory, "map.txt"));
			Map.LoadTilesFromArrays();

			// Temporary; create initial character
			//	TODO: This is horrible altogether, this should add to world.Actors also
			Actors.Add(CreateTestActor());
		}
	}
}
