﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LabelCreator
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class etykietyEntities : DbContext
    {
        public etykietyEntities()
            : base("name=etykietyEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<profile> profile { get; set; }
        public virtual DbSet<t1> t1 { get; set; }
        public virtual DbSet<t2> t2 { get; set; }
        public virtual DbSet<t3> t3 { get; set; }
        public virtual DbSet<uzytkownicy> uzytkownicy { get; set; }
        public virtual DbSet<wyroby> wyroby { get; set; }
    }
}
