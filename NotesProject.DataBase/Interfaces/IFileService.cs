using NotesProject.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NotesProject.DataBase.Interfaces
{
    public interface IFileService
    {
        int FindCurrentId();
        Task AddFile(NoteDetailsDto detailsDto, int id = 0);
        void DeleteFile(int id);
        Task EditFile(NoteDto noteDto);
        NoteDto GetDataFromFile(int id);
        bool IsFileExist(int id);
        List<NoteDto> GetDataFromAllFiles();
        List<int> AllIds();
        void CreateDirectory();
    }
}
