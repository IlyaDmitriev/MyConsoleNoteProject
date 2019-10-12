using NotesProject.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotesProject.Business.Services.Interfaces
{
	public interface INoteProvider
	{
		List<Note> CreateNoteList();
	}
}
