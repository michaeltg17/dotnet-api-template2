namespace Application.Exceptions;

public class NotFoundException<T>(long[] ids)
    : NotFoundException(typeof(T).Name, ids)
{
}