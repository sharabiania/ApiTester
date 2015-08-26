using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
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
            string username = textBox2.Text;
            string password = textBox3.Text;
            int count = (int)numericUpDown1.Value;
            progressBar1.Value = 0;
            progressBar1.Maximum = count;
            for (int i = 0; i < count; i++)
            {                
                int temp = i;
                progressBar1.Value ++;
               // new Thread(() => SendRequest(temp, method, url, username, password)).Start();
                SendRequest(temp, method, url, username, password);
            }

        }

    
   

        private async Task SendRequest(int requestNumber, string method, string url, string username = null, string password = null)
        {
            
            Debug.WriteLine("====Request " + requestNumber + ": sending...");
            SetText("Request " + requestNumber + ": sending...");
            method = method.ToLower();
            HttpClient client = new HttpClient();
            if (username != null && password != null)
            {
                var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", username, password)));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
            }

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

                Debug.WriteLine("++++Response " + requestNumber + ": " + response.StatusCode.ToString());
                SetText("Response " + requestNumber + ": " + response.StatusCode.ToString());
            }
            catch (Exception ex)
            {
                SetText("Error: " + ex.Message);
            }
            finally
            {
              
            }

        }


        #region Helper
        private void SetText(string text)
        {
            //this.richTextBox1.Text += "\n" + text;

            //richTextBox1.AppendText("\n" + text);
            //richTextBox1.ScrollToCaret();

            if (richTextBox1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                Invoke(d, new object[] { text });

            }
            else
            {
                richTextBox1.AppendText("\n" + text);
                richTextBox1.ScrollToCaret();
            }



        }
        #endregion

        [ThreadStatic]
        static RichTextBox console;
        public static RichTextBox ThreadSafeRTFConverter
        {
            get
            {
                if (console == null)
                {
                    console = new RichTextBox();

                }
                return console;
            }

        }


        private void progressBar1_Click(object sender, EventArgs e)
        {

        }
    }
}
