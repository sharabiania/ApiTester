using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ApiTester
{
    public partial class Form1 : Form
    {
        delegate void SetTextCallback(string text);

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 100; i++)
            {
                string file = @"C:\Test\apitest" + i + ".zip";
                new Task(() => { DownloadAs(file); }).Start();
            }

        }

        private void DownloadAs(string file)
        {
           SetText("\nDownloading \"" + file + "\" ...");
            WebClient client = new WebClient();
            client.Credentials = new NetworkCredential("Quanser", "Codex");
            try
            {
                client.DownloadFile(new Uri("http://repo-staging.codexdocs.com/api/v1/documents/download/ABE-Plots.zip"), file);
            }
            catch(Exception ex)
            {
               SetText("\nError: " + ex.Message);
            }
           SetText("\n\"" + file + "\" downloaded.");
        }


        #region Helper
        private void SetText(string text)
        {
            if (this.richTextBox1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.richTextBox1.Text += text;
            }

        }
        #endregion
    }
}
