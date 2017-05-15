using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EncryptionTester.BusObj.Asym
{
    public abstract class AsymBase : ConvBase, IDisposable
    {
        private enum HashType
        {
            SHA1,
            SHA256,
            SHA384,
            SHA512,
            SHA1Crypto
        }

        public static AsymBase GetInstance(string asymType)
        {
            HashType aTyp = (HashType)Enum.Parse(typeof(HashType), asymType);
            switch (aTyp)
            {
                case HashType.SHA1:
                    return new SHA1();
                case HashType.SHA256:
                    return new SHA256();
                case HashType.SHA384:
                    return new SHA384();
                case HashType.SHA512:
                    return new SHA512();
                case HashType.SHA1Crypto:
                    return new SHA1Crypto();
            }
            return null;
        }

        protected HashAlgorithm hash;
        public static ReadOnlyCollection<string> GetASymmetricTypes()
        {
            List<string> theReturn = new List<string>();
            //IEnumerable<Type> twitypes = System.Reflection.Assembly.GetAssembly(typeof(AsymmetricAlgorithm)).GetTypes().Where(type => type.IsSubclassOf(typeof(AsymmetricAlgorithm))).ToList();
            //foreach (Type t in twitypes)
            //{
            //    theReturn.Add(t.Name);
            //}
            foreach (HashType s in Enum.GetValues(typeof(HashType)))
            {
                theReturn.Add(s.ToString());
            }
            return new ReadOnlyCollection<string>(theReturn);
        }

        public string ComputeHash(string plainText)
        {
            // If salt is not specified, generate it on the fly.
            byte[] plainTextBytes = null;
            // Convert plain text into a byte array.
            plainTextBytes = System.Text.Encoding.Unicode.GetBytes(plainText);
            // Convert result into a base64-encoded string.
            return Convert.ToBase64String(ComputeHash(plainTextBytes));
        }

        public byte[] ComputeHash(byte[] plainTextBytes)
        {
            byte[] hashBytes = null;
            // Compute hash value of our plain text with appended salt.
            hashBytes = hash.ComputeHash(plainTextBytes);
            return hashBytes;
        }


        public void Dispose()
        {
            if (hash != null)
            {
                hash.Dispose();
            }
        }

    }
}
