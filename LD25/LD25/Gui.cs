// $Id$
// $Author: Matthias 'Sylence' Specht $

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD25
{
	class Gui : DrawableGameComponent
	{
		#region Constructor

		public Gui( Game game )
			: base( game )
		{
			Font = game.Content.Load<SpriteFont>( "MenuFont" );
			BigFont = game.Content.Load<SpriteFont>( "BigFont" );
			Batch = new SpriteBatch( game.GraphicsDevice );
			PrimBatch = new PrimitiveBatch( game.GraphicsDevice );
		}

		#endregion

		#region Methods

		public override void Draw( GameTime gameTime )
		{
			base.Draw( gameTime );

			Batch.Begin();

			if( Win == WinState.Playing )
			{
				if( DoorStart.HasValue )
				{
					float progress = (float)( ( DateTime.Now - DoorStart.Value ).TotalMilliseconds ) / (float)NeededDoorTime.TotalMilliseconds;

					int width = 400;
					int height = 50;

					int x = Game.GraphicsDevice.Viewport.Width / 2 - width / 2;
					int y = Game.GraphicsDevice.Viewport.Height / 2 - height / 2;

					PrimBatch.Begin( PrimitiveType.TriangleList );

					PrimBatch.AddVertex( new Vector2( x, y ), Color.Gray );
					PrimBatch.AddVertex( new Vector2( x + width, y ), Color.Gray );
					PrimBatch.AddVertex( new Vector2( x, y + height ), Color.Gray );

					PrimBatch.AddVertex( new Vector2( x + width, y ), Color.Gray );
					PrimBatch.AddVertex( new Vector2( x + width, y + height ), Color.Gray );
					PrimBatch.AddVertex( new Vector2( x, y + height ), Color.Gray );

					x += 5;
					y += 5;
					height -= 10;
					width -= 10;
					width = (int)( width * progress );

					PrimBatch.AddVertex( new Vector2( x, y ), Color.LightGray );
					PrimBatch.AddVertex( new Vector2( x + width, y ), Color.LightGray );
					PrimBatch.AddVertex( new Vector2( x, y + height ), Color.LightGray );

					PrimBatch.AddVertex( new Vector2( x + width, y ), Color.LightGray );
					PrimBatch.AddVertex( new Vector2( x + width, y + height ), Color.LightGray );
					PrimBatch.AddVertex( new Vector2( x, y + height ), Color.LightGray );

					PrimBatch.End();

					PrimBatch.Begin( PrimitiveType.LineList );

					width = 400;
					height += 10;
					x -= 5;
					y -= 5;

					PrimBatch.AddVertex( new Vector2( x, y ), Color.Black );
					PrimBatch.AddVertex( new Vector2( x + width, y ), Color.Black );

					PrimBatch.AddVertex( new Vector2( x + width, y ), Color.Black );
					PrimBatch.AddVertex( new Vector2( x + width, y + height ), Color.Black );

					PrimBatch.AddVertex( new Vector2( x + width, y + height ), Color.Black );
					PrimBatch.AddVertex( new Vector2( x, y + height ), Color.Black );

					PrimBatch.AddVertex( new Vector2( x, y + height ), Color.Black );
					PrimBatch.AddVertex( new Vector2( x, y ), Color.Black );

					PrimBatch.End();
				}

				Batch.DrawString( Font, "Escape the prison!", new Vector2( 1030, 5 ), Color.White );
				Batch.DrawString( Font, "Watch out for guards", new Vector2( 1030, 25 ), Color.White );
				Batch.DrawString( Font, "Controls:", new Vector2( 1030, 45 ), Color.White );
				Batch.DrawString( Font, "WASD - Move", new Vector2( 1030, 65 ), Color.White );
				Batch.DrawString( Font, "Hold Space - Open Door", new Vector2( 1030, 85 ), Color.White );
			}
			else
			{
				string text = Win == WinState.Lost ? "Game Over" : "You Win";
				Vector2 size = BigFont.MeasureString( text );
				Vector2 origin = size * 0.5f;
				Point pos = new Rectangle( 0, 0, 1280, 720 ).Center;

				Batch.DrawString( BigFont, "Game Over", new Vector2( pos.X, pos.Y ), Color.Red, 0, origin, 1, SpriteEffects.None, 0 );
			}

			Batch.DrawString( Font, string.Format( "Time: {0:hh\\:mm\\:ss}", DateTime.Now - StartTime ), new Vector2( 5, 5 ), Color.White );

			Batch.End();
		}

		public override void Update( GameTime gameTime )
		{
			base.Update( gameTime );
		}

		#endregion

		#region Properties

		public TimeSpan NeededDoorTime { get; set; }
		public DateTime? DoorStart { get; set; }
		public DateTime StartTime { get; set; }
		public WinState Win { get; set; }

		#endregion

		#region Attributes

		SpriteBatch Batch;
		PrimitiveBatch PrimBatch;
		SpriteFont Font;
		SpriteFont BigFont;

		#endregion
	}
}
