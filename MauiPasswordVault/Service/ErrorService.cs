namespace MauiPasswordVault.Service;

public class ErrorService
{
    public String LastErrorMessage { get; set; } = null!;
    public String LastErrorFull { get; set; } = null!;
    public bool CriticalError { get; set; } = false;
}
