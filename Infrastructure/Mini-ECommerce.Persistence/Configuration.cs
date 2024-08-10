using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Persistence
{
    internal static class Configuration
    {
        static public string ConnectionString
        {
            get
            {
                var configurationManager = new ConfigurationManager();

                // Set the base path to the root of the project
                var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../../Presentation/Mini-ECommerce.API");
                configurationManager.SetBasePath(basePath);

                try
                {
                    configurationManager.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                }
                catch (FileNotFoundException)
                {
                    // Fall back to the production configuration if the default is not found
                    configurationManager.AddJsonFile("appsettings.Production.json", optional: false, reloadOnChange: true);
                }

                var connectionString = configurationManager.GetConnectionString("PostgreSQL");

                // Return the connection string, or throw an exception if it's not found
                return connectionString ?? throw new InvalidOperationException("Connection string 'PostgreSQL' not found.");
            }
        }
    }
}
