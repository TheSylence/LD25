// $Id$
// $Author: Matthias 'Sylence' Specht $

using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD25
{
	class Guard : Entity
	{
		#region Constructor

		public Guard( Game game, SpriteBatch batch, Level level )
			: base( game )
		{
			Batch = batch;
			Texture = game.Content.Load<Texture2D>( "Tiles" );
			Level = level;
		}

		#endregion

		#region Methods

		public override void Draw( GameTime gameTime )
		{
			base.Draw( gameTime );

			Batch.Draw( Texture, new Rectangle( (int)Position.X, (int)Position.Y, Constants.TileDrawSize, Constants.TileDrawSize ),
				new Rectangle( Constants.TileSize, 0, Constants.TileSize, Constants.TileSize ), Color.White, Rotation,
				new Vector2( Constants.TileSize * 0.5f, Constants.TileSize * 0.5f ), SpriteEffects.None, 0 );
		}

		public override void Update( GameTime gameTime )
		{
			base.Update( gameTime );

			if( DateTime.Now - LastTurn > TimeSpan.FromSeconds( 1 ) )
			{
				if( Randomizer.Next( 100 ) < 10 )
				{
					Rotation += MathHelper.ToRadians( 90 ) * ( Randomizer.Next( 3 ) - 1 );
				}

				LastTurn = DateTime.Now;
			}
			else
			{
				Vector2 v = -Vector2.UnitY;
				float x = (float)( ( v.X * Math.Cos( Rotation ) ) - ( v.Y * Math.Sin( Rotation ) ) );
				float y = (float)( ( v.X * Math.Sin( Rotation ) ) + ( v.Y * Math.Cos( Rotation ) ) );
				v = new Vector2( x, y );
				v *= Constants.GuardMovementSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

				Vector2 oldPos = Position;
				Point oldLevelPos = Level.GetPointFromPosition( oldPos );

				bool canWalk = true;

				if( v.X != 0 && !Level.IsTileWalkable( oldLevelPos.X + ( v.X > 0 ? 1 : -1 ), oldLevelPos.Y ) )
				{
					canWalk = false;
				}
				if( v.Y != 0 && !Level.IsTileWalkable( oldLevelPos.X, oldLevelPos.Y + ( v.Y > 0 ? 1 : -1 ) ) )
				{
					canWalk = false;
				}

				if( canWalk )
				{
					Position += v;
				}

				if( !canWalk || !Level.IsTileWalkable( Level.GetPointFromPosition( Position ) ) )
				{
					Position = oldPos;
					Rotation += MathHelper.ToRadians( 90 ) * ( Randomizer.Next( 3 ) - 1 );
				}
			}

			while( Rotation < 0 )
			{
				Rotation += MathHelper.ToRadians( 360 );
			}
			while( Rotation > MathHelper.ToRadians( 360 ) )
			{
				Rotation -= MathHelper.ToRadians( 360 );
			}

			Debug.Assert( Rotation <= 360 );
		}

		bool Compare( float a, float b )
		{
			return Math.Abs( a - b ) < 0.001f;
		}

		public bool SeesPlayer( Player player )
		{
			float px = player.Center.X - Center.X;
			float py = player.Center.Y - Center.Y;

			float r = (float)Math.Sqrt( px * px + py * py );
			if( r <= Constants.GuardViewDistance )
			{
				if( Level.IsSightClear( Level.GetPointFromPosition( Center ), Level.GetPointFromPosition( player.Center ) ) )
				{
					float phi = (float)( Math.Atan2( py, px ) - Math.Atan2( -1, 0 ) );
					phi = MathHelper.ToDegrees( phi );

					while( phi < 0 )
					{
						phi += 360;
					}
					while( phi > 360 )
					{
						phi -= 360;
					}

					float rotationDegrees = MathHelper.ToDegrees( Rotation );
					float minAngle = rotationDegrees - Constants.GuardViewAngle * 0.5f;
					float maxAngle = rotationDegrees + Constants.GuardViewAngle * 0.5f;

					Debug.WriteLine( "{0} < {1} < {2}", minAngle, phi, maxAngle );

					return minAngle < phi && phi < maxAngle;
				}
			}

			return false;
		}

		#endregion

		#region Attributes

		Texture2D Texture;
		SpriteBatch Batch;
		DateTime LastTurn;
		Level Level;
		Rectangle _BoundingBox;

		#endregion
	}
}
