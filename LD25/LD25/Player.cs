// $Id$
// $Author: Matthias 'Sylence' Specht $

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LD25
{
	class Player : Entity
	{
		#region Constructor

		public Player( Game game, SpriteBatch batch, Level level )
			: base( game )
		{
			Batch = batch;
			Texture = game.Content.Load<Texture2D>( "Tiles" );
			Level = level;
			Position = Level.SpawnPosition * Constants.TileDrawSize;
		}

		#endregion

		#region Methods

		public override void Draw( GameTime gameTime )
		{
			base.Draw( gameTime );

			Batch.Draw( Texture, new Rectangle( (int)Position.X, (int)Position.Y, Constants.TileDrawSize, Constants.TileDrawSize ),
				new Rectangle( 0, 0, Constants.TileSize, Constants.TileSize ), Color.White, Rotation,
				new Vector2( Constants.TileSize * 0.5f, Constants.TileSize * 0.5f ), SpriteEffects.None, 0 );
		}

		public override void Update( GameTime gameTime )
		{
			base.Update( gameTime );

			Vector2 movement = Vector2.Zero;
			KeyboardState keyboard = Keyboard.GetState();

			if( keyboard.IsKeyDown( Keys.W ) )
			{
				movement.Y--;
			}
			else if( keyboard.IsKeyDown( Keys.S ) )
			{
				movement.Y++;
			}

			if( keyboard.IsKeyDown( Keys.A ) )
			{
				movement.X--;
			}
			else if( keyboard.IsKeyDown( Keys.D ) )
			{
				movement.X++;
			}

			if( movement != Vector2.Zero )
			{
				movement.Normalize();

				//Rotation = (float)Math.Atan2( movement.Y, movement.X ) + MathHelper.ToRadians( 90 );
			}

			movement *= Constants.MovementSpeed;
			movement *= (float)gameTime.ElapsedGameTime.TotalMilliseconds;

			Vector2 oldPos = Position;
			Point oldLevelPos = Level.GetPointFromPosition( oldPos );

			bool canWalk = true;

			if( movement.X != 0 && !Level.IsTileWalkable( oldLevelPos.X + ( movement.X > 0 ? 1 : -1 ), oldLevelPos.Y ) )
			{
				canWalk = false;
			}
			if( movement.Y != 0 && !Level.IsTileWalkable( oldLevelPos.X, oldLevelPos.Y + ( movement.Y > 0 ? 1 : -1 ) ) )
			{
				canWalk = false;
			}

			if( canWalk )
			{
				Position += movement;
			}

			if( !canWalk || !Level.IsTileWalkable( Level.GetPointFromPosition( Position ) ) )
			{
				Position = oldPos;
			}

			_BoundingBox.X = (int)Position.X;
			_BoundingBox.Y = (int)Position.Y;
			_BoundingBox.Width = Constants.TileDrawSize;
			_BoundingBox.Height = Constants.TileDrawSize;
		}

		#endregion


		#region Attributes

		SpriteBatch Batch;
		Texture2D Texture;
		Level Level;
		Rectangle _BoundingBox;

		#endregion
	}
}
