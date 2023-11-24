using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpNE
{

	public struct Vector2I
	{
		public int X { get; set; }
		public int Y { get; set; }

		public Vector2I(int x, int y)
		{
			X = x;
			Y = y;
		}

		public override string ToString()
		{
			return $"({X};{Y})";
		}

		public static Vector2I operator + (Vector2I a, Vector2I b)
		{
			return new(a.X + b.X, a.Y + b.Y);
		}
		public static Vector2I operator -(Vector2I a, Vector2I b)
		{
			return new(a.X - b.X, a.Y - b.Y);
		}
		public static Vector2I operator *(Vector2I a, Vector2I b)
		{
			return new(a.X * b.X, a.Y * b.Y);
		}
		public static Vector2I operator /(Vector2I a, Vector2I b)
		{
			return new((int)(a.X / (float)b.X), (int)(a.Y / (float)b.Y));
		}
	}
}
