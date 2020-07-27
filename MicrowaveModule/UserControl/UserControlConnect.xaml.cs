using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Management;

namespace MicrowaveModule
{
    /// <summary>
    /// Логика взаимодействия для UserControlConnect.xaml
    /// </summary>
    public partial class UserControlConnect //: UserControl
    {
        public UserControlConnect()
        {
            InitializeComponent();
        }

        private void buttonPortSurvey_Click(object sender, RoutedEventArgs e)
        {
            ManagementScope myScope = new ManagementScope("root\\CIMV2");
            SelectQuery q = new SelectQuery("Win32_SerialPort");
            ManagementObjectSearcher s = new ManagementObjectSearcher(myScope, q);
            foreach (var item in s.Get())
            {
                if (item["PNPDeviceID"].ToString().Contains("X2001"))
                {
                    ComPort.PortName = item["DeviceID"].ToString();
                }
            }

            ComboBoxPortsName.Items.Clear();
            ComboBoxPortsName.Text = "";
            ComboBoxPortsName.Items.AddRange(SerialPort.GetPortNames());
            ComboBoxPortsName.SelectedItem = ComPort.PortName;

            listBoxPortSettings.Items.Add("Найдены порты:");
            listBoxPortSettings.Items.AddRange(SerialPort.GetPortNames());
            listBoxPortSettings.Items.Add("");
            listBoxPortSettings.SelectedIndex = listBoxPortSettings.Items.Count - 1;
        }

        private void buttonOpenPort_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonСonnectionСheck_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
