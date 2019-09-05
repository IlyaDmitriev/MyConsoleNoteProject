using NotesProject.Business.Extensions;
using NotesProject.Test.BusinessTests.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace NotesProject.Test.BusinessTests
{
	public class StringExtensionsTests
	{		
		[Theory]
		[MemberData(nameof(StringExtensionsData.CapitalizeTest_When_DifferentString_Then_NoError_Data), MemberType = typeof(StringExtensionsData))]
		public void CapitalizeTest_When_DifferentString_Then_NoError(string value, string expected)
		{
			Assert.Equal(expected, value.Capitalize());
		}

		[Theory]
		[MemberData(nameof(StringExtensionsData.CapitalizeTest_When_Number_Then_NoError_Data), MemberType = typeof(StringExtensionsData))]
		public void CapitalizeTest_WhenNumberThenNoError(string value, string expected)
		{			
			Assert.Equal(expected, value.Capitalize());
		}

		[Theory]
		[MemberData(nameof(StringExtensionsData.CapitalizeTest_When_Null_Then_NullReferenceException_Data), MemberType = typeof(StringExtensionsData))]
		public void CapitalizeTest_WhenEmptyThenError(string value)
		{
			Assert.Throws<NullReferenceException>(() => value.Capitalize());
		}

		[Theory]
		[MemberData(nameof(StringExtensionsData.CapitalizeTest_When_Empty_Then_ArgumentOutOfRangeException_Data), MemberType = typeof(StringExtensionsData))]
		public void CapitalizeTest_WhenEmptyThenArgumentOutOfRangeException(string value)
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => value.Capitalize());
		}
	}
}
