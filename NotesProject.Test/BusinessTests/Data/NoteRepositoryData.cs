using System;
using System.Collections.Generic;
using System.Text;

namespace NotesProject.Test.BusinessTests.Data
{
	public class NoteRepositoryData
	{
		public static IEnumerable<object[]> AddNoteTest_ChangeListCount_Data =>
			new List<object[]>
			{
				new object[] { null, "Test"},
				new object[] { "Test", null },
				new object[] { string.Empty, "Test" },
				new object[] { "Test", string.Empty },
				new object[] { "test", "test" },
				new object[] { " test", " test" },
				new object[] { " test ", " test " },
				new object[] { "  Test", "  Test" },				
			};

		public static IEnumerable<object[]> AddNote_ListCountNoChanges_Data =>
			new List<object[]>
			{
				new object[] { null, null },
				new object[] { string.Empty, string.Empty },
				new object[] { null, string.Empty },
				new object[] { string.Empty, null },
				new object[] { "    ", "    " },
			};

		public static IEnumerable<object[]> AddNote_TextAndTitleHasSetValue_Data =>
			new List<object[]>
			{
				new object[] { null, "Test" },
				new object[] { "Test", null },
				new object[] { "test", "test" },
				new object[] { " test", " test" },
				new object[] { " test ", " test " },
				new object[] { "  Test", "  Test" },			
			};
		public static IEnumerable<object[]> EditNote_NotSuccessfulEdit_Data =>
			new List<object[]>
			{
				new object[] { 1, null, null },
				new object[] { 1, string.Empty, string.Empty },
				new object[] { 1, null, string.Empty },
				new object[] { 1, string.Empty, null },
				new object[] { 1, "    ", "    " },
				new object[] { 1, "    ", null },
				new object[] { 1, "    ", string.Empty },
				new object[] { 1, null, "    " },
				new object[] { 1, string.Empty, "    " }				
			};

		public static IEnumerable<object[]> EditNote_SuccessfulEdit_Data =>
			new List<object[]>
			{
				new object[] { 1, null, "Test"},
				new object[] { 1, "Test", null },
				new object[] { 1, string.Empty, "Test" },
				new object[] { 1, "Test", string.Empty },
				new object[] { 1, "Test", "Test" },
			};

	}
}
