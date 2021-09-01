using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

using Microsoft.Win32;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

namespace Shkila.LicenseManager
{
    public class LicenseManager
    {
        private static readonly string m_PasswordHash = "6274411";
        private static readonly string m_SaltKey = "Shki1@G27A41I";
        private static readonly string m_VIKey = "@1B2c3D4e5F6g7H8";
        private static LicenseData m_LicenseData = null;
        private static string m_eKey = String.Empty;
        private static string m_filePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "Shkila.lic");
        private static string m_keylogFilename = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "keylog.txt");

        public static string Create(LicenseData dt)
        {
            return Encrypt(dt.ToString());
        }

        public static void LicenseFile(LicenseData dt)
        {
            _LicenseFile(dt);
        }

        private static void _LicenseFile(LicenseData dt)
        {
            StreamWriter sw = new StreamWriter(m_filePath, false);
            sw.WriteLine(Encrypt(dt.ToString()));
            sw.Close();
        }

        private static string Encrypt(string plainText)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            byte[] keyBytes = new Rfc2898DeriveBytes(m_PasswordHash, Encoding.ASCII.GetBytes(m_SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(m_VIKey));

            byte[] cipherTextBytes;

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                    cryptoStream.Close();
                }
                memoryStream.Close();
            }
            return Convert.ToBase64String(cipherTextBytes);
        }

        private static string Decrypt(string encryptedText)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
            byte[] keyBytes = new Rfc2898DeriveBytes(m_PasswordHash, Encoding.ASCII.GetBytes(m_SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

            var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(m_VIKey));
            var memoryStream = new MemoryStream(cipherTextBytes);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
        }

        public static bool LicenseProcees(ref string error, bool log = false)
        {
            if (!IsVerified(ref error, log))
                return false;

            _Imprint(m_LicenseData,Registry.CurrentUser);
            return true;
        }

        public static bool IsVerified(ref string error, bool log)
        {
            return _IsVerified(ref error, log) && _CheckRegistry(ref error, log, Registry.CurrentUser);
        }

        private static bool _CheckRegistry(ref string error, bool log, RegistryKey root)
        {
            if (!m_LicenseData.ToRegistry)
                return true;

            RegistryKey key = root.OpenSubKey("Software", true);

            key = key.OpenSubKey("Shkila", true);
            if (key == null)
            {
                m_LicenseData.StartDate = DateTime.Now;
                return true;
            }

            key = key.OpenSubKey(m_LicenseData.ProductID.ToString(), true);
            if (key == null)
            {
                m_LicenseData.StartDate = DateTime.Now;
                return true;
            }

            object oKey = key.GetValue("key");
            if (oKey != null)
            {
                string regKey = oKey.ToString();
                if (!m_LicenseData.ToUpdate && regKey != m_eKey)
                {
                    if (log)
                    {
                        _WriteToLog("Info", regKey);
                        _WriteToLog("Info", m_eKey);
                    }

                    error = "Invalid License";
                    return false;
                }                
            }
            else
                m_LicenseData.StartDate = DateTime.Now;

            return true;
        }

        private static void _WriteToLog(string msgType, string msg)
        {
            StreamWriter sw = new StreamWriter(m_keylogFilename, true);
            sw.WriteLine(String.Format("{0} - {1} - {2}", DateTime.Now, msgType, msg));
            sw.Close();

        }
        private static bool _IsVerified(ref string error, bool compareMAC = true)
        {
            if (!File.Exists(m_filePath))
            {
                error = "Missing License File";
                return false;
            }

            StreamReader sr = new StreamReader(m_filePath);
            m_eKey = sr.ReadLine();
            sr.Close();
            if (String.IsNullOrEmpty(m_eKey))
            {
                error = "Missing License Key";
                return false;
            }

            string key = Decrypt(m_eKey);

            m_LicenseData = LicenseData.ToLicenseData(key);

            if (m_LicenseData == null)
            {
                error = "Invalid Data";
                return false; ;
            }

            if (m_LicenseData.LicenseType == LicenseType.Subscription)
            {
                if (DateTime.Now > m_LicenseData.StartDate.AddDays(m_LicenseData.Period) && !m_LicenseData.ToUpdate)
                {
                    error = "Timeout";
                    return false;
                }
            }

            if (m_LicenseData.ToUpdate == true)
                m_LicenseData.StartDate = DateTime.Now;

            if (compareMAC)
            {
                string firstMacAddress = _GetMacAddress();
                if (!String.IsNullOrEmpty(m_LicenseData.MacAddress) && !m_LicenseData.MacAddress.Equals(firstMacAddress))
                {
                    error = "Invalid Mac";
                    return false;
                }
            }

            return true;
        }

        private static void _Imprint(LicenseData dt, RegistryKey root)
        {
            if (!dt.ToRegistry)
            {
                if (dt.ToUpdate)
                {
                    dt.ToUpdate = false;
                    _LicenseFile(dt);
                }                
            }
            else
            {
                dt.MacAddress = _GetMacAddress();
                RegistryKey key = root.OpenSubKey("Software", true);

                key.CreateSubKey("Shkila");
                key = key.OpenSubKey("Shkila", true);

                key.CreateSubKey(dt.ProductID.ToString());
                key = key.OpenSubKey(dt.ProductID.ToString(), true);

                object oKey = key.GetValue("key");
                if (oKey == null)
                {
                    dt.ToUpdate = false;
                    _LicenseFile(dt);
                    key.SetValue("key", Encrypt(dt.ToString()));
                }
                else
                {
                    string regKey = oKey.ToString();
                    if (dt.ToUpdate)
                    {
                        LicenseData regLD = _GetLicenseDataFromKey("6274411", regKey);
                        dt.Period = dt.Period + Math.Abs((regLD.Period - Math.Max(0, (regLD.StartDate.AddDays(regLD.Period) - dt.StartDate).Days)));

                        dt.ToUpdate = false;
                        _LicenseFile(dt);
                        key.SetValue("key", Encrypt(dt.ToString()));
                    }
                }
            }
        }

        [DllImport("kernel32.dll")]
        private static extern long GetVolumeInformation(string PathName, StringBuilder VolumeNameBuffer, UInt32 VolumeNameSize, ref UInt32 VolumeSerialNumber, ref UInt32 MaximumComponentLength, ref UInt32 FileSystemFlags, StringBuilder FileSystemNameBuffer, UInt32 FileSystemNameSize);
        public static string GetVolumeSerial(string strDriveLetter)
        {
            uint serNum = 0;
            uint maxCompLen = 0;
            StringBuilder VolLabel = new StringBuilder(256); // Label
            UInt32 VolFlags = new UInt32();
            StringBuilder FSName = new StringBuilder(256); // File System Name
            strDriveLetter += ":\\"; // fix up the passed-in drive letter for the API call
            long Ret = GetVolumeInformation(strDriveLetter, VolLabel, (UInt32)VolLabel.Capacity, ref serNum, ref maxCompLen, ref VolFlags, FSName, (UInt32)FSName.Capacity);

            return Convert.ToString(serNum);
        }

        public static string GetMacAddress()
        {
            return _GetMacAddress();
        }

        private static string _GetMacAddress()
        {
            return GetVolumeSerial("C");
        }

        public static LicenseData GetLicenseDataFromFile(string password)
        {
            return _GetLicenseData(password, "File");
            
            
        }

        private static LicenseData _GetLicenseData(string password, string type)
        {
            if (password.Equals(m_PasswordHash))
            {
                switch(type)
                {
                    case "File":
                        string error = String.Empty;
                        if (_IsVerified(ref error,false))
                            return m_LicenseData;
                        return null;
                    case "Registry":
                        {
                            RegistryKey key = Registry.CurrentUser.OpenSubKey("Software", true);
                            key = key.OpenSubKey("Shkila", true);
                            if (key == null)
                                return null;
                            key = key.OpenSubKey(m_LicenseData.ProductID.ToString(), true);
                            if (key == null)
                                return null;
                            object oKey = key.GetValue("key");
                            if (oKey != null)
                                return LicenseData.ToLicenseData(Decrypt(oKey.ToString()));
                            return null;
                        }
                    case "Key":
                        {
                            string key = Decrypt(m_eKey);
                            m_LicenseData = LicenseData.ToLicenseData(key);
                            return m_LicenseData;
                        }
                    default: return null;
                }
            }
            return null;
        }

        public static LicenseData GetLicenseDataFromRegistry(string password)
        {

            return _GetLicenseData(password, "Registry");
            
        }

        public static LicenseData IsKeyOK(string key)
        {
            string keyD = Decrypt(key);

            m_LicenseData = LicenseData.ToLicenseData(keyD);

            return m_LicenseData;
        }

        public static void LicenseFileFromKey(string key)
        {
            _LicenseFileFromKey(key);
        }

        private static void _LicenseFileFromKey(string key)
        {
            _LicenseFile(LicenseManager.GetLicenseDataFromKey("6274411", key));
        }

        public static LicenseData GetLicenseDataFromKey(string password, string key)
        {
            return _GetLicenseDataFromKey(password, key);
        }

        private static LicenseData _GetLicenseDataFromKey(string password, string key)
        {
            m_eKey = key;
            return _GetLicenseData(password, "Key");
        }

        public static void WriteLocalMachineKey(string password, string key)
        {
            _WriteKey(password, key, Registry.LocalMachine);
        }

        public static void WriteCurrentUserKey(string password, string key)
        {
            _WriteKey(password, key, Registry.CurrentUser);
        }

        private static void _WriteKey(string password, string key, RegistryKey root)
        {
            if (password.Equals(m_PasswordHash))
            {
                LicenseData dt = _GetLicenseDataFromKey(password, key);
                _Imprint(dt, root);
            }
        }

        public static string ReadLocalMachineKey(string password)
        {
            return _ReadKey(password,Registry.LocalMachine);
        }

        public static string ReadCurrentUserKey(string password)
        {
            return _ReadKey(password, Registry.CurrentUser);
        }

        private static string _ReadKey(string password, RegistryKey root)
        {
            if (password.Equals(m_PasswordHash))
            {
                RegistryKey key = root.OpenSubKey("Software", true);

                key = key.OpenSubKey("Shkila", true);
                if (key == null)
                    return null;

                key = key.OpenSubKey(m_LicenseData.ProductID.ToString(), true);
                if (key == null)
                    return null;

                object oKey = key.GetValue("key");
                if (oKey == null) return null;

                return oKey.ToString();
            }
            else
                return null;

        }
    }

}
