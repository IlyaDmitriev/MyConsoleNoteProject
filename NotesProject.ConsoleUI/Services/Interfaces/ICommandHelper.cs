using NotesProject.ConsoleUI.Models.Enums;

namespace NotesProject.ConsoleUI.Services.Interfaces
{
	public interface ICommandHelper
	{
		void BackToTheRoots();
        (Command, bool) ParcingCommands(string strCommand);
    }
}
