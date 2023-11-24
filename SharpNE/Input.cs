
using System.Runtime.InteropServices;

namespace SharpNE
{
	[StructLayout(LayoutKind.Sequential)]
	public struct WinPoint
	{
		public int x;
		public int y;
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct WinRect
	{
		public int left;
		public int top;
		public int right;
		public int bottom;
	}

	static class DisableConsoleQuickEdit
	{

		const uint ENABLE_QUICK_EDIT = 0x0040;

		// STD_INPUT_HANDLE (DWORD): -10 is the standard input device.
		const int STD_INPUT_HANDLE = -10;

		[DllImport("kernel32.dll", SetLastError = true)]
		static extern IntPtr GetStdHandle(int nStdHandle);

		[DllImport("kernel32.dll")]
		static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

		[DllImport("kernel32.dll")]
		static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

		internal static bool Go()
		{

			IntPtr consoleHandle = GetStdHandle(STD_INPUT_HANDLE);

			// get current console mode
			uint consoleMode;
			if (!GetConsoleMode(consoleHandle, out consoleMode))
			{
				// ERROR: Unable to get console mode.
				return false;
			}

			// Clear the quick edit bit in the mode flags
			consoleMode &= ~ENABLE_QUICK_EDIT;

			// set the new mode
			if (!SetConsoleMode(consoleHandle, consoleMode))
			{
				// ERROR: Unable to set console mode
				return false;
			}

			return true;
		}
	}

	public enum SNEButton
	{
		MouseLeft = 0x01,
		MouseRight = 0x02,
		MouseMiddle = 0x04,
		Backspace = 0x08,
		Tab = 0x09,
		Enter = 0x0D,
		Shift = 0x10,
		Control = 0x11,
		Alt = 0x12,
		CapsLock = 0x14,
		Esc = 0x1B,
		Space = 0x20,
		PageUp = 0x21,
		PageDown = 0x22,
		End = 0x23,
		Home = 0x24,
		ArrowLeft = 0x25,
		ArrowUp = 0x26,
		ArrowRight = 0x27,
		ArrowDown = 0x28,
		Del = 0x2E,
		_0 = 0x30,
		_1 = 0x31,
		_2 = 0x32,
		_3 = 0x33,
		_4 = 0x34,
		_5 = 0x35,
		_6 = 0x36,
		_7 = 0x37,
		_8 = 0x38,
		_9 = 0x39,
		A = 0x41,
		B = 0x42,
		C = 0x43,
		D = 0x44,
		E = 0x45,
		F = 0x46,
		G = 0x47,
		H = 0x48,
		I = 0x49,
		J = 0x4A,
		K = 0x4B,
		L = 0x4C,
		M = 0x4D,
		N = 0x4E,
		O = 0x4F,
		P = 0x50,
		Q = 0x51,
		R = 0x52,
		S = 0x53,
		T = 0x54,
		U = 0x55,
		V = 0x56,
		W = 0x57,
		X = 0x58,
		Y = 0x59,
		Z = 0x5A,
		Num0 = 0x60,
		Num1 = 0x61,
		Num2 = 0x62,
		Num3 = 0x63,
		Num4 = 0x64,
		Num5 = 0x65,
		Num6 = 0x66,
		Num7 = 0x67,
		Num8 = 0x68,
		Num9 = 0x69,
		F1 = 0x70,
		F2 = 0x71,
		F3 = 0x72,
		F4 = 0x73,
		F5 = 0x74,
		F6 = 0x75,
		F7 = 0x76,
		F8 = 0x77,
		F9 = 0x78,
		F10 = 0x79,
		F11 = 0x7A,
		F12 = 0x7B,
		NumLock = 0x90,
		ScrollLock = 0x91
	}

	public static class SNEInputManager
	{
		static IntPtr ConsoleWindowHandle;

		[DllImport("user32.dll")]
		static extern bool GetCursorPos(out WinPoint point);

		[DllImport("kernel32.dll")]
		static extern IntPtr GetConsoleWindow();

		[DllImport("user32.dll")]
		static extern bool GetWindowRect(IntPtr  hWnd, out WinRect lpRect);

		[DllImport("user32.dll")]
		public static extern bool GetAsyncKeyState(int button);

		static SNEInputManager()
		{
			ConsoleWindowHandle = GetConsoleWindow();
		}
		public static Vector2I AbsoluteMousePos()
		{
			GetCursorPos(out WinPoint pos);
			return new(pos.x, pos.y);
		}
		public static Vector2I ConsoleMousePos
		{
			get => AbsoluteMousePos() - GetConsolePos();
		}
		public static Vector2I GetConsolePos()
		{
			GetWindowRect(ConsoleWindowHandle, out WinRect wr);
			return new Vector2I(wr.left, wr.top);
		}
		public static bool ButtonPressed(SNEButton button)
		{
			return GetAsyncKeyState((int)button);
		}
	}
	public enum InputType
	{
		JustPressed,
		JustReleased,
		Pressed,
		Released
	}
	public class InputConsumer
	{
		
		public Vector2I PrevMousePos = new();
		public Vector2I CurrentMousePos = new();

		public Dictionary<SNEButton, bool> PrevInputState = new();
		public Dictionary<SNEButton, bool> CurrentInputState = new();

		public void Update()
		{
			PrevInputState = new (CurrentInputState);
			foreach(var key in (SNEButton[])Enum.GetValues(typeof(SNEButton)))
			{
				CurrentInputState[key] = SNEInputManager.ButtonPressed(key);
			}
			PrevMousePos = CurrentMousePos;
			CurrentMousePos = SNEInputManager.ConsoleMousePos;
		}

		public InputType ButtonState(SNEButton button)
		{
			if (CurrentInputState.Count == 0 || PrevInputState.Count == 0)
				return InputType.Released;

			if (CurrentInputState[button])
			{
				if (PrevInputState[button])
					return InputType.Pressed;
				else
					return InputType.JustPressed;
			}
			else
			{
				if (PrevInputState[button])
					return InputType.JustReleased;
				else
					return InputType.Released;
			}
		}
	}
}
