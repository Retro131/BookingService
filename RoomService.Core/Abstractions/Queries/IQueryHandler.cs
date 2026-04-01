namespace RoomService.Core.Abstractions.Queries;

public interface IQueryHandler<in TQuery, TResponse >
    where TQuery : IQuery<TResponse>
{
    Task<TResponse> HandleAsync(TQuery query, CancellationToken cancellationToken);
}