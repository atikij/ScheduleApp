﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Day
{
    public int IdDay { get; set; }

    public string DayWeek { get; set; }

    public int IdWeek { get; set; }

    public DateOnly Date { get; set; }

    public virtual Week IdWeekNavigation { get; set; }

    public virtual ICollection<Pair> Pairs { get; set; } = new List<Pair>();
}