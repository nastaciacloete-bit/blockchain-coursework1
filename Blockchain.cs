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

        //adds a new block to the chain
        public void AddBlock()
        {
            Block newBlock = new Block(GetLastBlock());
            blocks.Add(newBlock);
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
    }
}