// $Id$
// $Author: Matthias 'Sylence' Specht $

namespace LD25
{
#if WINDOWS || XBOX

	internal static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		private static void Main( string[] args )
		{
			using( LDGame game = new LDGame() )
			{
				game.Run();
			}
		}
	}

#endif
}