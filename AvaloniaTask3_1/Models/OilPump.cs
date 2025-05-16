using System;
using System.Threading;
using System.Threading.Tasks;

namespace AvaloniaTask3_1.Models
{
    public class OilPump
    {
        public event Action<string>? LogMessage;
        public event Action<bool>? FireStatusChanged;
        public event Action<double>? OilExtracted;
        public event Action? LoaderCalled;
        
        public Guid Id { get; } = Guid.NewGuid();
        public string Name { get; }
        public double ExtractionRate { get; } // баррелей в минуту
        public double CurrentOil { get; private set; }
        public bool IsOnFire { get; private set; }
        public bool IsWorking { get; private set; }
        
        private readonly Random _random = new();
        private CancellationTokenSource? _extractionCts;

        public OilPump(string name, double extractionRate)
        {
            Name = name;
            ExtractionRate = extractionRate;
        }

        public void StartExtraction()
        {
            if (IsWorking) return;
            
            IsWorking = true;
            _extractionCts = new CancellationTokenSource();
            
            Task.Run(() => ExtractionProcess(_extractionCts.Token));
            LogMessage?.Invoke($"{Name}: Добыча нефти начата");
        }

        public void StopExtraction()
        {
            if (!IsWorking) return;
            
            _extractionCts?.Cancel();
            IsWorking = false;
            LogMessage?.Invoke($"{Name}: Добыча нефти остановлена");
        }

        private async Task ExtractionProcess(CancellationToken token)
        {
            while (IsWorking && !token.IsCancellationRequested)
            {
                await Task.Delay(1000, token); // Обновляем каждую секунду
                
                // Добыча нефти
                var extracted = ExtractionRate / 60; // Пересчет в баррели в секунду
                CurrentOil += extracted;
                OilExtracted?.Invoke(CurrentOil);
                
                // Проверка на возгорание (5% вероятность каждую секунду)
                if (_random.NextDouble() < 0.05)
                {
                    IsOnFire = true;
                    FireStatusChanged?.Invoke(true);
                    LogMessage?.Invoke($"{Name}: ВНИМАНИЕ! Возгорание на вышке!");
                    StopExtraction();
                }
                
                // Если накопилось достаточно нефти, вызываем погрузчик
                if (CurrentOil >= 10)
                {
                    LoaderCalled?.Invoke();
                }
            }
        }

        public void ExtinguishFire()
        {
            if (!IsOnFire) return;
            
            IsOnFire = false;
            FireStatusChanged?.Invoke(false);
            LogMessage?.Invoke($"{Name}: Пожар потушен");
        }

        public double CollectOil()
        {
            var oil = CurrentOil;
            CurrentOil = 0;
            OilExtracted?.Invoke(0);
            LogMessage?.Invoke($"{Name}: Нефть загружена в погрузчик ({oil:0.00} баррелей)");
            return oil;
        }
    }
}