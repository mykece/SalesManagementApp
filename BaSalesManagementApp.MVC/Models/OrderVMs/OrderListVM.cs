using System.ComponentModel.DataAnnotations;

namespace BaSalesManagementApp.MVC.Models.OrderVMs
{
    public class OrderListVM
    {
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        public bool isCompanyActive { get; set; }

        public Guid Id { get; set; }
        public int Quantity { get; set; }

        [Display(Name = "Total Price")]
        public decimal TotalPrice { get; set; }

        public bool IsActive { get; set; }

        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }
        [Display(Name = "Admin Name")]
        public string AdminName { get; set; }
    }
}