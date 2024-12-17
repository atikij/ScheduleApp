namespace ScheduleApp
{
    using global::ScheduleApp.Models;
    using Microsoft.EntityFrameworkCore;
    using ScheduleApp.Models;
    using System.Text;
    using System.Windows;

    public partial class MainWindow : Window
    {
        private ScheduleContext _context;

        public MainWindow()
        {
            InitializeComponent();

            var optionsBuilder = new DbContextOptionsBuilder<ScheduleContext>();
            optionsBuilder.UseMySql("Server=localhost;Database=mydb;User=root;Password=12345;", new MySqlServerVersion(new Version(8, 0, 21)));

            _context = new ScheduleContext(optionsBuilder.Options);
            LoadData();
        }

        private void LoadData()
        {
            _context.Semesters.Load();  
            semestersDataGrid.ItemsSource = _context.Semesters.Local.ToBindingList();

            _context.Pairs.Load();
            pairsDataGrid.ItemsSource = _context.Pairs.Local.ToBindingList();

            _context.StudentGroups.Load();
            groupsDataGrid.ItemsSource = _context.StudentGroups.Local.ToBindingList();

            _context.Teachers.Load();
            teachersDataGrid.ItemsSource = _context.Teachers.Local.ToBindingList();

            _context.Cabinets.Load();
            cabinetsDataGrid.ItemsSource = _context.Cabinets.Local.ToBindingList();

            _context.CabinetTypes.Load();
            cabinetTypesDataGrid.ItemsSource = _context.CabinetTypes.Local.ToBindingList();

            _context.Subjects.Load();
            subjectsDataGrid.ItemsSource = _context.Subjects.Local.ToBindingList();

            _context.SubjectLessons.Load();
            subjectLessonsDataGrid.ItemsSource = _context.SubjectLessons.Local.ToBindingList();

            _context.Days.Load();
            daysDataGrid.ItemsSource = _context.Days.Local.ToBindingList();

            _context.Weeks.Load();
            weeksDataGrid.ItemsSource = _context.Weeks.Local.ToBindingList();

            LoadGroupSchedule();
            LoadTeacherSchedule();
            LoadCabinetSchedule();
        }

        private void LoadGroupSchedule()
        {
            var today = DateOnly.FromDateTime(DateTime.Now); // Получаем текущую дату в формате DateOnly

            var groupSchedules = _context.Pairs
                .Include(p => p.IdGroupNavigation)
                .Include(p => p.IdTeacherNavigation)
                .Include(p => p.IdCabinetNavigation)
                .Include(p => p.IdSubjectNavigation)
                .Where(p => p.Date.HasValue && p.Date.Value == today)  // Сравниваем с DateOnly
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
                .Where(p => p.Date.HasValue && p.Date.Value == today)  // Сравниваем с DateOnly
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
                .Where(p => p.Date.HasValue && p.Date.Value == today)  
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
            return $"{pair.IdTeacherNavigation.NameTeacher}\n {pair.IdCabinetNavigation.NameCabinet}\n {pair.IdSubjectNavigation.NameSubject}";
        }
        private string FormatPairTeacher(Pair pair)
        {
            if (pair == null) return "";
            return $"{pair.IdGroupNavigation.IdGroup}\n {pair.IdCabinetNavigation.NameCabinet}\n {pair.IdSubjectNavigation.NameSubject}";
        }
        private string FormatPairCabinet(Pair pair)
        {
            if (pair == null) return "";
            return $"{pair.IdTeacherNavigation.NameTeacher}\n {pair.IdGroupNavigation.IdGroup}\n {pair.IdSubjectNavigation.NameSubject}";
        }

        private void SaveToFile(string content, string fileName)
        {
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                FileName = fileName,
                DefaultExt = ".csv",
                Filter = "CSV Files (*.csv)|*.csv"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                System.IO.File.WriteAllText(saveFileDialog.FileName, content);
            }
        }

        private void ExportGroupScheduleToCSV_Click(object sender, RoutedEventArgs e)
        {
            var groupSchedules = _context.Pairs
                .Include(p => p.IdGroupNavigation)
                .Include(p => p.IdTeacherNavigation)
                .Include(p => p.IdCabinetNavigation)
                .Include(p => p.IdSubjectNavigation)
                .ToList()  // Получаем все записи в память
                .Where(p => p.Date.HasValue && p.Date.Value.ToDateTime(TimeOnly.MinValue) == DateTime.Now.Date)  // Фильтруем в памяти
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



            var csvContent = new StringBuilder();
            csvContent.AppendLine("GroupName,Pair1,Pair2,Pair3,Pair4,Pair5,Pair6,Pair7");

            foreach (var schedule in groupSchedules)
            {
                csvContent.AppendLine($"{schedule.GroupName},{schedule.Pair1},{schedule.Pair2},{schedule.Pair3},{schedule.Pair4},{schedule.Pair5},{schedule.Pair6},{schedule.Pair7}");
            }

            SaveToFile(csvContent.ToString(), "GroupSchedule.csv");
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
        private void AddSemester_Click(object sender, RoutedEventArgs e)
        {
                var inputWindow = new InputWindow("Add Semester", "NumberSemester");
                if (inputWindow.ShowDialog() == true)
                {
                    var newSemester = new Semester
                    {
                        NumberSemester = int.Parse(inputWindow.Input1)
                    };
                    _context.Semesters.Add(newSemester);
                    _context.SaveChanges();
                    semestersDataGrid.Items.Refresh();
                }
        }

        private void EditSemester_Click(object sender, RoutedEventArgs e)
        {
                if (semestersDataGrid.SelectedItem is Semester selectedSemester)
                {
                    var inputWindow = new InputWindow("Edit Semester", "NumberSemester","", selectedSemester.NumberSemester.ToString());
                    if (inputWindow.ShowDialog() == true)
                    {
                        selectedSemester.NumberSemester = int.Parse(inputWindow.Input1);
                        _context.SaveChanges();
                        semestersDataGrid.Items.Refresh();
                    }
                }
        }

        private void DeleteSemester_Click(object sender, RoutedEventArgs e)
        {
                if (semestersDataGrid.SelectedItem is Semester selectedSemester)
                {
                    _context.Semesters.Remove(selectedSemester);
                    _context.SaveChanges();
                    semestersDataGrid.Items.Refresh();
                }
        }

        private void AddGroup_Click(object sender, RoutedEventArgs e)
        {
            var newGroupWindow = new InputWindow("Add Group", "NameGroup", "Course");
            if (newGroupWindow.ShowDialog() == true)
            {
                var newGroup = new StudentGroup
                {
                    NameGroup = newGroupWindow.Input1,
                    Course = newGroupWindow.Input2
                };
                _context.StudentGroups.Add(newGroup);
                _context.SaveChanges();
                groupsDataGrid.Items.Refresh();
            }
        }

        private void EditGroup_Click(object sender, RoutedEventArgs e)
        {
            if (groupsDataGrid.SelectedItem is StudentGroup selectedGroup)
            {
                var editGroupWindow = new InputWindow("Edit Group", "NameGroup", "Course", selectedGroup.NameGroup, selectedGroup.Course.ToString());
                if (editGroupWindow.ShowDialog() == true)
                {
                    selectedGroup.NameGroup = editGroupWindow.Input1;
                    selectedGroup.Course = editGroupWindow.Input2;
                    _context.SaveChanges();
                    groupsDataGrid.Items.Refresh();
                }
            }
        }

        private void DeleteGroup_Click(object sender, RoutedEventArgs e)
        {
            if (groupsDataGrid.SelectedItem is StudentGroup selectedGroup)
            {
                _context.StudentGroups.Remove(selectedGroup);
                _context.SaveChanges();
                groupsDataGrid.Items.Refresh();
            }
        }

        private void AddTeacher_Click(object sender, RoutedEventArgs e)
        {
            var newTeacherWindow = new InputWindow("Add Teacher", "NameTeacher", "IsActive");
            if (newTeacherWindow.ShowDialog() == true)
            {
                var newTeacher = new Teacher
                {
                    NameTeacher = newTeacherWindow.Input1,
                    IsActive = bool.Parse(newTeacherWindow.Input2)
                };
                _context.Teachers.Add(newTeacher);
                _context.SaveChanges();
                teachersDataGrid.Items.Refresh();
            }
        }

        private void EditTeacher_Click(object sender, RoutedEventArgs e)
        {
            if (teachersDataGrid.SelectedItem is Teacher selectedTeacher)
            {
                var editTeacherWindow = new InputWindow("Edit Teacher", "NameTeacher", "IsActive", selectedTeacher.NameTeacher, selectedTeacher.IsActive.ToString());
                if (editTeacherWindow.ShowDialog() == true)
                {
                    selectedTeacher.NameTeacher = editTeacherWindow.Input1;
                    selectedTeacher.IsActive = bool.Parse(editTeacherWindow.Input2);
                    _context.SaveChanges();
                    teachersDataGrid.Items.Refresh();
                }
            }
        }

        private void DeleteTeacher_Click(object sender, RoutedEventArgs e)
        {
            if (teachersDataGrid.SelectedItem is Teacher selectedTeacher)
            {
                _context.Teachers.Remove(selectedTeacher);
                _context.SaveChanges();
                teachersDataGrid.Items.Refresh();
            }
        }

        private void AddCabinet_Click(object sender, RoutedEventArgs e)
        {
            var cabinetTypes = _context.CabinetTypes.ToList();

            var inputWindow = new InputWindow("Add Cabinet", "NameCabinet", "TypeCabinet", "", "", cabinetTypes.Cast<object>().ToList());
            if (inputWindow.ShowDialog() == true)
            {
                var newCabinet = new Cabinet
                {
                    NameCabinet = inputWindow.Input1,
                    IdTypeNavigation = (CabinetType)inputWindow.SelectedItem
                };

                _context.Cabinets.Add(newCabinet);
                _context.SaveChanges();

                cabinetsDataGrid.Items.Refresh();
            }
        }


        private void EditCabinet_Click(object sender, RoutedEventArgs e)
        {
            if (cabinetsDataGrid.SelectedItem is Cabinet selectedCabinet)
            {
                var editCabinetWindow = new InputWindow("Edit Cabinet", "NameCabinet", "", selectedCabinet.NameCabinet);
                if (editCabinetWindow.ShowDialog() == true)
                {
                    selectedCabinet.NameCabinet = editCabinetWindow.Input1;
                    _context.SaveChanges();
                    cabinetsDataGrid.Items.Refresh();
                }
            }
        }

        private void DeleteCabinet_Click(object sender, RoutedEventArgs e)
        {
            if (cabinetsDataGrid.SelectedItem is Cabinet selectedCabinet)
            {
                _context.Cabinets.Remove(selectedCabinet);
                _context.SaveChanges();
                cabinetsDataGrid.Items.Refresh();
            }
        }

        private void AddCabinetType_Click(object sender, RoutedEventArgs e)
        {
            var newCabinetTypeWindow = new InputWindow("Add Cabinet Type", "TypeCabinet");
            if (newCabinetTypeWindow.ShowDialog() == true)
            {
                var newCabinetType = new CabinetType
                {
                    TypeCabinet = newCabinetTypeWindow.Input1
                };
                _context.CabinetTypes.Add(newCabinetType);
                _context.SaveChanges();
                cabinetTypesDataGrid.Items.Refresh();
            }
        }

        private void EditCabinetType_Click(object sender, RoutedEventArgs e)
        {
            if (cabinetTypesDataGrid.SelectedItem is CabinetType selectedCabinetType)
            {
                var editCabinetTypeWindow = new InputWindow("Edit Cabinet Type", "TypeCabinet", "", selectedCabinetType.TypeCabinet);
                if (editCabinetTypeWindow.ShowDialog() == true)
                {
                    selectedCabinetType.TypeCabinet = editCabinetTypeWindow.Input1;
                    _context.SaveChanges();
                    cabinetTypesDataGrid.Items.Refresh();
                }
            }
        }

        private void DeleteCabinetType_Click(object sender, RoutedEventArgs e)
        {
            if (cabinetTypesDataGrid.SelectedItem is CabinetType selectedCabinetType)
            {
                _context.CabinetTypes.Remove(selectedCabinetType);
                _context.SaveChanges();
                cabinetTypesDataGrid.Items.Refresh();
            }
        }

        private void AddSubject_Click(object sender, RoutedEventArgs e)
        {
            var newSubjectWindow = new InputWindow("Add Subject", "NameSubject");
            if (newSubjectWindow.ShowDialog() == true)
            {
                var newSubject = new Subject
                {
                    NameSubject = newSubjectWindow.Input1
                };
                _context.Subjects.Add(newSubject);
                _context.SaveChanges();
                subjectsDataGrid.Items.Refresh();
            }
        }

        private void EditSubject_Click(object sender, RoutedEventArgs e)
        {
            if (subjectsDataGrid.SelectedItem is Subject selectedSubject)
            {
                var editSubjectWindow = new InputWindow("Edit Subject", "NameSubject", "", selectedSubject.NameSubject);
                if (editSubjectWindow.ShowDialog() == true)
                {
                    selectedSubject.NameSubject = editSubjectWindow.Input1;
                    _context.SaveChanges();
                    subjectsDataGrid.Items.Refresh();
                }
            }
        }

        private void DeleteSubject_Click(object sender, RoutedEventArgs e)
        {
            if (subjectsDataGrid.SelectedItem is Subject selectedSubject)
            {
                _context.Subjects.Remove(selectedSubject);
                _context.SaveChanges();
                subjectsDataGrid.Items.Refresh();
            }
        }

        private void AddSubjectLesson_Click(object sender, RoutedEventArgs e)
        {
            var newSubjectLessonWindow = new InputWindow("Add Subject Lesson", "NameOfTypeLesson");
            if (newSubjectLessonWindow.ShowDialog() == true)
            {
                var newSubjectLesson = new SubjectLesson
                {
                    NameOfTypeLesson = newSubjectLessonWindow.Input1
                };
                _context.SubjectLessons.Add(newSubjectLesson);
                _context.SaveChanges();
                subjectLessonsDataGrid.Items.Refresh();
            }
        }

        private void EditSubjectLesson_Click(object sender, RoutedEventArgs e)
        {
            if (subjectLessonsDataGrid.SelectedItem is SubjectLesson selectedSubjectLesson)
            {
                var editSubjectLessonWindow = new InputWindow("Edit Subject Lesson", "NameOfTypeLesson", "", selectedSubjectLesson.NameOfTypeLesson);
                if (editSubjectLessonWindow.ShowDialog() == true)
                {
                    selectedSubjectLesson.NameOfTypeLesson = editSubjectLessonWindow.Input1;
                    _context.SaveChanges();
                    subjectLessonsDataGrid.Items.Refresh();
                }
            }
        }

        private void DeleteSubjectLesson_Click(object sender, RoutedEventArgs e)
        {
            if (subjectLessonsDataGrid.SelectedItem is SubjectLesson selectedSubjectLesson)
            {
                _context.SubjectLessons.Remove(selectedSubjectLesson);
                _context.SaveChanges();
                subjectLessonsDataGrid.Items.Refresh();
            }
        }

        private void AddDay_Click(object sender, RoutedEventArgs e)
        {
            var weeks = _context.Weeks.Include(w => w.IdSemesterNavigation).ToList();
            var inputWindow = new InputWindow("Add Day", "DayWeek", "Week", "", "", weeks.Cast<object>().ToList());
            if (inputWindow.ShowDialog() == true)
            {
                var newDay = new Day
                {
                    DayWeek = inputWindow.Input1,
                    IdWeekNavigation = (Week)inputWindow.SelectedItem
                };
                _context.Days.Add(newDay);
                _context.SaveChanges();
                daysDataGrid.Items.Refresh();
            }
        }

        private void EditDay_Click(object sender, RoutedEventArgs e)
        {
            if (daysDataGrid.SelectedItem is Day selectedDay)
            {
                var editDayWindow = new InputWindow("Edit Day", "DayWeek", "", selectedDay.DayWeek);
                if (editDayWindow.ShowDialog() == true)
                {
                    selectedDay.DayWeek = editDayWindow.Input1;
                    _context.SaveChanges();
                    daysDataGrid.Items.Refresh();
                }
            }
        }

        private void DeleteDay_Click(object sender, RoutedEventArgs e)
        {
            if (daysDataGrid.SelectedItem is Day selectedDay)
            {
                _context.Days.Remove(selectedDay);
                _context.SaveChanges();
                daysDataGrid.Items.Refresh();
            }
        }

        private void AddWeek_Click(object sender, RoutedEventArgs e)
        {
            var semesters = _context.Semesters.ToList();
            var inputWindow = new InputWindow("Add Week", "TypeWeek", "Semester", "", "", semesters.Cast<object>().ToList());
            if (inputWindow.ShowDialog() == true)
            {
                var newWeek = new Week
                {
                    TypeWeek = inputWindow.Input1,
                    IdSemesterNavigation = (Semester)inputWindow.SelectedItem
                };
                _context.Weeks.Add(newWeek);
                _context.SaveChanges();
                weeksDataGrid.Items.Refresh();
            }
        }

        private void EditWeek_Click(object sender, RoutedEventArgs e)
        {
            if (weeksDataGrid.SelectedItem is Week selectedWeek)
            {
                var editWeekWindow = new InputWindow("Edit Week", "TypeWeek", "", selectedWeek.TypeWeek);
                if (editWeekWindow.ShowDialog() == true)
                {
                    selectedWeek.TypeWeek = editWeekWindow.Input1;
                    _context.SaveChanges();
                    weeksDataGrid.Items.Refresh();
                }
            }
        }

        private void DeleteWeek_Click(object sender, RoutedEventArgs e)
        {
            if (weeksDataGrid.SelectedItem is Week selectedWeek)
            {
                _context.Weeks.Remove(selectedWeek);
                _context.SaveChanges();
                weeksDataGrid.Items.Refresh();
            }
        }
    }
}
