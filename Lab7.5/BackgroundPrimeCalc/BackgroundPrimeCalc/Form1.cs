using System;
using System.Windows.Forms;

namespace BackgroundPrimeCalc
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
            backgroundWorker1.RunWorkerAsync();
        }

        private void CalcPrimes(int first, int last)
        {
            for (var i = first; i <= last; ++i)
            {
                if (backgroundWorker1.CancellationPending)
                {
                    _working = false;
                    return;
                }
                FindPrime(i);
                backgroundWorker1.ReportProgress(i - first + 1);
            }
        }

        private void FindPrime(int i)
        {
            if (i < 2)
            {
                return;
            }
            var isPrime = true;
            for (var j = 2; j <= Math.Sqrt(i); ++j)
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
                progressBar1.Maximum = last - first + 1;
                return true;
            }
            MessageBox.Show(@"Wrong input");
            return false;
        }

        private void ClearListBox()
        {
            resultListBox.Items.Clear();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
            ClearListBox();
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            _working = true;
            CalcPrimes(int.Parse(firstTextBox.Text), int.Parse(lastTextBox.Text));
        }

        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show(progressBar1.Value != progressBar1.Maximum ? @"Operation Canceleld!" : @"Operation Completed!");
            progressBar1.Value = 0;
            _working = false;
        }
    }
}
