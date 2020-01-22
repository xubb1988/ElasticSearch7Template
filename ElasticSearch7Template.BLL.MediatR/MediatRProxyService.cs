using ElasticSearch7Template.Core;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ElasticSearch7Template.BLL.MediatR
{
    internal class MediatRProxyService : IMediatRProxyService, IAutoInject
    {
        private readonly IMediator mediator;
        public MediatRProxyService(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<TResponse> SendAsync<TResponse>(IMediatRRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            return await mediator.Send(request);
        }

        public async Task PublishAsync<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : IMediatRNotification
        {
            await mediator.Publish(notification);
        }


    }
}
