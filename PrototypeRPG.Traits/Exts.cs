using System;
using Microsoft.Xna.Framework;

namespace PrototypeRPG.Traits
{
	// TODO: Find a new home!
	public static class Exts
	{
		public static string F(this string fmt, params object[] objs)
		{
			return string.Format(fmt, objs);
		}
	}
}
