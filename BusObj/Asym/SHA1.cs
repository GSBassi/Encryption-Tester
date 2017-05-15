using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EncryptionTester.BusObj.Asym
{
    public class SHA1 : AsymBase
    {
        internal SHA1()
        {
            hash = new SHA1Managed();
        }
    }
}
