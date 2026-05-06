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

        public double reward = 1.0;
        public double fees = 0;
        public minerAddress;

        //genesis block constructor
        public Block()
        {
            timestamp = DateTime.Now;
            index = 0;
            prevHash = "";
            nonce = 0;
            minerAddress = "";
            hash = CreateHash();
        }

        //normal block constructor
        public Block(Block lastBlock, List<Transaction> transactions, string minerAddress)
        {
            timestamp = DateTime.Now;
            index = lastBlock.index + 1;
            prevHash = lastBlock.hash;
            nonce = 0;
            

            this.transactions = transactions;
            this.minerAddress = minerAddress;

            Mine(); //used for part 4
        }

        // part 4 rewards
        public void AddRewardTransaction()
        {
            fees = 0;

            foreach (Transaction transaction in transactions)
            {
                fees += transaction.fee;
            }

            Transaction rewardTransaction = new Transaction(
                "Mine Rewards",
                minerAddress,
                reward + fees,
                0,
                ""
            );

            transactions.Add(rewardTransaction);
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
                         + fees.ToString()
                         + minerAddress
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
                            "\nMiner Address: " + minerAddress +
                            "\nMining Reward: " + reward + 
                            "\nFees: " + fees +
                            "\nTransactions:";

            foreach (Transaction transaction in transactions)
            {
                output += "\n\n" + transaction.ReadTransaction();
            }

            return output;
        }
    }
}