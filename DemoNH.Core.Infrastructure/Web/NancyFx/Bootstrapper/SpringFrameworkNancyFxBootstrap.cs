using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.Diagnostics;
using Nancy.Responses;
using Nancy.TinyIoc;
using Nancy.ViewEngines;
using Spring.Aop.Framework.DynamicProxy;
using Spring.Context;
using Spring.Core.IO;

namespace DemoNH.Core.Infrastructure.Web.NancyFx.Bootstrapper
{

	/// <summary>
	/// TinyIoC bootstrapper - registers default route resolver and registers itself as
	/// INancyModuleCatalog for resolving modules but behaviour can be overridden if required.
	/// </summary>
	public class SpringFrameworkNancyFxBootstrap : NancyBootstrapperWithRequestContainerBase<TinyIoCContainer>, IApplicationContextAware
	{
		public IApplicationContext ApplicationContext { set; protected get; }

		public IRootPathProvider DefaultRootPathProvider { get; set; }

		public Type ViewLocationProviderType { get; set; }

		public List<IConventionConfigurer> ConventionConfigurers { get; set; }

		public Dictionary<Assembly, string> ResourceViewLocationProviderNamespaces { get; set; }

		public IResource FaviconResource { get; set; }


		protected override IRootPathProvider RootPathProvider { get { return this.DefaultRootPathProvider; } }

		public SpringFrameworkNancyFxBootstrap()
		{

		}

		/// <summary>
		/// Personalized Favicon
		/// </summary>
		protected override byte[] FavIcon
		{
			get
			{
				if (this.FaviconResource == null)
					return base.FavIcon;
				else
				{
					byte[] tempFavicon = new byte[this.FaviconResource.InputStream.Length];
					this.FaviconResource.InputStream.Read(tempFavicon, 0, (int)this.FaviconResource.InputStream.Length);
					return tempFavicon;
				}
			}
		}


		/// <summary>
		/// Gets the assemblies to ignore when autoregistering the application container
		/// Return true from the delegate to ignore that particular assembly, returning true
		/// does not mean the assembly *will* be included, a false from another delegate will
		/// take precedence.
		/// </summary>
		protected virtual IEnumerable<Func<Assembly, bool>> AutoRegisterIgnoredAssemblies
		{
			get { return DefaultNancyBootstrapper.DefaultAutoRegisterIgnoredAssemblies; }
		}

		/// <summary>
		/// Configures the container using AutoRegister followed by registration
		/// of default INancyModuleCatalog and IRouteResolver.
		/// </summary>
		/// <param name="container">Container instance</param>
		protected override void ConfigureApplicationContainer(TinyIoCContainer container)
		{
			AutoRegister(container, this.AutoRegisterIgnoredAssemblies);
		}

		/// <summary>
		/// Resolve INancyEngine
		/// </summary>
		/// <returns>INancyEngine implementation</returns>
		protected override sealed INancyEngine GetEngineInternal()
		{
			return this.ApplicationContainer.Resolve<INancyEngine>();
		}

		/// <summary>
		/// Create a default, unconfigured, container
		/// </summary>
		/// <returns>Container instance</returns>
		protected override TinyIoCContainer GetApplicationContainer()
		{
			return new TinyIoCContainer();
		}

		/// <summary>
		/// Register the bootstrapper's implemented types into the container.
		/// This is necessary so a user can pass in a populated container but not have
		/// to take the responsibility of registering things like INancyModuleCatalog manually.
		/// </summary>
		/// <param name="applicationContainer">Application container to register into</param>
		protected override sealed void RegisterBootstrapperTypes(TinyIoCContainer applicationContainer)
		{
			applicationContainer.Register<INancyModuleCatalog>(this);
		}

		/// <summary>
		/// Register the default implementations of internally used types into the container as singletons
		/// </summary>
		/// <param name="container">Container to register into</param>
		/// <param name="typeRegistrations">Type registrations to register</param>
		protected override sealed void RegisterTypes(TinyIoCContainer container, IEnumerable<TypeRegistration> typeRegistrations)
		{
			foreach (var typeRegistration in typeRegistrations)
			{
				switch (typeRegistration.Lifetime)
				{
					case Lifetime.Transient:
						container.Register(typeRegistration.RegistrationType, typeRegistration.ImplementationType).AsMultiInstance();
						break;
					case Lifetime.Singleton:
						container.Register(typeRegistration.RegistrationType, typeRegistration.ImplementationType).AsSingleton();
						break;
					case Lifetime.PerRequest:
						throw new InvalidOperationException("Unable to directly register a per request lifetime.");
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}

		/// <summary>
		/// Register the various collections into the container as singletons to later be resolved
		/// by IEnumerable{Type} constructor dependencies.
		/// </summary>
		/// <param name="container">Container to register into</param>
		/// <param name="collectionTypeRegistrationsn">Collection type registrations to register</param>
		protected override sealed void RegisterCollectionTypes(TinyIoCContainer container, IEnumerable<CollectionTypeRegistration> collectionTypeRegistrationsn)
		{
			foreach (var collectionTypeRegistration in collectionTypeRegistrationsn)
			{
				switch (collectionTypeRegistration.Lifetime)
				{
					case Lifetime.Transient:
						container.RegisterMultiple(collectionTypeRegistration.RegistrationType, collectionTypeRegistration.ImplementationTypes).AsMultiInstance();
						break;
					case Lifetime.Singleton:
						container.RegisterMultiple(collectionTypeRegistration.RegistrationType, collectionTypeRegistration.ImplementationTypes).AsSingleton();
						break;
					case Lifetime.PerRequest:
						throw new InvalidOperationException("Unable to directly register a per request lifetime.");
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}

		/// <summary>
		/// Register the given module types into the container
		/// </summary>
		/// <param name="container">Container to register into</param>
		/// <param name="moduleRegistrationTypes">NancyModule types</param>
		protected override sealed void RegisterRequestContainerModules(TinyIoCContainer container, IEnumerable<ModuleRegistration> moduleRegistrationTypes)
		{
			foreach (var moduleRegistrationType in moduleRegistrationTypes)
			{
				container.Register(
					typeof(INancyModule),
					moduleRegistrationType.ModuleType,
					moduleRegistrationType.ModuleType.FullName).
					AsSingleton();
			}
		}

		/// <summary>
		/// Register the given instances into the container
		/// </summary>
		/// <param name="container">Container to register into</param>
		/// <param name="instanceRegistrations">Instance registration types</param>
		protected override void RegisterInstances(TinyIoCContainer container, IEnumerable<InstanceRegistration> instanceRegistrations)
		{
			foreach (var instanceRegistration in instanceRegistrations)
			{
				container.Register(
					instanceRegistration.RegistrationType,
					instanceRegistration.Implementation);
			}
		}

		/// <summary>
		/// Creates a per request child/nested container
		/// </summary>
		/// <returns>Request container instance</returns>
		protected override sealed TinyIoCContainer CreateRequestContainer()
		{
			return this.ApplicationContainer.GetChildContainer();
		}

		/// <summary>
		/// Gets the diagnostics for initialisation
		/// </summary>
		/// <returns>IDiagnostics implementation</returns>
		protected override IDiagnostics GetDiagnostics()
		{
			return this.ApplicationContainer.Resolve<IDiagnostics>();
		}

		/// <summary>
		/// Gets all registered startup tasks
		/// </summary>
		/// <returns>An <see cref="IEnumerable{T}"/> instance containing <see cref="IApplicationStartup"/> instances. </returns>
		protected override IEnumerable<IApplicationStartup> GetApplicationStartupTasks()
		{
			return this.ApplicationContainer.ResolveAll<IApplicationStartup>(false);
		}

		/// <summary>
		/// Gets all registered request startup tasks
		/// </summary>
		/// <returns>An <see cref="IEnumerable{T}"/> instance containing <see cref="IRequestStartup"/> instances.</returns>
		protected override IEnumerable<IRequestStartup> RegisterAndGetRequestStartupTasks(TinyIoCContainer container, Type[] requestStartupTypes)
		{
			container.RegisterMultiple(typeof(IRequestStartup), requestStartupTypes);

			return container.ResolveAll<IRequestStartup>(false);
		}

		/// <summary>
		/// Gets all registered application registration tasks
		/// </summary>
		/// <returns>An <see cref="IEnumerable{T}"/> instance containing <see cref="IRegistrations"/> instances.</returns>
		protected override IEnumerable<IRegistrations> GetRegistrationTasks()
		{
			return this.ApplicationContainer.ResolveAll<IRegistrations>(false);
		}

		/// <summary>
		/// Retrieve all module instances from the container
		/// </summary>
		/// <param name="container">Container to use</param>
		/// <returns>Collection of NancyModule instances</returns>
		protected override sealed IEnumerable<INancyModule> GetAllModules(TinyIoCContainer container)
		{
			IEnumerable<INancyModule> modulesFromSpring = this.ApplicationContext.GetObjects<INancyModule>().Values;
			IEnumerable<INancyModule> modulesFromTinyIoCContainer = container.ResolveAll<INancyModule>(false);

			List<INancyModule> returnValue = new List<INancyModule>();
			foreach (INancyModule module in modulesFromTinyIoCContainer)
			{
				if (returnValue.Any(it => it.GetType() == module.GetType()) == false)
					returnValue.Add(module);
			}
			return returnValue;
		}

		/// <summary>
		/// Retreive a specific module instance from the container
		/// </summary>
		/// <param name="container">Container to use</param>
		/// <param name="moduleType">Type of the module</param>
		/// <returns>NancyModule instance</returns>
		protected override sealed INancyModule GetModule(TinyIoCContainer container, Type moduleType)
		{
			INancyModule returnValue = this.ApplicationContext.GetObject<INancyModule>(moduleType.Name);
			return returnValue;
		}

		/// <summary>
		/// Executes auto registation with the given container.
		/// </summary>
		/// <param name="container">Container instance</param>
		private static void AutoRegister(TinyIoCContainer container, IEnumerable<Func<Assembly, bool>> ignoredAssemblies)
		{
			Assembly[] excludedAssemblies = new Assembly[] { typeof(NancyEngine).Assembly, typeof(SpringFrameworkNancyFxBootstrap).Assembly };

			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			var query = from assembly in assemblies
						where
							ignoredAssemblies.Any(ia => ia(assembly)) == false
							&&
							assembly.IsDynamic == false
						select assembly;

			container.AutoRegister(query, DuplicateImplementationActions.RegisterMultiple, t => excludedAssemblies.Contains(t.Assembly) == false);
		}


		protected override void ConfigureConventions(Nancy.Conventions.NancyConventions nancyConventions)
		{
			foreach (IConventionConfigurer conventionConfigurer in this.ConventionConfigurers)
			{
				conventionConfigurer.ConfigureConventions(nancyConventions);
			}
		}

		protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
		{
			StaticConfiguration.DisableErrorTraces = false;
			StaticConfiguration.EnableRequestTracing = true;
			Nancy.Json.JsonSettings.MaxJsonLength = Int32.MaxValue;
		}
		protected override NancyInternalConfiguration InternalConfiguration
		{
			get
			{
				return NancyInternalConfiguration.WithOverrides(configurationInstance =>
				{
					configurationInstance.ViewLocationProvider = this.ViewLocationProviderType;
				});
			}
		}

	}
}
