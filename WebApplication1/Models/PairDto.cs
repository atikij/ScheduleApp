namespace WebApplication1.Models
{
    public class PairDto
    {
        public int IdPair { get; set; }
        public int IdTeacher { get; set; }
        public int IdCabinet { get; set; }
        public int IdGroup { get; set; }
        public int IdDay { get; set; }
        public int IdTypeLesson { get; set; }
        public int IdSubject { get; set; }
        public int IdSheduleNumber { get; set; }

        public string TeacherName { get; set; }
        public string CabinetName { get; set; }
        public string GroupName { get; set; }
        public string DayWeek { get; set; }
        public string SubjectName { get; set; }
        public string TypeLessonName { get; set; }
        public int SheduleNumber { get; set; }
    }
}
