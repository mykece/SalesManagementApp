namespace BaSalesManagementApp.Dtos.OrderDTOs
{
    public class OrderListDTO
    {
        public string ProductName { get; set; }
        public string CompanyName { get; set; }
        public bool isCompanyActive { get; set; }

        public bool IsActive { get; set; }
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public string AdminName { get; set; }
    }
}
