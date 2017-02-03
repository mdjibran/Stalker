using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Drawing.Imaging;
using System.IO;
using Microsoft.Win32;
using System.Threading;

namespace stalker
{
    public partial class Form1 : Form
    {
        private FilterInfoCollection CaptureDevice;     // array of all available capture devices
        private VideoCaptureDevice FinalSelectedDevice; // selected capture device

        RegistryKey reg = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run",true);

        int totalImages = Properties.Settings.Default.pics;
        
        public Form1()
        {
            reg.SetValue("Stalker", Application.ExecutablePath.ToString());
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Thread.Sleep(Properties.Settings.Default.startWait);
            CaptureDevice = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            
            foreach (FilterInfo device in CaptureDevice)
            {
                comboBox1.Items.Add(device.Name);
            }
            
            comboBox1.SelectedIndex = Properties.Settings.Default.selectedCam;
            int secs = Properties.Settings.Default.startWait;
            secs = secs / 1000;
            textBox1.Text = secs.ToString();
            textBox2.Text = Properties.Settings.Default.pics.ToString();
            textBox3.Text = Properties.Settings.Default.saveLocation;

            FinalSelectedDevice = new VideoCaptureDevice();
            getCam();       
        }

        /*
        private void _btnStart_Click(object sender, EventArgs e)
        {
            //FinalSelectedDevice = new VideoCaptureDevice(CaptureDevice[comboBox1.SelectedIndex].MonikerString);
            //FinalSelectedDevice.NewFrame += new NewFrameEventHandler(FinalSelectedDevice_NewFrame);
            //FinalSelectedDevice.Start();
        }

       

        private void _btnCapture_Click(object sender, EventArgs e)
        {
            //_pbcapturedImageBox.Image = (Bitmap)_pbCurrentImgBox.Image.Clone();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //DateTime dateObj = DateTime.Now;
            //string date = dateObj.ToLongDateString();
            //string time = dateObj.ToLongTimeString();
            //string conTime = time.Replace(":", "-");
            //string fileName = date + " - " + conTime;

            //label1.Text = fileName;

            //    if (_pbcapturedImageBox.Image != null)
            //        _pbCurrentImgBox.Image.Save(@"D:\test\" + fileName + ".bmp", ImageFormat.Bmp);
            //    else
            //    {
            //    }


        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {

            if (FinalSelectedDevice.IsRunning)
                FinalSelectedDevice.Stop();
        }
        */

       
        void FinalSelectedDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            _pbCurrentImgBox.Image = (Bitmap)eventArgs.Frame.Clone();
        }


       

        private void getCam()
        {           
                FinalSelectedDevice = new VideoCaptureDevice(CaptureDevice[comboBox1.SelectedIndex].MonikerString);
                FinalSelectedDevice.NewFrame += new NewFrameEventHandler(FinalSelectedDevice_NewFrame);
                FinalSelectedDevice.Start();            
        }

        private void saveImage()
        {
            DateTime dateObj = DateTime.Now;
            string date = dateObj.ToLongDateString();
            string time = dateObj.ToLongTimeString();
            string conTime = time.Replace(":", "-");
            string fileName = conTime;
            string savedPath = Properties.Settings.Default.saveLocation;
            string subPath = savedPath + "\\" + date; 

            bool isExists = System.IO.Directory.Exists(subPath);
            if (!isExists)
            {
                System.IO.Directory.CreateDirectory(subPath);
            }

            label1.Text = fileName;

            if (_pbcapturedImageBox.Image != null)
                _pbCurrentImgBox.Image.Save(subPath + "\\" + fileName + ".bmp", ImageFormat.Bmp);
            else
            {
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (totalImages >= 0)
                {
                    _pbcapturedImageBox.Image = (Bitmap)_pbCurrentImgBox.Image.Clone();
                    saveImage();
                    totalImages--;
                    this.Hide();
                }
                else
                {
                    timer1.Enabled = false;
                    this.Hide();
                    this.Close();
                }
            }
            catch
            {
                totalImages = 40;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (FinalSelectedDevice.IsRunning)
                FinalSelectedDevice.Stop();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (FinalSelectedDevice.IsRunning)
                FinalSelectedDevice.Stop();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.Opacity = 1;
            timer1.Enabled = false;
            if (FinalSelectedDevice.IsRunning)
                FinalSelectedDevice.Stop();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int selectedCam, numberPics, timeWait;
            selectedCam = comboBox1.SelectedIndex;
            if ((int.TryParse(textBox1.Text, out timeWait)) && (int.TryParse(textBox2.Text, out numberPics))) 
            {
                Properties.Settings.Default.pics = numberPics;
                timeWait = timeWait * 1000;
                Properties.Settings.Default.startWait = timeWait;
                Properties.Settings.Default.selectedCam = selectedCam;
                Properties.Settings.Default.saveLocation = textBox3.Text;
                try
                {
                    Properties.Settings.Default.Save();
                    MessageBox.Show("Stalker Sucessfully Configured.");
                }
                catch {
                    MessageBox.Show("Some technical issue occured");
                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                textBox3.Text = fbd.SelectedPath;
            }
        }

       
    }
}
