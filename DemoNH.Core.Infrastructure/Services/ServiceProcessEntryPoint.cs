using DemoNH.Core.Infrastructure.Services.Descriptor;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using Topshelf;
using IApplicationContext = Spring.Context.IApplicationContext;

namespace DemoNH.Core.Infrastructure.Services
{
	public static class ServiceProcessEntryPoint
	{
		volatile static Logger Logger;

		public static void Run<T>(params string[] args)
		{
			ServiceProcessEntryPoint.Logger = NLog.LogManager.GetLogger("Program");
			AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

			string executable = GetAssemblyPathOfType<T>();
			bool isDebug = (args != null && args.Contains("debug"));
			bool isTest = (args != null && args.Contains("test"));
			bool isConsole = (args != null && args.Contains("console"));

			if (isDebug)
				System.Diagnostics.Debugger.Launch();

			IApplicationContext applicationContext = null;
			Queue<string> paths = new Queue<string>();
			paths.Enqueue(executable + ".xml");
			paths.Enqueue("file://~/IoC.WindowsService.xml");
			RetryManager.Try(delegate
			{
				applicationContext = new Spring.Context.Support.XmlApplicationContext(paths.Dequeue());
			}, paths.Count, true, 10);

			//if (applicationContext == null)
			//{
			//	throw new System.IO.FileNotFoundException("XML de configuracao do servico windows nao foi encontrado nos paths '" + string.Join("','", paths.ToArray()) + "'");
			//}

			ServiceDescriptor serviceDescriptor = applicationContext.GetObject<ServiceDescriptor>();

			if (isConsole)
			{
				var serviceManager = applicationContext.GetObject<ServiceManager>("ServiceManager");
				{
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("#######################################################");
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Iniciando em modo console");
					Console.Write("Serviço: ");
					Console.ForegroundColor = ConsoleColor.Green;
					Console.Write(serviceDescriptor.Name + " ");
					Console.ForegroundColor = ConsoleColor.Red;
					Console.Write("( ");
					Console.ForegroundColor = ConsoleColor.Green;
					Console.Write(serviceDescriptor.FriendlyName);
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine(" )");
					Console.Write("Descrição: ");
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine(serviceDescriptor.Description);
					Console.WriteLine(string.Format("Timeout de Inicio: {0}  Timeout de Finaliação: {1}", serviceManager.StartTimeOut.Value.ToString(), serviceManager.StopTimeOut.Value.ToString()));
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("#######################################################");
					Console.ResetColor();
				}
				if (isTest)
				{
					serviceManager.Test();
				}
				else
				{
					serviceManager.Start(null);
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("#######################################################");
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Rodando... pressione a tecla 'ESC' tecla para finalizar...");
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("#######################################################");
					Console.ResetColor();
					ConsoleKeyInfo keyInfo;
					do
					{
						keyInfo = Console.ReadKey();
					} while (keyInfo.Key != ConsoleKey.Escape);

					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("#######################################################");
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Finalizando...");
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("#######################################################");
					Console.ResetColor();
					serviceManager.Stop(null);
				}
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine("#######################################################");
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine("Finalizado!! Pressione qualquer tecla para sair!");
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine("#######################################################");
				Console.ResetColor();
				Environment.Exit(0);
			}
			else
			{
				TopshelfExitCode exitCode = HostFactory.Run(hostConfig =>
				{
					hostConfig.UseNLog();
					hostConfig.Service<DemoNH.Core.Infrastructure.Services.ServiceManager>(serviceConfigurator =>
					{
						serviceConfigurator.ConstructUsing(() => applicationContext.GetObject<DemoNH.Core.Infrastructure.Services.ServiceManager>());
						serviceConfigurator.WhenStarted((serviceManagerInstance, hostControl) => serviceManagerInstance.Start(hostControl));
						serviceConfigurator.WhenStopped((serviceManagerInstance, hostControl) =>
						{
							var returnValue = serviceManagerInstance.Stop(hostControl);
							Environment.Exit(0);
							return returnValue;
						});
					});

					if (serviceDescriptor.Dependences != null)
					{
						foreach (string dependency in serviceDescriptor.Dependences)
						{
							hostConfig.AddDependency(dependency);
						}
					}
					switch (serviceDescriptor.StartMode)
					{
						case StartMode.Automatically: hostConfig.StartAutomatically(); break;
						case StartMode.AutomaticallyDelayed: hostConfig.StartAutomaticallyDelayed(); break;
						case StartMode.Disabled: hostConfig.Disabled(); break;
						case StartMode.Manually: hostConfig.StartManually(); break;
					}
					switch (serviceDescriptor.IdentityType)
					{
						case AccountType.LocalService: hostConfig.RunAsLocalService(); break;
						case AccountType.LocalSystem: hostConfig.RunAsLocalSystem(); break;
						case AccountType.NetworkService: hostConfig.RunAsNetworkService(); break;
						case AccountType.Prompt: hostConfig.RunAsPrompt(); break;
						case AccountType.Custom: hostConfig.RunAs(serviceDescriptor.CustomIdentityCredential.Username, serviceDescriptor.CustomIdentityCredential.Password); break;
					}
					hostConfig.SetServiceName(serviceDescriptor.Name);
					hostConfig.SetDisplayName(serviceDescriptor.FriendlyName);
					hostConfig.SetDescription(serviceDescriptor.Description);
				});
			}
		}

		private static string GetAssemblyPathOfType<T>()
		{
			var type = typeof(T);
			var assembly = type.Assembly;
			string assemblyPath = assembly.CodeBase;
			Uri assemblyPathUri = new Uri(assemblyPath);
			return assemblyPathUri.LocalPath;
		}

		static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			if (e.IsTerminating)
				ServiceProcessEntryPoint.Logger.Fatal((e.ExceptionObject as Exception).ToString());
			else
				ServiceProcessEntryPoint.Logger.Error((e.ExceptionObject as Exception).ToString());
		}

		static void CurrentDomain_FirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
		{
			ServiceProcessEntryPoint.Logger.Warn("FirstChanceException: " + e.Exception.ToString());
		}
	}
}