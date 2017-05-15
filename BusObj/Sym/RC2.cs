using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EncryptionTester.BusObj.Sym
{
    public class RC2 : SymBase
    {
        internal RC2() { }
        protected override SymmetricAlgorithm GetCryptProvider(string thePassword)
        {
            //Dim theAlgo As RijndaelManaged = New RijndaelManaged            
            SymmetricAlgorithm theAlgo = new RC2CryptoServiceProvider();
            BuildCryptProvider(theAlgo, thePassword);
            return theAlgo;
        }
    }
}
