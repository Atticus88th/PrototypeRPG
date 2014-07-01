using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PrototypeRPG.Traits
{
	public class Sprite
	{
		#region "Generic Vars"
		float timeElapsed;
		float timeToUpdate = 0.05f;
		Texture2D texture;
		#endregion

		#region "Animation Vars"
		Dictionary<string, Rectangle[]> spriteAnimations = new Dictionary<string, Rectangle[]>();
		Dictionary<string, int> spriteFramesCount = new Dictionary<string, int>();
		Dictionary<string, int> spriteFPS = new Dictionary<string, int>();
		int animationFrames;
		bool isAnimation = false;
		#endregion

		#region "Properties"
		public Vector2 Position { get; set; }
		public Color SpriteColor { get; set; }
		public Vector2 Origin { get; set; }
		public float Rotation { get; set; }
		public float Scale { get; set; }
		public float OriginalScale { get; private set; }
		public SpriteEffects Effects { get; set; }
		public Rectangle Bounds { get; private set; }
		public string CurrentAnimation { get; protected set; }
		public int FrameIndex { get; set; }
		public int AnimationHeight { get; private set; }
		public int AnimationWidth { get; private set; }
		public bool Loop { get; set; }
		public int FramesPerSecond
		{
			get { return (int)timeToUpdate; }
			set { timeToUpdate = (1f / value); }
		}
		#endregion

		public Sprite(Texture2D spriteTexture, int maxFrameCount, int animationCount)
		{
			texture = spriteTexture;
			animationFrames = maxFrameCount;
			AnimationWidth = texture.Width / maxFrameCount;
			AnimationHeight = texture.Height / animationCount;
			Origin = new Vector2(AnimationWidth / 2, AnimationHeight / 2);
			SpriteColor = Color.White;
			Rotation = 0f;
			Scale = 1f;
			OriginalScale = 1f;
		}

		public Rectangle UpdateBounds()
		{
			Bounds = new Rectangle((int)Position.X - (AnimationWidth / 2), (int)Position.Y - (AnimationHeight / 2), AnimationWidth, AnimationHeight);
			return Bounds;
		}

		public void AddAnimation(string animationID, int animationRow, int frameCount, int animationFPS)
		{
			if (!isAnimation)
				isAnimation = true;

			if (spriteAnimations.ContainsKey(animationID))
				return;

			var rectangles = new Rectangle[frameCount];

			for (int i = 0; i < frameCount; i++)
				rectangles[i] = new Rectangle(i * AnimationWidth, (animationRow - 1) * AnimationHeight, AnimationWidth, AnimationHeight);

			spriteAnimations.Add(animationID, rectangles);
			spriteFramesCount.Add(animationID, frameCount);
			spriteFPS.Add(animationID, animationFPS);
		}

		public bool SetAnimation(string animationID)
		{
			if (!spriteAnimations.ContainsKey(animationID))
				return false;

			CurrentAnimation = animationID;
			return true;
		}

		public void Draw(SpriteBatch spriteBatch, float alpha, GameTime gameTime)
		{
			var alphaMixer = SpriteColor;
			alphaMixer *= alpha;

			if (isAnimation)
			{
				timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
				timeToUpdate = (1f / spriteFPS[CurrentAnimation]);

				if (timeElapsed > timeToUpdate)
				{
					timeElapsed -= timeToUpdate;

					if (isAnimation)
					{
						if (FrameIndex < (spriteFramesCount[CurrentAnimation] - 1))
							FrameIndex++;
						else if (Loop)
							FrameIndex = 0;
					}
					else
					{
						if (FrameIndex < (animationFrames - 1))
							FrameIndex++;
						else if (Loop)
							FrameIndex = 0;
					}
				}

				if (FrameIndex > (spriteAnimations[CurrentAnimation].Length - 1) && texture != null)
					spriteBatch.Draw(texture, Position, spriteAnimations[CurrentAnimation][0], alphaMixer, Rotation, Origin, Scale, Effects, 0f);
				else
					spriteBatch.Draw(texture, Position, spriteAnimations[CurrentAnimation][FrameIndex], alphaMixer, Rotation, Origin, Scale, Effects, 0f);
			}
			else if (texture != null)
				spriteBatch.Draw(texture, Position, null, alphaMixer, Rotation, Origin, Scale, Effects, 0f);
		}
	}
}
