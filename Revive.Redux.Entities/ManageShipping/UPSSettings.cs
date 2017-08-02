using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Entities
{
    public class UPSSettings
    {

        public int Id { get; set; }

        [Display(Name = "User Name")]
        [Required(ErrorMessage = "Required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Required")]        
        public string Psw { get; set; }

        [Display(Name = "Shipment Description")]
        [Required(ErrorMessage = "Required")]
        public string ShipmentDescription { get; set; }

        [Display(Name = "Shipper Number")]
        [Required(ErrorMessage = "Required")]
        public string ShipperNumber { get; set; }

        [Display(Name = "Account Number")]
        [Required(ErrorMessage = "Required")]       
        public string AccountNumber { get; set; }

        [Display(Name = "Shipper Address Line")]
        [Required(ErrorMessage = "Required")]       
        public string ShipperAddressLine { get; set; }

        [Display(Name = "Shipper City")]
        [Required(ErrorMessage = "Required")]
        public string ShipperCity { get; set; }

        [Display(Name = "Shipper PostalCode")]
        [Required(ErrorMessage = "Required")]
      //  [Range(1, int.MaxValue, ErrorMessage = "The value must be greater than 0")]
        public int ShipperPostalCode { get; set; }

        [Display(Name = "Shipper StateProvinceCode")]
        [Required(ErrorMessage = "Required")]
       // [Range(1, int.MaxValue, ErrorMessage = "The value must be greater than 0")]
        public string ShipperStateProvinceCode { get; set; }

        //[Display(Name = "Shipper Address Line")]
        //[Required(ErrorMessage = "Required")]
        //public string ShipperAddressLine { get; set; }

        [Display(Name = "Shipper Country Code")]
        [Required(ErrorMessage = "Required")]
        public string ShipperCountryCode { get; set; }


        [Display(Name = "Shipper Name")]
        [Required(ErrorMessage = "Required")]
        public string ShipperName { get; set; }


        [Display(Name = "Shipper Attention Name")]
        [Required(ErrorMessage = "Required")]
        public string ShipperAttentionName { get; set; }


        [Display(Name = "Shipper Phone")]
        [Required(ErrorMessage = "Required")]
        public string ShipperPhone { get; set; }


        [Display(Name = "Ship From City")]
        [Required(ErrorMessage = "Required")]
        public string ShipFromCity { get; set; }


        [Display(Name = "Ship From Postal Code")]
        [Required(ErrorMessage = "Required")]
        [Range(1, int.MaxValue, ErrorMessage = "The value must be greater than 0")]
        public int ShipFromPostalCode { get; set; }


        [Display(Name = "Ship From StateProvinceCode")]
        [Required(ErrorMessage = "Required")]
        public string ShipFromStateProvinceCode { get; set; }

        [Display(Name = "Ship From CountryCode")]
        [Required(ErrorMessage = "Required")]
        public string ShipFromCountryCode { get; set; }

        [Display(Name = "Ship From AttentionName")]
        [Required(ErrorMessage = "Required")]
        public string ShipFromAttentionName { get; set; }



        [Display(Name = "Ship From AddressLine")]
        [Required(ErrorMessage = "Required")]
        public string ShipFromAddressLine { get; set; }
        [Display(Name = "Ship From Name")]
        [Required(ErrorMessage = "Required")]
        public string ShipFromName { get; set; }

        [Display(Name = "Shipment Charge Type")]
        [Required(ErrorMessage = "Required")]
        public string ShipmentChargeType { get; set; }

        [Display(Name = "Service Code")]
        [Required(ErrorMessage = "Required")]
        public string ServiceCode { get; set; }

        public bool? IsActive { get; set; }
        public Guid Created_by { get; set; }
        public DateTime? Created_Date { get; set; }
        public Guid Modified_by { get; set; }
        public DateTime? Modified_Date { get; set; }
        public Guid CurrentUserId { get; set; }       

    }
}
