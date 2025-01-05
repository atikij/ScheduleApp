namespace ScheduleApp
{
    using global::ScheduleApp.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Win32;
    using OfficeOpenXml;
    using OfficeOpenXml.Style;
    using QRCoder;
    using System.Drawing;
    using System.IO;
    using System.Text;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Imaging;

    public partial class MainWindow : Window
    {
        private ScheduleContext _context;

        private ScheduleExporter _exporter;

        public MainWindow()
        {
            InitializeComponent();

            var optionsBuilder = new DbContextOptionsBuilder<ScheduleContext>();
            optionsBuilder.UseMySql("Server=localhost;Database=mydb;User=root;Password=12345;", new MySqlServerVersion(new Version(8, 0, 21)));

            _context = new ScheduleContext(optionsBuilder.Options);
            _exporter = new ScheduleExporter(_context);
            LoadData();
        }

        private void LoadData()
        {
            _context.Semesters.Load();  
            semestersDataGrid.ItemsSource = _context.Semesters.Local.ToBindingList();

            _context.Pairs.Load();
            pairsDataGrid.ItemsSource = _context.Pairs.Local.ToBindingList();

            _context.Studentgroups.Load();
            groupsDataGrid.ItemsSource = _context.Studentgroups.Local.ToBindingList();

            _context.Teachers.Load();
            teachersDataGrid.ItemsSource = _context.Teachers.Local.ToBindingList();

            _context.Cabinets.Load();
            cabinetsDataGrid.ItemsSource = _context.Cabinets.Local.ToBindingList();

            _context.Cabinettypes.Load();
            cabinetTypesDataGrid.ItemsSource = _context.Cabinettypes.Local.ToBindingList();

            _context.Subjects.Load();
            subjectsDataGrid.ItemsSource = _context.Subjects.Local.ToBindingList();

            _context.Subjectlessons.Load();
            subjectLessonsDataGrid.ItemsSource = _context.Subjectlessons.Local.ToBindingList();

            _context.Days.Load();
            daysDataGrid.ItemsSource = _context.Days.Local.ToBindingList();

            _context.Weeks.Load();
            weeksDataGrid.ItemsSource = _context.Weeks.Local.ToBindingList();

            LoadGroupSchedule();
            LoadTeacherSchedule();
            LoadCabinetSchedule();
        }
        private void GenerateQRCode_Click(object sender, RoutedEventArgs e)
        {
            string data = "https://vkvideo.ru/video427644317_456239111";
            Bitmap qrCodeImage = GenerateQRCode(data);
            QRCodeImage.Source = ConvertBitmapToBitmapImage(qrCodeImage);
        }

        private Bitmap GenerateQRCode(string data)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            return qrCode.GetGraphic(20);
        }

        private BitmapImage ConvertBitmapToBitmapImage(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                memory.Position = 0;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }

        private void LoadGroupSchedule()
        {
            var today = DateOnly.FromDateTime(DateTime.Now); // Получаем текущую дату в формате DateOnly

            var groupSchedules = _context.Pairs
                .Include(p => p.IdGroupNavigation)
                .Include(p => p.IdTeacherNavigation)
                .Include(p => p.IdCabinetNavigation)
                .Include(p => p.IdSubjectNavigation)
                //.Where(p => p.Date.HasValue && p.Date.Value == today)  // Сравниваем с DateOnly
                .AsEnumerable()
                .GroupBy(p => p.IdGroupNavigation.NameGroup)
                .Select(g => new
                {
                    GroupName = g.Key,
                    Pair1 = FormatPairGroup(g.FirstOrDefault(p => p.IdSheduleNumber == 1)),
                    Pair2 = FormatPairGroup(g.FirstOrDefault(p => p.IdSheduleNumber == 2)),
                    Pair3 = FormatPairGroup(g.FirstOrDefault(p => p.IdSheduleNumber == 3)),
                    Pair4 = FormatPairGroup(g.FirstOrDefault(p => p.IdSheduleNumber == 4)),
                    Pair5 = FormatPairGroup(g.FirstOrDefault(p => p.IdSheduleNumber == 5)),
                    Pair6 = FormatPairGroup(g.FirstOrDefault(p => p.IdSheduleNumber == 6)),
                    Pair7 = FormatPairGroup(g.FirstOrDefault(p => p.IdSheduleNumber == 7))
                }).ToList();

            groupScheduleDataGrid.ItemsSource = groupSchedules;
        }

        private void LoadTeacherSchedule()
        {
            var today = DateOnly.FromDateTime(DateTime.Now); // Получаем текущую дату в формате DateOnly

            var teacherSchedules = _context.Pairs
                .Include(p => p.IdGroupNavigation)
                .Include(p => p.IdTeacherNavigation)
                .Include(p => p.IdCabinetNavigation)
                .Include(p => p.IdSubjectNavigation)
                //.Where(p => p.Date.HasValue && p.Date.Value == today)  // Сравниваем с DateOnly
                .AsEnumerable()
                .GroupBy(p => p.IdTeacherNavigation.NameTeacher)
                .Select(g => new
                {
                    TeacherName = g.Key,
                    Pair1 = FormatPairTeacher(g.FirstOrDefault(p => p.IdSheduleNumber == 1)),
                    Pair2 = FormatPairTeacher(g.FirstOrDefault(p => p.IdSheduleNumber == 2)),
                    Pair3 = FormatPairTeacher(g.FirstOrDefault(p => p.IdSheduleNumber == 3)),
                    Pair4 = FormatPairTeacher(g.FirstOrDefault(p => p.IdSheduleNumber == 4)),
                    Pair5 = FormatPairTeacher(g.FirstOrDefault(p => p.IdSheduleNumber == 5)),
                    Pair6 = FormatPairTeacher(g.FirstOrDefault(p => p.IdSheduleNumber == 6)),
                    Pair7 = FormatPairTeacher(g.FirstOrDefault(p => p.IdSheduleNumber == 7))
                }).ToList();

            teacherScheduleDataGrid.ItemsSource = teacherSchedules;
        }

        private void LoadCabinetSchedule()
        {
            var today = DateOnly.FromDateTime(DateTime.Now);

            var cabinetSchedules = _context.Pairs
                .Include(p => p.IdGroupNavigation)
                .Include(p => p.IdTeacherNavigation)
                .Include(p => p.IdCabinetNavigation)
                .Include(p => p.IdSubjectNavigation)
                //.Where(p => p.Date.HasValue && p.Date.Value == today)  
                .AsEnumerable()
                .GroupBy(p => p.IdCabinetNavigation.NameCabinet)
                .Select(g => new
                {
                    CabinetName = g.Key,
                    Pair1 = FormatPairCabinet(g.FirstOrDefault(p => p.IdSheduleNumber == 1)),
                    Pair2 = FormatPairCabinet(g.FirstOrDefault(p => p.IdSheduleNumber == 2)),
                    Pair3 = FormatPairCabinet(g.FirstOrDefault(p => p.IdSheduleNumber == 3)),
                    Pair4 = FormatPairCabinet(g.FirstOrDefault(p => p.IdSheduleNumber == 4)),
                    Pair5 = FormatPairCabinet(g.FirstOrDefault(p => p.IdSheduleNumber == 5)),
                    Pair6 = FormatPairCabinet(g.FirstOrDefault(p => p.IdSheduleNumber == 6)),
                    Pair7 = FormatPairCabinet(g.FirstOrDefault(p => p.IdSheduleNumber == 7))
                }).ToList();

            cabinetScheduleDataGrid.ItemsSource = cabinetSchedules;
        }
        public static string FormatPairGroup(Pair pair)
        {
            if (pair == null) return "";
            return $"{pair.IdTeacherNavigation.NameTeacher}\n каб.{pair.IdCabinetNavigation.NameCabinet}\n {pair.IdSubjectNavigation.NameSubject}";
        }
        private string FormatPairTeacher(Pair pair)
        {
            if (pair == null) return "";
            return $"{pair.IdGroupNavigation.NameGroup}\n каб.{pair.IdCabinetNavigation.NameCabinet}\n {pair.IdSubjectNavigation.NameSubject}";
        }
        private string FormatPairCabinet(Pair pair)
        {
            if (pair == null) return "";
            return $"{pair.IdTeacherNavigation.NameTeacher}\n {pair.IdGroupNavigation.NameGroup}\n {pair.IdSubjectNavigation.NameSubject}";
        }
        private void ExportGroupScheduleToExcel_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                FileName = "GroupSchedule.xlsx",
                Filter = "Excel Files|*.xlsx",
                Title = "Save Schedule as Excel File"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                _exporter.ExportGroupScheduleToExcel(saveFileDialog.FileName);
            }
        }

        private void ExportTeacherScheduleToExcel_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                FileName = "TeacherSchedule.xlsx",
                Filter = "Excel Files|*.xlsx",
                Title = "Save Schedule as Excel File"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                _exporter.ExportTeacherScheduleToExcel(saveFileDialog.FileName);
            }
        }
        private void ExportCabinetScheduleToExcel_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                FileName = "CabinetSchedule.xlsx",
                Filter = "Excel Files|*.xlsx",
                Title = "Save Schedule as Excel File"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                _exporter.ExportCabinetScheduleToExcel(saveFileDialog.FileName);
            }
        }

        private void DeleteCabinetSchedule_Click(object sender, RoutedEventArgs e)
        {
            var selectedSchedule = cabinetScheduleDataGrid.SelectedItem as dynamic;
            if (selectedSchedule != null)
            {
                var cabinetName = selectedSchedule.CabinetName;

                var scheduleToDelete = _context.Pairs
                    .Include(p => p.IdCabinetNavigation) 
                    .AsEnumerable() 
                    .FirstOrDefault(p => p.IdCabinetNavigation.NameCabinet == cabinetName);

                if (scheduleToDelete != null)
                {
                    var result = MessageBox.Show("Are you sure you want to delete this schedule?", "Confirm Delete", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        _context.Pairs.Remove(scheduleToDelete); 
                        _context.SaveChanges();  

                        LoadData();
                    }
                }
                else
                {
                    MessageBox.Show("Schedule not found.");
                }
            }
            else
            {
                MessageBox.Show("Please select a schedule to delete.");
            }
        }


        private void DeleteTeacherSchedule_Click(object sender, RoutedEventArgs e)
        {
            var selectedSchedule = teacherScheduleDataGrid.SelectedItem as dynamic;
            if (selectedSchedule != null)
            {
                var teacherName = selectedSchedule.TeacherName;

                var scheduleToDelete = _context.Pairs
                    .Include(p => p.IdTeacherNavigation)  
                    .AsEnumerable() 
                    .FirstOrDefault(p => p.IdTeacherNavigation.NameTeacher == teacherName);

                if (scheduleToDelete != null)
                {
                    var result = MessageBox.Show("Are you sure you want to delete this schedule?", "Confirm Delete", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        _context.Pairs.Remove(scheduleToDelete);  
                        _context.SaveChanges();

                        LoadData();
                    }
                }
                else
                {
                    MessageBox.Show("Schedule not found.");
                }
            }
            else
            {
                MessageBox.Show("Please select a schedule to delete.");
            }
        }


        private void DeleteGroupSchedule_Click(object sender, RoutedEventArgs e)
        {
            var selectedSchedule = groupScheduleDataGrid.SelectedItem as dynamic;
            if (selectedSchedule != null)
            {
                var groupName = selectedSchedule.GroupName;

                var scheduleToDelete = _context.Pairs
                    .Include(p => p.IdGroupNavigation)  
                    .AsEnumerable()  
                    .FirstOrDefault(p => p.IdGroupNavigation.NameGroup == groupName);

                if (scheduleToDelete != null)
                {
                    var result = MessageBox.Show("Are you sure you want to delete this schedule?", "Confirm Delete", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.Yes)
                    {
                        _context.Pairs.Remove(scheduleToDelete); 
                        _context.SaveChanges();
                        LoadData();
                    }
                }
                else
                {
                    MessageBox.Show("Schedule not found.");
                }
            }
            else
            {
                MessageBox.Show("Please select a schedule to delete.");
            }
        }

        private void OpenAddPairWindow_Click(object sender, RoutedEventArgs e)
        {
            var addPairWindow = new AddPairWindow(_context);
            addPairWindow.ShowDialog();
            LoadData();
        }
        private void EditPair_Click(object sender, RoutedEventArgs e)
        {
            if (pairsDataGrid.SelectedItem is Pair selectedPair)
            {
                var addPairWindow = new AddPairWindow(_context, selectedPair);
                addPairWindow.ShowDialog();
                pairsDataGrid.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Please select a pair to edit.");
            }
            pairsDataGrid.Items.Refresh();
        }
        private void DeletePair_Click(object sender, RoutedEventArgs e)
        {
            if (pairsDataGrid.SelectedItem is Pair selectedPair)
            {
                _context.Pairs.Remove(selectedPair);
                _context.SaveChanges();
                pairsDataGrid.Items.Refresh();
            }
        }
        // SubjectLesson
        private void AddSubjectLesson_Click(object sender, RoutedEventArgs e) => AddEntity<Subjectlesson>("Add Subject Lesson", "NameOfTypeLesson");
        private void EditSubjectLesson_Click(object sender, RoutedEventArgs e) => EditEntity<Subjectlesson>("Edit Subject Lesson", "NameOfTypeLesson", "", subjectLessonsDataGrid);
        private void DeleteSubjectLesson_Click(object sender, RoutedEventArgs e) => DeleteEntity<Subjectlesson>(subjectLessonsDataGrid);

        // Day
        private void AddDay_Click(object sender, RoutedEventArgs e)
        {
            var weeks = _context.Weeks.Include(w => w.IdSemesterNavigation).ToList();
            AddEntity<Day>("Add Day", "DayWeek", "Week", weeks.Cast<object>().ToList(), showDatePicker: true);
        }
        private void EditDay_Click(object sender, RoutedEventArgs e) => EditEntity<Day>("Edit Day", "DayWeek", "", daysDataGrid);
        private void DeleteDay_Click(object sender, RoutedEventArgs e) => DeleteEntity<Day>(daysDataGrid);

        // Week
        private void AddWeek_Click(object sender, RoutedEventArgs e)
        {
            var semesters = _context.Semesters.ToList();
            AddEntity<Week>("Add Week", "TypeWeek", "Semester", semesters.Cast<object>().ToList());
        }
        private void EditWeek_Click(object sender, RoutedEventArgs e) => EditEntity<Week>("Edit Week", "TypeWeek", "", weeksDataGrid);
        private void DeleteWeek_Click(object sender, RoutedEventArgs e) => DeleteEntity<Week>(weeksDataGrid);

        // Semester
        private void AddSemester_Click(object sender, RoutedEventArgs e) => AddEntity<Semester>("Add Semester", "NumberSemester");
        private void EditSemester_Click(object sender, RoutedEventArgs e) => EditEntity<Semester>("Edit Semester", "NumberSemester", "", semestersDataGrid);
        private void DeleteSemester_Click(object sender, RoutedEventArgs e) => DeleteEntity<Semester>(semestersDataGrid);

        // StudentGroup
        private void AddGroup_Click(object sender, RoutedEventArgs e) => AddEntity<Studentgroup>("Add Group", "NameGroup", "Course");
        private void EditGroup_Click(object sender, RoutedEventArgs e) => EditEntity<Studentgroup>("Edit Group", "NameGroup", "Course", groupsDataGrid);
        private void DeleteGroup_Click(object sender, RoutedEventArgs e) => DeleteEntity<Studentgroup>(groupsDataGrid);

        // Teacher
        private void AddTeacher_Click(object sender, RoutedEventArgs e) => AddEntity<Teacher>("Add Teacher", "NameTeacher", "IsActive");
        private void EditTeacher_Click(object sender, RoutedEventArgs e) => EditEntity<Teacher>("Edit Teacher", "NameTeacher", "IsActive", teachersDataGrid);
        private void DeleteTeacher_Click(object sender, RoutedEventArgs e) => DeleteEntity<Teacher>(teachersDataGrid);

        // Cabinet
        private void AddCabinet_Click(object sender, RoutedEventArgs e)
        {
            var cabinetTypes = _context.Cabinettypes.ToList();
            AddEntity<Cabinet>("Add Cabinet", "NameCabinet", "TypeCabinet", cabinetTypes.Cast<object>().ToList());
        }
        private void EditCabinet_Click(object sender, RoutedEventArgs e) => EditEntity<Cabinet>("Edit Cabinet", "NameCabinet", "", cabinetsDataGrid);
        private void DeleteCabinet_Click(object sender, RoutedEventArgs e) => DeleteEntity<Cabinet>(cabinetsDataGrid);

        // CabinetType
        private void AddCabinetType_Click(object sender, RoutedEventArgs e) => AddEntity<Cabinettype>("Add Cabinet Type", "TypeCabinet");
        private void EditCabinetType_Click(object sender, RoutedEventArgs e) => EditEntity<Cabinettype>("Edit Cabinet Type", "TypeCabinet", "", cabinetTypesDataGrid);
        private void DeleteCabinetType_Click(object sender, RoutedEventArgs e) => DeleteEntity<Cabinettype>(cabinetTypesDataGrid);

        // Subject
        private void AddSubject_Click(object sender, RoutedEventArgs e) => AddEntity<Subject>("Add Subject", "NameSubject");
        private void EditSubject_Click(object sender, RoutedEventArgs e) => EditEntity<Subject>("Edit Subject", "NameSubject", "", subjectsDataGrid);
        private void DeleteSubject_Click(object sender, RoutedEventArgs e) => DeleteEntity<Subject>(subjectsDataGrid);

        private void AddEntity<T>(string title, string inputLabel1, string inputLabel2 = "", List<object> items = null, bool showDatePicker = false) where T : class, new()
        {
            var inputWindow = new InputWindow(title, inputLabel1, inputLabel2, "", "", items, showDatePicker);
            if (inputWindow.ShowDialog() == true)
            {
                var newEntity = new T();

                if (newEntity is Subjectlesson newSubjectLesson)
                {
                    newSubjectLesson.NameOfTypeLesson = inputWindow.Input1;
                }
                else if (newEntity is Day newDay)
                {
                    newDay.DayWeek = inputWindow.Input1;
                    newDay.IdWeekNavigation = (Week)inputWindow.SelectedItem;
                }
                else if (newEntity is Week newWeek)
                {
                    newWeek.TypeWeek = inputWindow.Input1;
                    newWeek.IdSemesterNavigation = (Semester)inputWindow.SelectedItem;
                }
                else if (newEntity is Semester newSemester)
                {
                    newSemester.NumberSemester = int.Parse(inputWindow.Input1);
                }
                else if (newEntity is Studentgroup newGroup)
                {
                    newGroup.NameGroup = inputWindow.Input1;
                    newGroup.Course = inputWindow.Input2;
                }
                else if (newEntity is Teacher newTeacher)
                {
                    newTeacher.NameTeacher = inputWindow.Input1;
                    newTeacher.IsActive = bool.Parse(inputWindow.Input2);
                }
                else if (newEntity is Cabinet newCabinet)
                {
                    newCabinet.NameCabinet = inputWindow.Input1;
                    newCabinet.IdTypeNavigation = (Cabinettype)inputWindow.SelectedItem;
                }
                else if (newEntity is Cabinettype newCabinetType)
                {
                    newCabinetType.TypeCabinet = inputWindow.Input1;
                }
                else if (newEntity is Subject newSubject)
                {
                    newSubject.NameSubject = inputWindow.Input1;
                }

                _context.Set<T>().Add(newEntity);
                _context.SaveChanges();
                RefreshDataGrid<T>();
            }
        }

        private void EditEntity<T>(string title, string inputLabel1, string inputLabel2, DataGrid dataGrid) where T : class
        {
            if (dataGrid.SelectedItem is T selectedEntity)
            {
                var input1 = "";
                var input2 = "";

                if (selectedEntity is Subjectlesson subjectLesson)
                {
                    input1 = subjectLesson.NameOfTypeLesson;
                }
                else if (selectedEntity is Day day)
                {
                    input1 = day.DayWeek;
                }
                else if (selectedEntity is Week week)
                {
                    input1 = week.TypeWeek;
                }
                else if (selectedEntity is Semester semester)
                {
                    input1 = semester.NumberSemester.ToString();
                }
                else if (selectedEntity is Studentgroup group)
                {
                    input1 = group.NameGroup;
                    input2 = group.Course;
                }
                else if (selectedEntity is Teacher teacher)
                {
                    input1 = teacher.NameTeacher;
                    input2 = teacher.IsActive.ToString();
                }
                else if (selectedEntity is Cabinet cabinet)
                {
                    input1 = cabinet.NameCabinet;
                }
                else if (selectedEntity is Cabinettype cabinetType)
                {
                    input1 = cabinetType.TypeCabinet;
                }
                else if (selectedEntity is Subject subject)
                {
                    input1 = subject.NameSubject;
                }

                var editWindow = new InputWindow(title, inputLabel1, inputLabel2, input1, input2);
                if (editWindow.ShowDialog() == true)
                {
                    if (selectedEntity is Subjectlesson editedSubjectLesson)
                    {
                        editedSubjectLesson.NameOfTypeLesson = editWindow.Input1;
                    }
                    else if (selectedEntity is Day editedDay)
                    {
                        editedDay.DayWeek = editWindow.Input1;
                    }
                    else if (selectedEntity is Week editedWeek)
                    {
                        editedWeek.TypeWeek = editWindow.Input1;
                    }
                    else if (selectedEntity is Semester editedSemester)
                    {
                        editedSemester.NumberSemester = int.Parse(editWindow.Input1);
                    }
                    else if (selectedEntity is Studentgroup editedGroup)
                    {
                        editedGroup.NameGroup = editWindow.Input1;
                        editedGroup.Course = editWindow.Input2;
                    }
                    else if (selectedEntity is Teacher editedTeacher)
                    {
                        editedTeacher.NameTeacher = editWindow.Input1;
                        editedTeacher.IsActive = bool.Parse(editWindow.Input2);
                    }
                    else if (selectedEntity is Cabinet editedCabinet)
                    {
                        editedCabinet.NameCabinet = editWindow.Input1;
                    }
                    else if (selectedEntity is Cabinettype editedCabinetType)
                    {
                        editedCabinetType.TypeCabinet = editWindow.Input1;
                    }
                    else if (selectedEntity is Subject editedSubject)
                    {
                        editedSubject.NameSubject = editWindow.Input1;
                    }

                    _context.SaveChanges();
                    dataGrid.Items.Refresh();
                }
            }
        }

        private void DeleteEntity<T>(DataGrid dataGrid) where T : class
        {
            if (dataGrid.SelectedItem is T selectedEntity)
            {
                _context.Set<T>().Remove(selectedEntity);
                _context.SaveChanges();
                dataGrid.Items.Refresh();
            }
        }

        private void RefreshDataGrid<T>() where T : class
        {
            if (typeof(T) == typeof(Subjectlesson))
            {
                subjectLessonsDataGrid.Items.Refresh();
            }
            else if (typeof(T) == typeof(Day))
            {
                daysDataGrid.Items.Refresh();
            }
            else if (typeof(T) == typeof(Week))
            {
                weeksDataGrid.Items.Refresh();
            }
            else if (typeof(T) == typeof(Semester))
            {
                semestersDataGrid.Items.Refresh();
            }
            else if (typeof(T) == typeof(Studentgroup))
            {
                groupsDataGrid.Items.Refresh();
            }
            else if (typeof(T) == typeof(Teacher))
            {
                teachersDataGrid.Items.Refresh();
            }
            else if (typeof(T) == typeof(Cabinet))
            {
                cabinetsDataGrid.Items.Refresh();
            }
            else if (typeof(T) == typeof(Cabinettype))
            {
                cabinetTypesDataGrid.Items.Refresh();
            }
            else if (typeof(T) == typeof(Subject))
            {
                subjectsDataGrid.Items.Refresh();
            }
        }
    }
}
