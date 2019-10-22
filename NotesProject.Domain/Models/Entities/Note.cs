using NotesProject.Domain.Models.ValueObjects;

namespace NotesProject.Domain.Models.Entities
{
	public class Note
	{
		/// <summary>
		/// Уникальный идентификатор заметки.
		/// </summary>
		public int Id { get; set; }

        public NoteDetails Details { get; set; }
	}
}
