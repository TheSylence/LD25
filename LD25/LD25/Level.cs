// $Id$
// $Author: Matthias 'Sylence' Specht $

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LD25
{
	enum TileType
	{
		Grass,
		Wall,
		Spawn,
		Exit,
		Fence,
		Door,
		Floor,
		Guard,
		Inmate
	}

	class Level : DrawableGameComponent
	{
		#region Constructor

		public Level( Game game, SpriteBatch batch, Camera cam )
			: base( game )
		{
			Cam = cam;
			Batch = batch;
			TileTexture = game.Content.Load<Texture2D>( "Tiles" );

			LoadLayout();
		}

		#endregion

		#region Methods

		void LoadLayout()
		{
			Texture2D baseTexture = Game.Content.Load<Texture2D>( "Level" );

			Width = baseTexture.Width;
			Height = baseTexture.Height;

			Tiles = new TileType[Width, Height];

			Color[] data = new Color[Width * Height];
			baseTexture.GetData( data );

			for( int x = 0; x < Width; ++x )
			{
				for( int y = 0; y < Height; ++y )
				{
					TileType type = TileFromColor( data[y * Width + x] );
					Tiles[x, y] = type;

					if( type == TileType.Spawn )
					{
						SpawnPosition = new Vector2( x, y );
					}
				}
			}
		}

		public IEnumerable<Vector2> GetAllPositions( TileType type )
		{
			List<Vector2> list = new List<Vector2>();

			for( int x = 0; x < Width; ++x )
			{
				for( int y = 0; y < Height; ++y )
				{
					if( Tiles[x, y] == type )
					{
						list.Add( new Vector2( x, y ) );
					}
				}
			}

			return list;
		}

		TileType TileFromColor( Color col )
		{
			if( col == GrassColor )
			{
				return TileType.Grass;
			}
			else if( col == WallColor )
			{
				return TileType.Wall;
			}
			else if( col == DoorColor )
			{
				return TileType.Door;
			}
			else if( col == FenceColor )
			{
				return TileType.Fence;
			}
			else if( col == FloorColor )
			{
				return TileType.Floor;
			}
			else if( col == SpawnColor )
			{
				return TileType.Spawn;
			}
			else if( col == ExitColor )
			{
				return TileType.Exit;
			}
			else if( col == GuardColor )
			{
				return TileType.Guard;
			}
			else if( col == InmateColor )
			{
				return TileType.Inmate;
			}
			else
			{
				throw new Exception( "Unknown tile type in level" );
			}
		}

		public override void Draw( GameTime gameTime )
		{
			base.Draw( gameTime );

			for( int x = 0; x < Width; ++x )
			{
				for( int y = 0; y < Height; ++y )
				{
					int idx = (int)Tiles[x, y];

					Rectangle destRectangle = new Rectangle( x * Constants.TileDrawSize, y * Constants.TileDrawSize, Constants.TileDrawSize, Constants.TileDrawSize );

					Rectangle sourceRectangle = new Rectangle();
					sourceRectangle.Y = 7 * Constants.TileSize;
					sourceRectangle.Height = Constants.TileSize;
					sourceRectangle.Width = Constants.TileSize;
					sourceRectangle.X = Constants.TileSize * ( idx % 8 );

					if( Cam.ViewFrustum.Contains( destRectangle ) || Cam.ViewFrustum.Intersects( destRectangle ) )
					{
						Batch.Draw( TileTexture, destRectangle, sourceRectangle, Color.White );
					}
				}
			}
		}

		public void ReplaceTile( Point p, TileType type )
		{
			Tiles[p.X, p.Y] = type;
		}

		public bool IsTileWalkable( Point p )
		{
			return IsTileWalkable( p.X, p.Y );
		}

		public bool IsTileWalkable( int x, int y )
		{
			switch( Tiles[x, y] )
			{
				case TileType.Floor:
				case TileType.Spawn:
				case TileType.Exit:
				case TileType.Grass:
					return true;

				default:
					return false;
			}
		}

		public Point GetPointFromPosition( Vector2 position )
		{
			return new Point( (int)position.X / Constants.TileDrawSize, (int)position.Y / Constants.TileDrawSize );
		}

		public override void Update( GameTime gameTime )
		{
			base.Update( gameTime );
		}

		public bool IsDoubleDoor( Point pos )
		{
			for( int x = -1; x <= 1; ++x )
			{
				for( int y = -1; y <= 1; ++y )
				{
					if( x == 0 && y == 0 )
						continue;

					if( Tiles[x + pos.X, y + pos.Y] == TileType.Door )
					{
						return true;
					}
				}
			}

			return false;
		}

		public int GetDistanceToNext( Point pos, TileType type, int max = 100, bool includeSelf = true )
		{
			int dist = 0;

			if( includeSelf && Tiles[pos.X, pos.Y] == type )
				return 0;

			while( dist <= max )
			{
				for( int x = 0; x < dist; ++x )
				{
					for( int y = 0; y < dist; ++y )
					{
						if( pos.X + x >= Width || pos.X - x <= 0 || pos.Y + y >= Height || pos.Y - y < 0 )
						{
							return max + 1;
						}

						if( Tiles[pos.X + x, pos.Y + y] == type )
						{
							return dist;
						}

						if( Tiles[pos.X - x, pos.Y - y] == type )
						{
							return dist;
						}

						if( Tiles[pos.X + x, pos.Y - y] == type )
						{
							return dist;
						}

						if( Tiles[pos.X - x, pos.Y + y] == type )
						{
							return dist;
						}
					}
				}

				++dist;
			}

			return max + 1;
		}

		public TileType GetTileAt( Point point )
		{
			return Tiles[point.X, point.Y];
		}

		public Point GetNextTilePosition( Point pos, TileType type, int max )
		{
			int dist = 0;

			while( dist <= max )
			{
				for( int x = 0; x < dist; ++x )
				{
					for( int y = 0; y < dist; ++y )
					{
						if( x == 0 && y == 0 )
							continue;

						if( Tiles[pos.X + x, pos.Y + y] == type )
						{
							return new Point( pos.X + x, pos.Y + y );
						}

						if( Tiles[pos.X - x, pos.Y - y] == type )
						{
							return new Point( pos.X - x, pos.Y - y );
						}

						if( Tiles[pos.X + x, pos.Y - y] == type )
						{
							return new Point( pos.X + x, pos.Y - y );
						}

						if( Tiles[pos.X - x, pos.Y + y] == type )
						{
							return new Point( pos.X - x, pos.Y + y );
						}
					}
				}

				++dist;
			}

			return new Point( -1, -1 );
		}

		public bool IsSightClear( Point p1, Point p2 )
		{
			int xAdd = 0;
			if( p1.X != p2.X )
			{
				xAdd = p1.X < p2.X ? 1 : -1;
			}

			int yAdd = 0;
			if( p1.Y != p2.Y )
			{
				yAdd = p1.Y < p2.Y ? 1 : -1;
			}

			while( p1 != p2 )
			{
				p1.X += xAdd;
				p1.Y += yAdd;

				TileType type = Tiles[p1.X, p1.Y];
				if( type == TileType.Wall || type == TileType.Door )
					return false;

				yAdd = 0;
				if( p1.Y != p2.Y )
				{
					yAdd = p1.Y < p2.Y ? 1 : -1;
				}

				xAdd = 0;
				if( p1.X != p2.X )
				{
					xAdd = p1.X < p2.X ? 1 : -1;
				}
			}

			return true;
		}

		#endregion

		#region Properties

		public Vector2 SpawnPosition { get; private set; }

		#endregion

		#region Attributes

		int Width, Height;
		TileType[,] Tiles;
		Texture2D TileTexture;
		SpriteBatch Batch;
		Camera Cam;

		#endregion

		#region Constanstants

		static readonly Color GrassColor = new Color( 38, 127, 0 );
		static readonly Color WallColor = new Color( 128, 128, 128 );
		static readonly Color SpawnColor = new Color( 255, 255, 255 );
		static readonly Color ExitColor = new Color( 0, 0, 0 );
		static readonly Color FenceColor = new Color( 0, 38, 249 );
		static readonly Color DoorColor = new Color( 255, 106, 0 );
		static readonly Color FloorColor = new Color( 192, 192, 192 );
		static readonly Color GuardColor = new Color( 255, 0, 220 );
		static readonly Color InmateColor = new Color( 255, 216, 0 );

		#endregion
	}
}
