// $Id$
// $Author: Matthias 'Sylence' Specht $

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD25
{
	class Inmate : Entity
	{
		#region Constructor

		public Inmate( Game game, SpriteBatch batch, Level level )
			: base( game )
		{
			Batch = batch;
			Level = level;
			Texture = game.Content.Load<Texture2D>( "Tiles" );

			Black = Randomizer.Next( 2 ) == 0;
		}

		#endregion

		#region Methods

		public override void Draw( GameTime gameTime )
		{
			base.Draw( gameTime );

			Batch.Draw( Texture, new Rectangle( (int)Position.X, (int)Position.Y, Constants.TileDrawSize, Constants.TileDrawSize ),
				new Rectangle( Black ? Constants.TileSize : 0, Constants.TileSize, Constants.TileSize, Constants.TileSize ), Color.White, 0,
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
				v *= Constants.InmateMovementSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

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
		}

		#endregion

		#region Attributes

		Level Level;
		SpriteBatch Batch;
		DateTime LastTurn;
		Texture2D Texture;
		bool Black;

		#endregion
	}
}
