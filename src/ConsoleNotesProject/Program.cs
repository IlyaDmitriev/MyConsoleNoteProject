using ConsoleNotes.Services.Implementations;
using ConsoleNotes.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using NotesProject.Business.Services.Implementations;
using NotesProject.Business.Services.Interfaces;

namespace ConsoleNotes
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
			serviceCollection.AddTransient<ICommandService, CommandService>();
			serviceCollection.AddTransient<INoteRepository, NoteRepository>();
			serviceCollection.AddTransient<INoteService, NoteConsoleService>();

			// add app
			serviceCollection.AddTransient<NoteApp>();
		}
	}
}
