using Microsoft.Extensions.DependencyInjection;
using NotesProject.Application.Repositories;
using NotesProject.ConsoleUI.Helpers;
using NotesProject.ConsoleUI.Services.Implementations;
using NotesProject.ConsoleUI.Services.Interfaces;
using NotesProject.DataBase.FileServices;
using NotesProject.DataBase.Interfaces;
using NotesProject.DataBase.Services;
using NotesProject.Domain.Interfaces;
using NotesProject.Domain.Services;
using NotesProject.Infrastructure.Interfaces;

namespace NotesProject.ConsoleUI
{
	class Program 
	{
		public static void Main(string[] args)
		{
			// create service collection
			var serviceCollection = new ServiceCollection();
			ConfigureServices(serviceCollection);

			// create service provider
			var serviceProvider = serviceCollection.BuildServiceProvider();

			// entry to run app
			serviceProvider.GetService<NoteApp>().Run();
		}

		private static void ConfigureServices(IServiceCollection serviceCollection)
		{
			// add services
			serviceCollection.AddTransient<IConsoleProvider, ConsoleProvider>();
			serviceCollection.AddTransient<INoteRepository, NoteRepository>();
			serviceCollection.AddTransient<INoteService, NoteConsoleService>();
			serviceCollection.AddTransient<INoteProvider, NoteProvider>();
			serviceCollection.AddTransient<ICommandHelper, CommandHelper>();
            serviceCollection.AddTransient<IConsoleRepository, ConsoleRepository>();
            serviceCollection.AddTransient<IContext, DataBaseService>();
            serviceCollection.AddTransient<IFileService, FileService>();

			// add app
			serviceCollection.AddTransient<NoteApp>();
		}
	}
}
