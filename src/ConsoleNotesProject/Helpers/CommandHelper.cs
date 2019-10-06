using ConsoleNotes.Models;
using ConsoleNotes.Models.Enums;
using System;
using System.Collections.Generic;

namespace ConsoleNotes.Helpers
{
	public class CommandHelper
	{
		public static void BackToTheRoots()
		{
			Console.Clear();
			Console.WriteLine($"--->     {Constants.ProjectName} {Constants.Version}     <---");
			Console.WriteLine();
		}

		public static void ShowInitialWindow()
		{
			Console.WriteLine("#################################################################");
			Console.WriteLine("#                           Enter command                       #");
			Console.WriteLine($"#                Enter '{nameof(Command.Help)}', if you need help                 #");
			Console.WriteLine("#################################################################");
			Console.Write("> ");
		}

		public static void ShowHelp(Dictionary<Command, string> commandsWithDescription)
		{
			Console.WriteLine("#################################################################");
			Console.WriteLine("#                             List of commands                    ");

			foreach (var command in commandsWithDescription)
			{
				Console.WriteLine($"#      Enter '{command.Key}', if you need to {command.Value}");
			}

			Console.WriteLine("#################################################################");
		}

		public static void DoActionOnResponse(string response, Action actionYes, Action actionNo)
		{
			var formattedResult = response.Trim().ToLower();

			if (formattedResult == "y")
			{
				actionYes();
			}
			else if (formattedResult == "n")
			{
				actionNo();
			}
			else
			{
				Console.WriteLine("Wrong input! Pass only \"y\" or \"n\".");
				DoActionOnResponse(Console.ReadLine().Trim().ToLower(), actionYes, actionNo);
			}
		}
	}
}
