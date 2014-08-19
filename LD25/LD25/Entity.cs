// $Id$
// $Author: Matthias 'Sylence' Specht $

using Microsoft.Xna.Framework;

namespace LD25
{
	abstract class Entity : DrawableGameComponent
	{
		#region Constructor

		public Entity( Game game )
			: base( game )
		{

		}

		#endregion

		#region Properties

		public Vector2 Position { get; set; }
		public float Rotation { get; set; }

		public Vector2 Center
		{
			get
			{
				return Position + new Vector2( Constants.TileDrawSize, Constants.TileDrawSize ) * 0.5f;
			}
		}

		#endregion
	}
}
