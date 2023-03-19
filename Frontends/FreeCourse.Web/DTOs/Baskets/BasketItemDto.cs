namespace FreeCourse.Web.DTOs.Baskets
{
    public class BasketItemDto
    {
        public int Quantity { get; set; } = 1;
        public string CourseId { get; set; }
        public string CourseName { get; set; }
        public decimal Price { get; set; }
        private decimal? DiscountAppliedPrice { get; set; }

        public decimal GetCurrentPrice { get => DiscountAppliedPrice ?? Price; }

        public void AppliedDiscount(decimal discountAppliedPrice)
        {
            DiscountAppliedPrice = discountAppliedPrice;
        }

    }
}
