namespace NotesProject.Business.Models
{
	public class Note
	{
		/// <summary>
		/// Уникальный идентификатор заметки.
		/// </summary>
		public int Id { get; set; }

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
