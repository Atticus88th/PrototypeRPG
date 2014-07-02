using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PrototypeRPG.Traits
{
	public class Renderable : ITrait, ITickRender
	{
		public Rectangle Boundingbox;

		public Sprite Sprite { get; private set; }

		Actor self;

		public Renderable(Actor self, AnimationData animData)
		{
			this.self = self;

			Sprite = new Sprite(self, animData.Texture, animData.MaxFrameCount, animData.AnimationRowCount);

			if (animData.IsAnimated)
			{
				for (int i = 0; i < animData.AnimationRowCount; i++)
				{
					var animId = animData.AnimationReference[i];
					var animRow = i + 1;
					var animFrameCount = animData.AnimationFrameCount[animData.AnimationReference[i]];
					var animFPS = animData.AnimationFPS[animId];

					Sprite.AddAnimation(animId, animRow, animFrameCount, animFPS);
				}

				Sprite.Loop = animData.LoopAnimation;

				// TODO: Automatically set character to an idle position
				Sprite.SetAnimation("left");
			}
		}

		public void UpdateBoundingBox(int x, int y)
		{
			Boundingbox = Sprite.UpdateBounds();
		}

		public void TickRender(Actor self, SpriteBatch spriteBatch, GameTime gameTime)
		{
			if (self.IsDead)
				return;

			if (Sprite == null)
				throw new ArgumentNullException("No sprite provided for Actor{0} to render!".F(self.ActorID));

			var position = self.TraitOrDefault<Positionable>();
			if (position == null)
				throw new ArgumentNullException("No position trait for Actor{0}!".F(self.ActorID));

			Sprite.Position = position.Position;
			Sprite.Draw(spriteBatch, 1f, gameTime);
		}
	}
}
