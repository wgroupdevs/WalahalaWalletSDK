using WalaSDK.Model;

namespace WalaSDK.RPC.Interface
{
    public interface ITransaction {

        string Test();
        string GetPrivateKey(string words);
        string GetWallet(string privateKey);
       /// <summary>
       /// Verify the address is according to WHC algorithm or not
       /// </summary>
       /// <param name="address"></param>
       /// <returns>return true if the particular address is correct</returns>
        bool VerifyAddress(string address);
        /// <summary>
        /// Broadcast the signed transaction to the server
        /// </summary>
        /// <param name="transactionJson">signed transaction JSON</param>
        /// <param name="apiUrl">end point url</param>
        /// <returns></returns>
        string BroadCastTransaction(string transactionJson, string apiUrl);
        /// <summary>
        /// Get the total balance of particular address
        /// </summary>
        /// <param name="address">desired address</param>
        /// <param name="apiUrl">end point url</param>
        /// <returns></returns>
        string GetBalance(string address, string apiUrl);
        /// <summary>
        /// Generate sign transaction
        /// </summary>
        /// <param name="mnemonic">wordlist</param>
        /// <param name="toAddress">destination address</param>
        /// <param name="amount">number of WHC you want to send</param>
        /// <returns>json object of signed transaction</returns>
        string GenerateSignedTransaction(string mnemonic, string toAddress, decimal? amount);
        /// <summary>
        /// Get WHC wallet private key, public key and public address
        /// </summary>
        /// <param name="Mnemonic">wordlist</param>
        /// <returns></returns>
        Wallet GenerateWallet(string Mnemonic);
       /// <summary>
       /// Generate new wallet for WHC
       /// </summary>
       /// <returns></returns>
        Wallet GenerateWallet();
    }
}
