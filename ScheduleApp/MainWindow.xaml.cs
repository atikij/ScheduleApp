namespace ScheduleApp
{
    using global::ScheduleApp.Models;
    using Microsoft.EntityFrameworkCore;
    using OfficeOpenXml;
    using OfficeOpenXml.Style;
    using ScheduleApp.Models;
    using System.IO;
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
            var groupSchedules = _context.Pairs
                           .Include(p => p.IdGroupNavigation)
                           .Include(p => p.IdTeacherNavigation)
                           .Include(p => p.IdCabinetNavigation)
                           .Include(p => p.IdSubjectNavigation)
                           .ToList()
                           .GroupBy(p => p.IdGroupNavigation.NameGroup)
                           .Select(g => new
                           {
                               GroupName = g.Key,
                               Pairs = g.ToDictionary(p => p.IdSheduleNumber, p => p)
                           }).ToList();

            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                FileName = "GroupSchedule.xlsx",
                Filter = "Excel Files|*.xlsx",
                Title = "Save Schedule as Excel File"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                var file = new FileInfo(saveFileDialog.FileName);
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var package = new ExcelPackage(file))
                {
                    var worksheet = package.Workbook.Worksheets.Add("Schedule");

                    worksheet.Cells[1, 1].Value = "Группа";
                    worksheet.Cells[1, 2].Value = "Пара 1";
                    worksheet.Cells[1, 3].Value = "Пара 2";
                    worksheet.Cells[1, 4].Value = "Пара 3";
                    worksheet.Cells[1, 5].Value = "Пара 4";
                    worksheet.Cells[1, 6].Value = "Пара 5";
                    worksheet.Cells[1, 7].Value = "Пара 6";
                    worksheet.Cells[1, 8].Value = "Пара 7";

                    int row = 2;
                    foreach (var schedule in groupSchedules)
                    {
                        worksheet.Cells[row, 1].Value = schedule.GroupName;
                        for (int i = 1; i <= 7; i++)
                        {
                            var cell = worksheet.Cells[row, i + 1];
                            cell.Value = schedule.Pairs.ContainsKey(i) ? FormatPairGroup(schedule.Pairs[i]) : "";
                            cell.Style.WrapText = true; 
                        }
                        row++;
                    }

                    worksheet.Cells.AutoFitColumns();

                    for (int col = 2; col <= 8; col++)
                    {
                        var startRow = 2;
                        while (startRow <= worksheet.Dimension.End.Row)
                        {
                            var cellValue = worksheet.Cells[startRow, col].Text;
                            var endRow = startRow;

                            while (endRow <= worksheet.Dimension.End.Row && worksheet.Cells[endRow, col].Text == cellValue)
                            {
                                endRow++;
                            }

                            if (endRow > startRow + 1)
                            {
                                worksheet.Cells[startRow, col, endRow - 1, col].Merge = true;
                                worksheet.Cells[startRow, col, endRow - 1, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            }

                            startRow = endRow;
                        }
                    }

                    package.Save();
                }

                MessageBox.Show($"Расписание успешно экспортировано в {saveFileDialog.FileName}");
            }
        }

        private void ExportTeacherScheduleToExcel_Click(object sender, RoutedEventArgs e)
        {
            var teacherSchedules = _context.Pairs
                .Include(p => p.IdTeacherNavigation)
                .Include(p => p.IdGroupNavigation)
                .Include(p => p.IdCabinetNavigation)
                .Include(p => p.IdSubjectNavigation)
                .ToList()
                .GroupBy(p => p.IdTeacherNavigation.NameTeacher)
                .Select(g => new
                {
                    TeacherName = g.Key,
                    Pairs = g.ToDictionary(p => p.IdSheduleNumber, p => p)
                }).ToList();

            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                FileName = "TeacherSchedule.xlsx",
                Filter = "Excel Files|*.xlsx",
                Title = "Save Schedule as Excel File"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                var file = new FileInfo(saveFileDialog.FileName);
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var package = new ExcelPackage(file))
                {
                    var worksheet = package.Workbook.Worksheets.Add("Schedule");

                    worksheet.Cells[1, 1].Value = "Преподователь";
                    worksheet.Cells[1, 2].Value = "Пара 1";
                    worksheet.Cells[1, 3].Value = "Пара 2";
                    worksheet.Cells[1, 4].Value = "Пара 3";
                    worksheet.Cells[1, 5].Value = "Пара 4";
                    worksheet.Cells[1, 6].Value = "Пара 5";
                    worksheet.Cells[1, 7].Value = "Пара 6";
                    worksheet.Cells[1, 8].Value = "Пара 7";

                    int row = 2;
                    foreach (var schedule in teacherSchedules)
                    {
                        worksheet.Cells[row, 1].Value = schedule.TeacherName;
                        for (int i = 1; i <= 7; i++)
                        {
                            var cell = worksheet.Cells[row, i + 1];
                            cell.Value = schedule.Pairs.ContainsKey(i) ? FormatPairTeacher(schedule.Pairs[i]) : "";
                            cell.Style.WrapText = true; // Включаем перенос текста
                        }
                        row++;
                    }

                    worksheet.Cells.AutoFitColumns();
                    package.Save();
                }

                MessageBox.Show($"Расписание успешно экспортировано в {saveFileDialog.FileName}");
            }
        }
        private void ExportCabinetScheduleToExcel_Click(object sender, RoutedEventArgs e)
        {
            var cabinetSchedules = _context.Pairs
                .Include(p => p.IdGroupNavigation)
                .Include(p => p.IdTeacherNavigation)
                .Include(p => p.IdCabinetNavigation)
                .Include(p => p.IdSubjectNavigation)
                .ToList()
                .GroupBy(p => p.IdCabinetNavigation.NameCabinet)
                .Select(g => new
                {
                    CabinetName = g.Key,
                    Pairs = g.ToDictionary(p => p.IdSheduleNumber, p => p)
                }).ToList();

            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                FileName = "CabinetSchedule.xlsx",
                Filter = "Excel Files|*.xlsx",
                Title = "Save Schedule as Excel File"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                var file = new FileInfo(saveFileDialog.FileName);
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var package = new ExcelPackage(file))
                {
                    var worksheet = package.Workbook.Worksheets.Add("Schedule");

                    worksheet.Cells[1, 1].Value = "Кабинет";
                    worksheet.Cells[1, 2].Value = "Пара 1";
                    worksheet.Cells[1, 3].Value = "Пара 2";
                    worksheet.Cells[1, 4].Value = "Пара 3";
                    worksheet.Cells[1, 5].Value = "Пара 4";
                    worksheet.Cells[1, 6].Value = "Пара 5";
                    worksheet.Cells[1, 7].Value = "Пара 6";
                    worksheet.Cells[1, 8].Value = "Пара 7";

                    int row = 2;
                    foreach (var schedule in cabinetSchedules)
                    {
                        worksheet.Cells[row, 1].Value = schedule.CabinetName;
                        for (int i = 1; i <= 7; i++)
                        {
                            var cell = worksheet.Cells[row, i + 1];
                            cell.Value = schedule.Pairs.ContainsKey(i) ? FormatPairCabinet(schedule.Pairs[i]) : "";
                            cell.Style.WrapText = true;
                        }
                        row++;
                    }

                    worksheet.Cells.AutoFitColumns();

                    for (int col = 2; col <= 8; col++)
                    {
                        var startRow = 2;
                        while (startRow <= worksheet.Dimension.End.Row)
                        {
                            var cellValue = worksheet.Cells[startRow, col].Text;
                            var endRow = startRow;

                            while (endRow <= worksheet.Dimension.End.Row && worksheet.Cells[endRow, col].Text == cellValue)
                            {
                                endRow++;
                            }

                            if (endRow > startRow + 1)
                            {
                                worksheet.Cells[startRow, col, endRow - 1, col].Merge = true;
                                worksheet.Cells[startRow, col, endRow - 1, col].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            }

                            startRow = endRow;
                        }
                    }

                    package.Save();
                }

                MessageBox.Show($"Расписание успешно экспортировано в {saveFileDialog.FileName}");
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
                var newGroup = new Studentgroup
                {
                    NameGroup = newGroupWindow.Input1,
                    Course = newGroupWindow.Input2
                };
                _context.Studentgroups.Add(newGroup);
                _context.SaveChanges();
                groupsDataGrid.Items.Refresh();
            }
        }

        private void EditGroup_Click(object sender, RoutedEventArgs e)
        {
            if (groupsDataGrid.SelectedItem is Studentgroup selectedGroup)
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
            if (groupsDataGrid.SelectedItem is Studentgroup selectedGroup)
            {
                _context.Studentgroups.Remove(selectedGroup);
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
            var cabinetTypes = _context.Cabinettypes.ToList();

            var inputWindow = new InputWindow("Add Cabinet", "NameCabinet", "TypeCabinet", "", "", cabinetTypes.Cast<object>().ToList());
            if (inputWindow.ShowDialog() == true)
            {
                var newCabinet = new Cabinet
                {
                    NameCabinet = inputWindow.Input1,
                    IdTypeNavigation = (Cabinettype)inputWindow.SelectedItem
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
                var newCabinetType = new Cabinettype
                {
                    TypeCabinet = newCabinetTypeWindow.Input1
                };
                _context.Cabinettypes.Add(newCabinetType);
                _context.SaveChanges();
                cabinetTypesDataGrid.Items.Refresh();
            }
        }

        private void EditCabinetType_Click(object sender, RoutedEventArgs e)
        {
            if (cabinetTypesDataGrid.SelectedItem is Cabinettype selectedCabinetType)
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
            if (cabinetTypesDataGrid.SelectedItem is Cabinettype selectedCabinetType)
            {
                _context.Cabinettypes.Remove(selectedCabinetType);
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
                var newSubjectLesson = new Subjectlesson
                {
                    NameOfTypeLesson = newSubjectLessonWindow.Input1
                };
                _context.Subjectlessons.Add(newSubjectLesson);
                _context.SaveChanges();
                subjectLessonsDataGrid.Items.Refresh();
            }
        }

        private void EditSubjectLesson_Click(object sender, RoutedEventArgs e)
        {
            if (subjectLessonsDataGrid.SelectedItem is Subjectlesson selectedSubjectLesson)
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
            if (subjectLessonsDataGrid.SelectedItem is Subjectlesson selectedSubjectLesson)
            {
                _context.Subjectlessons.Remove(selectedSubjectLesson);
                _context.SaveChanges();
                subjectLessonsDataGrid.Items.Refresh();
            }
        }

        private void AddDay_Click(object sender, RoutedEventArgs e)
        {
            var weeks = _context.Weeks.Include(w => w.IdSemesterNavigation).ToList();
            var inputWindow = new InputWindow("Add Day", "DayWeek", "Week", "", "", weeks.Cast<object>().ToList(), showDatePicker: true);
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
