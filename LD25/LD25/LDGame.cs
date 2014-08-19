// $Id$
// $Author: Matthias 'Sylence' Specht $

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD25
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	internal class LDGame : Microsoft.Xna.Framework.Game
	{
		#region Constructor

		public LDGame()
		{
			graphics = new GraphicsDeviceManager( this );
			graphics.PreferMultiSampling = false;
			graphics.PreferredBackBufferHeight = 720;
			graphics.PreferredBackBufferWidth = 1280;

			Content.RootDirectory = "Content";

			this.IsMouseVisible = true;
		}

		#endregion Constructor

		#region Methods

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			StateManager = new GamestateManager( this );
			Components.Add( StateManager );

			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			StateManager.ChangeState( GamestateType.Game );
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update( GameTime gameTime )
		{
			base.Update( gameTime );
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw( GameTime gameTime )
		{
			GraphicsDevice.Clear( Color.CornflowerBlue );

			base.Draw( gameTime );
		}

		#endregion Methods

		#region Properties

		public GamestateManager StateManager { get; private set; }

		#endregion Properties

		#region Attributes

		private GraphicsDeviceManager graphics;

		#endregion Attributes
	}
}