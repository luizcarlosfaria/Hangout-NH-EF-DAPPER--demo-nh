using Spring.Context;
using Spring.Context.Support;
using System;
using Topshelf;

namespace DemoNH.Core.Infrastructure.Services
{
    public class ServiceManager : ServiceControl
    {
        public TimeSpan? StartTimeOut { get; set; }

        public TimeSpan? StopTimeOut { get; set; }

        private IService MainService { get; set; }

        public IApplicationContext ApplicationContext { set; get; }

        public ServiceManager()
        {
        }

        public string Name
        {
            get { return "ServiceManager"; }
        }

        public bool Start(HostControl hostControl)
        {
            if (this.StartTimeOut != null && hostControl != null)
                hostControl.RequestAdditionalTime(this.StartTimeOut.Value);
            this.InitializeSpringContext();
            this.StartMainService();
            return true;
        }

        public void Test()
        {
            this.InitializeSpringContext();
        }


        public bool Stop(HostControl hostControl)
        {
            if (this.StopTimeOut != null && hostControl != null)
                hostControl.RequestAdditionalTime(this.StopTimeOut.Value);

            this.StopMainService();
            this.FinalizeSpringContext();
            return true;
        }

        #region Spring Context Operations

        private void InitializeSpringContext()
        {
            if (this.ApplicationContext == null)
            {
                try
                {
                    this.ApplicationContext = ContextRegistry.GetContext();
                }
                catch (Exception ex)
                {

                    throw new ServiceStartException("Erro na inicialização do contexto spring.", ex);
                }
            }
        }

        private void FinalizeSpringContext()
        {
            try
            {
                if (this.ApplicationContext != null)
                {
                    this.ApplicationContext.Dispose();
                    System.Configuration.ConfigurationManager.RefreshSection("spring/context");
                }
                this.ApplicationContext = null;
            }
            catch (Exception ex)
            {

                throw new ServiceStopException("Erro na finalização do contexto spring.", ex);
            }
        }

        #endregion Spring Context Operations

        #region LifeCycle Management using MainService Operations (Get / Start / Stop)

        private void StartMainService()
        {
            RetryManager.Try(
                delegate
                {
                    this.MainService = this.ApplicationContext.GetObject<IService>("MainService");
                    this.MainService.Start();
                },
                delegate (Exception ex)
                {
                    this.MainService = null;
                    throw new ServiceStartException("Erro na iniciação do MainService.", ex);
                }
            );
        }

        private void StopMainService()
        {
            if (this.MainService != null)
            {
                RetryManager.Try(
                    delegate
                    {
                        this.MainService.Stop();
                        this.MainService = null;
                    },
                    delegate (Exception ex)
                    {
                        this.MainService = null;
                        throw new ServiceStopException("Erro na paralização do MainService.", ex);
                    }
                );
            }
        }

        #endregion LifeCycle Management using MainService Operations (Get / Start / Stop)
    }
}