﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public partial class Subject
{
    public int IdSubject { get; set; }

    public string NameSubject { get; set; }

    public virtual ICollection<Pair> Pairs { get; set; } = new List<Pair>();
}