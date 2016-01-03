using System;
using System.Configuration;
using System.Reflection;

namespace Gothandy.StartUp
{
    public class Tools
    {

        /// <summary>
        /// In AssemblyInfo set AssemblyVersion using * notation to get date time.
        /// </summary>
        public static DateTime GetBuildDateTime(Type type)
        {
            var entryAssembly = Assembly.GetAssembly(type);
            var assemblyName = entryAssembly.GetName();
            var version = assemblyName.Version;
            var timeSpan =
                new TimeSpan(
                    TimeSpan.TicksPerDay * version.Build + // days since 1 January 2000
                    TimeSpan.TicksPerSecond * 2 * version.Revision); // seconds since midnight, (multiply by 2 to get original)

            return new DateTime(2000, 1, 1).Add(timeSpan);
        }

        public static string CheckAndGetAppSettings(string name)
        {
            var value = ConfigurationManager.AppSettings[name];
            if (value == null) throw new ArgumentNullException(name);
            return value;
        }
    }
}
