using System;

namespace ConsoleNotes.Services.Interfaces
{
	public interface IConsoleProvider
	{
		void WriteLine(string input);
		void WriteLine();
		void Write(string input);
		string ReadLine();
		ConsoleKeyInfo ReadKey();
		void Clear();
		void SetTitle(string title);
	}
}
