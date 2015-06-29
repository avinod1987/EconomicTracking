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

        public DbSet<RMCode> RMCode { get; set; }

        public DbSet<OverHead> OverHead { get; set; }

        public DbSet<OverHeadCode> OverHeadCode { get; set; }

        public DbSet<UserLogin> UserLogin { get; set; }

        public DbSet<MasterLogin> MasterLogin { get; set; }

        public DbSet<ApplicationPassword> ApplicationPassword { get; set; }

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
                new Currency{CurrencyName="USD",IndianRate=100,CurrencyCode="CUR_010"},
                 new Currency{CurrencyName="AUD",IndianRate=80,CurrencyCode="CUR_009"},
                  new Currency{CurrencyName="EU",IndianRate=80,CurrencyCode="CUR_004"},
                   new Currency{CurrencyName="GBP",IndianRate=80,CurrencyCode="CUR_008"},
                    new Currency{CurrencyName="CAD",IndianRate=80,CurrencyCode="CUR_005"},
                     new Currency{CurrencyName="JPY",IndianRate=80,CurrencyCode="CUR_007"},
                      new Currency{CurrencyName="SGD",IndianRate=80,CurrencyCode="CUR_003"},
                       new Currency{CurrencyName="HKD",IndianRate=80,CurrencyCode="CUR_006"},
                        new Currency{CurrencyName="CNY",IndianRate=80,CurrencyCode="CUR_002"},
                         new Currency{CurrencyName="INR",IndianRate=80,CurrencyCode="CUR_001"}
            };
            var materials = new List<Material> { 
                new Material{MaterialName="Steel",MaterialCode="RM_001"},new Material{MaterialName="Bronze",MaterialCode="RM_002"},new Material{MaterialName="Aluminum",MaterialCode="RM_003"}
                ,new Material{MaterialName="Copper",MaterialCode="RM_004"},new Material{MaterialName="Zinc",MaterialCode="RM_005"},new Material{MaterialName="Rubber",MaterialCode="RM_006"},
                new Material{MaterialName="Petroleum",MaterialCode="RM_007"},new Material{MaterialName="Plastic- Grade 1",MaterialCode="RM_008"},
                new Material{MaterialName="Plastic- Grade 2",MaterialCode="RM_009"},new Material{MaterialName="Plastic- Grade 3",MaterialCode="RM_010"}
            };
            var Scraps = new List<Scrap> { 
                //new Scrap{ScrapName="HR Scrap"},new Scrap{ScrapName="CR Scrap"},
                new Scrap{ScrapName="AluminiumScrap",ScrapCode="SC_001"},new Scrap{ScrapName="CopperScrap",ScrapCode="SC_002"}
                ,new Scrap{ScrapName="OilScrap",ScrapCode="SC_003"},new Scrap{ScrapName="PlasticScrap",ScrapCode="SC_004"}
                ,new Scrap{ScrapName="Rubber Scrap",ScrapCode="SC_005"},new Scrap{ScrapName="SteelScrap",ScrapCode="SC_006"}
                ,new Scrap{ScrapName="Bronze scrap",ScrapCode="SCR_07"},new Scrap{ScrapName="Brass scrap",ScrapCode="SCR_08"},new Scrap{ScrapName="SS scrap",ScrapCode="SC_009"},new Scrap{ScrapName="Cast Iron Scrap",ScrapCode="SC_010"}
            };
            var Rmcode = new List<RMCode> { 
                //new Scrap{ScrapName="HR Scrap"},new Scrap{ScrapName="CR Scrap"},
                new RMCode{RMCodeId="RM_001", RMName="Steel"},new RMCode{RMCodeId="RM_002",RMName="Bronze"}
                ,new RMCode{RMCodeId="RM_003",RMName="Aluminum"},new RMCode{RMCodeId="RM_004",RMName="Copper"}
                ,new RMCode{RMCodeId="RM_005", RMName="Zinc"},new RMCode{RMCodeId="RM_006", RMName="Rubber"}
                ,new RMCode{ RMCodeId="RM_007",RMName="Petroleum"},new RMCode{ RMCodeId="RM_008",RMName="Plastic- Grade 1"},new RMCode{ RMCodeId="RM_009",RMName="Plastic- Grade 2"},new RMCode{ RMCodeId="RM_010",RMName="Plastic- Grade 3"}
            };

            var Ohcode = new List<OverHeadCode>
            {
                new OverHeadCode{OverHeadCd="OH_001",overheadtype="Assembly Cost"},new OverHeadCode{OverHeadCd="OH_002",overheadtype="Packaging Cost"},
                new OverHeadCode{OverHeadCd="OH_003",overheadtype="Shipping Cost"},new OverHeadCode{OverHeadCd="OH_004",overheadtype="Warehousing Cost"},
                new OverHeadCode{OverHeadCd="OH_005",overheadtype="Inventory Cost"},new OverHeadCode{OverHeadCd="OH_006",overheadtype="Credit Cost"}
                ,new OverHeadCode{OverHeadCd="OH_007",overheadtype="Depreciation"},new OverHeadCode{OverHeadCd="OH_008",overheadtype="Amortisation"}
                ,new OverHeadCode{OverHeadCd="OH_009",overheadtype="Admin OverHeads"},new OverHeadCode{OverHeadCd="OH_010",overheadtype="Profit"}
            };
            dbContext.OverHeadCode.AddRange(Ohcode);
            dbContext.RMCode.AddRange(Rmcode);
            dbContext.Currency.AddRange(currency);
            dbContext.Materials.AddRange(materials);
            dbContext.Scraps.AddRange(Scraps);
            dbContext.SaveChanges();
            base.Seed(dbContext);
        }
    }
}
