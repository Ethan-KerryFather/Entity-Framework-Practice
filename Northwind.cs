using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookPracEFcore
{
    internal class Northwind :DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }    

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // base.OnConfiguring(optionsBuilder);
            string connection = "Server=D662-ETHANLIM;Database=Northwind;Trusted_Connection=True;TrustServerCertificate=True;";
            optionsBuilder.UseSqlServer(connection);

            // 지연로딩 프록시 사용 설정
            //optionsBuilder.UseLazyLoadingProxies();

           
            
            // 로그 작성
            //optionsBuilder.LogTo(Console.WriteLine);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>()
                .Property(category => category.CategoryName)
                .IsRequired()
                .HasMaxLength(15);


            // 전역 필터 추가
            modelBuilder.Entity<Product>()
                .HasQueryFilter(product => product.Discontinued != true);
                
        }
    }
}
