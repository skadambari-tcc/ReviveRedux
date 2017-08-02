using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Entities
{
    public class ShipmentRequestData
    {
        public string AccessLicenseNumber { get; set; }
        public string UserName { get; set; }
        public string Psw { get; set; }
        public string ShipmentDescription { get; set; }
        // Shipper details
        public string ShipperNumber { get; set; }
        public string AccountNumber { get; set; }
        public string ShipperAddressLine { get; set; }
        public string ShipperCity { get; set; }
        public string ShipperPostalCode { get; set; }
        public string ShipperStateProvinceCode { get; set; }
        public string ShipperCountryCode { get; set; }
        public string ShipperName { get; set; }
        public string ShipperAttentionName { get; set; }
        public string ShipperPhone { get; set; }
        public string ShipmentChargeType { get; set; }
        // Ship From

        public string ShipFromAddressLine { get; set; }
        public string ShipFromCity { get; set; }
        public string ShipFromPostalCode { get; set; }
        public string ShipFromStateProvinceCode { get; set; }
        public string ShipFromCountryCode { get; set; }
        public string ShipFromName { get; set; }
        public string ShipFromAttentionName { get; set; }
        public string ShipFromPhone { get; set; }

        // SHIP to

        public string ShipToAddressLine { get; set; }
        public string ShipToCity { get; set; }
        public string ShipToPostalCode { get; set; }
        public string ShipToStateProvinceCode { get; set; }
        public string ShipToCountryCode { get; set; }
        public string ShipToName { get; set; }
        public string ShipToAttentionName { get; set; }
        public string ShipToPhone { get; set; }
        public string Address2 { get; set; }



        // Sold to
        public string SoldAttentionName { get; set; }
        public string SoldName { get; set; }
        public string soldPhoneNumber { get; set; }
        public string SoldExtension { get; set; }
        public string SoldAddressLine { get; set; }
        public string SoldCity { get; set; }
        public string SoldPostalCode { get; set; }
        public string SoldCountryCode { get; set; }

        // Commodity
        public string CommodityDescription { get; set; }
        public string CommodityCode { get; set; }
        public string OriginCountryCode { get; set; }
        public string UnitNumber { get; set; }
        public string UnitValue { get; set; }
        public string UOMProductCode { get; set; }
        public string ProductDescription { get; set; }
        public string ProductWeight { get; set; }
        public string UOMForWeightCode { get; set; }
        public string UOMForWeightDescription { get; set; }


        // Invoice details
        public string InvoiceNumber { get; set; }
        public string InvoiceDate { get; set; }
        public string PurchaseOrderNumber { get; set; }
        public string TermsOfShipment { get; set; }
        public string ReasonForExport { get; set; }
        public string Comments { get; set; }
        public string DeclarationStatement { get; set; }


        // Discount details
        public string DiscountMonetaryValue { get; set; }
        public string FreightMonetaryValue { get; set; }
        public string InsuranceMonetaryValue { get; set; }
        public string OtherChargeMonetaryValue { get; set; }
        public string OtherChargeDescription { get; set; }
        public string CurrencyCode { get; set; }

        // Package Details

        public string PackageWeight { get; set; }
        public string PackageUOMCode { get; set; }

        //Services
        public string ServiceCode { get; set; }


    }
}
