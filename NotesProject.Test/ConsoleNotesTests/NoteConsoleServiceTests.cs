using ConsoleNotes.Services.Implementations;
using ConsoleNotes.Services.Interfaces;
using Moq;
using NotesProject.Business.Models;
using NotesProject.Business.Services.Implementations;
using NotesProject.Business.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Utilities;
using Xunit;
using Xunit.Abstractions;

namespace NotesProject.Test.ConsoleNotesTests
{
	public class NoteConsoleServiceTests
	{
		private readonly Mock<INoteProvider> _noteProviderMock;
		private readonly Mock<IConsoleProvider> _consoleProviderMock;

		public NoteConsoleServiceTests()
		{
			_noteProviderMock = new Mock<INoteProvider>();
			_noteProviderMock.Setup(x => x.CreateNoteList()).Returns(new List<Note>());

			_consoleProviderMock = new Mock<IConsoleProvider>();
		}

		[Fact]
		public void ShowNoteTest_When_HasElements_Then_ShowList()
		{
			//_consoleProviderMock.Setup(x => x.ReadLine()).Returns(" ");

			var title = "title";
			var text = "text";
			var id = 1;

			var repository = new NoteRepository(_noteProviderMock.Object);
			repository.AddNote(title, text);

			var fakeConsoleProvider = new FakeConsoleProvider(null);
			var service = new NoteConsoleService(repository, fakeConsoleProvider);

			service.ShowNotes();

			Assert.Equal($"Here are list of all notes:{Environment.NewLine}" +
				$"id: {id} || title: '{title}' || text: '{text}'{Environment.NewLine}", fakeConsoleProvider.Output);
		}

		[Fact]
		public void ShowNoteTest_When_NoElements_Then_ShowInfoMessage()
		{
			var repository = new NoteRepository(_noteProviderMock.Object);

			var fakeConsoleProvider = new FakeConsoleProvider(null);
			var service = new NoteConsoleService(repository, fakeConsoleProvider);

			service.ShowNotes();

			Assert.Equal($"There are zero notes.{Environment.NewLine}", fakeConsoleProvider.Output);
		}

		[Fact]
		public void AddNoteTest_When_CorrectTitleOrText_Then_Add()
		{
			var title = "title";
			var text = "text";

			var repository = new NoteRepository(_noteProviderMock.Object);
			var linesToRead = new List<string>() { title, text };
			var fakeConsoleProvider = new FakeConsoleProvider(linesToRead);
			var service = new NoteConsoleService(repository, fakeConsoleProvider);

			service.AddNote();

			var expected = $"Enter note title{Environment.NewLine}" +
				$"> " +
				$"Enter note text{Environment.NewLine}" +
				$"> " +
				$"Note was added.{Environment.NewLine}";

			Assert.Equal(expected, fakeConsoleProvider.Output);
		}

		[Fact]
		public void AddNoteTest_When_NotCorrectTitleOrText_Then_NotAdd()
		{
			var title = "title";
			var text = "text";

			var repository = new NoteRepository(_noteProviderMock.Object);
			var linesToRead = new List<string>() { title, text };
			var fakeConsoleProvider = new FakeConsoleProvider(linesToRead);
			var service = new NoteConsoleService(repository, fakeConsoleProvider);

			service.AddNote();

			var expected = $"Enter note title{Environment.NewLine}" +
				$"> " +
				$"Enter note text{Environment.NewLine}" +
				$"> " +
				$"Note was not added. Enter title or text.{Environment.NewLine}";

			Assert.Equal(expected, fakeConsoleProvider.Output);
		}
	}
}
