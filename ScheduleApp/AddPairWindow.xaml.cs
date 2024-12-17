using ScheduleApp.Models;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ScheduleApp
{
    public partial class AddPairWindow : Window
    {
        private ScheduleContext _context;
        private Pair _pair;

        public AddPairWindow(ScheduleContext context, Pair pair = null)
        {
            InitializeComponent();
            _context = context;
            _pair = pair;
            LoadData();
            if (_pair != null)
            {
                LoadPairData();
            }
        }

        private void LoadData()
        {
            groupComboBox.ItemsSource = _context.Studentgroups.ToList();
            cabinetComboBox.ItemsSource = _context.Cabinets.ToList();
            subjectComboBox.ItemsSource = _context.Subjects.ToList();
            teacherComboBox.ItemsSource = _context.Teachers.ToList();
            typeLessonComboBox.ItemsSource = _context.Subjectlessons.ToList();
            dayComboBox.ItemsSource = _context.Days.ToList();
            dayComboBox.DisplayMemberPath = "DayWeek";

            for (int i = 1; i <= 7; i++)
            {
                scheduleNumberComboBox.Items.Add(new ComboBoxItem { Content = $"{i}", Tag = i });
            }
        }

        private void LoadPairData()
        {
            groupComboBox.SelectedItem = _pair.IdGroupNavigation;
            dayComboBox.SelectedItem = _pair.IdDayNavigation;
            cabinetComboBox.SelectedItem = _pair.IdCabinetNavigation;
            subjectComboBox.SelectedItem = _pair.IdSubjectNavigation;
            teacherComboBox.SelectedItem = _pair.IdTeacherNavigation;
            typeLessonComboBox.SelectedItem = _pair.IdTypeLessonNavigation;
            scheduleNumberComboBox.SelectedItem = scheduleNumberComboBox.Items.Cast<ComboBoxItem>().FirstOrDefault(item => (int)item.Tag == _pair.IdSheduleNumber);
        }

        private void AddPairButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedGroup = (Studentgroup)groupComboBox.SelectedItem;
            var selectedDay = (Day)dayComboBox.SelectedItem;
            var selectedCabinet = (Cabinet)cabinetComboBox.SelectedItem;
            var selectedSubject = (Subject)subjectComboBox.SelectedItem;
            var selectedTeacher = (Teacher)teacherComboBox.SelectedItem;
            var selectedTypeLesson = (Subjectlesson)typeLessonComboBox.SelectedItem;
            var selectedScheduleNumber = (ComboBoxItem)scheduleNumberComboBox.SelectedItem;

            if (selectedGroup != null && selectedDay != null && selectedCabinet != null && selectedSubject != null && selectedTeacher != null && selectedTypeLesson != null && selectedScheduleNumber != null )
            {
                if (_pair == null)
                {
                    var newPair = new Pair
                    {
                        IdGroup = selectedGroup.IdGroup,
                        IdDay = selectedDay.IdDay,
                        IdCabinet = selectedCabinet.IdCabinet,
                        IdSubject = selectedSubject.IdSubject,
                        IdTeacher = selectedTeacher.IdTeacher,
                        IdTypeLesson = selectedTypeLesson.IdTypeless,
                        IdSheduleNumber = (int)selectedScheduleNumber.Tag,
                    };

                    _context.Pairs.Add(newPair);
                    MessageBox.Show("Pair added successfully.");
                }
                else
                {
                    _context.Pairs.Remove(_pair);
                    _context.SaveChanges();

                    var updatedPair = new Pair
                    {
                        IdGroup = selectedGroup.IdGroup,
                        IdDay = selectedDay.IdDay,
                        IdCabinet = selectedCabinet.IdCabinet,
                        IdSubject = selectedSubject.IdSubject,
                        IdTeacher = selectedTeacher.IdTeacher,
                        IdTypeLesson = selectedTypeLesson.IdTypeless,
                        IdSheduleNumber = (int)selectedScheduleNumber.Tag,
                    };

                    _context.Pairs.Add(updatedPair);
                    MessageBox.Show("Pair updated successfully.");
                }

                _context.SaveChanges();
                Close();
            }
            else
            {
                MessageBox.Show("Please fill all the fields.");
            }
        }


    }
}
