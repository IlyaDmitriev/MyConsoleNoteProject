using System;
using System.Collections.Generic;
using System.Text;

namespace NotesProject.ConsoleUI.Services.Interfaces
{
    public interface IConsoleRepository
    {
        void AddNote();
        void EditNote();
        void DeleteNote();
        void ShowHelp();
        void ExitFromApp();
        void ShowNotes();
    }
}
