using MicrowaveModule.UserControl;
using System;
using System.Collections.Generic;
using System.IO.Ports;
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
//using System.Management;

namespace MicrowaveModule
{
    /// <summary>
    /// Логика взаимодействия для UserControlConnect.xaml
    /// </summary>
    public partial class UserControlConnect //: UserControl
    {
         
        SerialPort ComPort;
        public UserControlConnect()
        {

            InitializeComponent();
            

            ComPort = new System.IO.Ports.SerialPort("COM0", 460800, Parity.None, 8, StopBits.One);
            ComPort.ReadTimeout = 3000;
            ComPort.WriteTimeout = 3000;


            #region comboBoxPorts
            int[] speed = { 110, 134, 150, 300, 600, 1200, 1800, 2400, 4800, 7200, 9600, 14400, 19200, 38400, 57600, 115200, 128000 };
            comboBoxPorts.Items.Clear();
            foreach (var item in speed)
            {
                comboBoxSpeed.Items.Add(item);
            }
            comboBoxSpeed.Text = "115200";
            #endregion

            #region comboBoxDataBits
            comboBoxDataBits.Items.Clear();
            comboBoxDataBits.Items.Add(8);
            comboBoxDataBits.Text= Convert.ToString(8);
            #endregion

            #region comboBoxParity
            comboBoxParity.Items.Clear();
            comboBoxParity.Items.Add("Нет");
            comboBoxParity.Text = "Нет";
            #endregion


        }

        private void buttonPortSurvey_Click(object sender, RoutedEventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            comboBoxPorts.Items.Clear();
            foreach (var item in ports)
            {
                comboBoxPorts.Items.Add(item);
            }
            comboBoxPorts.SelectedItem = ComPort.PortName;

            listBoxConnect.Items.Add("Найдены порты:");
            foreach (var item in ports)
            {
                listBoxConnect.Items.Add(item);
            }
            listBoxConnect.Items.Add("");
            //listBoxConnect.SelectedIndex = listBoxConnect.Items.Count - 1;
        }

        private void buttonOpenPort_Click(object sender, RoutedEventArgs e)
        {
            if (!ComPort.IsOpen)
            {
                try
                {
                    ComPort.Open();
                    buttonOpenPort.Content = "Закрыть порт";
                    listBoxConnect.Items.Add("Порт успешно открыт.");
                    listBoxConnect.Items.Add("");
                    //listBoxConnect.SelectedIndex = listBoxConnect.Items.Count - 1;
                    buttonСonnectionСheck.IsEnabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка открытия порта: " + ex.Message);

                    return;
                }
            }
            else
            {
                try
                {
                    ComPort.Close();
                    buttonOpenPort.Content = "Открыть порт";
                    listBoxConnect.Items.Add("Порт успешно закрыт.");
                    listBoxConnect.Items.Add("");
                    //listBoxPortSettings.SelectedIndex = listBoxPortSettings.Items.Count - 1;
                    buttonСonnectionСheck.IsEnabled = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка закрытия порта порта: " + ex.Message);
                    return;
                }
            }
        }

        private void buttonСonnectionСheck_Click(object sender, RoutedEventArgs e)
        {
            if (ComPort.IsOpen)
            {
                try
                {
                    foreach (var item in InterfacingPCWithGene2.testConnection(ComPort))
                    {
                        listBoxConnect.Items.Add(item);
                    }
                    
                }
                catch (Exception)
                {
                    ;
                }
            }
            else
            {
                listBoxConnect.Items.Add("Ошибка порта, порт закрыт.");
            }
            listBoxConnect.Items.Add("");
            //listBoxConnect.SelectedIndex = listBoxConnect.Items.Count - 1;
        }


        private void comboBoxPorts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //проверка открыт ли порт
            if (ComPort.IsOpen)
            {
                ComPort.Close(); // если открыт, то закрываем (нужно сделать при открытом порте запрет менять)
            }
            ComPort.PortName = Convert.ToString(comboBoxPorts.SelectedItem); //присваеваем новое имя COMPORT

            if (Convert.ToString(comboBoxPorts.SelectedItem) != null && Convert.ToString(comboBoxPorts.SelectedItem) != "")
            {
                buttonOpenPort.IsEnabled = true;
                buttonСonnectionСheck.IsEnabled=true;
            }
        }
    }
}
