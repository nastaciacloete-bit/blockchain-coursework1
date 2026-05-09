using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Diagnostics;
using System.Threading;

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
        public int difficulty = 5;
        public List<Transaction> transactions = new List<Transaction>();

        public double reward = 25.0;
        public double fees = 0;
        public string minerAddress;

        //part 5
        public string merkleRoot;

        //6.1
        public long miningTimeMs;
        public int threadsUsed;


        //genesis block constructor
        public Block()
        {
            timestamp = DateTime.Now;
            index = 0;
            prevHash = "";
            nonce = 0;
            minerAddress = "";
            merkleRoot = "";
            hash = CreateHash();
        }

        //normal block constructor
        public Block(Block lastBlock, List<Transaction> transactions, string minerAddress, int threadCount)
        {
            timestamp = DateTime.Now;
            index = lastBlock.index + 1;
            prevHash = lastBlock.hash;
            nonce = 0;
            difficulty = 5; //adapted for 6.1 to better show threads
            

            this.transactions = transactions;
            this.minerAddress = minerAddress;
            AddRewardTransaction();

            merkleRoot = CalculateMerkleRoot(transactions); //used for part 5

            Mine(threadCount); //used for part 4, adapted for 6.1
        }

        //part 4 rewards
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


        //6.1
        public string CreateHashWithNonce(int testNonce)
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
                         + testNonce.ToString()
                         + difficulty.ToString()
                         + reward.ToString()
                         + fees.ToString()
                         + minerAddress
                         + transactionData
                         + merkleRoot;

            byte[] hashByte = hasher.ComputeHash(Encoding.UTF8.GetBytes(input));

            string hash = string.Empty;

            foreach (byte x in hashByte)
            {
                hash += String.Format("{0:x2}", x);
            }

            return hash;
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
                         + reward.ToString() //part 4
                         + fees.ToString() //part 4
                         + minerAddress //part 4
                         + transactionData
                         + merkleRoot;
                

            byte[] hashByte = hasher.ComputeHash(Encoding.UTF8.GetBytes(input));

            string hash = string.Empty;

            foreach (byte x in hashByte)
            {
                hash += String.Format("{0:x2}", x);
            }

            return hash;
        }

        public void Mine(int numberOfThreads) //adapted for 6.1
        {
            
            threadsUsed = numberOfThreads;

            bool blockFound = false;
            object lockObject = new object();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            Thread[] threads = new Thread[numberOfThreads];

            for (int i = 0; i < numberOfThreads; i++)
            {
                int threadID = i;

                threads[i] = new Thread(() =>
                {
                    int localNonce = threadID;
                    string localHash = "";

                    while (!blockFound)
                    {
                        localHash = CreateHashWithNonce(localNonce);

                        if (localHash.StartsWith(new string('0', difficulty)))
                        {
                            lock (lockObject)
                            {
                                if (!blockFound)
                                {
                                    blockFound = true;
                                    nonce = localNonce;
                                    hash = localHash;
                                }
                            }

                            break;
                        }

                        localNonce += numberOfThreads; //controls the next nonce it tests
                    }
                });

                threads[i].Start();
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            stopwatch.Stop();
            miningTimeMs = stopwatch.ElapsedMilliseconds;
        }

        //part 5: merkle root 
        public static string CalculateMerkleRoot(List<Transaction> transactions)
        {
            if (transactions == null || transactions.Count == 0)
            {
                return "";
            }

            List<string> hashes = new List<string>();

            foreach (Transaction transaction in transactions)
            {
                hashes.Add(transaction.hash);
            }

            while (hashes.Count > 1)
            {
                List<string> newHashes = new List<string>();

                for (int i = 0; i < hashes.Count; i += 2)
                {
                    if (i + 1 < hashes.Count)
                    {
                        newHashes.Add(HashTwoStrings(hashes[i], hashes[i + 1]));
                    }
                    else
                    {
                        newHashes.Add(hashes[i]);
                    }
                }

                hashes = newHashes;
            }

            return hashes[0];
        }

        //part 5:
        public static string HashTwoStrings(string hash1, string hash2)
        {
            SHA256 hasher = SHA256Managed.Create();

            string input = hash1 + hash2;
            byte[] hashByte = hasher.ComputeHash(Encoding.UTF8.GetBytes(input));

            string hash = string.Empty;

            foreach (byte x in hashByte)
            {
                hash += String.Format("{0:x2}", x);
            }

            return hash;
        }

        public string ReadBlock()
        {
            string output = "Block Index: " + index +
                            "\nTimestamp: " + timestamp +
                            "\nHash: " + hash +
                            "\nPrevious Hash: " + prevHash +
                            "\nNonce: " + nonce +
                            "\nDifficulty: " + difficulty +
                            "\nThreads Used: " + threadsUsed +
                            "\nMining Time: " + miningTimeMs + " ms" +
                            "\nMiner Address: " + minerAddress +
                            "\nMiner Reward: " + reward +
                            "\nFees: " + fees +
                            "\nMerkle Root: " + merkleRoot +
                            "\nTransactions:";


            foreach (Transaction transaction in transactions)
            {
                output += "\n\n" + transaction.ReadTransaction();
            }

            return output;
        }
    }
}