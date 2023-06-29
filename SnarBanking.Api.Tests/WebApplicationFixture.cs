using System;

using MediatR;

using static SnarBanking.Storage.Service;

namespace SnarBanking.Api.IntegrationTests
{
    public class WebApplicationFixture : IDisposable
    {
        public readonly SnarBankingTestWebApplicationFactory _factory;
        public readonly IServiceProvider _serviceProvider;

        public WebApplicationFixture()
        {
            _factory = new SnarBankingTestWebApplicationFactory();
            _serviceProvider = _factory.Services;
        }

        //public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        //{
        //    using var scope = _serviceProvider.CreateScope();

        //    var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

        //    return await mediator.Send(request);
        //}

        //public async Task SendASync(IBaseRequest request)
        //{
        //    using var scope = _serviceProvider.CreateScope();

        //    var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

        //    await mediator.Send(request);
        //}

        //public Task InitializeDb()
        //{
        //    using var scope = _serviceProvider.CreateScope();

        //    var db = scope.ServiceProvider.GetRequiredService<SnarBankingMongoDbService>();
        //    Utilities.Utilities.ReInitializeDbForTests(db);

        //    return Task.CompletedTask;
        //}

        public void Dispose()
        {
        }
    }
}

