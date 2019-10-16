using System;
using NotesProject.ConsoleUI.Models;
using NotesProject.ConsoleUI.Services.Interfaces;
using NotesProject.ConsoleUI.Models.Enums;

namespace NotesProject.ConsoleUI
{
	public class NoteApp
	{
		private readonly INoteService _noteService;
		private readonly ICommandHelper _commandHelper;
		private readonly IConsoleProvider _console;

		public NoteApp(
			INoteService noteService,
			ICommandHelper commandHelper,
			IConsoleProvider console)
		{
			_noteService = noteService ?? throw new ArgumentNullException(nameof(noteService));
			_commandHelper = commandHelper ?? throw new ArgumentNullException(nameof(commandHelper));
			_console = console ?? throw new ArgumentNullException(nameof(console));

			_console.SetTitle($"{Constants.ProjectName} {Constants.Version}");
		}

		public void Run()
		{
			while (true)
			{
				_commandHelper.BackToTheRoots();
				ShowInitialWindow();
				_noteService.Handle(_console.ReadLine().Trim());
			}
		}
		private void ShowInitialWindow()
		{
			_console.WriteLine("#################################################################");
			_console.WriteLine("#                           Enter command                       #");
			_console.WriteLine($"#                Enter '{nameof(Command.Help)}', if you need help                 #");
			_console.WriteLine("#################################################################");
			_console.Write("> ");
		}
	}
}
