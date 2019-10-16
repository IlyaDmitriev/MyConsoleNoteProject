using NotesProject.DataBase.Interfaces;
using NotesProject.Domain.Interfaces;
using NotesProject.Domain.Models.Entities;
using NotesProject.Domain.Models.ValueObjects;
using NotesProject.Infrastructure.Interfaces;
using NotesProject.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NotesProject.DataBase.Services
{
    public class DataBaseService : IContext
    {
        private static List<NoteDto> Notes;
        private static int CurrentId { get; set; }

        public DataBaseService(INoteProvider noteProvider)
        {
            Notes = noteProvider.CreateNoteList();
            CurrentId = Notes.Count != 0 ? Notes.OrderBy(x => x.Id).Last().Id + 1 : 1;
        }

        public void AddNote(NoteDetailsDto details)
        {
            Notes.Add(new NoteDto
            {
                Id = CurrentId,
                Details = new NoteDetailsDto { Title = details.Title, Text = details.Text }
            });
            CurrentId++;
        }

        public void DeleteNote(int id)
        {
            Notes.Remove(GetNote(id));
        }

        public void EditNote(Infrastructure.Models.NoteDto note)
        {
            var toEdit = GetNote(note.Id);

            toEdit.Details = note.Details;
        }

        public NoteDto GetNote(int id)
        {
            return Notes.First(x => x.Id == id);
        }

        public List<Infrastructure.Models.NoteDto> GetNotes()
        {
            return Notes;
        }

        public bool IsNoteExist(int id)
        {
            return Notes.Any(x => x.Id == id);
        }
    }
}
