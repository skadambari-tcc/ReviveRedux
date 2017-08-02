using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Revive.Redux.UI.WebApi.UPS
{
    public class TrustAllCertificatePolicy : System.Net.ICertificatePolicy
    {
        public TrustAllCertificatePolicy()
        { }

        public bool CheckValidationResult(System.Net.ServicePoint sp,System.Security.Cryptography.X509Certificates.X509Certificate cert, WebRequest req, int problem)
        {
            return true;
        }
    }
}