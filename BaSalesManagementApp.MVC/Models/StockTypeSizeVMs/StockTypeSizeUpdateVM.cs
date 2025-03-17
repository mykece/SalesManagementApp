using BaSalesManagementApp.Dtos.CategoryDTOs;

namespace BaSalesManagementApp.MVC.Models.StockTypeSizeVMs
{
    public class StockTypeSizeUpdateVM
    {
        public Guid Id { get; set; }
        public string Size { get; set; }
        public string Description { get; set; }
        public Guid? CategoryId { get; set; }
        public List<CategoryDTO>? Categories { get; set; }

    }
}
