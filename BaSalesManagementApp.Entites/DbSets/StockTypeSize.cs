using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaSalesManagementApp.Entites.DbSets
{
    public class StockTypeSize : AuditableEntity
    {
        public string Size { get; set; }
        public string Description { get; set; }
        public Guid? CategoryId { get; set; }
        public virtual Category? Category { get; set; }

    }
}
