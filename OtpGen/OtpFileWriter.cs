namespace OtpGen;

public class OtpFileWriter : IOtpCodeProcessor
{
    private readonly string _template;
    private readonly string _outputPath;
    private const string CODE_MARK = "{{code}}";
    private const string NEW_LINE_MARK = "\\n";

    public OtpFileWriter(string outputPath, string template)
    {
        _template = template.Replace(NEW_LINE_MARK, "\n");
        _outputPath = outputPath;
    }

    public Task Process(string code) => File.WriteAllTextAsync(_outputPath, _template.Replace(CODE_MARK, code));
}