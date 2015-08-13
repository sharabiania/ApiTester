using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
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
            richTextBox1.Text = string.Empty;
            string method = comboBox1.Text;
            string url = textBox1.Text;
            int count = (int)numericUpDown1.Value;
            for (int i = 0; i < count; i++)
            {
                Debug.WriteLine(i);        
                new Task(() => { SendRequest(i, method, url); }).Start();
            }

        }

    

        private async void SendRequest(int requestNumber, string method, string url)
        {
            SetText("Request " + requestNumber + ": sending...");
            method = method.ToLower();
            HttpClient client = new HttpClient();
            try
            {
                HttpResponseMessage response = new HttpResponseMessage();
                if (method == "get")
                    response = await client.GetAsync(url);
                else if (method == "post")
                    response = await client.PostAsync(url, null);
                else if (method == "put")
                    response = await client.PutAsync(url, null);
                else if (method == "delete")
                    response = await client.DeleteAsync(url);

                SetText("Response " + requestNumber + ": " + response.StatusCode.ToString());
            }
            catch (Exception ex)
            {
                SetText("Error: " + ex.Message);
            }
        }


        #region Helper
        private void SetText(string text)
        {
            Debug.WriteLine(text);
            if (this.richTextBox1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.richTextBox1.Text += "\n" + text;
            }

        }
        #endregion
    }
}
