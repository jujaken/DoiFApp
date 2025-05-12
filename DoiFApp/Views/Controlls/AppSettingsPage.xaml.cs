using DoiFApp.ViewModels;
using System.Windows.Controls;

namespace DoiFApp.Views.Controlls
{
    public partial class AppSettingsPage : UserControl
    {
        public AppSettingsPage()
        {
            InitializeComponent();
        }

        private void ColorPicker_SelectedColorChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<System.Windows.Media.Color> e)
        {
            (DataContext as ConfigColorViewModel)!.Value = e.NewValue;
        }
    }
}
