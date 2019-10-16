using Moq;
using NotesProject.Application.Repositories;
using NotesProject.ConsoleUI.Helpers;
using NotesProject.ConsoleUI.Models.Enums;
using NotesProject.ConsoleUI.Services.Implementations;
using NotesProject.ConsoleUI.Services.Interfaces;
using NotesProject.DataBase.Interfaces;
using NotesProject.DataBase.Services;
using NotesProject.Domain.Interfaces;
using NotesProject.Domain.Models;
using NotesProject.Domain.Models.Entities;
using NotesProject.Domain.Models.ValueObjects;
using NotesProject.Infrastructure.Interfaces;
using NotesProject.Infrastructure.Models;
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
			noteProviderMock.Setup(x => x.CreateNoteList()).Returns(new List<NoteDto>());
            var commandHandlerMock = new Mock<ICommandHelper>();
            commandHandlerMock.Setup(x => x.ParcingCommands(nameof(Command.List))).Returns((Command.List, true));
            var noteServiceMock = new Mock<INoteService>();

            var title = "title";
			var text = "text";
			var id = 1;

            var context = new DataBaseService(noteProviderMock.Object);
            var repository = new NoteRepository(context);
            repository.AddNote(new NoteDetails { Title = title, Text = text });
            var fakeConsoleProvider = new FakeConsoleProvider(null);

            var consoleRepositoryMock = new ConsoleRepository(fakeConsoleProvider, repository, commandHandlerMock.Object, noteServiceMock.Object);

            
			var service = new NoteConsoleService(
                commandHandlerMock.Object,
                consoleRepositoryMock);

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
			noteProviderMock.Setup(x => x.CreateNoteList()).Returns(new List<NoteDto>());
            var commandHandlerMock = new Mock<ICommandHelper>();
            commandHandlerMock.Setup(x => x.ParcingCommands(nameof(Command.List))).Returns((Command.List, true));
            var noteServiceMock = new Mock<INoteService>();

            var context = new DataBaseService(noteProviderMock.Object);
            var repository = new NoteRepository(context);

            var fakeConsoleProvider = new FakeConsoleProvider(null);

            var consoleRepositoryMock = new ConsoleRepository(fakeConsoleProvider, repository, commandHandlerMock.Object, noteServiceMock.Object);

            var service = new NoteConsoleService(
                commandHandlerMock.Object,
                consoleRepositoryMock);

			service.Handle(nameof(Command.List));
			var expected = $"There are zero notes.{Environment.NewLine}" +
				$"Press any key to return to the main window...{Environment.NewLine}";
			Assert.Equal(expected, fakeConsoleProvider.Output);
		}

		[Fact]
		public void AddNoteTest_When_CorrectTitleOrText_Then_Add()
		{
			var noteProviderMock = new Mock<INoteProvider>();
			noteProviderMock.Setup(x => x.CreateNoteList()).Returns(new List<NoteDto>());
            var commandHandlerMock = new Mock<ICommandHelper>();
            commandHandlerMock.Setup(x => x.ParcingCommands(nameof(Command.Add))).Returns((Command.Add, true));
            var noteServiceMock = new Mock<INoteService>();

            var title = "title";
			var text = "text";

            var context = new DataBaseService(noteProviderMock.Object);
            var repository = new NoteRepository(context);
            var linesToRead = new List<string>() { title, text, "n" };
			var fakeConsoleProvider = new FakeConsoleProvider(linesToRead);

            var consoleRepositoryMock = new ConsoleRepository(fakeConsoleProvider, repository, commandHandlerMock.Object, noteServiceMock.Object);

            var service = new NoteConsoleService(
                commandHandlerMock.Object,
                consoleRepositoryMock);

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
			noteProviderMock.Setup(x => x.CreateNoteList()).Returns(new List<NoteDto>());
            var commandHandlerMock = new Mock<ICommandHelper>();
            commandHandlerMock.Setup(x => x.ParcingCommands(nameof(Command.Add))).Returns((Command.Add, true));
            var noteServiceMock = new Mock<INoteService>();

            var title = string.Empty;
			var text = string.Empty;

            var context = new DataBaseService(noteProviderMock.Object);
            var repository = new NoteRepository(context);
            var linesToRead = new List<string>() { title, text, "n" };
			var fakeConsoleProvider = new FakeConsoleProvider(linesToRead);

            var consoleRepositoryMock = new ConsoleRepository(fakeConsoleProvider, repository, commandHandlerMock.Object, noteServiceMock.Object);

            var service = new NoteConsoleService(
                commandHandlerMock.Object,
                consoleRepositoryMock);

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
			noteProviderMock.Setup(x => x.CreateNoteList()).Returns(new List<NoteDto>());
            var commandHandlerMock = new Mock<ICommandHelper>();
            commandHandlerMock.Setup(x => x.ParcingCommands(nameof(Command.Delete))).Returns((Command.Delete, true));
            var noteServiceMock = new Mock<INoteService>();

            var id = "text";

            var context = new DataBaseService(noteProviderMock.Object);
            var repository = new NoteRepository(context);
            var linesToRead = new List<string>() { id, "n" };
			var fakeConsoleProvider = new FakeConsoleProvider(linesToRead);

            var consoleRepositoryMock = new ConsoleRepository(fakeConsoleProvider, repository, commandHandlerMock.Object, noteServiceMock.Object);

            var service = new NoteConsoleService(
                commandHandlerMock.Object,
                consoleRepositoryMock);

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
			noteProviderMock.Setup(x => x.CreateNoteList()).Returns(new List<NoteDto>());
            var commandHandlerMock = new Mock<ICommandHelper>();
            commandHandlerMock.Setup(x => x.ParcingCommands(nameof(Command.Delete))).Returns((Command.Delete, true));
            var noteServiceMock = new Mock<INoteService>();

            var id = "54";

            var context = new DataBaseService(noteProviderMock.Object);
            var repository = new NoteRepository(context);
            repository.AddNote(new NoteDetails { Title = "title", Text = "text" });
            var linesToRead = new List<string>() { id, "n" };
			var fakeConsoleProvider = new FakeConsoleProvider(linesToRead);

            var consoleRepositoryMock = new ConsoleRepository(fakeConsoleProvider, repository, commandHandlerMock.Object, noteServiceMock.Object);

            var service = new NoteConsoleService(
                commandHandlerMock.Object,
                consoleRepositoryMock);

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
			noteProviderMock.Setup(x => x.CreateNoteList()).Returns(new List<NoteDto>());
            var commandHandlerMock = new Mock<ICommandHelper>();
            commandHandlerMock.Setup(x => x.ParcingCommands(nameof(Command.Delete))).Returns((Command.Delete, true));
            var noteServiceMock = new Mock<INoteService>();

            var id = "1";

            var context = new DataBaseService(noteProviderMock.Object);
            var repository = new NoteRepository(context);
            repository.AddNote(new NoteDetails { Title = "title", Text = "text" });
            var linesToRead = new List<string>() { id, "n" };
			var fakeConsoleProvider = new FakeConsoleProvider(linesToRead);

            var consoleRepositoryMock = new ConsoleRepository(fakeConsoleProvider, repository, commandHandlerMock.Object, noteServiceMock.Object);

            var service = new NoteConsoleService(
                commandHandlerMock.Object,
                consoleRepositoryMock);

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
			noteProviderMock.Setup(x => x.CreateNoteList()).Returns(new List<NoteDto>());
            var commandHandlerMock = new Mock<ICommandHelper>();
            commandHandlerMock.Setup(x => x.ParcingCommands(nameof(Command.Edit))).Returns((Command.Edit, true));
            var noteServiceMock = new Mock<INoteService>();

            var id = "text";

            var context = new DataBaseService(noteProviderMock.Object);
            var repository = new NoteRepository(context);
            var linesToRead = new List<string>() { id, "n" };
			var fakeConsoleProvider = new FakeConsoleProvider(linesToRead);

            var consoleRepositoryMock = new ConsoleRepository(fakeConsoleProvider, repository, commandHandlerMock.Object, noteServiceMock.Object);

            var service = new NoteConsoleService(
                commandHandlerMock.Object,
                consoleRepositoryMock);

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
			noteProviderMock.Setup(x => x.CreateNoteList()).Returns(new List<NoteDto>());
            var commandHandlerMock = new Mock<ICommandHelper>();
            commandHandlerMock.Setup(x => x.ParcingCommands(nameof(Command.Edit))).Returns((Command.Edit, true));
            var noteServiceMock = new Mock<INoteService>();

            var id = "44";

            var context = new DataBaseService(noteProviderMock.Object);
            var repository = new NoteRepository(context);
            repository.AddNote(new NoteDetails { Title = "title", Text = "text" });
            var linesToRead = new List<string>() { id, "n" };
			var fakeConsoleProvider = new FakeConsoleProvider(linesToRead);

            var consoleRepositoryMock = new ConsoleRepository(fakeConsoleProvider, repository, commandHandlerMock.Object, noteServiceMock.Object);

            var service = new NoteConsoleService(
                commandHandlerMock.Object,
                consoleRepositoryMock);

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
			noteProviderMock.Setup(x => x.CreateNoteList()).Returns(new List<NoteDto>());
            var commandHandlerMock = new Mock<ICommandHelper>();
            commandHandlerMock.Setup(x => x.ParcingCommands(nameof(Command.Edit))).Returns((Command.Edit, true));
            var noteServiceMock = new Mock<INoteService>();

            var id = "1";
			var title = "title";
			var text = "text";
			var newTitle = "newTitle";
			var newText = "newText";
			var areYouSureAboutThat = "y";

            var context = new DataBaseService(noteProviderMock.Object);
            var repository = new NoteRepository(context);
            repository.AddNote(new NoteDetails { Title = title, Text = text });
            var linesToRead = new List<string>() { id, newTitle, areYouSureAboutThat, newText, "n" };
			var fakeConsoleProvider = new FakeConsoleProvider(linesToRead);

            var consoleRepositoryMock = new ConsoleRepository(fakeConsoleProvider, repository, commandHandlerMock.Object, noteServiceMock.Object);

            var service = new NoteConsoleService(
                commandHandlerMock.Object,
                consoleRepositoryMock);

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
			noteProviderMock.Setup(x => x.CreateNoteList()).Returns(new List<NoteDto>());
            var commandHandlerMock = new Mock<ICommandHelper>();
            commandHandlerMock.Setup(x => x.ParcingCommands(nameof(Command.Edit))).Returns((Command.Edit, true));
            var noteServiceMock = new Mock<INoteService>();

            var id = "1";
			var title = "title";
			var text = "text";
			var newTitle = string.Empty;
			var newText = string.Empty;
			var areYouSureAboutThat = "y";

            var context = new DataBaseService(noteProviderMock.Object);
            var repository = new NoteRepository(context);
            repository.AddNote(new NoteDetails { Title = title, Text = text });
            var linesToRead = new List<string>() { id, newTitle, areYouSureAboutThat, newText, "n" };
			var fakeConsoleProvider = new FakeConsoleProvider(linesToRead);

            var consoleRepositoryMock = new ConsoleRepository(fakeConsoleProvider, repository, commandHandlerMock.Object, noteServiceMock.Object);

            var service = new NoteConsoleService(
                commandHandlerMock.Object,
                consoleRepositoryMock);

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
			noteProviderMock.Setup(x => x.CreateNoteList()).Returns(new List<NoteDto>());
            var commandHandlerMock = new Mock<ICommandHelper>();
            commandHandlerMock.Setup(x => x.ParcingCommands(nameof(Command.Edit))).Returns((Command.Edit, true));
            var noteServiceMock = new Mock<INoteService>();

            var id = "1";
			var title = "title";
			var text = "text";
			var newTitle = string.Empty;
			var newText = string.Empty;
			var areYouSureAboutThat = "n";

            var context = new DataBaseService(noteProviderMock.Object);
            var repository = new NoteRepository(context);
            repository.AddNote(new NoteDetails { Title = title, Text = text });

            var linesToRead = new List<string>() { id, newTitle, areYouSureAboutThat, newText, "n" };
			var fakeConsoleProvider = new FakeConsoleProvider(linesToRead);

            var consoleRepositoryMock = new ConsoleRepository(fakeConsoleProvider, repository, commandHandlerMock.Object, noteServiceMock.Object);

            var service = new NoteConsoleService(
                commandHandlerMock.Object,
                consoleRepositoryMock);

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
			noteProviderMock.Setup(x => x.CreateNoteList()).Returns(new List<NoteDto>());
            var commandHandlerMock = new Mock<ICommandHelper>();
            commandHandlerMock.Setup(x => x.ParcingCommands(nameof(Command.Edit))).Returns((Command.Edit, true));
            var noteServiceMock = new Mock<INoteService>();

            var id = "1";
			var title = "title";
			var text = "text";
			var newTitle = string.Empty;
			var newText = string.Empty;
			var areYouSureAboutThatNotCorrect = "error";
			var areYouSureAboutThat = "y";

            var context = new DataBaseService(noteProviderMock.Object);
            var repository = new NoteRepository(context);
            repository.AddNote(new NoteDetails { Title = title, Text = text });

            var linesToRead = new List<string>() { id, newTitle, areYouSureAboutThatNotCorrect, areYouSureAboutThat, newText, "n" };
			var fakeConsoleProvider = new FakeConsoleProvider(linesToRead);

            var consoleRepositoryMock = new ConsoleRepository(fakeConsoleProvider, repository, commandHandlerMock.Object, noteServiceMock.Object);

            var service = new NoteConsoleService(
                commandHandlerMock.Object,
                consoleRepositoryMock);

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
