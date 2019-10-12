using NotesProject.Business.Models;
using System.Collections.Generic;

namespace NotesProject.Business.Services.Interfaces
{
	public interface INoteRepository
	{
		void AddNote(string title, string text);
		void EditNote(int id, string title, string text);
		void DeleteNote(int id);
		bool IsNoteExist(int id);
		Note GetNote(int id);
		List<Note> GetNotes();
	}
}
