using System;
using System.Linq;
using System.Collections.Generic;
using NotesProject.ConsoleUI.Models.Enums;
using NotesProject.ConsoleUI.Services.Interfaces;
using NotesProject.Domain.Interfaces;
using NotesProject.Domain.Extensions;
using NotesProject.Domain.Models.ValueObjects;
using NotesProject.Domain.Models.Entities;

namespace NotesProject.ConsoleUI.Services.Implementations
{
	public class NoteConsoleService : INoteService
	{
		private readonly ICommandHelper _commandHelper;
        private readonly IConsoleRepository _consoleRepository;

        public NoteConsoleService(
			ICommandHelper commandHelper,
            IConsoleRepository consoleRepository)
		{
			_commandHelper = commandHelper ?? throw new ArgumentNullException(nameof(commandHelper));
            _consoleRepository = consoleRepository ?? throw new ArgumentNullException(nameof(consoleRepository));
		}

		public void Handle(string strCommand)
		{
            (var command, var successful) = _commandHelper.ParcingCommands(strCommand);
            if (successful == false)
                return;

            Handle(command);
		}

		public void Handle(Command command)
		{
			_commandHelper.BackToTheRoots();

			switch (command)
			{
				case Command.Add:
                    _consoleRepository.AddNote();

					break;

				case Command.List:
                    _consoleRepository.ShowNotes();

					break;

				case Command.Delete:
                    _consoleRepository.DeleteNote();
					
					break;

				case Command.Edit:
                    _consoleRepository.EditNote();

					break;

				case Command.Help:
                    _consoleRepository.ShowHelp();

					break;

				case Command.Exit:
                    _consoleRepository.ExitFromApp();

					break;
			}
		}
	}
}
