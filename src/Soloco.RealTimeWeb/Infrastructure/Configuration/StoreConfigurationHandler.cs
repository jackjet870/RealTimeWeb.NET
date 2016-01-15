using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json.Linq;
using Soloco.RealTimeWeb.Common;
using Soloco.RealTimeWeb.Common.Messages;

namespace Soloco.RealTimeWeb.Infrastructure.Configuration
{
    public class StoreConfigurationHandler : IHandleMessage<StoreConfigurationCommand, Result>
    {
        private readonly IApplicationEnvironment _applicationEnvironment;

        public StoreConfigurationHandler(IApplicationEnvironment applicationEnvironment)
        {
            if (applicationEnvironment == null) throw new ArgumentNullException(nameof(applicationEnvironment));

            _applicationEnvironment = applicationEnvironment;
        }

        public Task<Result> Handle(StoreConfigurationCommand command)
        {
            var json = CreateJson(command);
            var fileName = "appsettings.private.json";
            var localConfigFileName = Path.Combine(_applicationEnvironment.ApplicationBasePath, fileName);

            File.WriteAllText(localConfigFileName, json);

            return Task.FromResult(Result.Success);
        }

        private string CreateJson(StoreConfigurationCommand command)
        {
            var config = new JObject
            {
                {
                    "authentication", new JObject
                    {
                        {
                            "google", new JObject
                            {
                                {"clientId", command.GoogleClientId},
                                {"clientSecret", command.GoogleClientSecret}
                            }
                        },
                        {
                            "facebook", new JObject
                            {
                                {"appId", command.FacebookAppId},
                                {"appSecret", command.FacebookAppSecret}
                            }
                        }
                    }
                },
                {
                    "general", new JObject
                    {
                        { "configured", true }
                    }
                }
            };

            return config.ToString();
        }
    }
}