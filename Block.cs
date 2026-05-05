using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace BlockchainAssignment
{
    public class Block
    {
        public DateTime timestamp;
        public int index;
        public string hash;
        public string prevHash;


        //part 4
        public int nonce;
        public int difficulty = 4;
        public List<Transaction> transactions = new List<Transaction>();


        //genesis block constructor
        public Block()
        {
            timestamp = DateTime.Now;
            index = 0;
            prevHash = "";
            nonce = 0;
            hash = CreateHash();
        }

        //normal block constructor
        public Block(Block lastBlock, List<Transaction> transactions)
        {
            timestamp = DateTime.Now;
            index = lastBlock.index + 1;
            prevHash = lastBlock.hash;
            nonce = 0;
            hash = CreateHash();

            this.transactions = transactions;

            Mine(); //used for part 4
        }

        public string CreateHash()
        {
            SHA256 hasher = SHA256Managed.Create();

            string transactionData = "";

            foreach (Transaction transaction in transactions)
            {
                transactionData += transaction.hash;
            }

            string input = index.ToString()
                         + timestamp.ToString()
                         + prevHash
                         + nonce.ToString()
                         + difficulty.ToString()
                         + transactionData;

            byte[] hashByte = hasher.ComputeHash(Encoding.UTF8.GetBytes(input));

            string hash = string.Empty;

            foreach (byte x in hashByte)
            {
                hash += String.Format("{0:x2}", x);
            }

            return hash;
        }

        public void Mine()
        {
            string target = new string('0', difficulty);

            hash = CreateHash();

            while (!hash.StartsWith(target))
            {
                nonce++;
                hash = CreateHash();
            }
        }

        public string ReadBlock()
        {
            string output = "Block Index: " + index +
                            "\nTimestamp: " + timestamp +
                            "\nHash: " + hash +
                            "\nPrevious Hash: " + prevHash +
                            "\nNonce: " + nonce +
                            "\nDifficulty: " + difficulty +
                            "\nTransactions:";

            foreach (Transaction transaction in transactions)
            {
                output += "\n\n" + transaction.ReadTransaction();
            }

            return output;
        }
    }
}