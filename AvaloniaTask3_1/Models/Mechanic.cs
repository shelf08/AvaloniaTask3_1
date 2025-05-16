using System;
using System.Threading.Tasks;

namespace AvaloniaTask3_1.Models
{
    public class Mechanic
    {
        public event Action<string>? LogMessage;
        
        public string Name { get; }
        public bool IsBusy { get; private set; }

        public Mechanic(string name)
        {
            Name = name;
        }

        public async Task RepairPump(OilPump pump)
        {
            if (IsBusy) return;
            
            IsBusy = true;
            LogMessage?.Invoke($"{Name}: Направляюсь к вышке {pump.Name} для тушения пожара");
            
            await Task.Delay(3000);
            
            pump.ExtinguishFire();
            LogMessage?.Invoke($"{Name}: Пожар на вышке {pump.Name} потушен");
            
            await Task.Delay(2000);
            
            pump.StartExtraction();
            LogMessage?.Invoke($"{Name}: Вышка {pump.Name} проверена и снова запущена");
            
            IsBusy = false;
        }
    }
}