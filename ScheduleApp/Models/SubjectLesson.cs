﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ScheduleApp.Models;

public partial class SubjectLesson
{
    public int IdTypeless { get; set; }

    public string NameOfTypeLesson { get; set; }

    public virtual ICollection<Pair> Pairs { get; set; } = new List<Pair>();
}