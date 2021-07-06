using System;
using System.ComponentModel;
using System.IO.Ports;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

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
            microwaveModuleDevice = new MicrowaveModuleDevice(); //наш дивайс
            timer = new DispatcherTimer(); //создаем таймер
            InitEvents();
            initComboBoxDacStep();
            initCheckBoxAtt();

            timer.Tick += new EventHandler(timer_Tick);     //добавляем событие таймера при запуске
            timer.Interval = new TimeSpan(0, 0, 0, 0, 500); //событие будет срабатывать через каждые 200 мили сек. 
        }

        #region Поля
        private MicrowaveModuleDevice microwaveModuleDevice;
        private DispatcherTimer timer;
        private byte valueAtt = 63;  //значение аттенюатора
        private byte valuePow = 0;  //значение питания
        private int codeDac = 2040;   //значение затворного напряжения cod DAC
        private int dacStep = 16;   //значение затворного напряжения DAC Step
        private bool flagEvent = true;
        #endregion


        /// <summary>
        /// инициализация комбо бокса ComboBoxDacStep
        /// </summary>
        private void initComboBoxDacStep()
        {
            int[] DacStep = { 1, 16, 32, 64, 128, 256 };
            comboBoxDacStep.Items.Clear();
            foreach (var item in DacStep)
            {
                comboBoxDacStep.Items.Add(item);
            }
            comboBoxDacStep.Text = "16";
        }

        /// <summary>
        /// Инициализация CheckBox для аттенюатора;
        /// </summary>
        private void initCheckBoxAtt()
        {
            checkBoxAtt1bit0.IsChecked = (true);
            checkBoxAtt1bit1.IsChecked = (true);
            checkBoxAtt1bit2.IsChecked = (true);
            checkBoxAtt1bit3.IsChecked = (true);
            checkBoxAtt1bit4.IsChecked = (true);
            checkBoxAtt1bit5.IsChecked = (true);
        }

        #region Events
        /// <summary>
        /// Инициализация всех событий
        /// </summary>
        private void InitEvents()
        {
            //TagleButton
            tbAdc.Checked += tbAdc_Checked;
            tbAdc.Unchecked += tbAdc_Unchecked;
            tbComPortOpen.Checked += tbComPortOpen_Checked;
            tbComPortOpen.Unchecked += tbComPortOpen_Unchecked;

            //CheckBox
            checkBoxAtt1bit5.Checked += checkBoxAtt1bit5_Checked;
            checkBoxAtt1bit4.Checked += checkBoxAtt1bit4_Checked;
            checkBoxAtt1bit3.Checked += checkBoxAtt1bit3_Checked;
            checkBoxAtt1bit2.Checked += checkBoxAtt1bit2_Checked;
            checkBoxAtt1bit1.Checked += checkBoxAtt1bit1_Checked;
            checkBoxAtt1bit0.Checked += checkBoxAtt1bit0_Checked;

            checkBoxAtt1bit5.Unchecked += checkBoxAtt1bit5_Unchecked;
            checkBoxAtt1bit4.Unchecked += checkBoxAtt1bit4_Unchecked;
            checkBoxAtt1bit3.Unchecked += checkBoxAtt1bit3_Unchecked;
            checkBoxAtt1bit2.Unchecked += checkBoxAtt1bit2_Unchecked;
            checkBoxAtt1bit1.Unchecked += checkBoxAtt1bit1_Unchecked;
            checkBoxAtt1bit0.Unchecked += checkBoxAtt1bit0_Unchecked;


            checkBoxGeneralPowerSupple.Checked += checkBoxPower_CheckedUnchecked;
            checkBoxStdnPower.Checked += checkBoxPower_CheckedUnchecked;

            checkBoxGeneralPowerSupple.Unchecked += checkBoxPower_CheckedUnchecked;
            checkBoxStdnPower.Unchecked += checkBoxPower_CheckedUnchecked;

            //button
            buttonUpTextBoxAtt.Click += buttonUpTextBoxAtt_Click;
            buttonUpTextBoxAtt.MouseWheel += buttonUpTextBoxAtt_MouseWheel;
            buttonDownTextBoxAtt.Click += buttonDownTextBoxAtt_Click;
            buttonDownTextBoxAtt.MouseWheel += buttonUpTextBoxAtt_MouseWheel;

            buttonUpTextBoxCodeDac.Click += buttonUpTextBoxCodeDac_Click;
            buttonUpTextBoxCodeDac.MouseWheel += buttonUpTextBoxCodeDac_MouseWheel;
            buttonDownTextBoxCodeDac.Click += buttonDownTextBoxCodeDac_Click;
            buttonDownTextBoxCodeDac.MouseWheel += buttonUpTextBoxCodeDac_MouseWheel;

            btClearLb.Click += btClearLb_Click;
            //textBox
            textBoxAttenuatorUpDown.TextChanged += textBoxAttenuatorUpDown_TextChanged;
            textBoxCodeDac12Bit.TextChanged += textBoxCodeDac12Bit_TextChanged;

            //comboBox
            comboBoxDacStep.SelectionChanged += comboBoxDacStep_SelectionChanged;
            cbNamePort.SelectionChanged += cbNamePort_SelectionChanged;
            cbNamePort.DropDownOpened += cbNamePort_DropDownOpened;

            //Window
            windowMain.Closing += WindowClosed;
        }

        void WindowClosed(object sender, CancelEventArgs e)
        {
            try
            {
                if (microwaveModuleDevice.ComPort.IsOpen)
                {
                    microwaveModuleDevice.sendControlPower(0); //отключаем питание (ON/OFF) и (STDN)
                    microwaveModuleDevice.sendControlAtt(63); //выставление максимального значения на аттенюаторах
                    microwaveModuleDevice.sendControlGateVoltage(2040); //выставление максимального значения на ЦАП (затворном напряжение)
                }
            }
            catch 
            {
                string msg = "Во время завершения было обработано исключение, при установке начальных параметров на приборе!\nХотите закрыть приложение? ";
                MessageBoxResult result =
                  MessageBox.Show(
                    msg,
                    "Окно информации",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);
                if (result == MessageBoxResult.No)
                {
                    // If user doesn't want to close, cancel closure
                    e.Cancel = true;
                }
            }
        }

        #region tagleButton
        void tbAdc_Checked(object sender, RoutedEventArgs e) 
        {
            if (microwaveModuleDevice.ComPort.IsOpen)
            {
                timer.Start();
                tbAdc.Content = "⏸";
            }
            else
            {
                tbAdc.IsChecked = false;
                lbReport.Items.Add("устройство не подключено");
            }
        }
        void tbAdc_Unchecked(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            tbAdc.Content = "ᐅ";
            textBlockAdc.Text = "_ _";
            lbTemperature.Content = "_ _ _С°";
        }
        void tbComPortOpen_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                microwaveModuleDevice.ComPort.Open();
                cbNamePort.IsEditable = false;
                tbComPortOpen.Content = "Закрыть";
                lbReport.Items.Add("Порт успешно открыт.");
                lbReport.Items.Add("");
            }
            catch (Exception ex)
            {
                lbReport.Items.Add("Ошибка открытия порта: " + ex.Message);
                lbReport.Items.Add("");
                tbComPortOpen.IsChecked = false;
                return;
            }
        }
        void tbComPortOpen_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                microwaveModuleDevice.ComPort.Close();
                cbNamePort.IsEditable = true;
                tbComPortOpen.Content = "Открыть";
                lbReport.Items.Add("Порт успешно закрыт.");
                lbReport.Items.Add("");
            }
            catch (Exception ex)
            {
                lbReport.Items.Add("Ошибка закрытия порта порта: " + ex.Message);
                lbReport.Items.Add("");
                tbComPortOpen.IsChecked = true;
                return;
            }
        }
        #endregion

        private void timer_Tick(object sender, EventArgs e)
        {
            if (!microwaveModuleDevice.ComPort.IsOpen)
            {
                timer.Stop();
                tbAdc.IsChecked = false;
            }
            else
            {
                try
                {
                    Adc_DoWork();
                    RequestTemperature();
                }
                catch (Exception ex)
                {
                    timer.Stop();
                    tbAdc.IsChecked = false;
                    MessageBox.Show($"Прибор не отвечает\nПЕРЕПОДКЛЮЧИТЕСЬ!\n\n{ex}","Внимание!",MessageBoxButton.OK,MessageBoxImage.Warning);
                }
            }
        } //событие запуска таймера
        private void btClearLb_Click(object sender, RoutedEventArgs e)
        {
            lbReport.Items.Clear();
        }
        private void cbNamePort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbNamePort.SelectedItem!=null)
            {
                microwaveModuleDevice.ComPort.PortName = Convert.ToString(cbNamePort.SelectedItem); //присваеваем новое имя COMPORT
            }
        }
        private void cbNamePort_DropDownOpened(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            cbNamePort.Items.Clear();
            foreach (var item in ports)
            {
                cbNamePort.Items.Add(item);
            }
            cbNamePort.SelectedItem = microwaveModuleDevice.ComPort.PortName;

            lbReport.Items.Add("Найдены порты:");
            foreach (var item in ports)
            {
                lbReport.Items.Add(item);
            }
            lbReport.Items.Add("");
            //listBoxConnect.SelectedIndex = listBoxConnect.Items.Count - 1;
        }

        #region Event checkBox Power
        private void checkBoxPower_CheckedUnchecked(object sender, RoutedEventArgs e)
        {
            GenerationNumericUpDownPower_Value();
            textBoxCodeDac12Bit.Text = "2040";
        }
        #endregion

        #region Event Attenuator
        private void checkBoxAtt1bit5_Checked(object sender, RoutedEventArgs e)
        {
            if (flagEvent)
            {
                GenerationNumericUpDownAtt1_Value();
            }
        }

        private void checkBoxAtt1bit4_Checked(object sender, RoutedEventArgs e)
        {
            if (flagEvent)
            {
                GenerationNumericUpDownAtt1_Value();
            }
        }

        private void checkBoxAtt1bit3_Checked(object sender, RoutedEventArgs e)
        {
            if (flagEvent)
            {
                GenerationNumericUpDownAtt1_Value();
            }
        }

        private void checkBoxAtt1bit2_Checked(object sender, RoutedEventArgs e)
        {
            if (flagEvent)
            {
                GenerationNumericUpDownAtt1_Value();
            }
        }

        private void checkBoxAtt1bit1_Checked(object sender, RoutedEventArgs e)
        {
            if (flagEvent)
            {
                GenerationNumericUpDownAtt1_Value();
            }
        }

        private void checkBoxAtt1bit0_Checked(object sender, RoutedEventArgs e)
        {
            if (flagEvent)
            {
                GenerationNumericUpDownAtt1_Value();
            }
        }

        private void checkBoxAtt1bit0_Unchecked(object sender, RoutedEventArgs e)
        {
            if (flagEvent)
            {
                GenerationNumericUpDownAtt1_Value();
            }
        }

        private void checkBoxAtt1bit1_Unchecked(object sender, RoutedEventArgs e)
        {
            if (flagEvent)
            {
                GenerationNumericUpDownAtt1_Value();
            }
        }

        private void checkBoxAtt1bit2_Unchecked(object sender, RoutedEventArgs e)
        {
            if (flagEvent)
            {
                GenerationNumericUpDownAtt1_Value();
            }
        }

        private void checkBoxAtt1bit3_Unchecked(object sender, RoutedEventArgs e)
        {
            if (flagEvent)
            {
                GenerationNumericUpDownAtt1_Value();
            }
        }

        private void checkBoxAtt1bit4_Unchecked(object sender, RoutedEventArgs e)
        {
            if (flagEvent)
            {
                GenerationNumericUpDownAtt1_Value();
            }
        }

        private void checkBoxAtt1bit5_Unchecked(object sender, RoutedEventArgs e)
        {
            if (flagEvent)
            {
                GenerationNumericUpDownAtt1_Value();
            }
        }

        private void buttonUpTextBoxAtt_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            // If the mouse wheel delta is positive, move the box up.
            if (e.Delta > 0)
            {
                int data = Int32.Parse(textBoxAttenuatorUpDown.Text);
                data++;
                textBoxAttenuatorUpDown.Text = Convert.ToString(data);
            }

            // If the mouse wheel delta is negative, move the box down.
            if (e.Delta < 0)
            {
                int data = Int32.Parse(textBoxAttenuatorUpDown.Text);
                data--;
                textBoxAttenuatorUpDown.Text = Convert.ToString(data);
            }
        }

        private void buttonUpTextBoxAtt_Click(object sender, RoutedEventArgs e)
        {

            if (valueAtt != 63)
            {
                valueAtt++;
            }
            DistributionOfCheckboxes(valueAtt);
            GenerationNumericUpDownAtt1_Value();
        }

        private void buttonDownTextBoxAtt_Click(object sender, RoutedEventArgs e)
        {

            if (valueAtt != 0)
            {
                valueAtt--;
            }
            DistributionOfCheckboxes(valueAtt);
            GenerationNumericUpDownAtt1_Value();
        }

        private void textBoxAttenuatorUpDown_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBoxAttenuatorUpDown.Text != "")
            {
                NumericUpDownCodeDac_ValueAttenuator();
            }
        }

        #endregion

        #region Event DAC (управление затворным напряжением)
        private void buttonUpTextBoxCodeDac_Click(object sender, RoutedEventArgs e)
        {
            codeDac += dacStep;
            if (codeDac > 2040)
            {
                codeDac = 2040;
            }
            textBoxCodeDac12Bit.Text = Convert.ToString(codeDac);
        }

        private void buttonDownTextBoxCodeDac_Click(object sender, RoutedEventArgs e)
        {
            codeDac -= dacStep;
            if (codeDac < 900)
            {
                codeDac = 900;
            }
            textBoxCodeDac12Bit.Text = Convert.ToString(codeDac);
        }

        private void textBoxCodeDac12Bit_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBoxCodeDac12Bit.Text != "")
            {
                NumericUpDownCodeDac_ValueGateVoltage();
            }
        }

        private void comboBoxDacStep_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dacStep = Convert.ToInt32(comboBoxDacStep.SelectedItem);
            lblDacStepVoltage.Content = "выбор шага в mV ≈ " + Convert.ToString(Math.Round((dacStep) * 0.8, 1));
        }

        void buttonUpTextBoxCodeDac_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            // If the mouse wheel delta is positive, move the box up.
            if (e.Delta > 0)
            {
                int data = Int32.Parse(textBoxCodeDac12Bit.Text);
                data += dacStep;
                textBoxCodeDac12Bit.Text = Convert.ToString(data);
            }

            // If the mouse wheel delta is negative, move the box down.
            if (e.Delta < 0)
            {
                int data = Int32.Parse(textBoxCodeDac12Bit.Text);
                data -= dacStep;
                textBoxCodeDac12Bit.Text = Convert.ToString(data);
            }
        }
        #endregion

        #endregion Events


        #region методы для Аттенюаторов
        /// <summary>
        /// управление аттенбатором из текст бокса
        /// </summary>
        private void NumericUpDownCodeDac_ValueAttenuator()
        {
            int number;
            textBoxAttenuatorUpDown.TextChanged -= new System.Windows.Controls.TextChangedEventHandler(this.textBoxAttenuatorUpDown_TextChanged);  // отключаем событие изменения textbox
            if (Int32.TryParse(textBoxAttenuatorUpDown.Text, out number))
            {
                if (number >= 0 && number <= 63)
                {
                    DistributionOfCheckboxes(number);
                    GenerationNumericUpDownAtt1_Value();
                }
                else if (number < 0)
                {
                    DistributionOfCheckboxes(0);
                    GenerationNumericUpDownAtt1_Value();
                }
                else
                {
                    DistributionOfCheckboxes(63);
                    GenerationNumericUpDownAtt1_Value();
                }
            }
            else
            {
                textBoxAttenuatorUpDown.Text = Convert.ToString(valueAtt);
            }
            textBoxAttenuatorUpDown.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.textBoxAttenuatorUpDown_TextChanged);  // включаем событие изменения textbox
        }

        /// <summary>
        /// метод распределенния флагов в чек боксах (аттенюаторы)
        /// </summary>
        /// <param name="value"> число для перевода в двоичную сис.сч. </param>
        private void DistributionOfCheckboxes(int value) //метод распределенния флагов в чек боксах
        {
            flagEvent = false;
            if (value / 32 != 0) checkBoxAtt1bit5.IsChecked = true; else checkBoxAtt1bit5.IsChecked = false;
            if (value % 32 / 16 != 0) checkBoxAtt1bit4.IsChecked = true; else checkBoxAtt1bit4.IsChecked = false;
            if (value % 32 % 16 / 8 != 0) checkBoxAtt1bit3.IsChecked = true; else checkBoxAtt1bit3.IsChecked = false;
            if (value % 32 % 16 % 8 / 4 != 0) checkBoxAtt1bit2.IsChecked = true; else checkBoxAtt1bit2.IsChecked = false;
            if (value % 32 % 16 % 8 % 4 / 2 != 0) checkBoxAtt1bit1.IsChecked = true; else checkBoxAtt1bit1.IsChecked = false;
            if (value % 32 % 16 % 8 % 4 % 2 / 1 != 0) checkBoxAtt1bit0.IsChecked = true; else checkBoxAtt1bit0.IsChecked = false;
            flagEvent = true;
        }

        /// <summary>
        /// перевод из двоичной сис.сч. методом сдвига битов влева (для аттенюаторов)
        /// </summary>
        private void GenerationNumericUpDownAtt1_Value()
        {
            valueAtt = Convert.ToByte(Convert.ToInt32(checkBoxAtt1bit0.IsChecked) |       //if (checkBoxAtt1bit0.IsChecked == true) newValue += 1;
                Convert.ToInt32(checkBoxAtt1bit1.IsChecked) << 1 |                        //if (checkBoxAtt1bit1.IsChecked == true) newValue += 2;
                Convert.ToInt32(checkBoxAtt1bit2.IsChecked) << 2 |                        //if (checkBoxAtt1bit2.IsChecked == true) newValue += 4;
                Convert.ToInt32(checkBoxAtt1bit3.IsChecked) << 3 |                        //if (checkBoxAtt1bit3.IsChecked == true) newValue += 8;
                Convert.ToInt32(checkBoxAtt1bit4.IsChecked) << 4 |                        //if (checkBoxAtt1bit4.IsChecked == true) newValue += 16;
                Convert.ToInt32(checkBoxAtt1bit5.IsChecked) << 5);                        //if (checkBoxAtt1bit5.IsChecked == true) newValue += 32;
            textBoxAttenuatorUpDown.Text = Convert.ToString(valueAtt);

            //------------------------------------------------------//
            if (microwaveModuleDevice.ComPort.IsOpen)
            {
                microwaveModuleDevice.sendControlAtt(valueAtt);
            }
            lblAttDb.Content = ((double)valueAtt / 2)+"dB";
        }
        #endregion

        /// <summary>
        /// управление затворным напряжением
        /// </summary>
        private void NumericUpDownCodeDac_ValueGateVoltage()
        {
            int number;
            textBoxCodeDac12Bit.TextChanged -= new System.Windows.Controls.TextChangedEventHandler(this.textBoxCodeDac12Bit_TextChanged);  // отключаем событие изменения textbox

            if (int.TryParse(textBoxCodeDac12Bit.Text, out number))
            {
                if (number >= 900 && number <= 2040)
                {
                    codeDac = number;
                    textBoxCodeDac12Bit.Text = Convert.ToString(codeDac);  // поставил, чтобы первый символ не мог быть 0
                    lblDacVoltage.Content = "Напряжение в mV ≈ " + Convert.ToString(Math.Round((codeDac - 900) * 0.8 + 700, 1));
                }
                else if (number < 900)
                {
                    number = 900;
                    textBoxCodeDac12Bit.Text = Convert.ToString(number);
                    codeDac = number;
                    lblDacVoltage.Content = "Напряжение в mV ≈ " + Convert.ToString(Math.Round((codeDac - 900 ) * 0.8 + 700, 1));
                }
                else
                {
                    number = 2040;
                    textBoxCodeDac12Bit.Text = Convert.ToString(number);
                    codeDac = number;
                    lblDacVoltage.Content = "Напряжение в mV ≈ " + Convert.ToString(Math.Round((codeDac - 900) * 0.8 + 700, 1));
                }
            }
            else
            {
                textBoxCodeDac12Bit.Text = Convert.ToString(codeDac);
                lblDacVoltage.Content = "Напряжение в mV ≈ " + Convert.ToString(Math.Round((codeDac - 900 ) * 0.8 + 700, 1));
            }

            textBoxCodeDac12Bit.TextChanged += new TextChangedEventHandler(this.textBoxCodeDac12Bit_TextChanged);  //включаем событие изменения textbox

            if (microwaveModuleDevice.ComPort.IsOpen)
            {
                microwaveModuleDevice.sendControlGateVoltage(codeDac);
            }
        }

        /// <summary>
        /// перевод из двоичной сис.сч. методом сдвига битов влева (для управления питанием)
        /// </summary>
        private void GenerationNumericUpDownPower_Value()
        {
            valuePow = Convert.ToByte(Convert.ToInt32(checkBoxGeneralPowerSupple.IsChecked) |
                       Convert.ToInt32(checkBoxStdnPower.IsChecked) << 1);

            if (microwaveModuleDevice.ComPort.IsOpen)
            {
                microwaveModuleDevice.sendControlPower(valuePow);
            }

        }

        /// <summary>
        /// Получение информации с АЦП
        /// </summary>
        void Adc_DoWork()
        {
            byte[] byteAdc = new byte[3];
            byteAdc = microwaveModuleDevice.requestAdcCode();                    //запрос кода с ADC. 
            textBlockAdc.Text = Convert.ToString(byteAdc[2] * 256 + byteAdc[1]); //берем данные с АЦП и выдаем в textBlock
        }

        /// <summary>
        /// Запрос температуры
        /// </summary>
        private void RequestTemperature()
        {
            lbTemperature.Content = (Convert.ToString(microwaveModuleDevice.requestTemperCode()))+"С°";
        }

    }
}
