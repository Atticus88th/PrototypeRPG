﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PrototypeRPG.Traits
{
	public interface ITrait { };
	public interface ITick { void Tick(Actor self); };
	public interface ITickRender { void TickRender(Actor self, SpriteBatch spriteBatch, GameTime gameTime); };

	public interface IMouseInteraction
	{
		void OnLeftClick(Actor self);
		void OnRightClick(Actor self);
	}
}
