using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AsyncDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private IEnumerable<int> CalcPrimes(int first, int last)
        {
            var list = new List<int>();
            var size = 0;
            for (var i = first; i <= last; ++i)
            {
                FindPrime(i, list, ref size);
            }
            var result = new int[size];
            list.CopyTo(result);
            return result;
        }

        private static void FindPrime(int i, List<int> list, ref int size)
        {
            var isPrime = true;
            for (var j = 2; j < i; ++j)
            {
                if (i%j != 0) continue;
                isPrime = false;
                break;
            }
            if (!isPrime) return;
            list.Add(i);
            ++size;
        }

        private void calculateButton_Click(object sender, EventArgs e)
        {
            ClearListBox();
            int first;
            int last;
            if (!ValidateInput(out first, out last)) return;
            var calcPrimesDelegate = new CalcPrimesDelegate(CalcPrimes);
            calcPrimesDelegate.BeginInvoke(first, last, iar =>
            {
                EndInvokeCalcPrimes(calcPrimesDelegate, iar);
            }, null);
        }

        private void EndInvokeCalcPrimes(CalcPrimesDelegate calcPrimesDelegate, IAsyncResult iar)
        {
            var primes = calcPrimesDelegate.EndInvoke(iar);
            BeginInvoke(new Action(() =>
            {
                foreach (var prime in primes)
                    resultListBox.Items.Add(prime);
            }));
        }

        private bool ValidateInput(out int first, out int last)
        {
            last = 0;
            var validInput = int.TryParse(firstTextBox.Text, out first) && int.TryParse(lastTextBox.Text, out last);
            if (validInput && first >= 2 && first <= last)
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
