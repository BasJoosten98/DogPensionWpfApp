using DogPensionWpfApp.EntityFramework;
using DogPensionWpfApp.Services;
using DogPensionWpfApp.ViewModels;
using DogPensionWpfApp.Views;
using Microsoft.AspNet.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DogPensionWpfApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = CreateHostBuilder().Build();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();

            Window window = _host.Services.GetRequiredService<MainWindow>();
            window.Show();

            base.OnStartup(e);
        }

        public static IHostBuilder CreateHostBuilder(string[] args = null)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(c =>
                {
                    //PUT CONFIG FILES HERE
                    c.AddJsonFile("appsettings.json");
                })
                .ConfigureServices((context, services) =>
                {
                    //PUT DEPENDENCY INJECTION STUFF HERE

                    //Setting up database
                    string databaseConnectionString = context.Configuration.GetConnectionString("default");
                    Action<DbContextOptionsBuilder> configureDbContext = options =>
                    {
                        options.UseSqlServer(databaseConnectionString);
                    };

                    services.AddDbContext<DogPensionDbContext>(configureDbContext); //for migrations
                    services.AddSingleton<DogPensionDbContextFactory>(new DogPensionDbContextFactory(configureDbContext)); //For creating DbContext object

                    //Services
                    services.AddTransient<IPasswordHasher, PasswordHasher>();
                    services.AddTransient<AccountService>();
                    services.AddTransient<OwnerService>();
                    services.AddTransient<DogService>();
                    services.AddTransient<ReservationService>();
                    services.AddSingleton<Authenticator>();
                    services.AddSingleton<Navigator>();

                    //ViewModels
                    services.AddSingleton<MainViewModel>();
                    services.AddTransient<LoginViewModel>();
                    services.AddTransient<RegisterViewModel>();
                    services.AddTransient<OwnerListViewModel>();
                    services.AddTransient<DogListViewModel>();
                    services.AddTransient<OwnerDetailsViewModel>();
                    services.AddTransient<DogDetailsViewModel>();
                    services.AddTransient<ReservationDetailsViewModel>();
                    services.AddTransient<ReservationListViewModel>();

                    //ViewModels factory
                    services.AddSingleton<ViewModelFactory>();

                    services.AddSingleton<CreateViewModel<LoginViewModel>>(services => () => services.GetRequiredService<LoginViewModel>());
                    services.AddSingleton<CreateViewModel<RegisterViewModel>>(services => () => services.GetRequiredService<RegisterViewModel>());
                    services.AddSingleton<CreateViewModel<OwnerListViewModel>>(services => () => services.GetRequiredService<OwnerListViewModel>());
                    services.AddSingleton<CreateViewModel<DogListViewModel>>(services => () => services.GetRequiredService<DogListViewModel>());
                    services.AddSingleton<CreateViewModel<OwnerDetailsViewModel>>(services => () => services.GetRequiredService<OwnerDetailsViewModel>());
                    services.AddSingleton<CreateViewModel<DogDetailsViewModel>>(services => () => services.GetRequiredService<DogDetailsViewModel>());
                    services.AddSingleton<CreateViewModel<ReservationDetailsViewModel>>(services => () => services.GetRequiredService<ReservationDetailsViewModel>());
                    services.AddSingleton<CreateViewModel<ReservationListViewModel>>(services => () => services.GetRequiredService<ReservationListViewModel>());

                    //MainWindow
                    services.AddSingleton<MainWindow>(s => new MainWindow(s.GetRequiredService<MainViewModel>()));
                });
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await _host.StopAsync();
            _host.Dispose();

            base.OnExit(e);
        }
    }
}
