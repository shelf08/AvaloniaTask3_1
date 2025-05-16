using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using Avalonia.Threading;
using AvaloniaTask3_1.Models;
using ReactiveUI;

namespace AvaloniaTask3_1.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly OilField _oilField = new();
        private string _logText = string.Empty;
        private double _totalOilCollected;
        
        public ObservableCollection<OilPumpViewModel> Pumps { get; } = new();
        public ObservableCollection<Mechanic> Mechanics { get; }
        public ObservableCollection<Loader> Loaders { get; }
        
        public string LogText
        {
            get => _logText;
            set => this.RaiseAndSetIfChanged(ref _logText, value);
        }
        
        public double TotalOilCollected
        {
            get => _totalOilCollected;
            set => this.RaiseAndSetIfChanged(ref _totalOilCollected, value);
        }
        
        public ReactiveCommand<Unit, Unit> AddPumpCommand { get; }
        public ReactiveCommand<Unit, Unit> StartAllCommand { get; }
        public ReactiveCommand<Unit, Unit> StopAllCommand { get; }
        
        public MainWindowViewModel()
        {
            Mechanics = new ObservableCollection<Mechanic>(_oilField.Mechanics);
            Loaders = new ObservableCollection<Loader>(_oilField.Loaders);
            
            _oilField.LogMessage += msg =>
            {
                Dispatcher.UIThread.Post(() =>
                {
                    LogText += $"{DateTime.Now:T} {msg}\n";
                });
            };
            
            foreach (var loader in _oilField.Loaders)
            {
                loader.OilCollected += oil => TotalOilCollected = oil;
            }
            
            AddPumpCommand = ReactiveCommand.Create(() => AddPump($"Вышка {Pumps.Count + 1}", new Random().Next(5, 20)));
            StartAllCommand = ReactiveCommand.Create(StartAllPumps);
            StopAllCommand = ReactiveCommand.Create(StopAllPumps);
            
            AddPump("Вышка 1", 10);
            AddPump("Вышка 2", 15);
            AddPump("Вышка 3", 8);
        }
        
        public void AddPump(string name, double extractionRate)
        {
            _oilField.AddPump(name, extractionRate);
            var pump = _oilField.Pumps.Last();
            var pumpVm = new OilPumpViewModel(pump);
            Pumps.Add(pumpVm);
        }
        
        public void StartAllPumps()
        {
            _oilField.StartAllPumps();
            foreach (var pump in Pumps)
            {
                pump.IsWorking = true;
            }
        }
        
        public void StopAllPumps()
        {
            _oilField.StopAllPumps();
            foreach (var pump in Pumps)
            {
                pump.IsWorking = false;
            }
        }
    }
}