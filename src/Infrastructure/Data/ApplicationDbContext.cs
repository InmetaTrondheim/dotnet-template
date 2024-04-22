﻿using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public DbSet<TodoItem> TodoItems => Set<TodoItem>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

}