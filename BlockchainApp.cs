using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlockchainAssignment
{
    public partial class BlockchainApp : Form
    {
        Blockchain blockchain;
        public BlockchainApp()
        {
            InitializeComponent();
            blockchain = new Blockchain();
            richTextBox1.Text = "New blockchain initialised!";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (int.TryParse(textBox1.Text, out int index))
            {
                richTextBox1.Text = blockchain.ReadBlock(index);
            }
            else
            {
                richTextBox1.Text = "Please enter a valid block index.";
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string privateKey;

            Wallet.Wallet newWallet = new Wallet.Wallet(out privateKey);

            textBox2.Text = newWallet.publicID;
            textBox3.Text = privateKey;

            richTextBox1.Text = "New wallet generated!";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            bool valid = Wallet.Wallet.ValidatePrivateKey(textBox3.Text, textBox2.Text);

            if (valid)
            {
                richTextBox1.Text = "Wallet is valid.";
            }
            else
            {
                richTextBox1.Text = "Wallet is invalid.";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            double amount;
            double fee;

            if (!double.TryParse(textBox5.Text, out amount))
            {
                richTextBox1.Text = "Please enter a valid amount.";
                return;
            }

            if (!double.TryParse(textBox6.Text, out fee))
            {
                richTextBox1.Text = "Please enter a valid fee.";
                return;
            }

            Transaction transaction = new Transaction(
                textBox2.Text,
                textBox4.Text,
                amount,
                fee,
                textBox3.Text
            );

            blockchain.AddTransaction(transaction);

            richTextBox1.Text = transaction.ReadTransaction();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = blockchain.ReadTransactionPool();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string minerAddress = textBox2.Text;

            if (string.IsNullOrWhiteSpace(minerAddress))
            {
                richTextBox1.Text = "Please generate or enter a miner public key first.";
                return;
            }

            blockchain.AddBlock(minerAddress);
            richTextBox1.Text = blockchain.ReadBlock(blockchain.blocks.Count - 1);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = blockchain.ReadAllBlocks();
        }
    }
}
