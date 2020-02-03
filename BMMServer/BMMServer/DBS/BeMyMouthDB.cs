using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using BMMServer.Models;

namespace BMMServer.DBS
{
    
    public partial class BeMyMouthDB : DbContext
    {


        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
                 => optionsBuilder.UseMySql(@"server=127.0.0.1;user id=root;password=123456;persistsecurityinfo=True;database=bemymouth;Character Set=utf8");
    }
}
