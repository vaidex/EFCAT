using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EFCAT.Annotation {
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class Encrypt : Attribute {

        HashAlgorithm algorithm = SHA256.Create();

        public Encrypt() { }

        public Encrypt(HashAlgorithm algorithm) {
            this.algorithm = algorithm;
        }

        public string Hash(string value) {
            StringBuilder Sb = new StringBuilder();


            using (SHA256 hash = SHA256.Create()) {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }
    }

    public class Decrypt {

        HashAlgorithm algorithm = SHA256.Create();

        public Decrypt(HashAlgorithm algorithm) {
            this.algorithm = algorithm;
        }

        public bool Verify(string crypted_value, string value) {
            Encrypt encrypt = new Encrypt(algorithm);
            if (encrypt.Hash(value) == crypted_value) return true;
            return false;
        }
    }
}
