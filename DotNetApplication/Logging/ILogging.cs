namespace DotNetApplication.Logging;

public interface ILogging
{
    public void Log(string message, string type);
}