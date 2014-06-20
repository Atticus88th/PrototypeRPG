using System;
using MonoMac.AppKit;
using MonoMac.Foundation;

namespace PrototypeRPG.Mac
{
	static class Program
	{
		static void Main(string[] args)
		{
			NSApplication.Init();
			
			using (var pool = new NSAutoreleasePool())
			{
				NSApplication.SharedApplication.Delegate = new AppDelegate();
				NSApplication.Main(args);
			}
		}
	}

	class AppDelegate : NSApplicationDelegate
	{
		Game game;

		public override void FinishedLaunching(MonoMac.Foundation.NSObject notification)
		{
			game = new Game();
			game.Run();
		}

		public override bool ApplicationShouldTerminateAfterLastWindowClosed(NSApplication sender)
		{
			return true;
		}
	}
}
