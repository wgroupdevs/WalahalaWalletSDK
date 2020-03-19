using System;
using System.Linq;
using System.Text;
using WalaSDK.Cryptography.ECC;
using WalaSDK.Utils;
using WalaSDK.VM;

namespace WalaSDK.Cryptography
{
    public class KeyPair
    {
        public readonly byte[] PrivateKey;
        public readonly byte[] PublicKey;
        public readonly byte[] CompressedPublicKey;
        public readonly UInt160 PublicKeyHash;
        public readonly string address;
        public readonly string WIF;

        public readonly UInt160 signatureHash;
        public readonly byte[] signatureScript;

        public KeyPair(byte[] privateKey)
        {
            //if (privateKey.Length != 32 && privateKey.Length != 96 && privateKey.Length != 104)
            //    throw new ArgumentException();
            this.PrivateKey = new byte[privateKey.Length];
            Buffer.BlockCopy(privateKey,0 , PrivateKey, 0, privateKey.Length);

            ECPoint pKey;
            pKey = ECCurve.Secp256r1.G * privateKey;

            ////if (privateKey.Length == 32)
            ////{
            
            ////}
            ////else
            ////{
            ////    pKey = ECPoint.FromBytes(privateKey, ECCurve.Secp256r1);
            ////}

            var bytes = pKey.EncodePoint(true).ToArray();
            this.CompressedPublicKey = bytes;

            this.PublicKey = pKey.EncodePoint(false).Skip(1).ToArray();

            this.PublicKeyHash = CryptoUtils.ToScriptHash(PublicKey);

            this.signatureScript = CreateSignatureScript(PublicKey);
            signatureHash = CryptoUtils.ToScriptHash(signatureScript);


            this.address = CryptoUtils.ToAddress(signatureHash);
            this.WIF = GetWIF();


        }

        public static KeyPair Mnemonickeypair(string menimonic)
        {
            var getmenimonicBytes = Encoding.ASCII.GetBytes(menimonic);
            var lengthofmenimonic = getmenimonicBytes.Length;

            byte[] data = new byte[lengthofmenimonic];
            data[0] = 0x57;
            Buffer.BlockCopy(getmenimonicBytes, 0, data, 1, lengthofmenimonic - 2);
            data[lengthofmenimonic - 1] = 0x01;
            string wif = data.Base58CheckEncode();
            Array.Clear(data, 0, data.Length);

            if (wif == null) throw new ArgumentNullException();
            byte[] data1 = wif.Base58CheckDecode();
            if (data1.Length != lengthofmenimonic || data1[0] != 0x57 || data1[lengthofmenimonic - 1] != 0x01)
                throw new FormatException();
            byte[] PrivateKeyOfmenimonic = new byte[lengthofmenimonic];
            Buffer.BlockCopy(data1, 1, PrivateKeyOfmenimonic, 0, PrivateKeyOfmenimonic.Length-1);
            Array.Clear(data1, 0, data1.Length);

            KeyPair menimonickeypair = new KeyPair(PrivateKeyOfmenimonic);


            var privatekeyBytes = menimonickeypair.PrivateKey;
            var pubKeyBytes = menimonickeypair.PublicKey;
            var privateKey = Base58.Encode(privatekeyBytes);
            var pubKey = Base58.Encode(pubKeyBytes);
            var sc = KeyPair.CreateSignatureScript(privatekeyBytes);
            var address = menimonickeypair.address;
            return menimonickeypair;
        }
        public static KeyPair FromWIF(string wif)
        {
            if (wif == null) throw new ArgumentNullException();
            byte[] data = wif.Base58CheckDecode();
            if (data.Length != 64 || data[0] != 0x80 || data[63] != 0x01)
                throw new FormatException();
            byte[] privateKey = new byte[32];
            Buffer.BlockCopy(data, 1, privateKey, 0, privateKey.Length);
            Array.Clear(data, 0, data.Length);
            return new KeyPair(privateKey);
        }

        private static System.Security.Cryptography.RandomNumberGenerator rnd = System.Security.Cryptography.RandomNumberGenerator.Create();

        public static KeyPair Generate()
        {
            var bytes = new byte[32];
            lock (rnd)
            {
                rnd.GetBytes(bytes);
            }
            return new KeyPair(bytes);
        }

        public static byte[] CreateSignatureScript(byte[] bytes)
        {
            var script = new byte[bytes.Length + 2];

            script[0] = (byte)OpCode.PUSHBYTES33;
            Array.Copy(bytes, 0, script, 1, bytes.Length);
            script[script.Length - 1] = (byte)OpCode.CHECKSIG;

            return script;
        }
        public static string PublickeyToAddress(string pubkey)
        {

            var bytes = Base58.Decode(pubkey);
            var PublicKeyHash = CryptoUtils.ToScriptHash(bytes);

            var signatureScript = CreateSignatureScript(bytes);
            var signatureHash = CryptoUtils.ToScriptHash(signatureScript);


            var address = CryptoUtils.ToAddress(signatureHash);

            return address;
        }
        private string GetWIF()
        {
            byte[] data = new byte[34];
            data[0] = 0x80;
            Buffer.BlockCopy(PrivateKey, 0, data, 1, 32);
            data[33] = 0x01;
            string wif = data.Base58CheckEncode();
            Array.Clear(data, 0, data.Length);
            return wif;
        }

        private static byte[] XOR(byte[] x, byte[] y)
        {
            if (x.Length != y.Length) throw new ArgumentException();
            return x.Zip(y, (a, b) => (byte)(a ^ b)).ToArray();
        }

        public static byte[] GetScriptHashFromAddress(string address)
        {
            var temp = address.Base58CheckDecode();
            temp = temp.SubArray(1, 20);
            return temp;
        }

        public override string ToString()
        {
            return this.address;
        }
    }
}
