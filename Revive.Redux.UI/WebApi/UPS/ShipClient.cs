using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Revive.Redux.UI.WebReference;
using System.ServiceModel;
using Revive.Redux.Entities;
using System.Net;
using System.Web.Script.Serialization;

namespace Revive.Redux.UI.WebApi.UPS
{
    public class ShipClient
    {

        //public ShipClient()
        //{
        //    System.Net.ServicePointManager.CertificatePolicy = new Revive.Redux.UI.WebApi.UPS.TrustAllCertificatePolicy();

        //}
        public ShipmentResponseData MakeShipment(ShipmentRequestData ShipmentRequestObj)
        {
            ShipmentResponseData ShipmentResponseObj = new ShipmentResponseData();
            try
            {

                //ServicePointManager.Expect100Continue = true;
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;

                ShipService shpSvc = new ShipService();
                ShipmentRequest shipmentRequest = new ShipmentRequest();
                UPSSecurity upss = new UPSSecurity();
                UPSSecurityServiceAccessToken upssSvcAccessToken = new UPSSecurityServiceAccessToken();
                upssSvcAccessToken.AccessLicenseNumber = ShipmentRequestObj.AccessLicenseNumber;
                upss.ServiceAccessToken = upssSvcAccessToken;
                UPSSecurityUsernameToken upssUsrNameToken = new UPSSecurityUsernameToken();
                upssUsrNameToken.Username = ShipmentRequestObj.UserName;
                upssUsrNameToken.Password = ShipmentRequestObj.Psw;
                upss.UsernameToken = upssUsrNameToken;
                shpSvc.UPSSecurityValue = upss;
                RequestType request = new RequestType();
                String[] requestOption = { "nonvalidate" };
                request.RequestOption = requestOption;
                shipmentRequest.Request = request;
                ShipmentType shipment = new ShipmentType();
                shipment.Description = ShipmentRequestObj.ShipmentDescription;
                ShipperType shipper = new ShipperType();
                shipper.ShipperNumber = ShipmentRequestObj.ShipperNumber;
                PaymentInfoType paymentInfo = new PaymentInfoType();
                ShipmentChargeType shpmentCharge = new ShipmentChargeType();
                BillShipperType billShipper = new BillShipperType();
                billShipper.AccountNumber = ShipmentRequestObj.AccountNumber;
                shpmentCharge.BillShipper = billShipper;


                // Third Party billing

                //BillThirdPartyChargeType objBillThirdParty = new BillThirdPartyChargeType();
                //objBillThirdParty.AccountNumber = ShipmentRequestObj.AccountNumber;

                //AccountAddressType ObjAddress = new AccountAddressType();
                //ObjAddress.PostalCode = "83501"; //ShipmentRequestObj.ShipperPostalCode;
                //ObjAddress.CountryCode = "US";//ShipmentRequestObj.ShipperCountryCode;
                //objBillThirdParty.Address = ObjAddress;
                //shpmentCharge.BillThirdParty = objBillThirdParty;


                shpmentCharge.Type = ShipmentRequestObj.ShipmentChargeType;
                ShipmentChargeType[] shpmentChargeArray = { shpmentCharge };
                paymentInfo.ShipmentCharge = shpmentChargeArray;
                shipment.PaymentInformation = paymentInfo;

                ShipAddressType shipperAddress = new ShipAddressType();
                String[] addressLine = { ShipmentRequestObj.ShipperAddressLine };

                shipperAddress.AddressLine = addressLine;
                shipperAddress.City = ShipmentRequestObj.ShipperCity;
                shipperAddress.PostalCode = ShipmentRequestObj.ShipperPostalCode;
                shipperAddress.StateProvinceCode = ShipmentRequestObj.ShipperStateProvinceCode;
                shipperAddress.CountryCode = ShipmentRequestObj.ShipperCountryCode;
                shipperAddress.AddressLine = addressLine;
                shipper.Address = shipperAddress;
                shipper.Name = ShipmentRequestObj.ShipperName;
                shipper.AttentionName = ShipmentRequestObj.ShipperAttentionName;
                ShipPhoneType shipperPhone = new ShipPhoneType();
                shipperPhone.Number = ShipmentRequestObj.ShipperPhone;
                shipper.Phone = shipperPhone;
                shipment.Shipper = shipper;
                ShipFromType shipFrom = new ShipFromType();
                ShipAddressType shipFromAddress = new ShipAddressType();
                String[] shipFromAddressLine = { ShipmentRequestObj.ShipFromAddressLine };
                shipFromAddress.AddressLine = addressLine;
                shipFromAddress.City = ShipmentRequestObj.ShipFromCity;
                shipFromAddress.PostalCode = ShipmentRequestObj.ShipFromPostalCode;
                shipFromAddress.StateProvinceCode = ShipmentRequestObj.ShipFromStateProvinceCode;
                shipFromAddress.CountryCode = ShipmentRequestObj.ShipFromCountryCode;
                shipFrom.Address = shipFromAddress;
                shipFrom.AttentionName = ShipmentRequestObj.ShipFromAttentionName;
                shipFrom.Name = ShipmentRequestObj.ShipFromName;
                shipment.ShipFrom = shipFrom;
                ShipToType shipTo = new ShipToType();
                ShipToAddressType shipToAddress = new ShipToAddressType();
                //String[] addressLine1 = { ShipmentRequestObj.Address2, ShipmentRequestObj.ShipToAddressLine }; // ShipmentRequestObj.ShipToAddressLine 
               
                String[] ToaddressLineBoth = new String[2];
                String[] ToaddressLine = new String[1];
                bool IsBothAddress = false;
                if (ShipmentRequestObj.Address2 != null)
                {
                    ToaddressLineBoth[0] = ShipmentRequestObj.Address2;
                    ToaddressLineBoth[1] = ShipmentRequestObj.ShipToAddressLine;
                    IsBothAddress = true;
                }
                else
                {
                    ToaddressLine[0] = ShipmentRequestObj.ShipToAddressLine;
                   
                }
                if(IsBothAddress==true)
                {
                    shipToAddress.AddressLine = ToaddressLineBoth;

                }
                else
                {
                    shipToAddress.AddressLine = ToaddressLine;
                }

                //shipToAddress.AddressLine = addressLine1;
                shipToAddress.City = ShipmentRequestObj.ShipToCity;
                shipToAddress.PostalCode = ShipmentRequestObj.ShipToPostalCode;
                shipToAddress.CountryCode = ShipmentRequestObj.ShipToCountryCode;
                shipToAddress.StateProvinceCode = ShipmentRequestObj.ShipToStateProvinceCode;
                shipTo.Address = shipToAddress;
                shipTo.AttentionName = ShipmentRequestObj.ShipToAttentionName;
                shipTo.Name = ShipmentRequestObj.ShipToName;
                ShipPhoneType shipToPhone = new ShipPhoneType();
                shipToPhone.Number = ShipmentRequestObj.ShipToPhone;
                shipTo.Phone = shipToPhone;
                shipment.ShipTo = shipTo;
                ServiceType service = new ServiceType();
                service.Code = ShipmentRequestObj.ServiceCode;
                shipment.Service = service;

                ShipmentTypeShipmentServiceOptions shpServiceOptions = new ShipmentTypeShipmentServiceOptions();

                /** **** International Forms ***** */
                InternationalFormType internationalForms = new InternationalFormType();

                /** **** Commercial Invoice ***** */
                String[] formTypeList = { "01" };
                internationalForms.FormType = formTypeList;

                /** **** Contacts and Sold To ***** */
                ContactType contacts = new ContactType();

                SoldToType soldTo = new SoldToType();
                soldTo.Option = "1";
                soldTo.AttentionName = ShipmentRequestObj.SoldAttentionName;
                soldTo.Name = ShipmentRequestObj.SoldName;
                PhoneType soldToPhone = new PhoneType();
                soldToPhone.Number = ShipmentRequestObj.soldPhoneNumber;
                soldToPhone.Extension = ShipmentRequestObj.SoldExtension;
                soldTo.Phone = soldToPhone;
                AddressType soldToAddress = new AddressType();
                String[] soldToAddressLine = { ShipmentRequestObj.SoldAddressLine };
                soldToAddress.AddressLine = soldToAddressLine;
                soldToAddress.City = ShipmentRequestObj.SoldCity;
                soldToAddress.PostalCode = ShipmentRequestObj.SoldPostalCode;
                soldToAddress.CountryCode = ShipmentRequestObj.SoldCountryCode;
                soldTo.Address = soldToAddress;
                contacts.SoldTo = soldTo;

                internationalForms.Contacts = contacts;

                /** **** Product ***** */
                ProductType product1 = new ProductType();
                String[] description = { ShipmentRequestObj.ProductDescription };
                product1.Description = description;
                product1.CommodityCode = ShipmentRequestObj.CommodityCode;
                product1.OriginCountryCode = ShipmentRequestObj.OriginCountryCode;
                UnitType unit = new UnitType();
                unit.Number = ShipmentRequestObj.UnitNumber;
                unit.Value = ShipmentRequestObj.UnitValue;
                UnitOfMeasurementType uomProduct = new UnitOfMeasurementType();
                uomProduct.Code = ShipmentRequestObj.UOMProductCode;
                uomProduct.Description = ShipmentRequestObj.ProductDescription;
                unit.UnitOfMeasurement = uomProduct;
                product1.Unit = unit;
                ProductWeightType productWeight = new ProductWeightType();
                productWeight.Weight = ShipmentRequestObj.ProductWeight;

                UnitOfMeasurementType uomForWeight = new UnitOfMeasurementType();
                uomForWeight.Code = ShipmentRequestObj.UOMForWeightCode;
                uomForWeight.Description = ShipmentRequestObj.UOMForWeightDescription;
                productWeight.UnitOfMeasurement = uomForWeight;
                product1.ProductWeight = productWeight;
                ProductType[] productList = { product1 };
                internationalForms.Product = productList;

                /** **** InvoiceNumber, InvoiceDate, PurchaseOrderNumber, TermsOfShipment, ReasonForExport, Comments and DeclarationStatement  ***** */
                internationalForms.InvoiceNumber = ShipmentRequestObj.InvoiceNumber;
                internationalForms.InvoiceDate = ShipmentRequestObj.InvoiceDate;
                internationalForms.PurchaseOrderNumber = ShipmentRequestObj.PurchaseOrderNumber;
                internationalForms.TermsOfShipment = ShipmentRequestObj.TermsOfShipment;
                internationalForms.ReasonForExport = ShipmentRequestObj.ReasonForExport;
                internationalForms.Comments = ShipmentRequestObj.Comments;
                internationalForms.DeclarationStatement = ShipmentRequestObj.DeclarationStatement;

                /** **** Discount, FreightCharges, InsuranceCharges, OtherCharges and CurrencyCode  ***** */
                IFChargesType discount = new IFChargesType();
                discount.MonetaryValue = ShipmentRequestObj.DiscountMonetaryValue;
                internationalForms.Discount = discount;
                IFChargesType freight = new IFChargesType();
                freight.MonetaryValue = ShipmentRequestObj.FreightMonetaryValue;
                internationalForms.FreightCharges = freight;
                IFChargesType insurance = new IFChargesType();
                insurance.MonetaryValue = ShipmentRequestObj.InsuranceMonetaryValue;
                internationalForms.InsuranceCharges = insurance;
                OtherChargesType otherCharges = new OtherChargesType();
                otherCharges.MonetaryValue = ShipmentRequestObj.OtherChargeMonetaryValue;
                otherCharges.Description = ShipmentRequestObj.OtherChargeDescription;
                internationalForms.OtherCharges = otherCharges;
                internationalForms.CurrencyCode = ShipmentRequestObj.CurrencyCode;


                shpServiceOptions.InternationalForms = internationalForms;
                shipment.ShipmentServiceOptions = shpServiceOptions;

                PackageType package = new PackageType();
                PackageWeightType packageWeight = new PackageWeightType();
                packageWeight.Weight = ShipmentRequestObj.ProductWeight;
                ShipUnitOfMeasurementType uom = new ShipUnitOfMeasurementType();
                uom.Code = "LBS";
                packageWeight.UnitOfMeasurement = uom;
                package.PackageWeight = packageWeight;
                PackagingType packType = new PackagingType();
                packType.Code = "02";
                package.Packaging = packType;
                PackageType[] pkgArray = { package };
                shipment.Package = pkgArray;
                LabelSpecificationType labelSpec = new LabelSpecificationType();
                LabelStockSizeType labelStockSize = new LabelStockSizeType();
                labelStockSize.Height = "7";
                labelStockSize.Width = "21";

                labelSpec.LabelStockSize = labelStockSize;
                LabelImageFormatType labelImageFormat = new LabelImageFormatType();
                labelImageFormat.Code = "GIF";
                labelSpec.LabelImageFormat = labelImageFormat;
                shipmentRequest.LabelSpecification = labelSpec;
                shipmentRequest.Shipment = shipment;
                // Console.WriteLine(shipmentRequest);

                System.Net.ServicePointManager.CertificatePolicy = new Revive.Redux.UI.WebApi.UPS.TrustAllCertificatePolicy();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                //var json = new JavaScriptSerializer().Serialize(shipmentRequest);

                ShipmentResponse shipmentResponse = shpSvc.ProcessShipment(shipmentRequest);



                ShipmentResponseObj.ShipmentIdentificationNumber = shipmentResponse.ShipmentResults.ShipmentIdentificationNumber;
                ShipmentResponseObj.LabelStreamData = shipmentResponse.ShipmentResults.PackageResults[0].ShippingLabel.GraphicImage.ToString();
                //ShipmentResponseObj.LabelStreamData = shipmentResponse.ShipmentResults.PackageResults[0].ShippingLabel.GraphicImage.ToString();


                ShipmentResponseObj.ResponseResult = "OK";
            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {

                ShipmentResponseObj.ResponseResult = ex.Detail.LastChild.InnerText;
                //Console.WriteLine("");
                //Console.WriteLine("---------Ship Web Service returns error----------------");
                //Console.WriteLine("---------\"Hard\" is user error \"Transient\" is system error----------------");
                //Console.WriteLine("SoapException Message= " + ex.Message);
                //Console.WriteLine("");
                //Console.WriteLine("SoapException Category:Code:Message= " + ex.Detail.LastChild.InnerText);
                //Console.WriteLine("");
                //Console.WriteLine("SoapException XML String for all= " + ex.Detail.LastChild.OuterXml);
                //Console.WriteLine("");
                //Console.WriteLine("SoapException StackTrace= " + ex.StackTrace);
                //Console.WriteLine("-------------------------");
                //Console.WriteLine("");
            }
            catch (System.ServiceModel.CommunicationException ex)
            {
                //Console.WriteLine("");
                //Console.WriteLine("--------------------");
                //Console.WriteLine("CommunicationException= " + ex.Message);
                //Console.WriteLine("CommunicationException-StackTrace= " + ex.StackTrace);
                //Console.WriteLine("-------------------------");
                //Console.WriteLine("");
                ShipmentResponseObj.ResponseResult = ex.Message.ToString();


            }
            catch (Exception ex)
            {
                //Console.WriteLine("");
                //Console.WriteLine("-------------------------");
                //Console.WriteLine(" General Exception= " + ex.Message);
                //Console.WriteLine(" General Exception-StackTrace= " + ex.StackTrace);
                //Console.WriteLine("-------------------------");
                ShipmentResponseObj.ResponseResult = ex.Message.ToString();

            }
            finally
            {
                string ss = "";
            }

            return ShipmentResponseObj;

        }

    }

}