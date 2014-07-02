using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PrototypeRPG.Traits
{
	public struct AnimationData
	{
		public int AnimationRowCount;
		public int MaxFrameCount;
		public Texture2D Texture;
		public Color SpriteColor;
		public float Scale;
		public bool IsAnimated;
		public bool LoopAnimation;
		public string[] AnimationReference;
		public Dictionary<string, int> AnimationFrameCount;
		public Dictionary<string, int> AnimationFPS;
	}
}
