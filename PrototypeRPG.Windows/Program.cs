using System;

namespace PrototypeRPG.Windows
{
	public static class Program
	{
		[STAThread]
		static void Main()
		{
			using (var game = new Game())
				game.Run();
		}
	}
}
