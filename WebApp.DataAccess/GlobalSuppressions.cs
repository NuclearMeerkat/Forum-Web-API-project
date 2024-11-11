// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1512:Single-line comments should not be followed by blank line", Justification = "<1>", Scope = "member", Target = "~M:WebApp.DataAccess.Data.ForumDbContext.OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder)")]
[assembly: SuppressMessage("Major Code Smell", "S125:Sections of code should not be commented out", Justification = "<1>", Scope = "member", Target = "~M:WebApp.DataAccess.Data.ForumDbContext.OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder)")]
[assembly: SuppressMessage("Style", "IDE0058:Expression value is never used", Justification = "<1>", Scope = "member", Target = "~M:WebApp.DataAccess.Data.DbInitializer.SeedAdminUser(System.IServiceProvider)")]
[assembly: SuppressMessage("Style", "IDE0058:Expression value is never used", Justification = "<1>", Scope = "member", Target = "~M:WebApp.DataAccess.Data.ForumDbContext.OnModelCreating(Microsoft.EntityFrameworkCore.ModelBuilder)")]
[assembly: SuppressMessage("Style", "IDE0058:Expression value is never used", Justification = "<1>", Scope = "member", Target = "~M:WebApp.DataAccess.Data.UnitOFWork.SaveAsync~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Style", "IDE0058:Expression value is never used", Justification = "<1>", Scope = "member", Target = "~M:WebApp.DataAccess.Migrations.Initial.Down(Microsoft.EntityFrameworkCore.Migrations.MigrationBuilder)")]
[assembly: SuppressMessage("Style", "IDE0058:Expression value is never used", Justification = "<1>", Scope = "member", Target = "~M:WebApp.DataAccess.Migrations.Initial.Up(Microsoft.EntityFrameworkCore.Migrations.MigrationBuilder)")]
[assembly: SuppressMessage("Style", "IDE0058:Expression value is never used", Justification = "<1>", Scope = "member", Target = "~M:WebApp.DataAccess.Repositories.GenericRepository`1.AddAsync(`0)~System.Threading.Tasks.Task{System.Object}")]
[assembly: SuppressMessage("Style", "IDE0058:Expression value is never used", Justification = "<1>", Scope = "member", Target = "~M:WebApp.DataAccess.Repositories.GenericRepository`1.Delete(`0)")]
[assembly: SuppressMessage("Style", "IDE0058:Expression value is never used", Justification = "<1>", Scope = "member", Target = "~M:WebApp.DataAccess.Repositories.GenericRepository`1.DeleteByIdAsync(System.Object[])~System.Threading.Tasks.Task")]
[assembly: SuppressMessage("Style", "IDE0058:Expression value is never used", Justification = "<1>", Scope = "member", Target = "~M:WebApp.DataAccess.Repositories.GenericRepository`1.Update(`0)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<1>", Scope = "member", Target = "~M:WebApp.DataAccess.Repositories.GenericRepository`1.AddAsync(`0)~System.Threading.Tasks.Task{System.Object}")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<1>", Scope = "member", Target = "~M:WebApp.DataAccess.Migrations.Initial.Down(Microsoft.EntityFrameworkCore.Migrations.MigrationBuilder)")]
[assembly: SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<1>", Scope = "member", Target = "~M:WebApp.DataAccess.Migrations.Initial.Up(Microsoft.EntityFrameworkCore.Migrations.MigrationBuilder)")]
[assembly: SuppressMessage("Minor Code Smell", "S6608:Prefer indexing instead of \"Enumerable\" methods on types implementing \"IList\"", Justification = "<1>", Scope = "member", Target = "~M:WebApp.DataAccess.Repositories.MessageRepository.GetByIdAsync(System.Object[])~System.Threading.Tasks.Task{WebApp.Infrastructure.Entities.Message}")]
[assembly: SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:Element should begin with upper-case letter", Justification = "<1>", Scope = "member", Target = "~P:WebApp.DataAccess.Repositories.GenericRepository`1.context")]
