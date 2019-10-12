using ConsoleNotes.Services.Interfaces;
using System;
using System.Collections.Generic;

namespace ConsoleNotes.Services.Implementations
{
	public class FakeConsoleProvider : IConsoleProvider
	{
		public string Output;
		public List<string> LinesToRead;
		private static int CurrentReadLineCall;

		public FakeConsoleProvider(List<string> linesToRead)
		{
			Output = string.Empty;
			LinesToRead = linesToRead;
		}

		public string ReadLine()
		{
			var line = LinesToRead.Count > CurrentReadLineCall
				? LinesToRead[CurrentReadLineCall]
				: string.Empty;

			CurrentReadLineCall++;
			return line;
		}

		public ConsoleKeyInfo ReadKey()
		{
			return new ConsoleKeyInfo();
		}

		public void Write(string input)
		{
			Output += input;
		}

		public void WriteLine(string input)
		{
			Output += input + Environment.NewLine;
		}

		public void WriteLine()
		{
			Output += Environment.NewLine;
		}

		public void Clear()
		{
			Output = string.Empty;
		}

		public void SetTitle(string title)
		{			
		}
	}
}