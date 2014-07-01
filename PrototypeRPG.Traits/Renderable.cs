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

		public Renderable(AnimationData animationData)
		{
            Sprite = new Sprite(animationData.Texture, animationData.MaxFrameCount, animationData.AnimationRowCount);
            if(animationData.IsAnimated)
            {
                for(int i = 0; i < animationData.AnimationRowCount; i++)
                    Sprite.AddAnimation(animationData.AnimationReference[i], i + 1, animationData.AnimationFrameCount[animationData.AnimationReference[i]], animationData.AnimationFPS[animationData.AnimationReference[i]]);

                Sprite.Loop = animationData.LoopAnimation;
                // TODO: Automatically set character to an idle position
                Sprite.SetAnimation("left");
            }


			// This is set to the bounds relative to the Window's 0,0
			// TODO: Set this to current location + bounds
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
