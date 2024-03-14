﻿using MediatR;
using System.Transactions;
using Wastage.Application.Interfaces.Behaviors;

namespace Wastage.Application.Behaviors.Transaction;

public class TransactionScopeBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, ITransactionalRequest
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        using TransactionScope transactionScope = new(TransactionScopeAsyncFlowOption.Enabled);
        TResponse response;

        try
        {
            response = await next();
            transactionScope.Complete();
        }
        catch (System.Exception)
        {
            transactionScope.Dispose();
            throw;
        }

        return response;
    }
}