using Microsoft.Extensions.Hosting;
using OtpNet;

namespace OtpGen;

public class OtpGenerator : IHostedService, IDisposable
{
    private readonly Totp _totp;
    private readonly TaskCompletionSource tcs = new();
    private readonly Timer _timer;
    private readonly IOtpCodeProcessor[] _processors;

    public OtpGenerator(string key, params IOtpCodeProcessor[] processors)
    {
        _totp = new Totp(Base32Encoding.ToBytes(key));
        _processors = processors;
        _timer = new Timer(Tick, null, Timeout.Infinite, Timeout.Infinite);
    }

    private async void Tick(object? state = null)
    {
        var code = _totp.ComputeTotp();
        await Task.WhenAll(_processors.Select(x => x.Process(code)));
        _timer.Change(_totp.RemainingSeconds() * 1000 + 1, Timeout.Infinite);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Tick();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer.Change(Timeout.Infinite, Timeout.Infinite);
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer.Dispose();
        GC.SuppressFinalize(this);
    }
}