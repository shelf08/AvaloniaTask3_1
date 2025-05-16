using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaTask3_1.ViewModels;

namespace AvaloniaTask3_1.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}