using NotesProject.DataBase.Interfaces;
using NotesProject.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesProject.DataBase.FileServices
{
    public class FileService : IFileService
    {
        private string Path => @"D:\NotesStorage";

        public async Task AddFile(NoteDetailsDto detailsDto, int id = 0)
        {
            if(id == 0)
            {
                id = FindCurrentId();
            }

            using (FileStream fstream = new FileStream(Path + $@"\{id}.txt", FileMode.Create))
            {
                byte[] input = Encoding.Default.GetBytes($"id: {id} || title: {detailsDto.Title} || text: {detailsDto.Text}");
                await fstream.WriteAsync(input, 0, input.Length);
            }
        }

        public void DeleteFile(int id)
        {
            if (IsFileExist(id))
            {
                var fileInf = new FileInfo(Path + $@"\{id}.txt");
                fileInf.Delete();
            }
        }

        public async Task EditFile(NoteDto noteDto)
        {
            if (IsFileExist(noteDto.Id))
            {
                DeleteFile(noteDto.Id);
                await AddFile(noteDto.Details, noteDto.Id);
            }
        }

        public int FindCurrentId()
        {
            var ids =  AllIds();
            return ids.Count != 0 ? ids.Max() + 1 : 1;
        }

        public List<NoteDto> GetDataFromAllFiles()
        {
            var notes = new List<NoteDto>();
            var ids = AllIds();
            if (ids.Count != 0)
            {
                ids.ForEach(x =>
                {
                    var data = GetDataFromFile(x);
                    if(data != null)
                    {
                        notes.Add(data);
                    }
                });
            }

            return notes;
        }

        public NoteDto GetDataFromFile(int id)
        {
            if (IsFileExist(id))
            {
                var fileData = File.ReadAllText(Path + $@"\{id}.txt");
                if (!string.IsNullOrWhiteSpace(fileData))
                {
                    var title = fileData.Split("title: ")[1].Split(" || text:")[0].Trim();
                    var text = fileData.Split(" || text:")[1].Trim();
                    return new NoteDto { Id = id, Details = new NoteDetailsDto { Title = title, Text = text } };
                }
            }

            return null;
        }

        public bool IsFileExist(int id)
        {
            var fileInf = new FileInfo(Path + $@"\{id}.txt");
            return fileInf.Exists;
        }

        public List<int> AllIds()
        {
            var validFiles = new List<int>();
            var filesname = Directory.GetFiles(Path, "*.txt").ToList<string>();
            filesname.ForEach(x =>
            {
                int.TryParse(x.Split(Path + @"\")[1].Split(".txt")[0], out var _id);
                if (_id != 0)
                {
                    validFiles.Add(_id);
                }
            });

            return validFiles;
        }

        public void CreateDirectory()
        {
            var dirInfo = new DirectoryInfo(Path);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();

                //using (FileStream fstream = new FileStream(Path + @"\AllNotes.txt", FileMode.Create))
                //{ }
            }
        }
    }
}
