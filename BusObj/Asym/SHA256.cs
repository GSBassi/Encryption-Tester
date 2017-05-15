using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EncryptionTester.BusObj.Asym
{
    public class SHA256 : AsymBase
    {
        internal SHA256()
        {
            hash = new SHA256Managed();
        }
    }
}
