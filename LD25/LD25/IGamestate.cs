// $Id$
// $Author: Matthias 'Sylence' Specht $

using Microsoft.Xna.Framework;

namespace LD25
{
	internal abstract class IGamestate : DrawableGameComponent
	{
		#region Constructor

		public IGamestate( Game game )
			: base( game )
		{
		}

		#endregion Constructor

		#region Methods

		public abstract void Load();

		public abstract void Unload();

		#endregion Methods
	}
}