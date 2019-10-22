using System;
using System.Collections.Generic;
using System.Text;

namespace NotesProject.Infrastructure.Models
{
    public class NoteDetailsDto
    {
        /// <summary>
		/// Текст заметки.
		/// </summary>
		public string Text { get; set; }

        /// <summary>
        /// Заголовок заметки.
        /// </summary>
        public string Title { get; set; }
    }
}
