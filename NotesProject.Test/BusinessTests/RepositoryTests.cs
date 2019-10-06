using AutoFixture;
using Moq;
using NotesProject.Business.Models;
using NotesProject.Business.Provider;
using NotesProject.Business.Services.Implementations;
using NotesProject.Business.Services.Interfaces;
using NotesProject.Test.BusinessTests.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace NotesProject.Test.BusinessTests
{
	public class RepositoryTests
	{
		private const string oldTitle = "OLD TITLE";
		private const string oldText = "OLD TEXT";
		private const string newTitle = "TITLE";
		private const string newText = "TEXT";


		private readonly Mock<INoteProvider> mock;
		public RepositoryTests()
		{
			mock = new Mock<INoteProvider>();
			mock.Setup(x => x.CreateNoteList()).Returns(new List<Note>());
		}

		#region [ AddNoteTests ]
		
		[Theory]
		[MemberData(nameof(NoteRepositoryData.AddNoteTest_ChangeListCount_Data), MemberType = typeof(NoteRepositoryData))]
		public void AddNoteTest_When_Add_Then_ChangeListCount(string title, string text)
		{						
			var repository = new NoteRepository(mock.Object);

			var countBefore = repository.GetNotes().Count;
			repository.AddNote(title, text);
			var countAfter = repository.GetNotes().Count;

			Assert.Equal(countBefore + 1, countAfter);
		}

		[Theory]
		[MemberData(nameof(NoteRepositoryData.AddNote_ListCountNoChanges_Data), MemberType = typeof(NoteRepositoryData))]
		public void AddNoteTest_When_Add_Then_ListCountNoChanges(string title, string text)
		{
			var repository = new NoteRepository(mock.Object);

			var countBefore = repository.GetNotes().Count;
			repository.AddNote(title, text);
			var countAfter = repository.GetNotes().Count;
			
			Assert.Equal(countBefore, countAfter);
		}

		[Theory]
		[MemberData(nameof(NoteRepositoryData.AddNote_TextAndTitleHasSetValue_Data), MemberType = typeof(NoteRepositoryData))]
		public void AddNoteTest_When_Add_Then_TextAndTitleHasSetValue(string title, string text)
		{
			var repository = new NoteRepository(mock.Object);

			repository.AddNote(title, text);
			var notes = repository.GetNotes();

			Assert.True(notes.Exists(x => x.Title == title && x.Text == text));
		}

		[Fact]
		public void AddNoteTest_When_AddAfterDelete_Then_CorrectId()
		{
			var repository = new NoteRepository(mock.Object);

			for(var i = 1; i <= 5; i++)
				repository.AddNote($"title{i}", $"text{i}");

			repository.DeleteNote(4);
			repository.AddNote($"title{6}", $"text{6}");
			var notes = repository.GetNotes();

			Assert.True(notes.FindAll(x => x.Id == 5).Count == 1);
		}

		#endregion
		#region [ DeleteNoteTests ]

		[Fact]
		public void DeleteNoteTest_When_DeleteElementFromListWithElements_Then_Delete()
		{
			var repository = new NoteRepository(mock.Object);

			repository.AddNote("test", "test");
			repository.AddNote("test1", "test1");
			repository.AddNote(newTitle, newText);
			repository.AddNote("test3", "test3");
			repository.AddNote("test4", "test4");

			repository.DeleteNote(3);

			Assert.False(repository.GetNotes().Exists(x => x.Title == newTitle && x.Text == newText));
		}

		[Theory]
		[InlineData(null)]
		public void DeleteNoteTest_When_IdIsNull_Then_Error(int number)
		{
			var repository = new NoteRepository(mock.Object);

			Assert.Throws<InvalidOperationException>(() => repository.DeleteNote(number));
		}

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(666)]
		public void DeleteNoteTest_When_ListElementsIsEmpty_Then_Error(int number)
		{
			var repository = new NoteRepository(mock.Object);

			Assert.Throws<InvalidOperationException>(() => repository.DeleteNote(number));
		}

		[Theory]
		[InlineData(0)]
		[InlineData(-1)]
		[InlineData(-2)]
		[InlineData(-666)]
		public void DeleteNoteTest_When_IdLessOrEqualZeroOrNull_Then_Error(int number)
		{
			var repository = new NoteRepository(mock.Object);

			repository.AddNote("test", "test");

			Assert.Throws<InvalidOperationException>(() => repository.DeleteNote(number));
		}

		[Theory]
		[InlineData(4)]
		[InlineData(15)]
		[InlineData(32)]
		public void DeleteNoteTest_When_IdMoreThenListCount_Then_Error(int number)
		{
			var repository = new NoteRepository(mock.Object);

			repository.AddNote("test", "test");

			Assert.Throws<InvalidOperationException>(() => repository.DeleteNote(number));
		}

		#endregion
		#region [ EditNoteTests ]

		[Theory]
		[MemberData(nameof(NoteRepositoryData.EditNote_NotSuccessfulEdit_Data), MemberType = typeof(NoteRepositoryData))]
		public void EditNoteTest_When_BadData_Then_NotEdit(int number, string title, string text)
		{
			var repository = new NoteRepository(mock.Object);

			for(var i = 0; i <= 5; i++)
			{
				repository.AddNote($"{oldTitle}{i}", $"{oldText}{i}");
			}		

			repository.EditNote(number, title, text);
			var notes = repository.GetNotes();

			Assert.False(notes.Exists(x => x.Title == title && x.Text == text)
				&& !notes.Exists(x => x.Title == $"{oldTitle}{number}" && x.Text == $"{oldText}{number}"));
		}

		[Theory]
		[MemberData(nameof(NoteRepositoryData.EditNote_SuccessfulEdit_Data), MemberType = typeof(NoteRepositoryData))]
		public void EditNoteTest_When_ValidData_Then_Edit(int number, string title, string text)
		{
			var repository = new NoteRepository(mock.Object);

			for (var i = 0; i <= number + 1; i++)
			{
				repository.AddNote($"{oldTitle}{i}", $"{oldTitle}{i}");
			}

			repository.EditNote(number, title, text);
			var notes = repository.GetNotes();

			Assert.True(!notes.Exists(x => x.Title == $"{oldTitle}{number}" && x.Text == $"{oldText}{number}")
				&& notes.Exists(x => x.Title == title && x.Text == text));
		}

		[Theory]
		[InlineData(0)]
		[InlineData(null)]
		public void EditNoteTest_When_IdIsNullOrZero_Then_Error(int number)
		{
			var repository = new NoteRepository(mock.Object);

			repository.AddNote(newTitle, newText);

			Assert.Throws<InvalidOperationException>(() => repository.EditNote(number, newTitle, newText));
		}

		[Theory]
		[InlineData(-1)]
		[InlineData(-2)]
		[InlineData(-32)]
		public void EditNoteTest_When_IdLessThenZero_Then_Error(int number)
		{
			var repository = new NoteRepository(mock.Object);

			repository.AddNote(newTitle, newText);

			Assert.Throws<InvalidOperationException>(() => repository.EditNote(number, newTitle, newText));
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(32)]
		public void EditNoteTest_When_IdMoreThenListCount_Then_Error(int number)
		{
			var repository = new NoteRepository(mock.Object);

			repository.AddNote(newTitle, newText);

			Assert.Throws<InvalidOperationException>(() => repository.EditNote(number + 5, newTitle, newText));
		}

		#endregion
		#region [ GetNoteTests ]

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(32)]
		public void GetNoteTest_When_ElementExistInList_Then_True(int number)
		{
			var repository = new NoteRepository(mock.Object);

			for (var i = 1; i <= number + 1; i++)
			{
				repository.AddNote($"{newTitle}{i}", $"{newText}{i}");
			}

			Assert.True(repository.GetNote(number).Title == $"{newTitle}{number}"
				&& repository.GetNote(number).Text == $"{newText}{number}");
		}

		[Theory]
		[InlineData(0)]
		[InlineData(-1)]
		[InlineData(-2)]
		[InlineData(-32)]
		public void GetNoteTest_When_ElementIdIsLessOrEquelZero_Then_Error(int number)
		{
			var repository = new NoteRepository(mock.Object);
			
			repository.AddNote(newTitle, newText);			
			
			Assert.Throws<InvalidOperationException>(() => repository.GetNote(number));
		}

		[Theory]
		[InlineData(null)]
		[InlineData(0)]
		public void GetNoteTest_When_ElementEqualNullOrZero_Then_Error(int number)
		{
			var repository = new NoteRepository(mock.Object);

			repository.AddNote(newTitle, newText);

			Assert.Throws<InvalidOperationException>(() => repository.GetNote(number));
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(32)]
		public void GetNoteTest_When_ElementMoreThenListCount_Then_Error(int number)
		{
			var repository = new NoteRepository(mock.Object); ;

			repository.AddNote(newTitle, newText);

			Assert.Throws<InvalidOperationException>(() => repository.GetNote(number + 5));
		}

		#endregion
		#region [ IsNoteExistTests ]

		[Theory]
		[InlineData(null)]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(666)]		
		public void IsNoteExistTest_When_ListElementsIsEmpty_Then_False(int number)
		{
			var repository = new NoteRepository(mock.Object);

			Assert.False(repository.IsNoteExist(number));
		}

		[Theory]
		[InlineData(1)]
		[InlineData(2)]
		[InlineData(32)]
		public void IsNoteExistTest_When_ElementExistInList_Then_True(int number)
		{
			var repository = new NoteRepository(mock.Object);

			for (var i = 0; i <= number + 1; i++)
			{
				repository.AddNote($"{newTitle}{i}", $"{newText}{i}");
			}

			Assert.True(repository.IsNoteExist(number));
		}

		[Theory]
		[InlineData(4)]
		[InlineData(13)]
		[InlineData(32)]
		public void IsNoteExistTest_When_ElementNotExistInList_Then_False(int number)
		{
			var repository = new NoteRepository(mock.Object);

			repository.AddNote(newTitle, newText);

			Assert.False(repository.IsNoteExist(number));
		}

		[Theory]
		[InlineData(0)]
		[InlineData(-1)]
		[InlineData(-2)]
		[InlineData(-32)]
		public void IsNoteExistTest_When_ElementIdIsLessOrEquelZero_Then_False(int number)
		{
			var repository = new NoteRepository(mock.Object);

			for (var i = 0; i <= number + 1; i++)
			{
				repository.AddNote(newTitle + $"{i}", newText + $"{i}");
			}

			Assert.False(repository.IsNoteExist(number));
		}

		#endregion
	}
}
