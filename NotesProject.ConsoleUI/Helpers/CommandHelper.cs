using NotesProject.ConsoleUI.Models;
using NotesProject.ConsoleUI.Models.Enums;
using NotesProject.ConsoleUI.Services.Interfaces;
using NotesProject.Domain.Extensions;
using System;

namespace NotesProject.ConsoleUI.Helpers
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

		public (Command, bool) ParcingCommands(string strCommand)
        {
            if (!Enum.TryParse(strCommand.Capitalize(), out Command command))
            {
                _console.WriteLine($"Wrong input! Press any key to proceed...");
                _console.ReadKey();
                return (command, false);
            }

            return (command, true);
        }
	}
}
