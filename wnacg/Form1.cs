using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace wnacg
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private int radioType = -1;

        private void button2_Click(object sender, EventArgs e)
        {
            if (radioType == -1)
            {
                MessageBox.Show("请选择要解析的本子类型");
                return;
            }

            button2.Enabled = false;
            Collector cl = new Collector(SynchronizationContext.Current, int.Parse(textBox1.Text), int.Parse(textBox2.Text), radioType);
            cl.CollectorLog += (o, text) => 
            {
                this.textCollectorLog.AppendText(text + "\r\n");
            };
            cl.DownloadList += (o, text) =>
            {
                this.dlList.AppendText(text);
            };
            cl.Start();
        }
        
        

        private void download_Click(object sender, EventArgs e)
        {
            try
            {
                if (dlList.Text.Split('|').Length < 1 || dlList.Text == "")
                {
                    MessageBox.Show("请先解析页面");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("请先解析页面");
                return;
            }
            

            tabControl1.SelectTab(1);

            download.Enabled = false;
            Download dw = new Download(SynchronizationContext.Current, dlList.Text);
            dw.DownloadLog += (o, text) => {
                this.textCollectorLog.AppendText(text);
            };
            dw.DownloadStart += (o, test) =>
            {
                //int c = this.dlPanel.Controls.Count;
                string[] strs = ((string)test).Split('|');
                Panel p = new System.Windows.Forms.Panel();
                p.SuspendLayout();
                p.Size = new System.Drawing.Size(420, 43);
                //p.Location = new System.Drawing.Point(6, 49 * c + 6);
                p.Location = new System.Drawing.Point(6, 55);
                p.Margin = new System.Windows.Forms.Padding(6,6,6,6);
                p.BackColor = System.Drawing.Color.Gray;
                p.Name = strs[0];

                Label name = new System.Windows.Forms.Label();
                name.Name = "bzName";
                name.AutoSize = true;
                name.Location = new System.Drawing.Point(3, 5);
                name.Size = new System.Drawing.Size(41, 12);
                name.TabIndex = 1;
                name.Text = strs[1];

                Label speed = new System.Windows.Forms.Label();
                speed.Name = "bzSpeed";
                speed.AutoSize = true;
                speed.Location = new System.Drawing.Point(3, 25);
                speed.Size = new System.Drawing.Size(41, 12);
                speed.TabIndex = 1;
                speed.Text = "任务创建中...";

                p.Controls.Add(name);
                p.Controls.Add(speed);

                this.flowDlPanel.Controls.Add(p);     
            };
            dw.DownloadSpeed += (o, test) =>
            {
                string[] strs = ((string)test).Split('|');
                Panel p = (Panel)this.flowDlPanel.Controls.Find(strs[0],false)[0];
                Label speed = (Label)p.Controls.Find("bzSpeed", false)[0];
                speed.Text = strs[1];

                if (strs[1] == "完成")
                {
                    this.flowDlPanel.Controls.Remove(p);
                    this.flowOkPanel.Controls.Add(p);
                }
            };
            dw.Start();
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            //单行本
            this.radioType = 9;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            //短篇
            this.radioType = 10;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            this.radioType = 1;
        }
    }//class
}
