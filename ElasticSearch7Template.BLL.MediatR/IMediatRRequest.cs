using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearch7Template.BLL.MediatR
{
    public interface IMediatRRequest<out TResponse> : IRequest<TResponse>
    {
    }
}
