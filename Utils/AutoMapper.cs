using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace Utils
{
    public static class AutoMapper
    {
        private static readonly ConcurrentDictionary<string, IMapper> _mappers = new();

        /// <summary>
        /// Khởi tạo AutoMapper, tự động quét tất cả Profile trong Assembly.
        /// </summary>
        /// <param name="key">Tên định danh mapper (thường là tên service hoặc namespace)</param>
        /// <param name="assemblies">Danh sách Assembly chứa Profile</param>
        /// <returns>IMapper instance</returns>
        public static IMapper GetMapper(string key, params Assembly[] assemblies)
        {
            if (_mappers.ContainsKey(key))
            {
                return _mappers[key];
            }

            var config = new MapperConfiguration(cfg =>
            {
                foreach (var assembly in assemblies)
                {
                    cfg.AddMaps(assembly);
                }
            });

            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();
            _mappers[key] = mapper;

            return mapper;
        }

        /// <summary>
        /// Shortcut: Load tất cả profile từ assembly hiện tại + shared
        /// </summary>
        public static IMapper GetDefaultMapper()
        {
            string key = "DefaultMapper";

            if (_mappers.TryGetValue(key, out var existingMapper))
                return existingMapper;

            var current = Assembly.GetExecutingAssembly();
            var shared = typeof(AutoMapper).Assembly;

            return GetMapper(key, current, shared);
        }
    }
}
