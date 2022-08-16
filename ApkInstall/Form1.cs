using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ApkInstall
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = "开始安装，请耐心等待安装结果...";
            label2.Text = "";
            button1.Enabled = false;

            new Thread(()=> {
                Process p = new Process();
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.CreateNoWindow = true;
                p.Start();

                p.StandardInput.WriteLine("adb devices");

                p.StandardInput.WriteLine("adb install app-release.apk");

                p.StandardInput.WriteLine("adb shell am start -n com.feigle.maintenance/com.feigle.maintenance.MainActivity&exit");

                p.StandardInput.AutoFlush = true;

                string output = p.StandardOutput.ReadToEnd();
                string str1 = output.Substring(output.IndexOf("Pe"));
                str1 = str1.Substring(0,str1.IndexOf("\nD"));

                label1.BeginInvoke(new Action(() => {
                    label1.Text = str1;
                    button1.Enabled = true;
                }));

                if (str1.Contains("Success"))
                {
                    label2.BeginInvoke(new Action(()=> {
                        label2.Text = "安装完成";
                    }));
                }
                else
                {
                    label2.BeginInvoke(new Action(() => {
                        label2.Text = "安装失败";
                    }));
                }

                p.WaitForExit();
                p.Close();
            }).Start();
            
        }
    }
}
