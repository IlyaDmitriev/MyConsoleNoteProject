using ConsoleNotes.Helpers;
using ConsoleNotes.Services.Interfaces;
using ConsoleNotes.Models.Enums;
using NotesProject.Business.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleNotes.Services.Implementations
{
	public class NoteConsoleService : INoteService
	{
		private readonly Dictionary<Command, string> _commandsWithDescription;
		private readonly INoteRepository _noteRepository;
		private readonly IConsoleProvider _console;

		public NoteConsoleService(INoteRepository noteRepository, IConsoleProvider console)
		{
			_noteRepository = noteRepository ?? throw new ArgumentNullException(nameof(noteRepository));
			_console = console ?? throw new ArgumentNullException(nameof(console));

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

		public void AddNote()
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
		}

		public void ShowNotes()
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
		}

		public void DeleteNote()
		{
			_console.WriteLine("Please enter id of note to delete:");
			var successfulParsing = Int32.TryParse(_console.ReadLine(), out var id);

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
				_console.WriteLine($"Id [{id}] is not a number.");
			}
		}

		public void EditNote()
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
						_console.WriteLine("Current text of this note:");
						_console.WriteLine(toEdit.Text);
						_console.WriteLine("Now you can change the text:");
						_console.Write("> ");

						var newText = _console.ReadLine();
						if ((!string.IsNullOrWhiteSpace(newTitle) || !string.IsNullOrWhiteSpace(newText)))
						{
							_noteRepository.EditNote(toEdit.Id, newTitle, newText);
							_console.WriteLine("Text was successfully changed.");
						}
						else
						{
							_console.WriteLine("Text was NOT successfully changed. Enter title or text");
						}
					};

					CommandHelper.DoActionOnResponse(response, actionYes, () => { CommandHelper.BackToTheRoots(); });
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
		}

		public void EditNote1(int id, string newTitle)
		{
			_console.WriteLine("Please enter id of note to edit:");
			var input = _console.ReadLine();

			if (Int32.TryParse(input, out id))
			{
				if (_noteRepository.IsNoteExist(id))
				{
					var toEdit = _noteRepository.GetNote(id);
					_console.WriteLine($"Current title of this note: {toEdit.Title}. Pick a new title:");
					_console.Write("> ");

					newTitle = _console.ReadLine();
					_console.WriteLine($"Are you sure (y/n)?");

					var response = _console.ReadLine();

					Action actionYes = () =>
					{
						_console.WriteLine("Title was successfully changed.");
						_console.WriteLine("Current text of this note:");
						_console.WriteLine(toEdit.Text);
						_console.WriteLine("Now you can change the text:");
						_console.Write("> ");

						var newText = _console.ReadLine();
						_noteRepository.EditNote(toEdit.Id, newTitle, newText);
						_console.WriteLine("Text was successfully changed.");
					};

					CommandHelper.DoActionOnResponse(response, actionYes, () => { CommandHelper.BackToTheRoots(); });
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
		}

		public void ShowHelp()
		{
			CommandHelper.ShowHelp(_commandsWithDescription);
		}

		public void ExitFromApp()
		{
			Environment.Exit(0);
		}
	}
}
