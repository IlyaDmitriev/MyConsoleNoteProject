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

		public NoteConsoleService(INoteRepository noteRepository)
		{
			_noteRepository = noteRepository ?? throw new ArgumentNullException(nameof(noteRepository));

			_commandsWithDescription = new Dictionary<Command, string>
			{
				{ Command.Add, "create new note" },
				{ Command.List, "show all created notes" },
				{ Command.Edit, "edit note" },
				{ Command.Delete, "delete note" },
				{ Command.Help, "get help" },
			};
		}

		public void AddNote()
		{
			string getConsoleString(string message)
			{
				Console.WriteLine(message);
				Console.Write("> ");
				return Console.ReadLine();
			}

			var title = getConsoleString("Enter note title");
			var text = getConsoleString("Enter note text");
			 
			_noteRepository.AddNote(title, text);			

			Console.WriteLine("Note was added.");
		}

		public void ShowNotes()
		{
			var notes = _noteRepository.GetNotes();

			if (notes.Any())
			{
				Console.WriteLine("Here are list of all notes:");

				notes.ForEach(x =>
				{
					Console.WriteLine($"[{x.Id}] - {x.Title}");
				});
			}
			else
			{
				Console.WriteLine("There are zero notes.");
			}
		}

		public void DeleteNote()
		{
			Console.WriteLine("Please enter id of note to delete:");
			var successfulParsing = Int32.TryParse(Console.ReadLine(), out var id);

			if (successfulParsing)
			{
				if (_noteRepository.IsNoteExist(id))
				{
					_noteRepository.DeleteNote(id);
					Console.WriteLine($"The note with id [{id}] was successfully deleted.");
				}
				else
				{
					Console.WriteLine($"The note with id [{id}] is not exist in the list of notes.");
				}
			}
			else
			{
				Console.WriteLine($"Id [{id}] is not a number.");
			}
		}

		public void EditNote()
		{
			Console.WriteLine("Please enter id of note to edit:");
			var input = Console.ReadLine();

			if (Int32.TryParse(input, out var id))
			{
				if (_noteRepository.IsNoteExist(id))
				{
					var toEdit = _noteRepository.GetNote(id);
					Console.WriteLine($"Current title of this note: {toEdit.Title}. Pick a new title:");
					Console.Write("> ");

					var newTitle = Console.ReadLine();
					Console.WriteLine($"Are you sure (y/n)?");

					var response = Console.ReadLine();

					Action actionYes = () =>
					{
						Console.WriteLine("Title was successfully changed.");
						Console.WriteLine("Current text of this note:");
						Console.WriteLine(toEdit.Text);
						Console.WriteLine("Now you can change the text:");
						Console.Write("> ");

						var newText = Console.ReadLine();
						_noteRepository.EditNote(toEdit.Id, newTitle, newText);
						Console.WriteLine("Text was successfully changed.");
					};

					CommandHelper.DoActionOnResponse(response, actionYes, () => { CommandHelper.BackToTheRoots(); });
				}
				else
				{
					Console.WriteLine($"The note with id [{id}] is not exist in the list of notes.");
				}
			}
			else
			{
				Console.WriteLine($"Id [{input}] is not a number.");
			}
		}

		public void ShowHelp()
		{
			CommandHelper.ShowHelp(_commandsWithDescription);
		}
	}
}
