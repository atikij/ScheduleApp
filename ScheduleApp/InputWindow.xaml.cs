using System.Collections.Generic;
using System.Windows;

namespace ScheduleApp
{
    public partial class InputWindow : Window
    {
        public string Input1 { get; private set; }
        public string Input2 { get; private set; }
        public object SelectedItem { get; private set; }

        public InputWindow(string title, string label1, string label2 = "", string defaultInput1 = "", string defaultInput2 = "", List<object> items = null)
        {
            InitializeComponent();
            Title = title;
            InputLabel1.Content = label1;
            InputTextBox1.Text = defaultInput1;

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

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Input1 = InputTextBox1.Text;
            Input2 = InputTextBox2.Text;
            SelectedItem = InputComboBox2.SelectedItem;
            DialogResult = true;
        }
    }
}
