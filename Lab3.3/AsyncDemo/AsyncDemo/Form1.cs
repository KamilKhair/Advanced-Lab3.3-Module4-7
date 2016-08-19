using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AsyncDemo
{
    public partial class Form1 : Form
    {
        // Why STATIC ??
        private static bool _working;
        public Form1()
        {
            InitializeComponent();
        }

        private static IEnumerable<int> CalcPrimes(int first, int last)
        {
            _working = true;
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
        // Why static?? 
        // Don't use ref parameter, you can make function that return "int"
        private static void FindPrime(int i, ICollection<int> list, ref int size)
        {
            if (i < 2)
            {
                return;
            }
            var isPrime = true;
            for (var j = 2; j <= Math.Sqrt(i); ++j)
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
            if (!ValidateInput())
            {
                return;
            }
            if (_working)
            {
                MessageBox.Show(@"Working, Please wait");
                return;
            }
            var calcPrimesDelegate = new CalcPrimesDelegate(CalcPrimes);
            // use TryParse next time
            calcPrimesDelegate.BeginInvoke(int.Parse(firstTextBox.Text), int.Parse(lastTextBox.Text), iar =>
            {
                EndInvokeCalcPrimes(calcPrimesDelegate, iar);
            }, null);
        }
        //Bad name for the method
        private void EndInvokeCalcPrimes(CalcPrimesDelegate calcPrimesDelegate, IAsyncResult iar)
        {
            var primes = calcPrimesDelegate.EndInvoke(iar);
            // Invoke = is synchronic , beginInvoke = Asynchronic
            // The UI blocked while calculating primes
            resultListBox.Invoke(new Action(() =>
            {
                foreach (var prime in primes)
                    resultListBox.Items.Add(prime);
                _working = false;
            }));
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
