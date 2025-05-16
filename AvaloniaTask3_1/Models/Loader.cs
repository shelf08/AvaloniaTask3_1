using System;
using System.Threading.Tasks;

namespace AvaloniaTask3_1.Models
{
    public class Loader
    {
        public event Action<string>? LogMessage;
        public event Action<double>? OilCollected;
        
        public string Name { get; }
        public bool IsBusy { get; private set; }
        public double TotalOilCollected { get; private set; }

        public Loader(string name)
        {
            Name = name;
        }

        public async Task CollectOil(OilPump pump)
        {
            if (IsBusy) return;
            
            IsBusy = true;
            LogMessage?.Invoke($"{Name}: Направляюсь к вышке {pump.Name} для загрузки нефти");
            
            await Task.Delay(2000);
            
            var oil = pump.CollectOil();
            TotalOilCollected += oil;
            OilCollected?.Invoke(TotalOilCollected);
            
            LogMessage?.Invoke($"{Name}: Нефть с вышки {pump.Name} загружена. Всего собрано: {TotalOilCollected:0.00} баррелей");
            
            await Task.Delay(2000);
            
            IsBusy = false;
        }
    }
}