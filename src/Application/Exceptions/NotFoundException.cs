namespace Application.Exceptions;

public class NotFoundException(string entityName, long id) : TemplateException($"{entityName} with id '{id}' was not found.")
{
}