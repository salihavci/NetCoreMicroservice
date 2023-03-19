namespace FreeCourse.Web.DTOs.Orders
{
    public class OrderCreatedDto
    {
        public int OrderId { get; set; }
        public string Error { get; set; }
        public bool IsSuccessful { get; set; }
    }
}
