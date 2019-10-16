namespace NotesProject.Domain.Extensions
{
	public static class StringExtensions
	{
		public static string Capitalize(this string text)
		{
			return text.Trim().Substring(0, 1).ToUpper() + text.Trim().Substring(1).ToLower();
		}
	}
}
