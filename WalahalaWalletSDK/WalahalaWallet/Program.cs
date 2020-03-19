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

             // var res = transaction.GenerateWallet("host blur addict matter leg beyond super sausage hawk deny ring spin practice midnight gold spider mail shove oval raccoon object upon rescue rather report arm rubber output skate hold slam maze educate");
             // var res1 = transaction.GenerateWallet(" rotate notice logic silver pen live sting frozen defy latin ten noise enjoy figure");
             // var w =transaction.GetWallet(res.PrivateKey);
            // var res =transaction.Test();
            // var wall =transaction.GetWallet(res);



            /// GENERATE WALLET
            /// 
            var wallet = transaction.GenerateWallet("host blur addict matter leg beyond super sausage hawk deny ring spin practice midnight gold spider mail shove oval raccoon object upon rescue rather report arm rubber output skate hold slam maze educate");
            Console.WriteLine(wallet.Address);
            /// GET BALANCE
            /// 
            var balance = transaction.GetBalance("WPGBc4quEmCUhA6DgJdbQBD4tA8zrYffqf", "http://coin.walahala.org/api/");
            Console.WriteLine(balance);

            /// Generate a signed transaction ofline
            var trans = transaction.GenerateSignedTransaction(wallet.Mnemonics, "WPGBc4quEmCUhA6DgJdbQBD4tA8zrYffqf", 1);
            Console.WriteLine(trans);

            //// Broadcast transaction to network
            var res = transaction.BroadCastTransaction(trans, "http://coin.walahala.org/api/");
            Console.WriteLine(res);

            Console.ReadKey();
        }
    }
}
