using System;
using System.Collections.Generic;
using System.Text;

namespace NotesProject.Test.BusinessTests.Data
{
	public class NoteRepositoryData
	{
		public static IEnumerable<object[]> AddNoteTest_When_Add_Then_ChangeListCount_Data =>
			new List<object[]>
			{
				new object[] {"test","test"},
				new object[] {" test"," test"},
				new object[] {" test "," test "},
				new object[] {"  Test","  Test"},
				new object[] { string.Empty, string.Empty },
				new object[] {null,null},
				new object[] {"    ","    "},
				new object[] {null, string.Empty},
				new object[] {null, "Test"},
				new object[] { string.Empty, null },
				new object[] { "Test", null },
			};

		public static IEnumerable<object[]> AddNoteTest_When_Add_Then_TextAndTitleHasSetValue_Data =>
			new List<object[]>
			{
				new object[] {"test","test"},
				new object[] {" test"," test"},
				new object[] {" test "," test "},
				new object[] {"  Test","  Test"},
				new object[] { string.Empty, string.Empty },
				new object[] {null,null},
				new object[] {"    ","    "},
				new object[] {null, string.Empty},
				new object[] {null, "Test"},
				new object[] { string.Empty, null },
				new object[] { "Test", null },
			};

		public static IEnumerable<object[]> DeleteNoteWithNoError_Data =>
			new List<object[]>
			{
				new object[] {"title","text"},				
			};
		
	}
}
