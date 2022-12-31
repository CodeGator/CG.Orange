using Microsoft.Extensions.Hosting;

// The line of code below prevents this error, while running efcore / Visual Studio
//   command line tools, such as add-migration: "An error occurred while accessing
//   the Microsoft.Extensions.Hosting services. Continuing without the application
//   service provider. Error: The entry point exited without ever building an IHost."

new HostBuilder().Build();
