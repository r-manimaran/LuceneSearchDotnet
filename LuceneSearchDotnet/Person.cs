﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuceneSearchDotnet;

public class Person
{
    public  Guid Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Title { get; set; }
    public  string? Email { get; set; }
    public string? Company { get; set; }
    public  string? Description { get; set; }

}
