using System;
using System.Collections.Generic;
using System.Text;

namespace NotesProject.Test.BusinessTests.Data
{
	public class StringExtensionsData
	{
		public static IEnumerable<object[]> CapitalizeTest_When_DifferentString_Then_NoError_Data =>
			new List<object[]>
			{
				new object[] {"test","Test"},
				new object[] {"TEST","Test"},
				new object[] {"tEST","Test"},
				new object[] {"Test","Test"},
				new object[] {"TeSt","Test"},
				new object[] {" "," "},
				new object[] {"    ","    "},
				new object[] {" Test   "," test   "},
				new object[] {"  Test   ","  test   "},
				new object[] {"    test   ","    test   "},
			};

		public static IEnumerable<object[]> CapitalizeTest_When_Number_Then_NoError_Data =>
			new List<object[]>
			{
				new object[] {"0","0"},
				new object[] {"22","22"},
				new object[] {"  22  ","  22  "},
				new object[] {"  22  ","  22  "},
				new object[] {"22  ","22  "},
				new object[] {"22  ","22  "},
			};

		public static IEnumerable<object[]> CapitalizeTest_When_Null_Then_NullReferenceException_Data =>
			new List<object[]>
			{
				new object[] {null}
			};

		public static IEnumerable<object[]> CapitalizeTest_When_Empty_Then_ArgumentOutOfRangeException_Data =>
			new List<object[]>
			{
				new object[] {string.Empty},
			};
	}
}
