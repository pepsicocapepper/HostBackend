namespace Application.Common.Abstractions;

public interface IUserContext
{
    Guid? UserId { get; }
}