namespace Hale.Lib.Utilities
{
    using System;
    using System.Collections.Generic;

    public static class ServiceProvider
    {
        private static Dictionary<Type, object> instances = new Dictionary<Type, object>();

        /// <summary>
        ///  Returns the singleton instance of type T or null if it is missing
        /// </summary>
        /// <typeparam name="T">Type of singleton</typeparam>
        /// <returns>Singleton instance of type T or null</returns>
        public static T GetService<T>()
            where T : class
        {
            var t = typeof(T);
            if (instances.ContainsKey(t))
            {
                return (T)instances[t];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        ///  Returns the singleton instance of type T or throws an exception if it is missing
        /// </summary>
        /// <typeparam name="T">Type of singleton</typeparam>
        /// <returns>Singleton instance of type T</returns>
        public static T GetServiceCritical<T>()
            where T : class
        {
            T instance = GetService<T>();
            if (instance == null)
            {
                throw new CriticalServiceProviderNotFoundException(typeof(T));
            }

            return instance;
        }

        public static bool ServiceAvailable<T>()
        {
            return instances.ContainsKey(typeof(T));
        }

        public static void SetService<T>(T instance)
        {
            instances[typeof(T)] = instance;
        }
    }
}
