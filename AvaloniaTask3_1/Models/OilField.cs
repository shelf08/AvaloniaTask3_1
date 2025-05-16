using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvaloniaTask3_1.Models
{
    public class OilField
    {
        private readonly List<OilPump> _pumps = new();
        private readonly List<Mechanic> _mechanics = new();
        private readonly List<Loader> _loaders = new();
        
        public event Action<string>? LogMessage;
        
        public IReadOnlyList<OilPump> Pumps => _pumps;
        public IReadOnlyList<Mechanic> Mechanics => _mechanics;
        public IReadOnlyList<Loader> Loaders => _loaders;

        public OilField()
        {
            // Создаем механиков и погрузчики по умолчанию
            _mechanics.Add(new Mechanic("Механик 1"));
            _mechanics.Add(new Mechanic("Механик 2"));
            
            _loaders.Add(new Loader("Погрузчик 1"));
            _loaders.Add(new Loader("Погрузчик 2"));
            
            foreach (var mechanic in _mechanics)
            {
                mechanic.LogMessage += msg => LogMessage?.Invoke(msg);
            }
            
            foreach (var loader in _loaders)
            {
                loader.LogMessage += msg => LogMessage?.Invoke(msg);
            }
        }

        public void AddPump(string name, double extractionRate)
        {
            var pump = new OilPump(name, extractionRate);
            pump.LogMessage += msg => LogMessage?.Invoke(msg);
            pump.FireStatusChanged += isOnFire =>
            {
                if (isOnFire)
                {
                    var freeMechanic = _mechanics.FirstOrDefault(m => !m.IsBusy);
                    freeMechanic?.RepairPump(pump);
                }
            };
            pump.LoaderCalled += () =>
            {
                var freeLoader = _loaders.FirstOrDefault(l => !l.IsBusy);
                freeLoader?.CollectOil(pump);
            };
            
            _pumps.Add(pump);
            LogMessage?.Invoke($"Добавлена новая вышка: {name}");
        }

        public void StartAllPumps()
        {
            foreach (var pump in _pumps.Where(p => !p.IsOnFire))
            {
                pump.StartExtraction();
            }
        }

        public void StopAllPumps()
        {
            foreach (var pump in _pumps)
            {
                pump.StopExtraction();
            }
        }
    }
}