using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revive.Redux.Entities
{
    public class MachineFaultModel
    {
      
        private string _processFault;
        private string _equipmentFault;
        private string _boardFault;
        public string processFaults
        {
            get
            {

               
                return _processFault;
            }
            set
            {
                _processFault = value;
                string binaryErroFormat = "";
                if (_processFault!=null )
                {
                    int errorNum = Convert.ToInt32(_processFault);
                    binaryErroFormat = GetIntBinaryString(errorNum);
                }
                _processFault = binaryErroFormat;
            }
        }
        public string equipmentFaults
        {
            get
            {


                return _equipmentFault;
            }
            set
            {
                _equipmentFault = value;
                string binaryErroFormat = "";
                if (_equipmentFault != null)
                {
                    int errorNum = Convert.ToInt32(_equipmentFault);
                    binaryErroFormat = GetIntBinaryString(errorNum);
                }
                _equipmentFault = binaryErroFormat;
            }
        }
        public string boardFaults
        {
            get
            {


                return _boardFault;
            }
            set
            {
                _boardFault = value;
                string binaryErroFormat = "";
                if (_boardFault != null)
                {
                    int errorNum = Convert.ToInt32(_boardFault);
                    binaryErroFormat = GetIntBinaryString(errorNum);
                }
                _boardFault = binaryErroFormat;
            }
        }




      // Convert Integer to Binary format;
        private  string GetIntBinaryString(int n)
        {
            char[] b = new char[16];
            int pos = 15;
            int i = 0;

            while (i < 16)
            {
                if ((n & (1 << i)) != 0)
                {
                    b[pos] = '1';
                }
                else
                {
                    b[pos] = '0';
                }
                pos--;
                i++;
            }
            return new string(b);
        }
        
    }


}
