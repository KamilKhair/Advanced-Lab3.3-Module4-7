using System;
using System.Threading;
using System.Windows.Forms;

namespace PrimesCalculator
{
    public partial class Form1 : Form
    {
        private bool _working;
        public Form1()
        {
            InitializeComponent();
        }

        private void calcButton_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
            {
                return;
            }
            if (_working)
            {
                MessageBox.Show(@"An operation is already started, please wait");
                return;
            }
            ClearListBox();
            var thread = new Thread(() => CalcPrimes(int.Parse(firstTextBox.Text), int.Parse(lastTextBox.Text)));
            thread.Start();
        }

        private void CalcPrimes(int first, int last)
        {
            _working = true;
            for (var i = first; i <= last; ++i)
            {
                FindPrime(i);
            }
            _working = false;
        }

        private void FindPrime(int i)
        {
            if (i < 2)
            {
                return;
            }
            var isPrime = true;
            for (var j = 2; j < i; ++j)
            {
                if (i % j != 0) continue;
                isPrime = false;
                break;
            }
            if (!isPrime) return;
            resultListBox.Invoke((MethodInvoker)delegate {
                resultListBox.Items.Add(i);
            });
        }

        private bool ValidateInput()
        {
            int first;
            var last = 0;
            var validInput = int.TryParse(firstTextBox.Text, out first) && int.TryParse(lastTextBox.Text, out last);
            if (validInput && first <= last)
            {
                return true;
            }
            MessageBox.Show(@"Wrong input");
            return false;
        }

        private void ClearListBox()
        {
            resultListBox.Items.Clear();
        }
    }
}
