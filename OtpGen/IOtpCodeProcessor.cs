namespace OtpGen;

public interface IOtpCodeProcessor
{
    Task Process(string code);
}