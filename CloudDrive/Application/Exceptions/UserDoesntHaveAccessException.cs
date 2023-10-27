
namespace Application.Exceptions;

public class UserDoesntHaveAccessException : CustomException
{
    public UserDoesntHaveAccessException() : base("У пользователя нет доступов") { }
}
