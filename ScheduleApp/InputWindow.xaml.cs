using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace ScheduleApp
{
    public partial class InputWindow : Window
    {
        public string Input1 { get; private set; } // DayWeek
        public string Input2 { get; private set; }
        public object SelectedItem { get; private set; }
        public DateTime SelectedDate { get; private set; }

        public InputWindow(string title, string label1, string label2 = "", string defaultInput1 = "", string defaultInput2 = "", List<object> items = null, bool showDatePicker = false)
        {
            InitializeComponent();
            Title = title;
            InputLabel1.Content = label1;
            InputTextBox1.Text = defaultInput1;

            // Если showDatePicker == true, то показываем DatePicker
            if (showDatePicker)
            {
                InputDatePicker.Visibility = Visibility.Visible;
            }

            if (!string.IsNullOrEmpty(label2))
            {
                InputLabel2.Content = label2;
                if (items != null)
                {
                    InputComboBox2.ItemsSource = items;
                    InputComboBox2.DisplayMemberPath = "DisplayProperty"; // Указываем свойство для отображения
                    InputComboBox2.SelectedItem = defaultInput2;
                    InputComboBox2.Visibility = Visibility.Visible;
                }
                else
                {
                    InputTextBox2.Text = defaultInput2;
                    InputTextBox2.Visibility = Visibility.Visible;
                }
                InputLabel2.Visibility = Visibility.Visible;
            }
        }

        // Обработчик для изменения выбранной даты
        private void InputDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (InputDatePicker.SelectedDate.HasValue)
            {
                SelectedDate = InputDatePicker.SelectedDate.Value;

                // Вычисляем день недели и записываем в InputTextBox1
                InputTextBox1.Text = SelectedDate.ToString("dddd", CultureInfo.InvariantCulture);
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Input1 = InputTextBox1.Text; // Это будет DayWeek
            Input2 = InputTextBox2.Text;
            SelectedItem = InputComboBox2.SelectedItem;

            // Получаем выбранную дату из DatePicker, если он видим
            if (InputDatePicker.Visibility == Visibility.Visible)
            {
                SelectedDate = InputDatePicker.SelectedDate ?? DateTime.Now;
            }

            DialogResult = true;
        }
    }
}
