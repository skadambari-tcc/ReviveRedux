using Revive.Redux.Entities;
using Revive.Redux.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Services
{
    public class CustomerManagement : ICustomerManagement
    {
        ICustomerManagementRespository _ICustomerManagement = null;
        
        public CustomerManagement()
        {
            _ICustomerManagement = new CustomerManagementRespository();

        }

        public IEnumerable<ManageCustomersModel> GetCustomersList()
        {
            return _ICustomerManagement.GetCustomersList();
        }

        public  IEnumerable<ManageCustomersModel> GetCustomersList(Guid AccountManagerId)
        {
            return _ICustomerManagement.GetCustomersList(AccountManagerId);
        }
        public IEnumerable<ManageCustomersModel> GetCustomersList(int customerId)
        {
            return _ICustomerManagement.GetCustomersList(customerId);
        }
        public string FilePathUpload(List<CustomerDocumentsModel> Object)
        {
            return _ICustomerManagement.FilePathUpload(Object); 
        }
        public ManageCustomersModel CreateCustomer(ManageCustomersModel CustomerRecord)
        {
            return _ICustomerManagement.CreateCustomer(CustomerRecord);
        }
        public ManageCustomersModel GetCustomerDetailsById(int CustID)
        {
            return _ICustomerManagement.GetCustomerDetailsById(CustID);
        }   
       
        public IEnumerable<CustomerDocumentsModel> GetCustomerDocsList(int CustID)
        {
            return _ICustomerManagement.GetCustomerDocsList(CustID);
        }
        public CustomerDocumentsModel DeleteCustomerDocumentById(CustomerDocumentsModel Obj)
        {
            return _ICustomerManagement.DeleteCustomerDocumentById(Obj);
        }
        public CustomerDocumentsModel GetCustomerDocumentDetailById(CustomerDocumentsModel Obj)
        {
            return _ICustomerManagement.GetCustomerDocumentDetailById(Obj);
        }

        public IEnumerable<MembershipConfigModel> GetCustomerMembership(int nCustomerId)
        {
            return _ICustomerManagement.GetCustomerMembership(nCustomerId);
        }

        public bool CheckCustomerDocExists(int id)
        {
            return _ICustomerManagement.CheckCustomerDocExists(id);
        }

        public bool UpdateCustomerMembership(MembershipConfigModel objCustomerMembership)
        {
            return _ICustomerManagement.UpdateCustomerMembership(objCustomerMembership);
        }
        public Boolean CheckCheckCustomerNameExists(String Name,int Customer_ID)
        {
            return _ICustomerManagement.CheckCheckCustomerNameExists(Name,Customer_ID);
        }
    }
}
