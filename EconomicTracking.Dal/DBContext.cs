using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EconomicTracking.Dal
{
    public class EconomicsTrackingDbContext : DbContext
    {
        public EconomicsTrackingDbContext()
            : base("EconomicTracking.DbConnection")
        {
            
            Configuration.ProxyCreationEnabled = false;
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<EconomicsTrackingDbContext>());
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Settlement> Settlements { get; set; }
        public DbSet<SettlementCurrency> SettlementCurrencies { get; set; }
        public DbSet<SettlementCommodity> SettlementCommodities { get; set; }
        public DbSet<SettlementScarp> SettlementScraps { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<Scrap> Scraps { get; set; }
        public DbSet<Currency> Currency { get; set; }
        public DbSet<SalesQty> SalesQty { get; set; }
        public DbSet<CustomerAssembly> CustomerAssembly { get; set; }
        public DbSet<BillOfMaterial> BillOfMaterial { get; set; }
        public DbSet<FinalReport> FinalReport { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
            modelBuilder.Conventions.Remove<IncludeMetadataConvention>();
            Database.SetInitializer<EconomicsTrackingDbContext>(new MyDbContextInitializer());
            base.OnModelCreating(modelBuilder);
            //new MyDbContextInitializer().InitializeDatabase(new EconomicsTrackingDbContext());
        }
    }
    public class MyDbContextInitializer : DropCreateDatabaseIfModelChanges<EconomicsTrackingDbContext>
    {
        protected override void Seed(EconomicsTrackingDbContext dbContext)
        {
            // seed data
            var currency = new List<Currency> { 
                new Currency{CurrencyCode="USD",IndianRate=100},
                 new Currency{CurrencyCode="AUD",IndianRate=80},
                  new Currency{CurrencyCode="EU",IndianRate=80},
                   new Currency{CurrencyCode="GBP",IndianRate=80},
                    new Currency{CurrencyCode="CAD",IndianRate=80},
                     new Currency{CurrencyCode="JPY",IndianRate=80},
                      new Currency{CurrencyCode="SGD",IndianRate=80},
                       new Currency{CurrencyCode="HKD",IndianRate=80},
                        new Currency{CurrencyCode="CNY",IndianRate=80},
                         new Currency{CurrencyCode="INR",IndianRate=80}
            };
            var materials = new List<Material> { 
                new Material{MaterialName="Steel"},new Material{MaterialName="Bronze"},new Material{MaterialName="Aluminum"}
                ,new Material{MaterialName="Copper"},new Material{MaterialName="Zinc"},new Material{MaterialName="Rubber"},
                new Material{MaterialName="Petroleum"},new Material{MaterialName="Plastic- Grade 1"},
                new Material{MaterialName="Plastic- Grade 2"},new Material{MaterialName="Plastic- Grade 3"}
            };
            var Scraps = new List<Scrap> { 
                //new Scrap{ScrapName="HR Scrap"},new Scrap{ScrapName="CR Scrap"},
                new Scrap{ScrapName="CopperScrap"},new Scrap{ScrapName="AluminiumScrap"}
                ,new Scrap{ScrapName="PlasticScrap"},new Scrap{ScrapName="OilScrap"}
                ,new Scrap{ScrapName="Rubber Scrap"},new Scrap{ScrapName="Bronze scrap"}
                ,new Scrap{ScrapName="Brass scrap"},new Scrap{ScrapName="SS scrap"},new Scrap{ScrapName="Cast Iron Scrap"}
            };
            dbContext.Currency.AddRange(currency);
            dbContext.Materials.AddRange(materials);
            dbContext.Scraps.AddRange(Scraps);
            dbContext.SaveChanges();
            base.Seed(dbContext);
        }
    }
}
