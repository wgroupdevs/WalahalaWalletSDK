using NBitcoin;
using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web.Script.Serialization;
using WalaSDK.Cryptography;
using WalaSDK.Model;
using WalaSDK.RPC.Interface;
using WalaSDK.Utils;

namespace WalaSDK.RPC.Implementation
{
    public class Transaction :ITransaction
    {

        public string GetWallet(string privateKey)
        {
            return Encoding.UTF8.GetString( Base58.Decode(privateKey));

            
      
        }
        public string Test()
        {
            return "";
            
        }
        public string GetPrivateKey(string words)
        {
            return Base58.Encode(Encoding.UTF8.GetBytes(words));
        }
        /// <summary>
        /// Generate new wallet for WHC
        /// </summary>
        /// <returns></returns>
        public Wallet GenerateWallet()
        {
            
            Wallet wallet = new Wallet();
            Mnemonic mnemonic = new Mnemonic(Wordlist.English, WordCount.Eighteen);
            Mnemonic mnemonic1 = new Mnemonic(Wordlist.English, WordCount.Fifteen);

            wallet.Mnemonics = String.Concat(mnemonic, " ", mnemonic1);

            var Keypair = KeyPair.Mnemonickeypair(wallet.Mnemonics);
            var privatekeyBytes = Keypair.PrivateKey;
            var pubKeyBytes = Keypair.PublicKey;
            wallet.PrivateKey = Base58.Encode(privatekeyBytes);
            wallet.PublicKey = Base58.Encode(pubKeyBytes);
            wallet.Address = Keypair.address;
            return wallet;
        }
        /// <summary>
        /// Get WHC wallet private key, public key and public address
        /// </summary>
        /// <param name="Mnemonic">wordlist</param>
        /// <returns></returns>
        public Wallet GenerateWallet(string Mnemonic)
        {
            Wallet wallet = new Wallet();
            wallet.Mnemonics = Mnemonic;
            var Keypair = KeyPair.Mnemonickeypair(wallet.Mnemonics);
            var privatekeyBytes = Keypair.PrivateKey;
            var pubKeyBytes = Keypair.PublicKey;
            wallet.PrivateKey = Base58.Encode(privatekeyBytes);
            wallet.PublicKey = Base58.Encode(pubKeyBytes);
            wallet.Address = Keypair.address;
            return wallet;
        }
        private  byte[] _signTransaction(string jsonObj, string mini)
        {
            var getBytes = Encoding.ASCII.GetBytes(jsonObj);

            var keys = KeyPair.Mnemonickeypair(mini);
            var privatekeyBytes = keys.PrivateKey;
            var pubKeyBytes = keys.PublicKey;

            var sign = CryptoUtils.Sign(getBytes, privatekeyBytes, pubKeyBytes);

            return sign;
        }
        private  string _computeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        /// <summary>
        /// Generate sign transaction
        /// </summary>
        /// <param name="mnemonic">wordlist</param>
        /// <param name="toAddress">destination address</param>
        /// <param name="amount">number of WHC you want to send</param>
        /// <returns>json object of signed transaction</returns>
        public string GenerateSignedTransaction(string mnemonic, string toAddress, decimal? amount)
        {
            try
            {
                var keys = KeyPair.Mnemonickeypair(mnemonic);
                var pubKeyBytes = keys.PublicKey;
                var pubkey = Base58.Encode(pubKeyBytes);
                var address = keys.address;

                TransactionModel transaction = new TransactionModel
                {
                    TxId = 0,
                    ParentHash = "",
                    TxHash = "",
                    SenderPubKey = pubkey,
                    SenderAddress = address,
                    ReceiverAddress = toAddress,
                    Amount = amount ?? 0,
                    TimeStamp = DateTime.UtcNow,
                    VerificationCount = 0,
                    Signature = ""
                };

                var jsonObject = new JavaScriptSerializer().Serialize(transaction);
                var sign = _signTransaction(jsonObject, mnemonic);

                var signbyte = Encoding.ASCII.GetString(sign);


                var EncodedSign = Base58.Encode(sign);
                transaction.Signature = EncodedSign;

                var Hash = _computeSha256Hash(jsonObject);
                transaction.TxHash = Hash;
                var finaljsonObject = new JavaScriptSerializer().Serialize(transaction);
                return finaljsonObject;
            }
            catch (Exception)
            {
                throw;
            }

        }
        /// <summary>
        /// Get the total balance of particular address
        /// </summary>
        /// <param name="address">desired address</param>
        /// <param name="apiUrl">end point url</param>
        /// <returns></returns>
        public string GetBalance(string address, string apiUrl)
        {
            try
            {
                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                var json = client.DownloadString(apiUrl + "/GetBalance/" + address);
                return json;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Broadcast the signed transaction to the server
        /// </summary>
        /// <param name="transactionJson">signed transaction JSON</param>
        /// <param name="apiUrl">end point url</param>
        /// <returns></returns>
        public string BroadCastTransaction(string transactionJson, string apiUrl)
        {
            try
            {
                WebClient client = new WebClient();
                client.Headers["Content-type"] = "application/json";
                client.Encoding = Encoding.UTF8;
                var json = client.UploadString(apiUrl + "/VerifyUnit", "POST", transactionJson);
                return json;
            }
            catch (Exception)
            {

                throw;
            }

        }
        /// <summary>
        /// Verify the address is according to WHC algorithm or not
        /// </summary>
        /// <param name="address"></param>
        /// <returns>return true if the particular address is correct</returns>
        public bool VerifyAddress(string address)
        {

            if (!string.IsNullOrEmpty(address))
            {
                if (address.Length == 34)
                {
                    if (address[0] != 'W' && address[1] != 'p')
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

    }
}
