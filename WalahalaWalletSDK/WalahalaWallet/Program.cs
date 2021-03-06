﻿using System;
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
            /// UPDATE: MNEMONICS ARE CONVERTING TO 32 BYTE HASH (Please check KeyPair.cs file)
             var wallet = transaction.GenerateWallet("host blur addict matter leg beyond super sausage hawk deny ring spin practice midnight gold spider mail shove oval raccoon object upon rescue rather report arm rubber output skate hold slam maze educate");
             // var wallet = transaction.GenerateWallet();
            Console.WriteLine(wallet.Address);
            /// GET BALANCE
            /// 
            var balance = transaction.GetBalance(wallet.Address, "http://coin.walahala.org/api/");
            Console.WriteLine(balance);

            /// Generate a signed transaction ofline
            var trans = transaction.GenerateSignedTransaction(wallet.Mnemonics, "WPZjGB2dpDEi9qw6vHYqdK7agMAm7e6jyz", 1);
            Console.WriteLine(trans);

            //// Broadcast transaction to network
            var res = transaction.BroadCastTransaction(trans, "http://coin.walahala.org/api/");
            Console.WriteLine(res);

            Console.ReadKey();
        }
    }
}
