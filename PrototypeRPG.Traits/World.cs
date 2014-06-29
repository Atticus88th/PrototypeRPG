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
		public Map Map;

		readonly ContentManager content;
		readonly GraphicsDevice graphics;
		Random random = new Random();

		int windowSize;

		public World(SpriteBatch spriteBatch, ContentManager content, GraphicsDeviceManager gdm)
		{
			SpriteBatch = spriteBatch;
			this.content = content;
			this.graphics = gdm.GraphicsDevice;

			Map = new Map(this, 40, 16);
			Map.MapTileset = content.Load<Texture2D>("lttp");

			windowSize = Map.TilesInMapSquare * Map.TileSize;

			gdm.PreferredBackBufferHeight = windowSize;
			gdm.PreferredBackBufferWidth = windowSize;

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

		public void TickRender()
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
				{
					var sourceRect = render.SourceRect;
					var destRect = new Rectangle((int)position.Position.X, (int)position.Position.Y, sourceRect.Width, sourceRect.Height);
					trait.TickRender(actor, destRect, sourceRect, SpriteBatch);
				}
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
			var vx = random.Next(16, windowSize - 17);
			var vy = random.Next(24, windowSize - 25);
			position.Position = new Vector2(vx, vy);

			var texture = content.Load<Texture2D>("link");

			var sourceRect = GetSourceRectangle(texture, 0, 16, 24, 0);
			var bounding = new Rectangle((int)position.Position.X, (int)position.Position.Y, sourceRect.Width, sourceRect.Height);
			var renderable = new Renderable(texture, bounding, sourceRect);

			var keyMove = new KeyboardMovement();

			newActor.AddTrait(health);
			newActor.AddTrait(position);
			newActor.AddTrait(renderable);
			newActor.AddTrait(keyMove);

			return newActor;
		}

		public Actor CreateActorSpriteIndex(int index)
		{
			var newActor = new Actor();
			newActor.World = this;
			newActor.Owner = HumanPlayer;

			var health = new Health(100);

			var position = new Positionable(newActor);
			position.Position = new Vector2(index * 16, 0);

			var texture = content.Load<Texture2D>("link");

			var sourceRect = GetSourceRectangle(texture, index, 16, 24, 0);
			var bounding = new Rectangle((int)position.Position.X, (int)position.Position.Y, sourceRect.Width, sourceRect.Height);
			var renderable = new Renderable(texture, bounding, sourceRect);

			newActor.AddTrait(health);
			newActor.AddTrait(position);
			newActor.AddTrait(renderable);

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

			for (var i = 0; i < 32; i++)
				Actors.Add(CreateActorSpriteIndex(i));

			Map.CreateMapTiles();
//			map.LoadTilesFromFile(Path.Combine(content.RootDirectory, "map.txt"));
		}
	}
}
