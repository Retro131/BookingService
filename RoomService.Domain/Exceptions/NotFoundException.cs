using System.Net;
using RoomService.Core.Abstractions;

namespace RoomService.Domain.Exceptions;

public class NotFoundException(string message) : BaseException(message, HttpStatusCode.NotFound);