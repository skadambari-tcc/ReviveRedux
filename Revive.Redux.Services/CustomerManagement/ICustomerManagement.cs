using Revive.Redux.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Services
{
    public interface ICustomerManagement
    {

        IEnumerable<ManageCustomersModel> GetCustomersList();
        IEnumerable<ManageCustomersModel> GetCustomersList(Guid AccountManagerId);
        IEnumerable<ManageCustomersModel> GetCustomersList(int customerId);
        string FilePathUpload(List<CustomerDocumentsModel> Object);
        ManageCustomersModel CreateCustomer(ManageCustomersModel CustomerRecord);
        ManageCustomersModel GetCustomerDetailsById(int CustID);
        IEnumerable<CustomerDocumentsModel> GetCustomerDocsList(int CustID);
       
        //bool DeleteCustomerDocumentById(CustomerDocumentsModel Obj);
        IEnumerable<MembershipConfigModel> GetCustomerMembership(int nCustomerId);
        bool UpdateCustomerMembership(MembershipConfigModel objCustomerMembership);
        CustomerDocumentsModel DeleteCustomerDocumentById(CustomerDocumentsModel Obj);
        CustomerDocumentsModel GetCustomerDocumentDetailById(CustomerDocumentsModel Obj);
         bool CheckCustomerDocExists(int id);
         Boolean CheckCheckCustomerNameExists(String Name, int Customer_ID);
    }
}
