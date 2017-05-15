using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EncryptionTester.BusObj.Asym
{
    public class SHA1Crypto: AsymBase
    {
        internal SHA1Crypto()
        {
            hash = new SHA1CryptoServiceProvider();
        }
    }
}
