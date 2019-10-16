using NotesProject.Domain.Models;
using NotesProject.Domain.Models.Entities;
using NotesProject.Domain.Models.ValueObjects;
using NotesProject.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotesProject.Infrastructure.Interfaces
{
    public interface IContext
    {
        void AddNote(NoteDetailsDto details);
        void EditNote(NoteDto note);
        void DeleteNote(int id);
        bool IsNoteExist(int id);
        NoteDto GetNote(int id);
        List<NoteDto> GetNotes();
    }
}
