using NotesProject.Domain.Models.Entities;
using NotesProject.Infrastructure.Models;
using System.Collections.Generic;

namespace NotesProject.DataBase.Interfaces
{
	public interface INoteProvider
	{
		List<NoteDto> CreateNoteList();
	}
}
