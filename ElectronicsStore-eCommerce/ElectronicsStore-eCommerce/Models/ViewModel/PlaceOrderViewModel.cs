using System.ComponentModel.DataAnnotations;

namespace ElectronicsStore_eCommerce.Models.ViewModel
{
    public class PlaceOrderViewModel
    {
        public List<ShippingInfo> AvailableShippingInfos { get; set; }
        [Required(ErrorMessage = "Shipping Information Required")]
        public int SelectedShippingInfoId { get; set; }
        public List<string> AvailablePaymentMethods { get; set; }
        public string SelectedPaymentMethod { get; set; }
    }
}
