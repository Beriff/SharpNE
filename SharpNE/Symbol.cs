using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpNE
{
	public class SNEException : Exception
	{
		public SNEException(string message) : base(message) { }
	}
	public struct SNEColor
	{
		public byte R;
		public byte G;
		public byte B;
		public readonly string Foreground { get => $"\x1b[38;2;{R};{G};{B}m"; }
		public readonly string Background { get => $"\x1b[48;2;{R};{G};{B}m"; }
		public static string Reset { get => "\u001b[0m"; }

		public SNEColor(byte r, byte g, byte b)
		{
			R = r;
			G = g;
			B = b;
		}
	}
}
