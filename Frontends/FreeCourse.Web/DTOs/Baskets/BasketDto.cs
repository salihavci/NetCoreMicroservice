using System;
using System.Collections.Generic;
using System.Linq;

namespace FreeCourse.Web.DTOs.Baskets
{
    public class BasketDto
    {
        public BasketDto()
        {
            BasketItems = new List<BasketItemDto>();
        }
        public string UserId { get; set; }
        public string DiscountCode { get; set; }
        public int? DiscountRate { get; set; }
        private List<BasketItemDto> _BasketItems { get; set; }
        public List<BasketItemDto> BasketItems
        {
            get
            {
                if (HasDiscount)
                {
                    _BasketItems.ForEach(x =>
                    {
                        var discountPrice = x.Price * ((decimal)DiscountRate.Value / 100);
                        x.AppliedDiscount(Math.Round(x.Price - discountPrice,2));
                    });
                }
                return _BasketItems;
            }
            set 
            {
                _BasketItems = value; 
            }
        }
        public decimal TotalPrice { get => BasketItems.Sum(x => x.GetCurrentPrice); }
        public bool HasDiscount { get => !string.IsNullOrWhiteSpace(DiscountCode) && DiscountRate.HasValue; }
        public void CancelDiscount()
        {
            DiscountCode = null;
            DiscountRate = null;
        }
        public void ApplyDiscount(string code, int? rate)
        {
            DiscountCode = code;
            DiscountRate = rate;
        }
    }
}
