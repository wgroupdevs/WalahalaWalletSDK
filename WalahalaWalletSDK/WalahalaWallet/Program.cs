using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalaSDK.RPC.Implementation;
using WalaSDK.RPC.Interface;

namespace WalahalaWallet
{
    class Program
    {
        /// <summary>
        /// TEST APPLICATION
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            ///Transaction object
            ITransaction transaction = new Transaction();
            
            /// GENERATE WALLET
            /// 
            var wallet =transaction.GenerateWallet();
            Console.WriteLine(wallet.Address);
            /// GET BALANCE
            /// 
            var balance =transaction.GetBalance("WPGBc4quEmCUhA6DgJdbQBD4tA8zrYffqf", "http://coin.walahala.org/api/");
            Console.WriteLine(balance);
            
            /// Generate a signed transaction ofline
            var trans =transaction.GenerateSignedTransaction(wallet.Mnemonics, "WPGBc4quEmCUhA6DgJdbQBD4tA8zrYffqf", 5);
            Console.WriteLine(trans);

            //// Broadcast transaction to network
            var res =transaction.BroadCastTransaction(trans, "http://coin.walahala.org/api/");
            Console.WriteLine(res);

            Console.ReadKey();
        }
    }
}
