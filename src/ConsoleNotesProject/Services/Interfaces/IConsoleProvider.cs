using System;

namespace ConsoleNotes.Services.Interfaces
{
	public interface IConsoleProvider
	{
		void WriteLine(string input);
		void Write(string input);
		string ReadLine();
		ConsoleKeyInfo ReadKey();
	}
}
