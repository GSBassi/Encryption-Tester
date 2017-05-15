using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EncryptionTester.BusObj.Sym
{
    public abstract class SymBase : ConvBase
    {

        private enum SymType
        {
            RC2,
            DES,
            TripleDES,
            RijndaelManaged
        }
        

        public static SymBase GetInstance(string symType)
        {
            SymType aTyp = (SymType)Enum.Parse(typeof(SymType), symType);
            switch (aTyp)
            {
                case SymType.TripleDES:
                    return new TripleDES();
                case SymType.RijndaelManaged:
                    return new RijndaelManaged();
                case SymType.RC2:
                    return new RC2();
                case SymType.DES:
                    return new DES();
            }
            return null;
        }

        public static ReadOnlyCollection<string> GetSymmetricTypes()
        {
            List<string> theReturn = new List<string>();

            //IEnumerable<Type> twitypes = System.Reflection.Assembly.GetAssembly(typeof(SymmetricAlgorithm)).GetTypes().Where(type => type.IsSubclassOf(typeof(SymmetricAlgorithm))).ToList();
            //foreach (Type t in twitypes)
            //{
            //    theReturn.Add(t.Name);
            //}
            foreach (SymType s in Enum.GetValues(typeof(SymType)))
            {
                theReturn.Add(s.ToString());
            }
            return new ReadOnlyCollection<string>(theReturn);
        }

        /// <summary>
        /// This method encrypts a string.
        /// </summary>
        /// <param name="uncoded">the text to be encrypted</param>
        /// <param name="password">the private key in string format</param>
        /// <returns>note that it takes unicode text, and coverts it's encryption to base64</returns>
        /// <remarks></remarks>
        public string Encrypt(string uncoded, string password)
        {
            byte[] plainTextByteArray = System.Text.Encoding.Unicode.GetBytes(uncoded);

            return Convert.ToBase64String(Encrypt(plainTextByteArray, password));
        }

        public byte[] Encrypt(byte[] plainTextByteArray, string password)
        {
            byte[] theOutput = null;

            using (ICryptoTransform enCryp = GetCryptProvider(password).CreateEncryptor())
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cryptStrm = new CryptoStream(ms, enCryp, CryptoStreamMode.Write))
                    {
                        cryptStrm.Write(plainTextByteArray, 0, plainTextByteArray.Length);
                        cryptStrm.FlushFinalBlock();
                    }
                    theOutput = ms.ToArray();
                }
            }

            return theOutput;
        }

        protected abstract SymmetricAlgorithm GetCryptProvider(string thePassword);

        protected void BuildCryptProvider(SymmetricAlgorithm theAlgo, string thePassword)
        {
            if (theAlgo != null)
            {
                byte[] salt1 = new byte[8];
                using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
                {
                    // Fill the array with a random value.
                    rngCsp.GetBytes(salt1);
                }
                using (Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(thePassword, salt1))
                {
                    theAlgo.Key = key.GetBytes(Convert.ToInt32(theAlgo.KeySize / 8));
                    theAlgo.IV = key.GetBytes(Convert.ToInt32(theAlgo.BlockSize / 8));
                }
            }
        }

    }
}
