using ConsoleNotes.Models;
using ConsoleNotes.Models.Enums;
using ConsoleNotes.Services.Interfaces;
using System;

namespace ConsoleNotes.Helpers
{
	public class CommandHelper : ICommandHelper
	{
		private readonly IConsoleProvider _console;

		public CommandHelper(IConsoleProvider console)
		{
			_console = console ?? throw new ArgumentNullException(nameof(console));
		}

		public void BackToTheRoots()
		{
			_console.Clear();
			_console.WriteLine($"--->     {Constants.ProjectName} {Constants.Version}     <---");
			_console.WriteLine();
		}

		
	}
}
