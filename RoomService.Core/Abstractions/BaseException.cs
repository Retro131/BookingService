using System.Net;

namespace RoomService.Core.Abstractions;

public abstract class BaseException(string message, HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError) : Exception(message)
{
    public HttpStatusCode HttpStatusCode { get; } = httpStatusCode;
}