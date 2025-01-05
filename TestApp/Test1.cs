using Microsoft.VisualStudio.TestTools.UnitTesting;
using ScheduleApp.Models;
using ScheduleApp;
using System.Collections.Generic;

namespace TestApp
{
    [TestClass]
    public sealed class Test1
    {
        private ScheduleExporter _exporter;

        [TestInitialize]
        public void Initialize()
        {
            _exporter = new ScheduleExporter(null);
        }

        [TestMethod]
        public void FormatPairGroup_ShouldReturnCorrectString()
        {
            var pair = new Pair
            {
                IdSubjectNavigation = new Subject { NameSubject = "Math" },
                IdTeacherNavigation = new Teacher { NameTeacher = "Mr. Smith" },
                IdCabinetNavigation = new Cabinet { NameCabinet = "Cabinet1" }
            };

            var result = _exporter.FormatPairGroup(pair);
            var expected = "Mr. Smith\n каб.Cabinet1\n Math";

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void FormatPairTeacher_ShouldReturnCorrectString()
        {
            var pair = new Pair
            {
                IdGroupNavigation = new Studentgroup { NameGroup = "Group1" },
                IdSubjectNavigation = new Subject { NameSubject = "Math" },
                IdCabinetNavigation = new Cabinet { NameCabinet = "Cabinet1" }
            };

            var result = _exporter.FormatPairTeacher(pair);
            var expected = "Group1\n каб.Cabinet1\n Math";

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void FormatPairCabinet_ShouldReturnCorrectString()
        {
            var pair = new Pair
            {
                IdGroupNavigation = new Studentgroup { NameGroup = "Group1" },
                IdSubjectNavigation = new Subject { NameSubject = "Math" },
                IdTeacherNavigation = new Teacher { NameTeacher = "Mr. Smith" }
            };

            var result = _exporter.FormatPairCabinet(pair);
            var expected = "Mr. Smith\n Group1\n Math";

            Assert.AreEqual(expected, result);
        }

    }
}
