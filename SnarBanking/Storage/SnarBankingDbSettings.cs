using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace SnarBanking.Storage
{
    public static class Settings
    {
        public record SnarBankingDbSettings
        {
            public const string SectionName = "SnarBankingDatabase";
            public string ConnectionString { get; set; } = default!;
            public string DatabaseName { get; set; } = default!;
            public string DefaultCollectionName { get; set; } = default!;
        }

        public static IServiceCollection AddSnarBankingDbSettings(this IServiceCollection services) =>
            services
                .AddSingleton(sp => sp.GetRequiredService<IOptions<SnarBankingDbSettings>>().Value);
    }
}

