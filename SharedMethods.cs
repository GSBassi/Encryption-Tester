using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EncryptionTester
{
    internal class SharedMethods
    {

        #region "Asymmetric"

        internal enum AsymType
        {
            TripleDES
        }

        internal static ReadOnlyCollection<string> GetASymmetricTypes()
        {
            List<string> theReturn = new List<string>();
            foreach (AsymType s in Enum.GetValues(typeof(AsymType)))
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
        public static string Encrypt(string uncoded, string password)
        {
            byte[] plainTextByteArray = System.Text.Encoding.Unicode.GetBytes(uncoded);
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

            return Convert.ToBase64String(theOutput);
        }

        private static TripleDESCryptoServiceProvider GetCryptProvider(string thePassword)
        {
            //Dim theAlgo As RijndaelManaged = New RijndaelManaged            
            TripleDESCryptoServiceProvider theAlgo = new TripleDESCryptoServiceProvider();
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
            return theAlgo;
        }



        #endregion


        #region "Symmetric"
        internal enum HashType
        {
            SHA1,
            SHA256,
            SHA384,
            SHA512,
            SHA1Crypto
        }

        internal static ReadOnlyCollection<string> GetSymmetricTypes()
        {
            List<string> theReturn = new List<string>();
            foreach (HashType s in Enum.GetValues(typeof(HashType)))
            {
                theReturn.Add(s.ToString());
            }
            return new ReadOnlyCollection<string>(theReturn);
        }

        /// <summary>
        ///  Generates a hash for the given plain text value and returns a 
        /// base64-encoded result. 
        /// </summary>
        /// <param name="plainText">Plaintext value to be hashed. The function does not check 
        /// whether this parameter is null.</param>
        /// <param name="theHashAlgorithm">  Name of the hash algorithm. Allowed values are: 
        /// "MD5", "SHA1", "SHA256", "SHA384", and "SHA512" (if any other value is specified 
        /// MD5 hashing algorithm will be used). This value is case-insensitive.</param>
        /// <returns>Hash value formatted as a base64-encoded string.</returns>
        /// <remarks></remarks>
        internal static string ComputeHash(string plainText, HashType theHashAlgorithm)
        {
            // If salt is not specified, generate it on the fly.
            byte[] plainTextBytes = null;
            HashAlgorithm hash = null;
            byte[] hashBytes = null;
            string hashValue = null;

            // Convert plain text into a byte array.
            plainTextBytes = System.Text.Encoding.Unicode.GetBytes(plainText);
            try
            {
                // Initialize appropriate hashing algorithm class.
                switch (theHashAlgorithm)
                {
                    case HashType.SHA1:
                        hash = new SHA1Managed();
                        break;
                    case HashType.SHA256:
                        hash = new SHA256Managed();
                        break;
                    case HashType.SHA384:
                        hash = new SHA384Managed();
                        break;
                    case HashType.SHA512:
                        hash = new SHA512Managed();
                        break;
                    case HashType.SHA1Crypto:
                        //the ONLY FIPS compliant one here
                        hash = new SHA1CryptoServiceProvider();
                        break;
                    default:
                        hash = new MD5CryptoServiceProvider();
                        break;
                }

                // Compute hash value of our plain text with appended salt.
                hashBytes = hash.ComputeHash(plainTextBytes);

                // Convert result into a base64-encoded string.
                hashValue = Convert.ToBase64String(hashBytes);
            }
            finally
            {
                if (hash != null)
                {
                    hash.Dispose();
                }
            }
            // Return the result.
            return hashValue;
        }
        #endregion

    }
}
