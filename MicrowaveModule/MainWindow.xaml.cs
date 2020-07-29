using System;
using System.Collections.Generic;
using System.IO.Ports;
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

namespace MicrowaveModule
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ControlMenuContent.Content = UCConnect;
            buttonConnectMenu.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x74, 0xFD)); 
            buttonControlMenu.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xDD, 0xDD, 0xDD));
            buttonMemoryMenu.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xDD, 0xDD, 0xDD));
        }

        UserControlConnect UCConnect = new UserControlConnect();
        UserControlControl UCControl = new UserControlControl();
        UserControlMemory UCMemory = new UserControlMemory();

        private void buttonConnectMenu_Click(object sender, RoutedEventArgs e)
        {
            ControlMenuContent.Content= UCConnect;
            buttonConnectMenu.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x74, 0xFD));
            buttonControlMenu.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xDD, 0xDD, 0xDD));
            buttonMemoryMenu.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xDD, 0xDD, 0xDD));
        }

        private void buttonControlMenu_Click(object sender, RoutedEventArgs e)
        {
            ControlMenuContent.Content = UCControl;
            buttonConnectMenu.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xDD, 0xDD, 0xDD));
            buttonControlMenu.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x74, 0xFD));
            buttonMemoryMenu.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xDD, 0xDD, 0xDD));
        }

        private void buttonMemoryMenu_Click(object sender, RoutedEventArgs e)
        {
            ControlMenuContent.Content = UCMemory;
            buttonConnectMenu.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xDD, 0xDD, 0xDD));
            buttonControlMenu.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xDD, 0xDD, 0xDD));
            buttonMemoryMenu.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x74, 0xFD));
        }
    }
}
