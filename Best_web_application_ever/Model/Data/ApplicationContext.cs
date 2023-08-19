using Microsoft.EntityFrameworkCore;

namespace Best_web_application_ever.Model.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }

        // пользователи сайта
        public DbSet<User> Users { get; set; }

        public string connectionString;

        //public ApplicationContext(string connectionString)
        //{
        //    this.connectionString = connectionString;   // получаем извне строку подключения
        //    Database.EnsureCreated();
        //}

        public ApplicationContext()
        {
            this.connectionString = "Server=localhost;port=8080;Database=based;User Id=root;Password=123;";
            //Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Person>().HasData(
            //      new Person { Id = 1, Name = "Tosaadsm", Age = 37 },
            //      new Person { Id = 2, Name = "Boasdasdb", Age = 41 },
            //      new Person { Id = 3, Name = "Sadsadm", Age = 24 }
            //);

            modelBuilder.Entity<User>().ToTable(t => t.HasCheckConstraint("Login", "CHAR_LENGTH(Login) < 20") );

            //modelBuilder.ApplyConfiguration(new UserConfiguration());
            //modelBuilder.Entity<User>().Property(u => u.sinus).HasDefaultValueSql("SIN(30)"); // DATETIME('now')
        }
    }
}
