namespace OtpGen;

public class OtpConsoleWriter : IOtpCodeProcessor
{
    public Task Process(string code)
    {
        Console.WriteLine(code);
        return Task.CompletedTask;
    }
}