using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ElasticSearch7Template.BLL.MediatR
{
    public interface IMediatRProxyService
    {
        Task<TResponse> SendAsync<TResponse>(IMediatRRequest<TResponse> request, CancellationToken cancellationToken = default);


        Task PublishAsync<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : IMediatRNotification;
         
    }
}
