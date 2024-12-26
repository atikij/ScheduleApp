using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupScheduleController : ControllerBase
    {
        private readonly mydbContext _context;

        public GroupScheduleController(mydbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PairDto>>> GetGroupSchedules()
        {
            var schedules = await _context.Pairs
                .Include(p => p.IdTeacherNavigation)
                .Include(p => p.IdCabinetNavigation)
                .Include(p => p.IdGroupNavigation)
                .Include(p => p.IdDayNavigation)
                .Include(p => p.IdSheduleNumberNavigation)
                .Include(p => p.IdSubjectNavigation)
                .Include(p => p.IdTypeLessonNavigation)
                .Select(p => new PairDto
                {
                    IdPair = p.IdPair,
                    IdTeacher = p.IdTeacher,
                    IdCabinet = p.IdCabinet,
                    IdGroup = p.IdGroup,
                    IdDay = p.IdDay,
                    IdTypeLesson = p.IdTypeLesson,
                    IdSubject = p.IdSubject,
                    IdSheduleNumber = p.IdSheduleNumber,
                    TeacherName = p.IdTeacherNavigation.NameTeacher,
                    CabinetName = p.IdCabinetNavigation.NameCabinet,
                    GroupName = p.IdGroupNavigation.NameGroup,
                    DayWeek = p.IdDayNavigation.DayWeek,
                    SubjectName = p.IdSubjectNavigation.NameSubject,
                    TypeLessonName = p.IdTypeLessonNavigation.NameOfTypeLesson,
                    SheduleNumber = p.IdSheduleNumberNavigation.NumberPair1 ?? 0 // Используем значение по умолчанию, если NumberPair1 является null
                })
                .ToListAsync();

            return Ok(schedules);
        }
    }
}
