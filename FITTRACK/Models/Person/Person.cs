﻿

namespace FITTRACK.Models;

public abstract class Person
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}
