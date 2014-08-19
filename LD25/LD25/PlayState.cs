// $Id$
// $Author: Matthias 'Sylence' Specht $

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LD25
{
	enum WinState
	{
		Playing,
		Won,
		Lost
	}

	internal class PlayState : IGamestate
	{
		#region Constructor

		public PlayState( Game game )
			: base( game )
		{
			Win = WinState.Playing;
			Batch = new SpriteBatch( game.GraphicsDevice );

			Cam = new Camera( game );
			Level = new Level( game, Batch, Cam );

			Player = new LD25.Player( game, Batch, Level );

			foreach( Vector2 p in Level.GetAllPositions( TileType.Guard ) )
			{
				Guard g = new Guard( game, Batch, Level );
				g.Position = p * Constants.TileDrawSize;

				Guards.Add( g );

				TileType newType = Level.GetTileAt( new Point( (int)p.X + 1, (int)p.Y ) );
				Level.ReplaceTile( new Point( (int)p.X, (int)p.Y ), newType );
			}

			foreach( Vector2 p in Level.GetAllPositions( TileType.Inmate ) )
			{
				Inmate i = new Inmate( game, Batch, Level );
				i.Position = p * Constants.TileDrawSize;

				Inmates.Add( i );

				TileType newType = Level.GetTileAt( new Point( (int)p.X + 1, (int)p.Y ) );
				Level.ReplaceTile( new Point( (int)p.X, (int)p.Y ), newType );
			}

			TileType tile = Level.GetTileAt( new Point( (int)Level.SpawnPosition.X + 1, (int)Level.SpawnPosition.Y ) );
			Level.ReplaceTile( new Point( (int)Level.SpawnPosition.X, (int)Level.SpawnPosition.Y ), tile );

			Gui = new Gui( game );
		}

		#endregion Constructor

		#region Methods

		public override void Load()
		{
			Gui.StartTime = DateTime.Now;
			Game.Components.Add( Cam );
		}

		public override void Unload()
		{
			Game.Components.Remove( Cam );
		}

		public override void Draw( Microsoft.Xna.Framework.GameTime gameTime )
		{
			base.Draw( gameTime );

			Batch.Begin( SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, Cam.ViewMatrix );
			{
				Level.Draw( gameTime );
				Player.Draw( gameTime );

				foreach( Guard g in Guards )
				{
					g.Draw( gameTime );
				}

				foreach( Inmate i in Inmates )
				{
					i.Draw( gameTime );
				}
			}
			Batch.End();

			Gui.Win = Win;
			Gui.Draw( gameTime );
		}

		public override void Update( Microsoft.Xna.Framework.GameTime gameTime )
		{
			base.Update( gameTime );

			if( Win == WinState.Playing )
			{
				Player.Update( gameTime );

				foreach( Guard g in Guards )
				{
					g.Update( gameTime );

					if( g.SeesPlayer( Player ) )
					{
						Win = WinState.Lost;
					}
				}

				foreach( Inmate i in Inmates )
				{
					i.Update( gameTime );
				}

				Cam.Position = Player.Position;
				Cam.Update( gameTime );

				Point playerPos = Level.GetPointFromPosition( Player.Position );

				KeyboardState keyboard = Keyboard.GetState();
				if( keyboard.IsKeyDown( Keys.Space ) )
				{
					if( Level.GetDistanceToNext( playerPos, TileType.Door, 20 ) <= 2 )
					{
						if( Gui.DoorStart == null )
						{
							DoorPosition = Level.GetNextTilePosition( playerPos, TileType.Door, 20 );
							DoubleDoor = Level.IsDoubleDoor( DoorPosition );

							Gui.DoorStart = DateTime.Now;
							Gui.NeededDoorTime = TimeSpan.FromSeconds( Constants.DoorOpenTime * ( DoubleDoor ? 2 : 1 ) );
						}
						else if( DateTime.Now - Gui.DoorStart.Value >= Gui.NeededDoorTime )
						{
							Level.ReplaceTile( DoorPosition, TileType.Floor );
							if( DoubleDoor )
							{
								DoorPosition = Level.GetNextTilePosition( DoorPosition, TileType.Door, 2 );
								Level.ReplaceTile( DoorPosition, TileType.Floor );
							}
							Gui.DoorStart = null;
						}
					}
					else
					{
						Gui.DoorStart = null;
					}
				}
				else
				{
					Gui.DoorStart = null;
				}

				if( Level.GetDistanceToNext( playerPos, TileType.Exit, 20 ) <= 2 )
				{
					Win = WinState.Won;
				}
			}
		}

		#endregion Methods

		#region Properties

		public Camera Cam { get; private set; }
		public Player Player { get; private set; }
		public Level Level { get; private set; }
		public Gui Gui { get; private set; }

		#endregion Properties

		#region Attributes

		List<Guard> Guards = new List<Guard>();
		List<Inmate> Inmates = new List<Inmate>();
		SpriteBatch Batch;
		Point DoorPosition;
		bool DoubleDoor;
		WinState Win;

		#endregion
	}
}