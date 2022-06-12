using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace ComunicClientSalineira
{
    public partial class Form1 : Form
    {
        string path;
        long lastChange;
        public Form1()
        {
            InitializeComponent();
        }

        private void WriteLastModfReg()
        {
            StreamWriter streamWriter = new StreamWriter("lastModifiedRegistry.bin");
            DateTime fileChangeDate = File.GetLastWriteTime(path);
            streamWriter.Write(ConvertDateToUnixTS(fileChangeDate).ToString());
            streamWriter.Close();
            ReadLastModReg();
        }
        
        private long ConvertDateToUnixTS(DateTime date)
        {
            return ((DateTimeOffset)date).ToUnixTimeSeconds();
        }
        private void ReadLastModReg()
        {
            StreamReader streamReader = new StreamReader("lastModifiedRegistry.bin");
            string lastMod = streamReader.ReadToEnd();
            streamReader.Close();
            lastChange = (long)Convert.ToDouble(lastMod);
        }

        private void PrintMessageContent()
        {
            StreamReader streamReader = new StreamReader(path);
            string message = streamReader.ReadToEnd();
            streamReader.Close();
            MessageBox.Show(message);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            StreamReader streamReader = new StreamReader("messagePath.bin");
            string messagePath = streamReader.ReadToEnd();
            streamReader.Close();
            path = Path.GetFullPath(messagePath);

            if (File.Exists("lastModifiedRegistry.bin"))
            {
                ReadLastModReg();
            }
            else
            {
                WriteLastModfReg();
            }
            timer1.Enabled = true;

        }


        private void timer1_Tick_1(object sender, EventArgs e)
        {
            DateTime fileChangeDate = File.GetLastWriteTime(path);
            label1.Text = ConvertDateToUnixTS(fileChangeDate).ToString() + " | " + lastChange;
            if (ConvertDateToUnixTS(fileChangeDate) - lastChange != 0)
            {
               WriteLastModfReg();
               PrintMessageContent();

            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
