using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EncryptionTester.BusObj.Asym
{
    public class SHA512 : AsymBase
    {
        internal SHA512()
        {
            hash = new SHA512Managed();
        }
    }
}
