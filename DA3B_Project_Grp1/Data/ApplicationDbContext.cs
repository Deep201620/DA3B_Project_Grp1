﻿using DA3B_Project_Grp1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DA3B_Project_Grp1.Data
{

    public class ApplicationDbContext : IdentityDbContext<MyIdentityUser,MyIdentityRole, Guid>
    { 
        public DbSet<Student> Students { get; set; }
        public DbSet<Faculty> Faculty { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<SubmissionDetails> Submissions { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        
    }
}
