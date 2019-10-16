using NotesProject.ConsoleUI.Models.Enums;

namespace NotesProject.ConsoleUI.Services.Interfaces
{
	public interface INoteService
	{
		void Handle(string command);
        void Handle(Command command);
    }
}
