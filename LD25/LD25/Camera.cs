// $Id$
// $Author: Matthias 'Sylence' Specht $

using Microsoft.Xna.Framework;

namespace LD25
{
	class Camera : GameComponent
	{
		#region Constructor

		public Camera( Game game )
			: base( game )
		{
			Zoom = 1.75f;
		}

		#endregion

		#region Methods

		public Vector2 GetWorldPosition( Vector2 screenPos )
		{
			return Vector2.Transform( screenPos, Matrix.Invert( ViewMatrix ) );
		}

		public override void Update( GameTime gameTime )
		{
			base.Update( gameTime );

			ViewMatrix = Matrix.CreateTranslation( new Vector3( -Position.X, -Position.Y, 0 ) ) * Matrix.CreateScale( Zoom, Zoom, 1 ) *
				Matrix.CreateTranslation( new Vector3( Game.GraphicsDevice.Viewport.Width * 0.5f, Game.GraphicsDevice.Viewport.Height * 0.5f, 0 ) );

			Vector2 world = GetWorldPosition( new Vector2( Game.GraphicsDevice.Viewport.X, Game.GraphicsDevice.Viewport.Y ) );
			_ViewFrustum.X = (int)world.X;
			_ViewFrustum.Y = (int)world.Y;

			Vector2 world2 = GetWorldPosition( new Vector2( Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height ) );
			_ViewFrustum.Width = (int)( world2.X - world.X );
			_ViewFrustum.Height = (int)( world2.Y - world.Y );
		}

		#endregion

		#region Properties

		public float Zoom { get; private set; }
		public Vector2 Position { get; set; }
		public Matrix ViewMatrix { get; private set; }
		public Matrix ProjectionMatrix { get; private set; }

		public Rectangle ViewFrustum
		{
			get
			{
				return _ViewFrustum;
			}
		}

		#endregion

		#region Attributes

		Rectangle _ViewFrustum;

		#endregion
	}
}
