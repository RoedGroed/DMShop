namespace DataAccess;

public class DataAccessException : Exception
{
    public DataAccessException(string message) : base(message) { }
}
