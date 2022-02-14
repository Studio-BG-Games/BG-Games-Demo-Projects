using System;
using System.Collections.Generic;

namespace Baby_vs_Aliens
{
    public static class ServiceLocator
    {
        #region Fields

        private static readonly Dictionary<Type, object> _services =
                new Dictionary<Type, object>();

        #endregion

        #region Methods

        public static void AddService<T>(T value) where T : class
        {
            var typeValue = typeof(T);
            if (!_services.ContainsKey(typeValue))
            {
                _services[typeValue] = value;
            }
        }

        public static T GetService<T>()
        {
            var type = typeof(T);

            if (_services.ContainsKey(type))
            {
                return (T)_services[type];
            }

            return default;
        }

        #endregion
    }
}