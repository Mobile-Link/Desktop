using System;
using System.Collections.Generic;
using System.Timers;
using Microsoft.Extensions.DependencyInjection;

namespace MobileLink_Desktop.Utils;

public class TransferenceTimer()
{
    private readonly Dictionary<int, System.Timers.Timer?> _lastChunkReceivedByTransaction = new Dictionary<int, System.Timers.Timer?>();
    public void ChunkReceived(int idTransference)
    {
        if (_lastChunkReceivedByTransaction.ContainsKey(idTransference))
        {
            _lastChunkReceivedByTransaction[idTransference]?.Stop();
            _lastChunkReceivedByTransaction[idTransference]?.Dispose();
        }

        var minutes = int.Parse("1");// TODO 30 
        var timer = new Timer(new TimeSpan(0, minutes, 0));
        timer.AutoReset = false;
        timer.Elapsed += (object? o, ElapsedEventArgs e) =>
        {
            using (var scope = App.AppServiceProvider.CreateScope())
            {
                var transferService = scope.ServiceProvider.GetRequiredService<TransferenceHandler>();
                transferService.TimeoutTransference(idTransference).ContinueWith((_) => { });
                RemoveMonitor(idTransference);
            }
        };
        timer.Start();
        _lastChunkReceivedByTransaction[idTransference] = timer;
    }

    public void RemoveMonitor(int idTransference)
    {
        if (_lastChunkReceivedByTransaction.TryGetValue(idTransference, out var timer))
        {
            timer?.Stop();
            timer?.Dispose();

            _lastChunkReceivedByTransaction.Remove(idTransference);
        }
        else
        {
            Console.WriteLine($"Chave {idTransference} não encontrada no dicionário.");
        }
    }
}