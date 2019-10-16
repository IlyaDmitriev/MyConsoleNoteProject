using ConsoleNotes.Services.Interfaces;
using ConsoleNotes.Models.Enums;
using System;
using System.Linq;
using System.Collections.Generic;

namespace ConsoleNotes.Services.Implementations
{
	public class NoteConsoleService : INoteService
	{
		private readonly Dictionary<Command, string> _commandsWithDescription;
		private readonly INoteRepository _noteRepository;
		private readonly IConsoleProvider _console;
		private readonly ICommandHelper _commandHelper;

		public NoteConsoleService(
			INoteRepository noteRepository,
			IConsoleProvider console,
			ICommandHelper commandHelper)
		{
			_noteRepository = noteRepository ?? throw new ArgumentNullException(nameof(noteRepository));
			_console = console ?? throw new ArgumentNullException(nameof(console));
			_commandHelper = commandHelper ?? throw new ArgumentNullException(nameof(commandHelper));

			_commandsWithDescription = new Dictionary<Command, string>
			{
				{ Command.Add, "create new note" },
				{ Command.List, "show all created notes" },
				{ Command.Edit, "edit note" },
				{ Command.Delete, "delete note" },
				{ Command.Help, "get help" },
				{ Command.Exit, "exit from application" }
			};
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
			_commandHelper.BackToTheRoots();

			switch (command)
			{
				case Command.Add:
					AddNote();

					break;

				case Command.List:
					ShowNotes();

					break;

				case Command.Delete:
					DeleteNote();
					
					break;

				case Command.Edit:
					EditNote();

					break;

				case Command.Help:
					ShowHelp();

					break;

				case Command.Exit:
					ExitFromApp();

					break;
			}
		}

		private void AddNote()
		{
			string getConsoleString(string message)
			{
				_console.WriteLine(message);
				_console.Write("> ");
				return _console.ReadLine();
			}

			var title = getConsoleString("Enter note title");
			var text = getConsoleString("Enter note text");

			if (!(string.IsNullOrWhiteSpace(title)
				&& string.IsNullOrWhiteSpace(text)))
			{
				_noteRepository.AddNote(title, text);
				_console.WriteLine("Note was added.");
			}
			else
			{
				_console.WriteLine("Note was not added. Enter title or text.");
			}

			_console.WriteLine("Do you want to add another note? (y/n)");
			DoActionOnResponse(
				_console.ReadLine(),
				() => { Handle(Command.Add); },
				() => { _commandHelper.BackToTheRoots(); });
		}

		private void ShowNotes()
		{
			var notes = _noteRepository.GetNotes();

			if (notes.Any())
			{
				_console.WriteLine("Here are list of all notes:");

				notes.ForEach(x =>
				{
					_console.WriteLine($"id: {x.Id} || title: '{x.Title}' || text: '{x.Text}'");
				});
			}
			else
			{
				_console.WriteLine("There are zero notes.");
			}

			_console.WriteLine("Press any key to return to the main window...");
			_console.ReadKey();
			DoActionOnResponse("y", () => { _commandHelper.BackToTheRoots(); }, () => { });
		}

		private void DeleteNote()
		{
			_console.WriteLine("Please enter id of note to delete:");
			var input = _console.ReadLine();
			var successfulParsing = Int32.TryParse(input, out var id);

			if (successfulParsing)
			{
				if (_noteRepository.IsNoteExist(id))
				{
					_noteRepository.DeleteNote(id);
					_console.WriteLine($"The note with id [{id}] was successfully deleted.");
				}
				else
				{
					_console.WriteLine($"The note with id [{id}] is not exist in the list of notes.");
				}
			}
			else
			{
				_console.WriteLine($"Id [{input}] is not a number.");
			}

			_console.WriteLine("Do you want to delete another note? (y/n)");
			DoActionOnResponse(
				_console.ReadLine(),
				() => { Handle(Command.Delete); },
				() => { _commandHelper.BackToTheRoots(); });
		}

		private void EditNote()
		{
			_console.WriteLine("Please enter id of note to edit:");
			var input = _console.ReadLine();

			if (Int32.TryParse(input, out var id))
			{
				if (_noteRepository.IsNoteExist(id))
				{
					var toEdit = _noteRepository.GetNote(id);
					_console.WriteLine($"Current title of this note: {toEdit.Title}. Pick a new title:");
					_console.Write("> ");

					var newTitle = _console.ReadLine();
					_console.WriteLine($"Are you sure (y/n)?");

					var response = _console.ReadLine();

					Action actionYes = () =>
					{
						_console.WriteLine($"Current text of this note: {toEdit.Text}. Pick a new text:");
						_console.Write("> ");

						var newText = _console.ReadLine();
						if ((!string.IsNullOrWhiteSpace(newTitle) || !string.IsNullOrWhiteSpace(newText)))
						{
							_noteRepository.EditNote(toEdit.Id, newTitle, newText);
							_console.WriteLine("Text was successfully changed.");
						}
						else
						{
							_console.WriteLine("Text was NOT successfully changed. Enter title or text.");
						}
					};

					DoActionOnResponse(
						response,
						actionYes,
						() => { _commandHelper.BackToTheRoots(); });
				}
				else
				{
					_console.WriteLine($"The note with id [{id}] is not exist in the list of notes.");
				}
			}
			else
			{
				_console.WriteLine($"Id [{input}] is not a number.");
			}

			_console.WriteLine("Do you want to edit another note? (y/n)");
			DoActionOnResponse(
				_console.ReadLine(),
				() => { Handle(Command.Edit); },
				() => { _commandHelper.BackToTheRoots(); });
		}

		private void ShowHelp()
		{
			_console.WriteLine("#################################################################");
			_console.WriteLine("#                             List of commands                    ");

			foreach (var command in _commandsWithDescription)
			{
				_console.WriteLine($"#      Enter '{command.Key}', if you need to {command.Value}");
			}

			_console.WriteLine("#################################################################");

			_console.WriteLine("Press any key to return to the main window...");
			_console.ReadKey();
			DoActionOnResponse(
				"y",
				() => { _commandHelper.BackToTheRoots(); },
				() => { });
		}

		private void ExitFromApp()
		{
			Environment.Exit(0);
		}

		private void DoActionOnResponse(string response, Action actionYes, Action actionNo)
		{
			var formattedResult = response.Trim().ToLower();

			if (formattedResult == "y")
			{
				actionYes();
			}
			else if (formattedResult == "n")
			{
				actionNo();
			}
			else
			{
				_console.WriteLine("Wrong input! Pass only \"y\" or \"n\".");
				DoActionOnResponse(_console.ReadLine().Trim().ToLower(), actionYes, actionNo);
			}
		}
	}
}
