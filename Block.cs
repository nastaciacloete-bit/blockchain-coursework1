using System;
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

        //genesis block constructor
        public Block()
        {
            timestamp = DateTime.Now;
            index = 0;
            prevHash = "";
            hash = CreateHash();
        }

        //normal block constructor
        public Block(Block lastBlock)
        {
            timestamp = DateTime.Now;
            index = lastBlock.index + 1;
            prevHash = lastBlock.hash;
            hash = CreateHash();
        }

        public string CreateHash()
        {
            SHA256 hasher = SHA256Managed.Create();

            string input = index.ToString() + timestamp.ToString() + prevHash;
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
            return "Block Index: " + index +
                   "\nTimestamp: " + timestamp +
                   "\nHash: " + hash +
                   "\nPrevious Hash: " + prevHash;
        }
    }
}