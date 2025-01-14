﻿namespace RealWorld.WebAPI.Models;

public sealed class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public DateOnly DateOfBirth { get; set; }
}