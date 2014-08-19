// $Id$
// $Author: Matthias 'Sylence' Specht $

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LD25
{
	static class Randomizer
	{
		#region Constructor

		static Randomizer()
		{
			Rand = new Random();
		}

		#endregion

		#region Methods

		public static int Next( int max = int.MaxValue )
		{
			return Rand.Next( max );
		}

		#endregion

		#region Attributes

		static Random Rand;

		#endregion
	}
}
