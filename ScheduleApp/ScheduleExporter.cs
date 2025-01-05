using OfficeOpenXml;
using OfficeOpenXml.Style;
using ScheduleApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace ScheduleApp
{
    public class ScheduleExporter
    {
        private readonly ScheduleContext _context;

        public ScheduleExporter(ScheduleContext context)
        {
            _context = context;
        }

        public void ExportGroupScheduleToExcel(string filePath)
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

            var file = new FileInfo(filePath);
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(filePath);
            }

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

            MessageBox.Show($"Расписание успешно экспортировано в {filePath}");
        }

        public void ExportTeacherScheduleToExcel(string filePath)
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

            var file = new FileInfo(filePath);
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(filePath);
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(file))
            {
                var worksheet = package.Workbook.Worksheets.Add("Schedule");

                worksheet.Cells[1, 1].Value = "Преподаватель";
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
                        cell.Style.WrapText = true;
                    }
                    row++;
                }

                worksheet.Cells.AutoFitColumns();

                package.Save();
            }

            MessageBox.Show($"Расписание успешно экспортировано в {filePath}");
        }

        public void ExportCabinetScheduleToExcel(string filePath)
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

            var file = new FileInfo(filePath);
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(filePath);
            }

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

            MessageBox.Show($"Расписание успешно экспортировано в {filePath}");
        }
        public string FormatPairGroup(Pair pair)
        {
            if (pair == null) return "";
            return $"{pair.IdTeacherNavigation.NameTeacher}\n каб.{pair.IdCabinetNavigation.NameCabinet}\n {pair.IdSubjectNavigation.NameSubject}";
        }
        public string FormatPairTeacher(Pair pair)
        {
            if (pair == null) return "";
            return $"{pair.IdGroupNavigation.NameGroup}\n каб.{pair.IdCabinetNavigation.NameCabinet}\n {pair.IdSubjectNavigation.NameSubject}";
        }
        public string FormatPairCabinet(Pair pair)
        {
            if (pair == null) return "";
            return $"{pair.IdTeacherNavigation.NameTeacher}\n {pair.IdGroupNavigation.NameGroup}\n {pair.IdSubjectNavigation.NameSubject}";
        }
    }
}
