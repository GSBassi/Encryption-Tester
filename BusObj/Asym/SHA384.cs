using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EncryptionTester.BusObj.Asym
{
    public class SHA384 : AsymBase
    {
        internal SHA384()
        {
            hash = new SHA384Managed();
        }
    }
}
