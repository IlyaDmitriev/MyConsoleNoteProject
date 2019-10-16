using NotesProject.Domain.Interfaces;
using NotesProject.Domain.Models;
using NotesProject.Domain.Models.Entities;
using NotesProject.Domain.Models.ValueObjects;
using NotesProject.Infrastructure.Interfaces;
using NotesProject.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NotesProject.Application.Repositories
{
	public class NoteRepository : INoteRepository
	{
        private readonly IContext _context;

		public NoteRepository(IContext context)
		{
            _context = context ?? throw new ArgumentNullException(nameof(context));
		}

		public void AddNote(NoteDetails details)
		{
            _context.AddNote(new NoteDetailsDto { Title = details.Title, Text = details.Text });
		}

		public void EditNote(Note note)
		{
            _context.EditNote(new NoteDto { Id = note.Id, Details = new NoteDetailsDto { Title = note.Details.Title, Text = note.Details.Text } });
		}

		public void DeleteNote(int id)
		{
            _context.DeleteNote(id);
		}

		public bool IsNoteExist(int id)
		{
            return _context.IsNoteExist(id);
		}

		public Note GetNote(int id)
		{
            var noteDto = _context.GetNote(id);
            return new Note { Id = noteDto.Id, Details = new NoteDetails { Title = noteDto.Details.Title, Text = noteDto.Details.Text } };
		}

        public List<Note> GetNotes()
        {
            var notesDto = _context.GetNotes();
            var notes = new List<Note>();
            notesDto.ForEach(x => notes.Add(new Note { Id = x.Id, Details = new NoteDetails { Title = x.Details.Title, Text = x.Details.Text } }));

            return notes;
        }
	}
}
