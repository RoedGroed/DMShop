namespace Service;

public class ServiceValidationException : Exception
{
    public ServiceValidationException(string message) : base(message) { }
}
