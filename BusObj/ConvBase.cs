using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptionTester.BusObj
{
    public abstract class ConvBase
    {

        public byte[] ConvertFileToBytes(string theFilePath)
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

        public void ConvertBytesToFile(byte[] theBytes, string theFilePath)
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
