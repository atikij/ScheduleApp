﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Week
{
    public int IdWeek { get; set; }

    public string TypeWeek { get; set; }

    public int IdSemester { get; set; }

    public virtual ICollection<Day> Days { get; set; } = new List<Day>();

    public virtual Semester IdSemesterNavigation { get; set; }
}