using Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class DataBaseContext : IdentityDbContext
    {
        public DataBaseContext() : base() { }
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

        }
        public DbSet<User> User { get; set; }
        public DbSet<Bank> Bank { get; set; }
        public DbSet<Region> Region { get; set; }
        public DbSet<AppSession> AppSession { get; set; }
        public DbSet<LogSession> LogSession { get; set; }
        public DbSet<ATM> ATM { get; set; }
        public DbSet<ATMModel> ATMModel { get; set; }
        public DbSet<ATMErrorCode> ATMErrorCode { get; set; }
        public DbSet<ATMSoft> ATMSoft { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<CompanyContact> CompanyContact { get; set; }
        public DbSet<Instruction> Instruction { get; set; }
        public DbSet<ProblemSolution> ProblemSolution { get; set; }
        public DbSet<RequestReason> RequestReason { get; set; }
        public DbSet<SupportRequest> SupportRequest { get; set; }
    }
}