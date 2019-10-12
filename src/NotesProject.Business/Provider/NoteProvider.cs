using NotesProject.Business.Models;
using NotesProject.Business.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace NotesProject.Business.Provider
{
	public class NoteProvider : INoteProvider
	{
		public List<Note> CreateNoteList()
		{
			return new List<Note>();
		}
	}
}
