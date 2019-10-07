using ConsoleNotes.Services.Implementations;
using ConsoleNotes.Services.Interfaces;
using ConsoleNotes.Models.Enums;
using Moq;
using NotesProject.Business.Models;
using NotesProject.Business.Services.Implementations;
using NotesProject.Business.Services.Interfaces;
using System;
using System.Collections.Generic;
using Xunit;

namespace NotesProject.Test.ConsoleNotesTests
{
	public class NoteConsoleServiceTests
	{
		[Fact]
		public void ShowNoteTest_When_HasElements_Then_ShowList()
		{
			var noteProviderMock = new Mock<INoteProvider>();
			noteProviderMock.Setup(x => x.CreateNoteList()).Returns(new List<Note>());

			var title = "title";
			var text = "text";
			var id = 1;

			var repository = new NoteRepository(noteProviderMock.Object);
			repository.AddNote(title, text);

			var fakeConsoleProvider = new FakeConsoleProvider(null);
			var service = new NoteConsoleService(
				repository,
				fakeConsoleProvider,
				new Mock<ICommandHelper>().Object);

			service.Handle(nameof(Command.List));

			var expected = $"Here are list of all notes:{Environment.NewLine}" +
				$"id: {id} || title: '{title}' || text: '{text}'{Environment.NewLine}" +
				$"Press any key to return to the main window...{Environment.NewLine}";

			Assert.Equal(expected, fakeConsoleProvider.Output);
		}

		[Fact]
		public void ShowNoteTest_When_NoElements_Then_ShowInfoMessage()
		{
			var noteProviderMock = new Mock<INoteProvider>();
			noteProviderMock.Setup(x => x.CreateNoteList()).Returns(new List<Note>());

			var repository = new NoteRepository(noteProviderMock.Object);

			var fakeConsoleProvider = new FakeConsoleProvider(null);
			var service = new NoteConsoleService(
				repository,
				fakeConsoleProvider,
				new Mock<ICommandHelper>().Object);

			service.Handle(nameof(Command.List));
			var expected = $"There are zero notes.{Environment.NewLine}" +
				$"Press any key to return to the main window...{Environment.NewLine}";
			Assert.Equal(expected, fakeConsoleProvider.Output);
		}

		[Fact]
		public void AddNoteTest_When_CorrectTitleOrText_Then_Add()
		{
			var noteProviderMock = new Mock<INoteProvider>();
			noteProviderMock.Setup(x => x.CreateNoteList()).Returns(new List<Note>());

			var title = "title";
			var text = "text";

			var repository = new NoteRepository(noteProviderMock.Object);
			var linesToRead = new List<string>() { title, text, "n" };
			var fakeConsoleProvider = new FakeConsoleProvider(linesToRead);
			var service = new NoteConsoleService(
				repository, 
				fakeConsoleProvider,
				new Mock<ICommandHelper>().Object);

			service.Handle(nameof(Command.Add));

			var expected = $"Enter note title{Environment.NewLine}" +
				$"> Enter note text{Environment.NewLine}" +
				$"> Note was added.{Environment.NewLine}" +
				$"Do you want to add another note? (y/n){Environment.NewLine}";

			Assert.Equal(expected, fakeConsoleProvider.Output);
		}

		[Fact]
		public void AddNoteTest_When_NotCorrectTitleOrText_Then_NotAdd()
		{
			var noteProviderMock = new Mock<INoteProvider>();
			noteProviderMock.Setup(x => x.CreateNoteList()).Returns(new List<Note>());

			var title = string.Empty;
			var text = string.Empty;

			var repository = new NoteRepository(noteProviderMock.Object);
			var linesToRead = new List<string>() { title, text, "n" };
			var fakeConsoleProvider = new FakeConsoleProvider(linesToRead);
			var service = new NoteConsoleService(
				repository,
				fakeConsoleProvider,
				new Mock<ICommandHelper>().Object);

			service.Handle(nameof(Command.Add));

			var expected = $"Enter note title{Environment.NewLine}" +
				$"> Enter note text{Environment.NewLine}" +
				$"> Note was not added. Enter title or text.{Environment.NewLine}" +
				$"Do you want to add another note? (y/n){Environment.NewLine}";

			Assert.Equal(expected, fakeConsoleProvider.Output);
		}

		[Fact]
		public void DeleteNoteTest_When_IdNotCorrect_Then_NotDeleted()
		{
			var noteProviderMock = new Mock<INoteProvider>();
			noteProviderMock.Setup(x => x.CreateNoteList()).Returns(new List<Note>());

			var id = "text";

			var repository = new NoteRepository(noteProviderMock.Object);
			var linesToRead = new List<string>() { id, "n" };
			var fakeConsoleProvider = new FakeConsoleProvider(linesToRead);
			var service = new NoteConsoleService(
				repository,
				fakeConsoleProvider,
				new Mock<ICommandHelper>().Object);

			service.Handle(nameof(Command.Delete));

			var expected = $"Please enter id of note to delete:{Environment.NewLine}" +
				$"Id [{id}] is not a number.{Environment.NewLine}" +
				$"Do you want to delete another note? (y/n){Environment.NewLine}";

			Assert.Equal(expected, fakeConsoleProvider.Output);
		}

		[Fact]
		public void DeleteNoteTest_When_CorrectIdAndNoteNotExists_Then_NotDeleted()
		{
			var noteProviderMock = new Mock<INoteProvider>();
			noteProviderMock.Setup(x => x.CreateNoteList()).Returns(new List<Note>());

			var id = "54";

			var repository = new NoteRepository(noteProviderMock.Object);
			repository.AddNote("title", "text");
			var linesToRead = new List<string>() { id, "n" };
			var fakeConsoleProvider = new FakeConsoleProvider(linesToRead);
			var service = new NoteConsoleService(
				repository,
				fakeConsoleProvider,
				new Mock<ICommandHelper>().Object);

			service.Handle(nameof(Command.Delete));

			var expected = $"Please enter id of note to delete:{Environment.NewLine}" +
				$"The note with id [{id}] is not exist in the list of notes.{Environment.NewLine}" +
				$"Do you want to delete another note? (y/n){Environment.NewLine}";

			Assert.Equal(expected, fakeConsoleProvider.Output);
		}

		[Fact]
		public void DeleteNoteTest_When_CorrectIdAndNoteExists_Then_Delete()
		{
			var noteProviderMock = new Mock<INoteProvider>();
			noteProviderMock.Setup(x => x.CreateNoteList()).Returns(new List<Note>());

			var id = "1";

			var repository = new NoteRepository(noteProviderMock.Object);
			repository.AddNote("title", "text");
			var linesToRead = new List<string>() { id, "n" };
			var fakeConsoleProvider = new FakeConsoleProvider(linesToRead);
			var service = new NoteConsoleService(
				repository,
				fakeConsoleProvider,
				new Mock<ICommandHelper>().Object);

			service.Handle(nameof(Command.Delete));

			var expected = $"Please enter id of note to delete:{Environment.NewLine}" +
				$"The note with id [{id}] was successfully deleted.{Environment.NewLine}" +
				$"Do you want to delete another note? (y/n){Environment.NewLine}";

			Assert.Equal(expected, fakeConsoleProvider.Output);
		}

		[Fact]
		public void EditNoteTest_When_IdNotCorrect_Then_NotEdit()
		{
			var noteProviderMock = new Mock<INoteProvider>();
			noteProviderMock.Setup(x => x.CreateNoteList()).Returns(new List<Note>());

			var id = "text";

			var repository = new NoteRepository(noteProviderMock.Object);
			var linesToRead = new List<string>() { id, "n" };
			var fakeConsoleProvider = new FakeConsoleProvider(linesToRead);
			var service = new NoteConsoleService(
				repository,
				fakeConsoleProvider,
				new Mock<ICommandHelper>().Object);

			service.Handle(nameof(Command.Edit));

			var expected = $"Please enter id of note to edit:{Environment.NewLine}" +
				$"Id [{id}] is not a number.{Environment.NewLine}" +
				$"Do you want to edit another note? (y/n){Environment.NewLine}";

			Assert.Equal(expected, fakeConsoleProvider.Output);
		}

		[Fact]
		public void EditNoteTest_When_IdNotCorrectAndNotExist_Then_NotEdit()
		{
			var noteProviderMock = new Mock<INoteProvider>();
			noteProviderMock.Setup(x => x.CreateNoteList()).Returns(new List<Note>());

			var id = "44";

			var repository = new NoteRepository(noteProviderMock.Object);
			repository.AddNote("title", "text");
			var linesToRead = new List<string>() { id, "n" };
			var fakeConsoleProvider = new FakeConsoleProvider(linesToRead);
			var service = new NoteConsoleService(
				repository,
				fakeConsoleProvider,
				new Mock<ICommandHelper>().Object);

			service.Handle(nameof(Command.Edit));

			var expected = $"Please enter id of note to edit:{Environment.NewLine}" +
				$"The note with id [{id}] is not exist in the list of notes.{Environment.NewLine}" +
				$"Do you want to edit another note? (y/n){Environment.NewLine}";

			Assert.Equal(expected, fakeConsoleProvider.Output);
		}

		[Fact]
		public void EditNoteTest_When_DataCorrect_Then_Edit()
		{
			var noteProviderMock = new Mock<INoteProvider>();
			noteProviderMock.Setup(x => x.CreateNoteList()).Returns(new List<Note>());

			var id = "1";
			var title = "title";
			var text = "text";
			var newTitle = "newTitle";
			var newText = "newText";
			var areYouSureAboutThat = "y";

			var repository = new NoteRepository(noteProviderMock.Object);
			repository.AddNote(title, text);
			var linesToRead = new List<string>() { id, newTitle, areYouSureAboutThat, newText, "n" };
			var fakeConsoleProvider = new FakeConsoleProvider(linesToRead);
			var service = new NoteConsoleService(
				repository,
				fakeConsoleProvider,
				new Mock<ICommandHelper>().Object);

			service.Handle(nameof(Command.Edit));

			var expected = $"Please enter id of note to edit:{Environment.NewLine}" +
				$"Current title of this note: {title}. Pick a new title:{Environment.NewLine}" +
				$"> Are you sure (y/n)?{Environment.NewLine}" +
				$"Current text of this note: {text}. Pick a new text:{Environment.NewLine}" +
				$"> Text was successfully changed.{Environment.NewLine}" +
				$"Do you want to edit another note? (y/n){Environment.NewLine}";

			Assert.Equal(expected, fakeConsoleProvider.Output);
		}

		[Fact]
		public void EditNoteTest_When_TitleAndTextAreEmpty_Then_NotEdit()
		{
			var noteProviderMock = new Mock<INoteProvider>();
			noteProviderMock.Setup(x => x.CreateNoteList()).Returns(new List<Note>());

			var id = "1";
			var title = "title";
			var text = "text";
			var newTitle = string.Empty;
			var newText = string.Empty;
			var areYouSureAboutThat = "y";

			var repository = new NoteRepository(noteProviderMock.Object);
			repository.AddNote(title, text);
			var linesToRead = new List<string>() { id, newTitle, areYouSureAboutThat, newText, "n" };
			var fakeConsoleProvider = new FakeConsoleProvider(linesToRead);
			var service = new NoteConsoleService(
				repository,
				fakeConsoleProvider,
				new Mock<ICommandHelper>().Object);

			service.Handle(nameof(Command.Edit));

			var expected = $"Please enter id of note to edit:{Environment.NewLine}" +
				$"Current title of this note: {title}. Pick a new title:{Environment.NewLine}" +
				$"> Are you sure (y/n)?{Environment.NewLine}" +
				$"Current text of this note: {text}. Pick a new text:{Environment.NewLine}" +
				$"> Text was NOT successfully changed. Enter title or text.{Environment.NewLine}" +
				$"Do you want to edit another note? (y/n){Environment.NewLine}";

			Assert.Equal(expected, fakeConsoleProvider.Output);
		}

		[Fact]
		public void EditNoteTest_When_YouNotSureAfterChangeTitle_Then_NotEdit()
		{
			var noteProviderMock = new Mock<INoteProvider>();
			noteProviderMock.Setup(x => x.CreateNoteList()).Returns(new List<Note>());

			var id = "1";
			var title = "title";
			var text = "text";
			var newTitle = string.Empty;
			var newText = string.Empty;
			var areYouSureAboutThat = "n";

			var repository = new NoteRepository(noteProviderMock.Object);
			repository.AddNote(title, text);

			var linesToRead = new List<string>() { id, newTitle, areYouSureAboutThat, newText, "n" };
			var fakeConsoleProvider = new FakeConsoleProvider(linesToRead);

			var service = new NoteConsoleService(
				repository,
				fakeConsoleProvider,
				new Mock<ICommandHelper>().Object);

			service.Handle(nameof(Command.Edit));

			var expected = $"Please enter id of note to edit:{Environment.NewLine}" +
				$"Current title of this note: {title}. Pick a new title:{Environment.NewLine}" +
				$"> Are you sure (y/n)?{Environment.NewLine}" +
				$"Do you want to edit another note? (y/n){Environment.NewLine}" +
				$"Wrong input! Pass only \"y\" or \"n\".{Environment.NewLine}";

			Assert.Equal(expected, fakeConsoleProvider.Output);
		}

		[Fact]
		public void EditNoteTest_When_NotCorrectareYouSureAboutThat_Then_NotEdit()
		{
			var noteProviderMock = new Mock<INoteProvider>();
			noteProviderMock.Setup(x => x.CreateNoteList()).Returns(new List<Note>());

			var id = "1";
			var title = "title";
			var text = "text";
			var newTitle = string.Empty;
			var newText = string.Empty;
			var areYouSureAboutThatNotCorrect = "error";
			var areYouSureAboutThat = "y";

			var repository = new NoteRepository(noteProviderMock.Object);
			repository.AddNote(title, text);

			var linesToRead = new List<string>() { id, newTitle, areYouSureAboutThatNotCorrect, areYouSureAboutThat, newText, "n" };
			var fakeConsoleProvider = new FakeConsoleProvider(linesToRead);

			var service = new NoteConsoleService(
				repository,
				fakeConsoleProvider,
				new Mock<ICommandHelper>().Object);

			service.Handle(nameof(Command.Edit));

			var expected = $"Please enter id of note to edit:{Environment.NewLine}" +
				$"Current title of this note: {title}. Pick a new title:{Environment.NewLine}" +
				$"> Are you sure (y/n)?{Environment.NewLine}" +
				$"Wrong input! Pass only \"y\" or \"n\".{Environment.NewLine}" +
				$"Current text of this note: {text}. Pick a new text:{Environment.NewLine}" +
				$"> Text was NOT successfully changed. Enter title or text.{Environment.NewLine}" +
				$"Do you want to edit another note? (y/n){Environment.NewLine}";

			Assert.Equal(expected, fakeConsoleProvider.Output);
		}
	}
}
