using ConsoleNotes.Services.Interfaces;
using System;

namespace ConsoleNotes.Services.Implementations
{
	public class ConsoleProvider : IConsoleProvider
	{
		public string ReadLine()
		{
			return Console.ReadLine();
		}

		public ConsoleKeyInfo ReadKey()
		{
			return Console.ReadKey();
		}

		public void Write(string input)
		{
			Console.Write(input);
		}

		public void WriteLine(string input)
		{
			Console.WriteLine(input);
		}
	}
}
