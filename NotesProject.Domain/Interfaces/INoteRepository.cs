using NotesProject.Domain.Models.Entities;
using NotesProject.Domain.Models.ValueObjects;
using System.Collections.Generic;

namespace NotesProject.Domain.Interfaces
{
	public interface INoteRepository
	{
		void AddNote(NoteDetails details);
		void EditNote(Note note);
		void DeleteNote(int id);
		bool IsNoteExist(int id);
		Note GetNote(int id);
		List<Note> GetNotes();
	}
}
