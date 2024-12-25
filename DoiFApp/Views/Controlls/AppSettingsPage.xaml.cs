using ag.WPF.ColorPicker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DoiFApp.Views.Controlls
{
    /// <summary>
    /// Логика взаимодействия для AppSettingsPage.xaml
    /// </summary>
    public partial class AppSettingsPage : UserControl
    {
        public AppSettingsPage()
        {
            InitializeComponent();
        }

        private void OpenColorPicker(object sender, RoutedEventArgs e)
        {
            var colorPanel = new ColorPanel();
        }
    }
}
