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

        //adapted for part 4
        //each block takes up to 5 pending transactions from pool
        //mines block
        //adds mined block to chain
        //removes transactions from pool
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
    }
}