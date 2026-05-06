using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BlockchainAssignment
{
    public class Blockchain
    {
        //list to store all  the blocks
        public List<Block> blocks = new List<Block>();

        //part 3: list to store pending transactions
        public List<Transaction> transactionPool = new List<Transaction>();

        //constructor to create genesis block
        public Blockchain()
        {
            Block genesisBlock = new Block(); // calls empty constructor
            blocks.Add(genesisBlock);
        }

        //gets the latest block in the chain
        public Block GetLastBlock()
        {
            return blocks[blocks.Count - 1];
        }

        //adapted for part 5
        public void AddBlock(string minerAddress)
        {
            List<Transaction> chosenTransactions = transactionPool.Take(5).ToList();

            Block newBlock = new Block(GetLastBlock(), chosenTransactions, minerAddress);

            blocks.Add(newBlock);

            transactionPool = transactionPool.Except(chosenTransactions).ToList();
        }

        //part 3: adds a transaction to the transaction pool
        public void AddTransaction(Transaction transaction)
        {
            transactionPool.Add(transaction);
        }

        //reads a specific block by index
        public string ReadBlock(int index)
        {
            if (index >= 0 && index < blocks.Count)
            {
                return blocks[index].ReadBlock();
            }
            else
            {
                return "Invalid block index.";
            }
        }

        //reads all blocks
        public string ReadAllBlocks()
        {
            StringBuilder output = new StringBuilder();

            foreach (Block block in blocks)
            {
                output.AppendLine(block.ReadBlock());
                output.AppendLine("---------------------------");
            }

            return output.ToString();
        }

        //part 3: reads all transactions waiting in the pool
        public string ReadTransactionPool()
        {
            if (transactionPool.Count == 0)
            {
                return "Transaction pool is empty.";
            }

            StringBuilder output = new StringBuilder();

            foreach (Transaction transaction in transactionPool)
            {
                output.AppendLine(transaction.ReadTransaction());
                output.AppendLine("---------------------------");
            }

            return output.ToString();
        }


        //part 5:
        public string ValidateBlockchain()
        {
            for (int i = 1; i < blocks.Count; i++)
            {
                Block currentBlock = blocks[i];
                Block previousBlock = blocks[i - 1];

                if (currentBlock.prevHash != previousBlock.hash)
                {
                    return "Blockchain invalid: previous hash does not match at block " + i;
                }

                string recalculatedMerkleRoot = Block.CalculateMerkleRoot(currentBlock.transactions);

                if (currentBlock.merkleRoot != recalculatedMerkleRoot)
                {
                    return "Blockchain invalid: Merkle Root does not match at block " + i;
                }

                string recalculatedHash = currentBlock.CreateHash();

                if (currentBlock.hash != recalculatedHash)
                {
                    return "Blockchain invalid: block hash does not match at block " + i;
                }

                string target = new string('0', currentBlock.difficulty);

                if (!currentBlock.hash.StartsWith(target))
                {
                    return "Blockchain invalid: proof-of-work failed at block " + i;
                }

                foreach (Transaction transaction in currentBlock.transactions)
                {
                    if (!transaction.ValidateHash())
                    {
                        return "Blockchain invalid: transaction hash invalid in block " + i;
                    }

                    if (!transaction.ValidateTransaction())
                    {
                        return "Blockchain invalid: transaction signature invalid in block " + i;
                    }
                }
            }

            return "Blockchain is valid.";
        }


        //part 5:
        public double GetBalance(string walletAddress)
        {
            double balance = 0;

            foreach (Block block in blocks)
            {
                foreach (Transaction transaction in block.transactions)
                {
                    if (transaction.recipientAddress == walletAddress)
                    {
                        balance += transaction.amount;
                    }

                    if (transaction.senderAddress == walletAddress)
                    {
                        balance -= transaction.amount;
                        balance -= transaction.fee;
                    }
                }
            }

            return balance;
        }

        //part 5
        public string ReadBalance(string walletAddress)
        {
            double balance = GetBalance(walletAddress);

            StringBuilder output = new StringBuilder();

            output.AppendLine("Wallet Address:");
            output.AppendLine(walletAddress);
            output.AppendLine();
            output.AppendLine("Balance: " + balance);
            output.AppendLine();
            output.AppendLine("Transactions involving this wallet:");

            foreach (Block block in blocks)
            {
                foreach (Transaction transaction in block.transactions)
                {
                    if (transaction.senderAddress == walletAddress || transaction.recipientAddress == walletAddress)
                    {
                        output.AppendLine("---------------------------");
                        output.AppendLine(transaction.ReadTransaction());
                    }
                }
            }

            return output.ToString();
        }

    }
}