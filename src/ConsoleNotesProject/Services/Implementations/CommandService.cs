using ConsoleNotes.Helpers;
using ConsoleNotes.Services.Interfaces;
using ConsoleNotes.Models.Enums;
using NotesProject.Business.Extensions;
using System;

namespace ConsoleNotes.Services.Implementations
{
	public class CommandService : ICommandService
	{
		private readonly INoteService _noteService;
		private readonly IConsoleProvider _console;

		public CommandService(INoteService noteService, IConsoleProvider console)
		{
			_noteService = noteService ?? throw new ArgumentNullException(nameof(noteService));
			_console = console ?? throw new ArgumentNullException(nameof(console));
		}

		public void Handle(string strCommand)
		{
			if (!Enum.TryParse(strCommand.Capitalize(), out Command command))
			{
				_console.WriteLine($"Wrong input! Press any key to proceed...");
				_console.ReadKey();
				return;
			}

			Handle(command);
		}

		public void Handle(Command command)
		{
			CommandHelper.BackToTheRoots();

			switch (command)
			{
				case Command.Add:
					_noteService.AddNote();

					_console.WriteLine("Do you want to add another note? (y/n)");
					CommandHelper.DoActionOnResponse(_console.ReadLine(), () => { Handle(Command.Add); }, () => { CommandHelper.BackToTheRoots(); });

					break;

				case Command.List:
					_noteService.ShowNotes();

					_console.WriteLine("Press any key to return to the main window...");
					_console.ReadKey();
					CommandHelper.DoActionOnResponse("y", () => { CommandHelper.BackToTheRoots(); }, () => { });

					break;

				case Command.Delete:
					_noteService.DeleteNote();

					_console.WriteLine("Do you want to delete another note? (y/n)");
					CommandHelper.DoActionOnResponse(_console.ReadLine(), () => { Handle(Command.Delete); }, () => { CommandHelper.BackToTheRoots(); });

					break;

				case Command.Edit:
					_noteService.EditNote();

					_console.WriteLine("Do you want to edit another note? (y/n)");
					CommandHelper.DoActionOnResponse(_console.ReadLine(), () => { Handle(Command.Edit); }, () => { CommandHelper.BackToTheRoots(); });

					break;

				case Command.Help:
					_noteService.ShowHelp();

					_console.WriteLine("Press any key to return to the main window...");
					_console.ReadKey();
					CommandHelper.DoActionOnResponse("y", () => { CommandHelper.BackToTheRoots(); }, () => { });

					break;

				case Command.Exit:
					_noteService.ExitFromApp();

					break;
			}
		}
	}
}
