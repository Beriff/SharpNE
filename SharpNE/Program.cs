

using System.Text;

namespace SharpNE
{
	public class Program
	{

		static void Main(string[] args)
		{

			SNEColor mint = new(207, 255, 209);

			Console.WriteLine($"{mint.Foreground}Hello SharpNE{SNEColor.Reset}");
		}
	}
}