using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace LibraryManagementSystem.WebUI.Utilities.Security {
    public class Cryptography {
        private readonly ServiceProviderEnum mAlgorithm;
        private readonly SymmetricAlgorithm mCryptoService;

        public static string CryptoKeyValue => "410a5552f4074e8ab66294258651accb";

        public static string CryptoVector => "9f8df8f8454b49ab9f4a94751111aad9";

        private void SetLegalIV() {
            if (this.mAlgorithm == ServiceProviderEnum.Rijndael)
                this.mCryptoService.IV = new byte[16]
                {
          (byte) 15,
          (byte) 111,
          (byte) 19,
          (byte) 46,
          (byte) 53,
          (byte) 194,
          (byte) 205,
          (byte) 249,
          (byte) 5,
          (byte) 70,
          (byte) 156,
          (byte) 234,
          (byte) 168,
          (byte) 75,
          (byte) 115,
          (byte) 204
                };
            else
                this.mCryptoService.IV = new byte[8]
                {
          (byte) 15,
          (byte) 111,
          (byte) 19,
          (byte) 46,
          (byte) 53,
          (byte) 194,
          (byte) 205,
          (byte) 249
                };
        }

        public Cryptography() {
            RijndaelManaged rijndaelManaged = new RijndaelManaged();
            rijndaelManaged.Mode = CipherMode.CBC;
            this.mCryptoService = (SymmetricAlgorithm)rijndaelManaged;
            this.mAlgorithm = ServiceProviderEnum.Rijndael;
        }

        public Cryptography(ServiceProviderEnum serviceProvider) {
            switch (serviceProvider) {
                case ServiceProviderEnum.Rijndael:
                    this.mCryptoService = (SymmetricAlgorithm)new RijndaelManaged();
                    this.mAlgorithm = ServiceProviderEnum.Rijndael;
                    break;
                case ServiceProviderEnum.RC2:
                    this.mCryptoService = (SymmetricAlgorithm)new RC2CryptoServiceProvider();
                    this.mAlgorithm = ServiceProviderEnum.RC2;
                    break;
                case ServiceProviderEnum.DES:
                    this.mCryptoService = (SymmetricAlgorithm)new DESCryptoServiceProvider();
                    this.mAlgorithm = ServiceProviderEnum.DES;
                    break;
                case ServiceProviderEnum.TripleDES:
                    this.mCryptoService = (SymmetricAlgorithm)new TripleDESCryptoServiceProvider();
                    this.mAlgorithm = ServiceProviderEnum.TripleDES;
                    break;
            }
            this.mCryptoService.Mode = CipherMode.CBC;
        }

        public Cryptography(string serviceProviderName) {
            try {
                string lower = serviceProviderName.ToLower(CultureInfo.InvariantCulture);
                if (!(lower == "rijndael")) {
                    if (!(lower == "rc2")) {
                        if (!(lower == "des")) {
                            if (lower == "tripledes") {
                                serviceProviderName = "TripleDES";
                                this.mAlgorithm = ServiceProviderEnum.TripleDES;
                            }
                        } else {
                            serviceProviderName = "DES";
                            this.mAlgorithm = ServiceProviderEnum.DES;
                        }
                    } else {
                        serviceProviderName = "RC2";
                        this.mAlgorithm = ServiceProviderEnum.RC2;
                    }
                } else {
                    serviceProviderName = "Rijndael";
                    this.mAlgorithm = ServiceProviderEnum.Rijndael;
                }
                this.mCryptoService = (SymmetricAlgorithm)CryptoConfig.CreateFromName(serviceProviderName);
                this.mCryptoService.Mode = CipherMode.CBC;
            } catch {
                throw;
            }
        }

        public virtual byte[] GetLegalKey() {
            if ((uint)this.mCryptoService.LegalKeySizes.Length > 0U) {
                int num1 = this.Key.Length * 8;
                int minSize = this.mCryptoService.LegalKeySizes[0].MinSize;
                int maxSize = this.mCryptoService.LegalKeySizes[0].MaxSize;
                int skipSize = this.mCryptoService.LegalKeySizes[0].SkipSize;
                if (num1 > maxSize)
                    this.Key = this.Key.Substring(0, maxSize / 8);
                else if (num1 < maxSize) {
                    int num2 = num1 <= minSize ? minSize : num1 - num1 % skipSize + skipSize;
                    if (num1 < num2)
                        this.Key = this.Key.PadRight(num2 / 8, '*');
                }
            }
            return new PasswordDeriveBytes(this.Key, Encoding.ASCII.GetBytes(this.Salt)).GetBytes(this.Key.Length);
        }

        public virtual string Encrypt(string plainText) {
            byte[] bytes = Encoding.UTF8.GetBytes(plainText);
            this.mCryptoService.Key = this.GetLegalKey();
            this.SetLegalIV();
            ICryptoTransform encryptor = this.mCryptoService.CreateEncryptor();
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(bytes, 0, bytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] array = memoryStream.ToArray();
            return Convert.ToBase64String(array, 0, array.GetLength(0));
        }

        public virtual string Decrypt(string cryptoText) {
            byte[] buffer = Convert.FromBase64String(cryptoText);
            this.mCryptoService.Key = this.GetLegalKey();
            this.SetLegalIV();
            ICryptoTransform decryptor = this.mCryptoService.CreateDecryptor();
            try {
                return new StreamReader((Stream)new CryptoStream((Stream)new MemoryStream(buffer, 0, buffer.Length), decryptor, CryptoStreamMode.Read)).ReadToEnd();
            } catch {
                return (string)null;
            }
        }

        public string Key { get; set; } = CryptoKeyValue;

        public string Salt { get; set; } = CryptoVector;

        public enum ServiceProviderEnum {
            Rijndael,
            RC2,
            DES,
            TripleDES,
        }
    }
}