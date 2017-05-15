using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EncryptionTester.BusObj.Sym
{
    public class RijndaelManaged : SymBase
    {
        internal RijndaelManaged() { }
        protected override SymmetricAlgorithm GetCryptProvider(string thePassword)
        {
            //Dim theAlgo As RijndaelManaged = New RijndaelManaged            
            SymmetricAlgorithm theAlgo = new System.Security.Cryptography.RijndaelManaged();
            BuildCryptProvider(theAlgo, thePassword);
            return theAlgo;
        }
    }
}
