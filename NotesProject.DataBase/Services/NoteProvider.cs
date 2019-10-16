using NotesProject.DataBase.Interfaces;
using NotesProject.Domain.Interfaces;
using NotesProject.Domain.Models.Entities;
using NotesProject.Infrastructure.Models;
using System.Collections.Generic;

namespace NotesProject.Domain.Services
{
	public class NoteProvider : INoteProvider
	{
		public List<NoteDto> CreateNoteList()
		{
			return new List<NoteDto>();
		}
	}
}
