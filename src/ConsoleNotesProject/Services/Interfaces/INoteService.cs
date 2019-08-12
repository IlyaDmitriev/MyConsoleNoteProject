namespace ConsoleNotes.Services.Interfaces
{
	public interface INoteService
	{
		void AddNote();
		void ShowNotes();
		void DeleteNote();
		void EditNote();
		void ShowHelp();
	}
}
