using System;
using System.Collections.Generic;
using System.Text;

namespace NotesProject.Infrastructure.Models
{
    public class NoteDto
    {
        /// <summary>
		/// Уникальный идентификатор заметки.
		/// </summary>
		public int Id { get; set; }

        public NoteDetailsDto Details { get; set; }
    }
}
