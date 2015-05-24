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

        #region "Symmetric"

        internal enum SymType
        {
            RC2,
            DES,
            TripleDES,
            RijndaelManaged
        }

        internal static ReadOnlyCollection<string> GetSymmetricTypes()
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
        public static string Encrypt(string uncoded, string password, SymType encryptType)
        {
            byte[] plainTextByteArray = System.Text.Encoding.Unicode.GetBytes(uncoded);

            return Convert.ToBase64String(Encrypt(plainTextByteArray, password, encryptType));
        }

        public static byte[] Encrypt(byte[] plainTextByteArray, string password, SymType encryptType)
        {
            byte[] theOutput = null;

            using (ICryptoTransform enCryp = GetCryptProvider(password, encryptType).CreateEncryptor())
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

        private static SymmetricAlgorithm GetCryptProvider(string thePassword, SymType encryptType)
        {
            //Dim theAlgo As RijndaelManaged = New RijndaelManaged            
            SymmetricAlgorithm theAlgo = null;
            switch (encryptType)
            {
                case SymType.TripleDES:
                    theAlgo = new TripleDESCryptoServiceProvider();
                    break;
                case SymType.RijndaelManaged:
                    theAlgo = new RijndaelManaged();
                    break;
                case SymType.RC2:
                    theAlgo = new RC2CryptoServiceProvider();
                    break;
                case SymType.DES:
                    theAlgo = new DESCryptoServiceProvider();
                    break;
            }
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
            return theAlgo;
        }



        #endregion


        #region "ASymmetric"
        internal enum HashType
        {
            SHA1,
            SHA256,
            SHA384,
            SHA512,
            SHA1Crypto
        }

        internal static ReadOnlyCollection<string> GetASymmetricTypes()
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
            // Convert plain text into a byte array.
            plainTextBytes = System.Text.Encoding.Unicode.GetBytes(plainText);
            // Convert result into a base64-encoded string.
            return Convert.ToBase64String(ComputeHash(plainTextBytes, theHashAlgorithm));
        }

        internal static byte[] ComputeHash(byte[] plainTextBytes, HashType theHashAlgorithm)
        {
            HashAlgorithm hash = null;
            byte[] hashBytes = null;
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
            }
            finally
            {
                if (hash != null)
                {
                    hash.Dispose();
                }
            }
            return hashBytes;
        }

        #endregion


        public static byte[] ConvertFileToBytes(string theFilePath)
        {
            byte[] theBytes = null;
            if (System.IO.File.Exists(theFilePath))
            {
                using (System.IO.FileStream fs = new System.IO.FileStream(theFilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read))
                {
                    using (System.IO.BinaryReader br = new System.IO.BinaryReader(fs))
                    {
                        System.IO.FileInfo fInfo = new System.IO.FileInfo(theFilePath);
                        theBytes = br.ReadBytes(Convert.ToInt32(fInfo.Length));
                    }
                }
            }
            return theBytes;
        }

        public static void ConvertBytesToFile(byte[] theBytes, string theFilePath)
        {
            string theDir = System.IO.Path.GetDirectoryName(theFilePath);
            //in case the folder doesn't exist
            using (System.IO.FileStream fs = new System.IO.FileStream(theFilePath, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                using (System.IO.BinaryWriter bw = new System.IO.BinaryWriter(fs, System.Text.Encoding.Unicode))
                {
                    bw.Write(theBytes);
                }
            }
        }

    }
}
