using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Web.Mvc;
using sellmarket.Migrations;

namespace sellmarket.Models
{
    public class MarketContext : DbContext
    {
        public MarketContext() : base("DefaultConnection")
        {
            /*Database.SetInitializer
                (new MigrateDatabaseToLatestVersion<MarketContext
                ,Configuration>());*/
            Database.SetInitializer
                (new DropCreateDatabaseIfModelChanges<MarketContext>());
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Images)
                .WithOptional(i => i.Product)
                .HasForeignKey(i => i.ProductId);


            modelBuilder.Entity<Product>()
                .HasMany(p => p.Articles)
                .WithOptional(i => i.Product)
                .HasForeignKey(i => i.ProductId);
            
            
            /*modelBuilder.Entity<Product>()
                .HasMany(p => p.Orders)
                .WithRequired(i => i.Product)
                .HasForeignKey(i => i.ProductId);*/
        }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Article> Articles { get; set; }
    }

    public class Product : HasImages,IModel
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }


        public decimal Cost { get; set; }

        public virtual List<Image> Images { get; set; }
        public virtual List<Article> Articles { get; set; }
        //public virtual List<Order> Orders { get; set; }
        
        
    }
    
    public class Order : IModel
    {
        public long Id { get; set; }
        
        /// <summary>
        /// محصول انتخاب شده
        /// </summary>
     //   public long ProductId{ get; set; }
      //  public virtual Product Product{ get; set; }
        public string ProductName{ get; set; }

        /// <summary>
        /// توضیحات اضافی
        /// </summary>
        public string Description { get; set; }

        
        /// <summary>
        /// آیا درخواست نمونه است
        /// </summary>
        public bool IsSample { get; set; }
        
        /// <summary>
        /// شماره تلفن
        /// </summary>
        public string Tel1 { get; set; }
        
        
        /// <summary>
        ///email
        /// </summary>
        public string Email { get; set; }
    }

    public class Article:IModel
    {
        public long Id { get; set; }
        public long? ProductId { get; set; }
        public virtual Product Product { get; set; }

        [AllowHtml] public string Text { get; set; }
    }

    public class Image:IModel
    {
        public long Id { get; set; }
        public long? ProductId { get; set; }
        
        [Column("FileFromNet")]
        public string FileFromNet { get; set; }
        
        
        public byte[] Source { get; set; }
        public virtual Product Product { get; set; }

        public ImageType ImageType { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
    }

    public interface HasImages
    {
        List<Image> Images { get; set; }
    }
    
    public interface IModel
    {
       long Id { get; set; }
    }

    public enum ImageType
    {
        Main,
        Slider
    }
}