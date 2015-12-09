﻿using System.Web.Http;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Owin;
using Soloco.RealTimeWeb;
using Soloco.RealTimeWeb.Common;
using Soloco.RealTimeWeb.Common.Infrastructure;
using Soloco.RealTimeWeb.Common.Infrastructure.DryIoc;
using Soloco.RealTimeWeb.Membership;
using Soloco.RealTimeWeb.Membership.Messages.Commands;
using IDependencyResolver = System.Web.Http.Dependencies.IDependencyResolver;

[assembly: OwinStartup(typeof(Startup))]

namespace Soloco.RealTimeWeb
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            LoggingInitializer.Initialize();

            var httpConfiguration = HttpConfiguration();
            app
                .ConfigureOAuth(httpConfiguration)
                .UseWebApi(httpConfiguration)
                .UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll)
                .MapSignalR("/sockets", new HubConfiguration());

            InitializeDatabase(httpConfiguration.DependencyResolver);
        }

        private static void InitializeDatabase(IDependencyResolver dependencyResolver)
        {
            var messageDispatcher = dependencyResolver.GetMessageDispatcher(); 

            var command = new InitializeDatabaseCommand();
            messageDispatcher.Execute(command).Wait();
        }

        private static HttpConfiguration HttpConfiguration()
        {
            return new HttpConfiguration()
                .MapRoutes()
                .FormatJsonCamelCase()
                .RegisterDependencyResolver(configure => configure
                    .RegisterCommon()
                    .RegisterMembership()
                    .RegisterApiControllers()
                );
        }
    }
}