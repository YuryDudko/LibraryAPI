﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public class Book
{
    public int Id { get; set; }
    public string ISBN { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    //author foreighn key
    public int AuthorId { get; set; }
    public Author Author { get; set; } = null!;

    public DateTime BorrowedAt { get; set; }
    public DateTime ReturnBy { get; set; }
}
