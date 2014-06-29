using System;
using System.Collections.Generic;
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

		// TODO: If I get file -> tile[] working, uncomment this
//		public static int[] ToIntArray(this string value, char separator)
//		{
//			return Array.ConvertAll(value.Split(separator), s => int.Parse(s));
//		}
	}
}
