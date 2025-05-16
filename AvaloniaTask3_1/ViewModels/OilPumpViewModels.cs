using System;
using AvaloniaTask3_1.Models;
using ReactiveUI;

namespace AvaloniaTask3_1.ViewModels
{
    public class OilPumpViewModel : ViewModelBase
    {
        private readonly OilPump _pump;
        
        public string Name => _pump.Name;
        public Guid Id => _pump.Id;
        
        private double _currentOil;
        public double CurrentOil
        {
            get => _currentOil;
            set => this.RaiseAndSetIfChanged(ref _currentOil, value);
        }
        
        private bool _isOnFire;
        public bool IsOnFire
        {
            get => _isOnFire;
            set => this.RaiseAndSetIfChanged(ref _isOnFire, value);
        }
        
        private bool _isWorking;
        public bool IsWorking
        {
            get => _isWorking;
            set => this.RaiseAndSetIfChanged(ref _isWorking, value);
        }
        
        public OilPumpViewModel(OilPump pump)
        {
            _pump = pump;
            _pump.OilExtracted += oil => CurrentOil = oil;
            _pump.FireStatusChanged += isOnFire => IsOnFire = isOnFire;
            
            // Используем рефлексию для получения свойств базового объекта
            var workingProp = pump.GetType().GetProperty("IsWorking");
            if (workingProp != null)
            {
                IsWorking = (bool)workingProp.GetValue(pump)!;
            }
        }
    }
}