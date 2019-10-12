using NotesProject.Business.Services.Interfaces;
using NotesProject.Business.Models;
using System.Collections.Generic;
using System.Linq;
using NotesProject.Business.Provider;

namespace NotesProject.Business.Services.Implementations
{
	public class NoteRepository : INoteRepository
	{
		private static List<Note> Notes { get; set; }
		private static int CurrentId { get; set; }
		public NoteRepository(INoteProvider noteProvider)
		{
			Notes = noteProvider.CreateNoteList();
			CurrentId = Notes.Count != 0 ? Notes.OrderBy(x => x.Id).Last().Id + 1 : 1; 
		}

		public void AddNote(string title, string text)
		{
			Notes.Add(new Note
			{
				Id = CurrentId,
				Title = title,
				Text = text,
			});
			CurrentId++;
		}

		public void EditNote(int id, string title, string text)
		{
			var toEdit = GetNote(id);

			toEdit.Title = title;
			toEdit.Text = text;
		}

		public void DeleteNote(int id)
		{
			Notes.Remove(GetNote(id));
		}

		public bool IsNoteExist(int id)
		{
			return Notes.Any(x => x.Id == id);
		}

		public Note GetNote(int id)
		{
			return Notes.First(x => x.Id == id);
		}

		public List<Note> GetNotes()
		{
			return Notes;
		}
	}
}
