// $Id$
// $Author: Matthias 'Sylence' Specht $

using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace LD25
{
	internal enum GamestateType
	{
		Game,
		Menu
	}

	internal class GamestateManager : DrawableGameComponent
	{
		#region Constructor

		public GamestateManager( Game game )
			: base( game )
		{
			StateMap.Add( GamestateType.Game, new PlayState( game ) );
		}

		#endregion Constructor

		#region Methods

		public void ChangeState( GamestateType state )
		{
			if( CurrentState != null )
				CurrentState.Unload();

			CurrentState = StateMap[state];

			if( CurrentState != null )
				CurrentState.Load();
		}

		public override void Update( GameTime gameTime )
		{
			base.Update( gameTime );

			if( CurrentState != null )
				CurrentState.Update( gameTime );
		}

		public override void Draw( GameTime gameTime )
		{
			base.Draw( gameTime );

			if( CurrentState != null )
				CurrentState.Draw( gameTime );
		}

		#endregion Methods

		#region Properties

		public IGamestate CurrentState { get; private set; }

		#endregion Properties

		#region Attributes

		private Dictionary<GamestateType, IGamestate> StateMap = new Dictionary<GamestateType, IGamestate>();

		#endregion Attributes
	}
}