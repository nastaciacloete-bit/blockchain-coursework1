using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainAssignment
{
    public class Transaction
    {
        public string hash;
        public string signature;
        public string senderAddress;
        public string recipientAddress;
        public DateTime timestamp;
        public double amount;
        public double fee;

        public Transaction(string senderAddress, string recipientAddress, double amount, double fee, string privateKey)
        {
            this.senderAddress = senderAddress;
            this.recipientAddress = recipientAddress;
            this.amount = amount;
            this.fee = fee;
            this.timestamp = DateTime.Now;

            hash = CreateHash();

            if (senderAddress == "Mine Rewards")
            {
                signature = "";
            }
            else
            {
                signature = Wallet.Wallet.CreateSignature(senderAddress, privateKey, hash);
            }
        }

        public string CreateHash()
        {
            SHA256 hasher = SHA256Managed.Create();

            string input = senderAddress + recipientAddress + timestamp.ToString() + amount.ToString() + fee.ToString();

            byte[] hashByte = hasher.ComputeHash(Encoding.UTF8.GetBytes(input));

            string hash = string.Empty;

            foreach (byte x in hashByte)
            {
                hash += String.Format("{0:x2}", x);
            }

            return hash;
        }

        public string ReadTransaction()
        {
            return "Transaction Hash: " + hash +
                   "\nSignature: " + signature +
                   "\nSender: " + senderAddress +
                   "\nRecipient: " + recipientAddress +
                   "\nTimestamp: " + timestamp +
                   "\nAmount: " + amount +
                   "\nFee: " + fee;
        }
    }
}