using NotesProject.DataBase.Interfaces;
using NotesProject.Domain.Interfaces;
using NotesProject.Domain.Models.Entities;
using NotesProject.Domain.Models.ValueObjects;
using NotesProject.Infrastructure.Interfaces;
using NotesProject.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NotesProject.DataBase.Services
{
    public class DataBaseService : IContext
    {
        private static List<NoteDto> Notes;

        private readonly IFileService _fileService;
        private static int CurrentId { get; set; }

        public DataBaseService(INoteProvider noteProvider, IFileService fileService)
        {
            _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            Notes = noteProvider.CreateNoteList();
            CurrentId = Notes.Count != 0 ? Notes.OrderBy(x => x.Id).Last().Id + 1 : 1;

            _fileService.CreateDirectory();
        }

        public void AddNote(NoteDetailsDto details)
        {
            //Notes.Add(new NoteDto
            //{
            //    Id = CurrentId,
            //    Details = new NoteDetailsDto { Title = details.Title, Text = details.Text }
            //});
            //CurrentId++;

            _fileService.AddFile(details);
        }

        public void DeleteNote(int id)
        {
            //Notes.Remove(GetNote(id));

            _fileService.DeleteFile(id);
        }

        public void EditNote(NoteDto note)
        {
            //var toEdit = GetNote(note.Id);

            //toEdit.Details = note.Details;

            _fileService.EditFile(note);
        }

        public NoteDto GetNote(int id)
        {
            //return Notes.First(x => x.Id == id);

            return _fileService.GetDataFromFile(id);
        }

        public List<NoteDto> GetNotes()
        {
            //return Notes;

            return _fileService.GetDataFromAllFiles();
        }

        public bool IsNoteExist(int id)
        {
            //return Notes.Any(x => x.Id == id);

            return _fileService.IsFileExist(id);
        }
    }
}
