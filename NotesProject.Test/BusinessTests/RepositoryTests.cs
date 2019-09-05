using NotesProject.Business.Provider;
using NotesProject.Business.Services.Implementations;
using NotesProject.Test.BusinessTests.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace NotesProject.Test.BusinessTests
{
	public class RepositoryTests
	{
		#region [ AddNoteTests ]

		[Theory]
		[MemberData(nameof(NoteRepositoryData.AddNoteTest_When_Add_Then_ChangeListCount_Data), MemberType = typeof(NoteRepositoryData))]
		public void AddNoteTest_When_Add_Then_ChangeListCount(string title, string text)
		{
			var provider = new NoteProvider();
			var repository = new NoteRepository(provider);

			var countBefore = repository.GetNotes().Count;
			repository.AddNote(title, text);
			var countAfter = repository.GetNotes().Count;

			Assert.Equal(countBefore + 1, countAfter);
		}

		[Theory]
		[MemberData(nameof(NoteRepositoryData.AddNoteTest_When_Add_Then_ChangeListCount_Data), MemberType = typeof(NoteRepositoryData))]
		public void AddNoteTest_When_Add_Then_CountBeforeNotEqualCountAfter(string title, string text)
		{
			var provider = new NoteProvider();
			var repository = new NoteRepository(provider);

			var countBefore = repository.GetNotes().Count;
			repository.AddNote(title, text);
			var countAfter = repository.GetNotes().Count;

			Assert.NotEqual(countBefore, countAfter);
		}

		[Theory]
		[MemberData(nameof(NoteRepositoryData.AddNoteTest_When_Add_Then_TextAndTitleHasSetValue_Data), MemberType = typeof(NoteRepositoryData))]
		public void AddNoteTest_When_Add_Then_TextAndTitleHasSetValue(string title, string text)
		{
			var provider = new NoteProvider();
			var repository = new NoteRepository(provider);

			repository.AddNote(title, text);
			var notes = repository.GetNotes();

			Assert.True(notes.Exists(x => x.Title == title && x.Text == text));
		}

		#endregion
		#region [ DeleteNoteTests ]

		[Theory]
		[MemberData(nameof(NoteRepositoryData.DeleteNoteWithNoError_Data), MemberType = typeof(NoteRepositoryData))]
		public void DeleteNoteTest_When_DeleteFromListWithOneElement_Then_ChangeListCount(string title, string text)
		{
			var provider = new NoteProvider();
			var repository = new NoteRepository(provider);

			repository.AddNote(title, text);
			var countBefore = repository.GetNotes().Count;
			repository.DeleteNote(countBefore);
			var countAfter = repository.GetNotes().Count;

			Assert.Equal(countBefore - 1, countAfter);
		}

		[Theory]
		[MemberData(nameof(NoteRepositoryData.DeleteNoteWithNoError_Data), MemberType = typeof(NoteRepositoryData))]
		public void DeleteNoteTest_When_DeleteFirstElementFromListWithElements_Then_DeleteFirst(string title, string text)
		{
			var provider = new NoteProvider();
			var repository = new NoteRepository(provider);

			repository.AddNote(title, text);
			repository.AddNote("test1", "test1");
			repository.AddNote("test2", "test2");
			repository.AddNote("test3", "test3");
			repository.AddNote("test4", "test4");

			repository.DeleteNote(1);

			Assert.False(repository.GetNotes().Exists(x => x.Title == "test" && x.Text == "test"));
		}

		[Theory]
		[MemberData(nameof(NoteRepositoryData.DeleteNoteWithNoError_Data), MemberType = typeof(NoteRepositoryData))]
		public void DeleteNoteTest_When_DeleteMiddleElementFromListWithElements_Then_DeleteFirst(string title, string text)
		{
			var provider = new NoteProvider();
			var repository = new NoteRepository(provider);

			repository.AddNote("test", "test");
			repository.AddNote("test1", "test1");
			repository.AddNote(title, text);
			repository.AddNote("test3", "test3");
			repository.AddNote("test4", "test4");

			repository.DeleteNote(3);

			Assert.False(repository.GetNotes().Exists(x => x.Title == title && x.Text == text));
		}

		[Theory]
		[MemberData(nameof(NoteRepositoryData.DeleteNoteWithNoError_Data), MemberType = typeof(NoteRepositoryData))]
		public void DeleteNoteTest_When_DeleteLastElementFromListWithElements_Then_DeleteFirst(string title, string text)
		{
			var provider = new NoteProvider();
			var repository = new NoteRepository(provider);

			repository.AddNote("test", "test");
			repository.AddNote("test1", "test1");
			repository.AddNote("test2", "test2");
			repository.AddNote("test3", "test3");
			repository.AddNote(title, text);

			repository.DeleteNote(repository.GetNotes().Count);

			Assert.False(repository.GetNotes().Exists(x => x.Title == title && x.Text == text));
		}

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(666)]
		[InlineData(null)]
		public void DeleteNoteTest_When_ListElementsIsEmpty_Then_Error(int number)
		{
			var provider = new NoteProvider();
			var repository = new NoteRepository(provider);

			Assert.Throws<InvalidOperationException>(() => repository.DeleteNote(number));
		}

		#endregion
		#region [ EditNoteTests ]

		[Theory]
		[MemberData(nameof(NoteRepositoryData.DeleteNoteWithNoError_Data), MemberType = typeof(NoteRepositoryData))]
		public void EditNoteTest_When_EditSome_Then_NoChangeListCount(string title, string text)
		{
			var provider = new NoteProvider();
			var repository = new NoteRepository(provider);

			repository.AddNote(title, text);
			var countBefore = repository.GetNotes().Count;
			repository.EditNote(countBefore, title, text);
			var countAfter = repository.GetNotes().Count;

			Assert.Equal(countBefore, countAfter);
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(32)]
		public void EditNoteTest_When_EditFirstElementFromList_Then_ChangeNote(int number)
		{
			var provider = new NoteProvider();
			var repository = new NoteRepository(provider);

			for (var i = 1; i <= number + 1; i++)
			{
				repository.AddNote($"title{i}", $"text{i}");
			}

			repository.EditNote(1, $"new title{1}", $"new text{1}");
			var notes = repository.GetNotes();

			Assert.True(!notes.Exists(x => x.Title == $"title{1}" && x.Text == $"text{1}")
				&& notes.Exists(x => x.Title == $"new title{1}" && x.Text == $"new text{1}"));
		}

		[Theory]
		[InlineData(5)]
		[InlineData(10)]
		[InlineData(32)]
		public void EditNoteTest_When_EditSomeElementFromList_Then_ChangeNote(int number)
		{
			var provider = new NoteProvider();
			var repository = new NoteRepository(provider);

			for (var i = 1; i <= number + 1; i++)
			{
				repository.AddNote($"title{i}", $"text{i}");
			}
			repository.EditNote(number - 2, $"new title{number - 2}", $"new text{number - 2}");
			var notes = repository.GetNotes();

			Assert.True(!notes.Exists(x => x.Title == $"title{number - 2}" && x.Text == $"text{number - 2}")
				&& notes.Exists(x => x.Title == $"new title{number - 2}" && x.Text == $"new text{number - 2}"));
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(32)]
		public void EditNoteTest_When_EditLastElementFromList_Then_ChangeNote(int number)
		{
			var provider = new NoteProvider();
			var repository = new NoteRepository(provider);

			for (var i = 1; i <= number + 1; i++)
			{
				repository.AddNote($"title{i}", $"text{i}");
			}

			repository.EditNote(number, $"new title{number}", $"new text{number}");
			var notes = repository.GetNotes();

			Assert.True(!notes.Exists(x => x.Title == $"title{number}" && x.Text == $"text{number}")
				&& notes.Exists(x => x.Title == $"new title{number}" && x.Text == $"new text{number}"));
		}

		[Theory]
		[InlineData(0)]
		[InlineData(null)]
		public void EditNoteTest_When_IdNullOrZero_Then_Error(int number)
		{
			var provider = new NoteProvider();
			var repository = new NoteRepository(provider);

			repository.AddNote("test", "test");

			Assert.Throws<InvalidOperationException>(() => repository.EditNote(number, "title", "text"));
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(32)]
		public void EditNoteTest_When_IdMoreThenListCount_Then_Error(int number)
		{
			var provider = new NoteProvider();
			var repository = new NoteRepository(provider);

			repository.AddNote("test", "test");

			Assert.Throws<InvalidOperationException>(() => repository.EditNote(number + 5, "title", "text"));
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(32)]
		public void EditNoteTest_When_IdLessThenZero_Then_Error(int number)
		{
			var provider = new NoteProvider();
			var repository = new NoteRepository(provider);

			repository.AddNote("test", "test");

			Assert.Throws<InvalidOperationException>(() => repository.EditNote(-number, "title", "text"));
		}

		#endregion
		#region [ GetNoteTests ]

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(32)]
		public void GetNoteTest_When_ElementExistInList_Then_True(int number)
		{
			var provider = new NoteProvider();
			var repository = new NoteRepository(provider);

			for (var i = 1; i <= number + 1; i++)
			{
				repository.AddNote($"title{i}", $"text{i}");
			}

			Assert.True(repository.GetNote(number).Title == $"title{number}"
				&& repository.GetNote(number).Text == $"text{number}");
		}

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(32)]
		public void GetNoteTest_When_ElementIdIsLessOrEquelZero_Then_Error(int number)
		{
			var provider = new NoteProvider();
			var repository = new NoteRepository(provider);

			for (var i = 0; i <= number + 1; i++)
			{
				repository.AddNote($"title{i}", $"text{i}");
			}
			
			Assert.Throws<InvalidOperationException>(() => repository.GetNote(-number));
		}

		[Theory]
		[InlineData(null)]
		[InlineData(0)]
		public void GetNoteTest_When_ElementEqualNullOrZero_Then_Error(int number)
		{
			var provider = new NoteProvider();
			var repository = new NoteRepository(provider);

			for (var i = 0; i <= number + 1; i++)
			{
				repository.AddNote($"title{i}", $"text{i}");
			}

			Assert.Throws<InvalidOperationException>(() => repository.GetNote(number));
		}

		[Theory]
		[InlineData(null)]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(32)]
		public void GetNoteTest_When_ElementMoreThenListCount_Then_Error(int number)
		{
			var provider = new NoteProvider();
			var repository = new NoteRepository(provider);

			for (var i = 0; i <= number + 1; i++)
			{
				repository.AddNote($"title{i}", $"text{i}");
			}

			Assert.Throws<InvalidOperationException>(() => repository.GetNote(number + 5));
		}

		[Fact]
		public void GetNoteTest_When_GetSome_Then_NoChangeListCount()
		{
			var provider = new NoteProvider();
			var repository = new NoteRepository(provider);

			repository.AddNote("title", "text");
			var countBefore = repository.GetNotes().Count;
			repository.GetNote(countBefore);
			var countAfter = repository.GetNotes().Count;

			Assert.Equal(countBefore, countAfter);
		}

		#endregion
		#region [ IsNoteExistTests ]

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(666)]
		[InlineData(null)]
		public void IsNoteExistTest_When_ListElementsIsEmpty_Then_False(int number)
		{
			var provider = new NoteProvider();
			var repository = new NoteRepository(provider);

			Assert.False(repository.IsNoteExist(number));
			Assert.Throws<InvalidOperationException>(() => repository.DeleteNote(number));
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(32)]
		public void IsNoteExistTest_When_ElementExistInList_Then_True(int number)
		{
			var provider = new NoteProvider();
			var repository = new NoteRepository(provider);

			for (var i = 0; i <= number + 1; i++)
			{
				repository.AddNote($"title{i}", $"text{i}");
			}

			Assert.True(repository.IsNoteExist(number));
		}

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(32)]
		public void IsNoteExistTest_When_ElementNotExistInList_Then_False(int number)
		{
			var provider = new NoteProvider();
			var repository = new NoteRepository(provider);

			for (var i = 0; i <= number + 1; i++)
			{
				repository.AddNote($"title{i}", $"text{i}");
			}

			Assert.False(repository.IsNoteExist(number + 3));
		}

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(32)]
		public void IsNoteExistTest_When_ElementIdIsLessOrEquelZero_Then_False(int number)
		{
			var provider = new NoteProvider();
			var repository = new NoteRepository(provider);

			for (var i = 0; i <= number + 1; i++)
			{
				repository.AddNote($"title{i}", $"text{i}");
			}

			Assert.False(repository.IsNoteExist(-number));
		}

		[Fact]
		public void IsNoteExistTest_When_IsNoteExist_Then_NoChangeListCount()
		{
			var provider = new NoteProvider();
			var repository = new NoteRepository(provider);

			repository.AddNote("title", "text");
			var countBefore = repository.GetNotes().Count;
			repository.IsNoteExist(countBefore);
			var countAfter = repository.GetNotes().Count;

			Assert.Equal(countBefore, countAfter);
		}

		#endregion
	}
}
