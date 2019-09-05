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

		public CommandService(INoteService noteService)
		{
			_noteService = noteService ?? throw new ArgumentNullException(nameof(noteService));
		}

		public void Handle(string strCommand)
		{
			if (!Enum.TryParse(strCommand.Capitalize(), out Command command))
			{
				Console.WriteLine($"Wrong input! Press any key to proceed...");
				Console.ReadKey();
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

					Console.WriteLine("Do you want to add another note? (y/n)");
					CommandHelper.DoActionOnResponse(Console.ReadLine(), () => { Handle(Command.Add); }, () => { CommandHelper.BackToTheRoots(); });

					break;

				case Command.List:
					_noteService.ShowNotes();

					Console.WriteLine("Press any key to return to the main window...");
					Console.ReadKey();
					CommandHelper.DoActionOnResponse("y", () => { CommandHelper.BackToTheRoots(); }, () => { });

					break;

				case Command.Delete:
					_noteService.DeleteNote();

					Console.WriteLine("Do you want to delete another note? (y/n)");
					CommandHelper.DoActionOnResponse(Console.ReadLine(), () => { Handle(Command.Delete); }, () => { CommandHelper.BackToTheRoots(); });

					break;

				case Command.Edit:
					_noteService.EditNote();

					Console.WriteLine("Do you want to edit another note? (y/n)");
					CommandHelper.DoActionOnResponse(Console.ReadLine(), () => { Handle(Command.Edit); }, () => { CommandHelper.BackToTheRoots(); });

					break;

				case Command.Help:
					_noteService.ShowHelp();

					Console.WriteLine("Press any key to return to the main window...");
					Console.ReadKey();
					CommandHelper.DoActionOnResponse("y", () => { CommandHelper.BackToTheRoots(); }, () => { });

					break;

				case Command.Exit:
					_noteService.ExitFromApp();

					break;
			}
		}
	}
}
