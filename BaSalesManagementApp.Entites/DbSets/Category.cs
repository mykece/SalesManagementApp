
namespace BaSalesManagementApp.Entites.DbSets
{
    public class Category : AuditableEntity
    {
        public string Name { get; set; }
        public virtual ICollection<StockTypeSize>? StockTypeSizes { get; set; }

    }
}
