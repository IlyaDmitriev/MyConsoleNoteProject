using ConsoleNotes.Helpers;
using ConsoleNotes.Services.Interfaces;
using ConsoleNotes.Models;
using System;

namespace ConsoleNotes
{
	public class NoteApp
	{
		private readonly ICommandService _commandService;

		public NoteApp(ICommandService commandService)
		{
			_commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

			Console.Title = $"{Constants.ProjectName} {Constants.Version}";
		}

		public void Run()
		{
			while (true)
			{
				CommandHelper.BackToTheRoots();
				CommandHelper.ShowInitialWindow();
				_commandService.Handle(Console.ReadLine().Trim());
			}
		}
	}
}
