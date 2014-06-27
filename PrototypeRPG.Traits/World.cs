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
		const int TileSize = 16;
		const int TilesOnWindow = 40;
		const int WindowSize = TileSize * TilesOnWindow;

		public SpriteBatch SpriteBatch { get; private set; }
		public List<Actor> Actors = new List<Actor>();
		public readonly Player WorldPlayer;
		public Player HumanPlayer;
		public readonly Actor WorldActor;

		public int TotalTickCount { get { return TickCount + TickRenderCount; } }
		public int TickCount { get; private set; }
		public int TickRenderCount { get; private set; }

		Tile[ , ] tiles2D = new Tile[TilesOnWindow,TilesOnWindow];

		readonly ContentManager content;
		readonly GraphicsDevice graphics;
		readonly Texture2D worldTileSheet;
		Random random = new Random();

		public World(SpriteBatch spriteBatch, ContentManager content, GraphicsDeviceManager gdm)
		{
			SpriteBatch = spriteBatch;
			this.content = content;
			this.graphics = gdm.GraphicsDevice;

			gdm.PreferredBackBufferHeight = WindowSize;
			gdm.PreferredBackBufferWidth = WindowSize;

			worldTileSheet = content.Load<Texture2D>("lttp");

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

			for (var x = 0; x < tiles2D.GetLength(0); x++)
				for (var y = 0; y < tiles2D.GetLength(1); y++)
				{
					var tile = tiles2D[x,y];
					var destRect = new Rectangle(x * TileSize, y * TileSize, TileSize, TileSize);
					var sourceRect = GetSourceRectangleSquare(worldTileSheet, tile.TileType, TileSize, 0);

					tile.Render(worldTileSheet, destRect, sourceRect, SpriteBatch);
				}

			foreach (var actor in Actors.ToList())
			{
				var render = actor.TraitOrDefault<Renderable>();
				if (render == null)
					continue;

				var position = actor.TraitOrDefault<Positionable>();
				if (position == null)
					throw new NullReferenceException("No position for Actor{0} in World.TickRender()".F(actor.ActorID));

				foreach (var trait in actor.TraitsImplementing<ITickRender>())
				{
					var sourceRect = GetSourceRectangle(render.Texture, 0, 16, 24, 0);
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
			var spritesPerColumn = texture.Height / paddedSpriteH;
			var x = paddedSpriteW * (spriteWidth % spritesPerRow);
			var y = paddedSpriteH * (spriteHeight % spritesPerColumn);

			return new Rectangle(x, y, spriteWidth, spriteHeight);
		}

		public Actor CreateTestActor()
		{
			var newActor = new Actor();
			newActor.World = this;
			newActor.Owner = HumanPlayer;

			var health = new Health(100);

			var position = new Positionable(newActor);
			var vx = random.Next(16, WindowSize - 17);
			var vy = random.Next(24, WindowSize - 25);
			position.Position = new Vector2(vx, vy);

			var texture = content.Load<Texture2D>("link");

			// BUG: sprite index picking doesn't work
			var sourceRect = GetSourceRectangle(texture, 20, 16, 24, 0);
			var destRect = new Rectangle((int)position.Position.X, (int)position.Position.Y, sourceRect.Width, sourceRect.Height);
			var renderable = new Renderable(texture, destRect, sourceRect);

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

			Actors.Add(CreateTestActor());

			CreateTiles();
		}

		void CreateTiles()
		{
			var destRect = new Rectangle();
			var sourceRect = new Rectangle();

			for (var x = 0; x < tiles2D.GetLength(0); x++)
			{
				for (var y = 0; y < tiles2D.GetLength(1); y++)
				{
					var typeNum = random.Next(0, 30);
					sourceRect = GetSourceRectangleSquare(worldTileSheet, typeNum, TileSize, 0);
					destRect = new Rectangle(x * TileSize, y * TileSize, TileSize, TileSize);

					tiles2D[x,y] = new Tile(typeNum);
				}
			}
		}
	}
}
