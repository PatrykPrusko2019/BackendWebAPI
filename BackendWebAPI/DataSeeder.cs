using BackendWebAPI.Entities;

namespace BackendWebAPI
{
    public class DataSeeder
    {
        private readonly DocumentDbContext _dbContext;

        public DataSeeder(DocumentDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Seed()
        {
            if (!_dbContext.Database.CanConnect())
            {
                if (!_dbContext.Providers.Any())
                {
                    var providers = GetProviders();
                    _dbContext.Providers.AddRange(providers);
                    _dbContext.SaveChanges();
                }

                if (!_dbContext.Storages.Any())
                {
                    var storages = GetStorages();
                    _dbContext.Storages.AddRange(storages);
                    _dbContext.SaveChanges();
                }


                if (!_dbContext.Documents.Any())
                {
                    var documents = GetDocuments();
                    _dbContext.Documents.AddRange(documents);
                    _dbContext.SaveChanges();
                }
            }
        }

        private IEnumerable<Storage> GetStorages()
        {
            var storages = new List<Storage>()
            {
               new Storage()
               {
                   Name = "Cars",
                   Symbol = "cicle"
               }
            };

            return storages;
        }

        private IEnumerable<Provider> GetProviders()
        {
            var providers = new List<Provider>()
            {
               new Provider()
               {
                   CompanyName = "BTX_Company",
                   Address = new Address() { City = "Krakow", Street = "Sobieskiego", ZipCode = "99-999"}
               }
            };

            return providers;
        }

        private IEnumerable<AdmissionDocument> GetDocuments()
        {
            var provider = _dbContext.Providers.FirstOrDefault(p => p.CompanyName == "BTX_Company");
            var storage = _dbContext.Storages.FirstOrDefault(s => s.Name == "Cars");

            var documents = new List<AdmissionDocument>()
            {
                new AdmissionDocument()
                {
                    TargetWarehouse = "Cars",
                    Vendor = "BTX_Company",
                    ProviderId = provider.Id,
                    StorageId = storage.Id,
                    LabelNames = "",
                    ApprovedDocument = "WAIT",
                    Products = new List<Product>()
                    {
                        new Product() 
                        {
                            Name = "Car Audi",
                            Code = "1",
                            Price = 22000,
                            UnitPieces = 1
                        },
                        new Product()
                        {
                            Name = "Car Bmw",
                            Code = "2",
                            Price = 30000,
                            UnitPieces = 1
                        }
                    },
                    Labels = new List<Label>()
                    {
                        new Label() { Name = "First"},
                        new Label() { Name = "Second"},
                    }
                }
            };

            return documents;
        }
    }
}
