using System;
using AutoMapper;

namespace Course.Services.Order.Application.Mapping
{
    public static class ObjectMapping
    {
        private static readonly Lazy<IMapper> Lazy = new(() =>
        {
            MapperConfiguration configuration = new(cfg =>
            {
                cfg.AddProfile<CustomMapping>();
            });
            return configuration.CreateMapper();
        });

        public static IMapper Mapper => Lazy.Value;
    }
}
