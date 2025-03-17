using BaSalesManagementApp.Dtos.CategoryDTOs;

namespace BaSalesManagementApp.MVC.Models.StockTypeSizeVMs
{
    public class StockTypeSizeCreateVM
    {

        public string Size { get; set; } = null!;
        public string Description { get; set; } = null!;

        public Guid? CategoryId { get; set; }
        public List<CategoryDTO>? Categories { get; set; }

    }
}
