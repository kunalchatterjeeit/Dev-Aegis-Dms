using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public static class GeneralSecurity
    {
        public static DataTable LogOn(string userName)
        {
            return DataLayer.Auth.Login(userName);
        }

        public static int Login_Save(Entity.Auth auth)
        {
            return DataLayer.Auth.Login_Save(auth);
        }

        public static string Permission_ByRoleId(int userId)
        {
            return DataLayer.Auth.Permission_ByRoleId(userId);
        }

        public static string ToEncrypt(this string toEncrypt, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            // Get the key from config file
            string key = (string)(new AppSettingsReader()).GetValue("HashKey", typeof(String));

            //If hashing use get hashcode regards to your key
            if (useHashing)
            {
                using (MD5CryptoServiceProvider hash = new MD5CryptoServiceProvider())
                {
                    keyArray = hash.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    //Always release the resources and flush data
                    // of the Cryptographic service provide. Best Practice

                    hash.Clear();
                }
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            using (TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider())
            {
                //set the secret key for the tripleDES algorithm
                tdes.Key = keyArray;
                //mode of operation. there are other 4 modes.
                //We choose ECB(Electronic code Book)
                tdes.Mode = CipherMode.ECB;
                //padding mode(if any extra byte added)

                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateEncryptor();
                //transform the specified region of bytes array to resultArray
                byte[] resultArray =
                  cTransform.TransformFinalBlock(toEncryptArray, 0,
                  toEncryptArray.Length);
                //Release resources held by TripleDes Encryptor
                tdes.Clear();
                //Return the encrypted data into unreadable string format
                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
        }

        public static string ToDecrypt(this string cipherCode, bool useHashing)
        {
            byte[] keyArray;
            //get the byte code of the string
            cipherCode = cipherCode.Replace(" ", "+");
            byte[] toEncryptArray = Convert.FromBase64String(cipherCode);

            //Get your key from config file to open the lock!
            string key = (string)(new AppSettingsReader()).GetValue("HashKey", typeof(String));

            if (useHashing)
            {
                //if hashing was used get the hash code with regards to your key
                using (MD5CryptoServiceProvider hash = new MD5CryptoServiceProvider())
                {
                    keyArray = hash.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    //release any resource held by the MD5CryptoServiceProvider

                    hash.Clear();
                }
            }
            else
            {
                //if hashing was not implemented get the byte code of the key
                keyArray = UTF8Encoding.UTF8.GetBytes(key);
            }

            using (TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider())
            {
                //set the secret key for the tripleDES algorithm
                tdes.Key = keyArray;
                //mode of operation. there are other 4 modes. 
                //We choose ECB(Electronic code Book)

                tdes.Mode = CipherMode.ECB;
                //padding mode(if any extra byte added)
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(
                                     toEncryptArray, 0, toEncryptArray.Length);
                //Release resources held by TripleDes Encryptor                
                tdes.Clear();
                //return the Clear decrypted TEXT

                return UTF8Encoding.UTF8.GetString(resultArray);
            }
        }

        public static void EncryptFile(string decryptedOriginalFileNameWithPath, string encryptPhysicalFileNameWithPath)
        {
            string EncryptionKey = (string)(new AppSettingsReader()).GetValue("FileHashKey", typeof(String));
            using (Aes encryptor = Aes.Create())
            {
                using (Rfc2898DeriveBytes pdb =
                    new Rfc2898DeriveBytes(EncryptionKey,
                        new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 }))
                {
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                }
                using (FileStream fsOutput = new FileStream(encryptPhysicalFileNameWithPath, FileMode.Create))
                {
                    using (CryptoStream cs = new CryptoStream(fsOutput, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        using (FileStream fsInput = new FileStream(decryptedOriginalFileNameWithPath, FileMode.Open))
                        {
                            int data;
                            while ((data = fsInput.ReadByte()) != -1)
                            {
                                cs.WriteByte((byte)data);
                            }
                        }
                    }
                }
            }
        }

        public static void DecryptFile(string encryptedPhysicalFileNameWithPath, string decryptOriginalFileNameWithPath)
        {
            try
            {
                string EncryptionKey = (string)(new AppSettingsReader()).GetValue("FileHashKey", typeof(String));
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (FileStream fsInput = new FileStream(encryptedPhysicalFileNameWithPath, FileMode.Open))
                    {
                        using (CryptoStream cs = new CryptoStream(fsInput, encryptor.CreateDecryptor(), CryptoStreamMode.Read))
                        {
                            using (FileStream fsOutput = new FileStream(decryptOriginalFileNameWithPath, FileMode.Create))
                            {
                                int data;
                                while ((data = cs.ReadByte()) != -1)
                                {
                                    fsOutput.WriteByte((byte)data);
                                }
                            }
                        }
                    }
                }
            }
            catch (CustomException ex)
            {
                ex.Log("GeneralSecurity", 0);
            }
        }
    }
}
