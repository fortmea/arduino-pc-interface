using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Management;
using System.Threading;

namespace Arduino_PC_data
{


    public partial class Form1 : Form
    {
        SerialPort port;
        readonly PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        readonly PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available MBytes");
        private int totalram = 0;
        private bool status = false;
        public Form1()
        {
            InitializeComponent();
        }
        public void setsettings(int ram)
        {
            this.totalram = ram;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Globals.form = this;
            String[] ports = SerialPort.GetPortNames();
            if (ports.Length == 0)
            {
                portlist.Items.Add("None");
            }
            else
            {
                foreach (string n in ports)
                {
                    portlist.Items.Add(n);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                port = new SerialPort(portlist.SelectedItem.ToString(), 9600);
                port.Open();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void onclose(object sender, FormClosedEventArgs e)
        {
            if (port != null && port.IsOpen)
            {
                port.Close();
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (port != null && port.IsOpen)
            {
                port.Close();
            }
            else
            {
                MessageBox.Show("Not Connected!");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            port.Write("Hello!_Hello!");
        }
        public string getCurrentCpuUsage()
        {
            return cpuCounter.NextValue().ToString("0") + "%";
        }
        public string battery()
        {
            PowerStatus pwr = SystemInformation.PowerStatus;
            return pwr.BatteryLifePercent.ToString() + "%";
        }
        public string getAvailableRAM()
        {
            if (totalram == 0)
            {
                return ramCounter.NextValue() + "MB";
            }
            else
            {
                return totalram - ramCounter.NextValue() + "MB";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            status = true;
            if (port != null && port.IsOpen)
            {
                sendcpu();
            }
            else
            {
                MessageBox.Show("Not Connected!");
            }
        }
        private void sendcpu()
        {
            timer1.Start();
        }
        private string GetGPUUsage()
        {
            try
            {
                var category = new PerformanceCounterCategory("GPU Engine");
                var counterNames = category.GetInstanceNames();
                var gpuCounters = new List<PerformanceCounter>();
                var result = 0f;

                foreach (string counterName in counterNames)
                {
                    if (counterName.EndsWith("engtype_3D"))
                    {
                        foreach (PerformanceCounter counter in category.GetCounters(counterName))
                        {
                            if (counter.CounterName == "Utilization Percentage")
                            {
                                gpuCounters.Add(counter);
                            }
                        }
                    }
                }

                gpuCounters.ForEach(x =>
                {
                    _ = x.NextValue();
                });

                gpuCounters.ForEach(x =>
                {
                    result += x.NextValue();
                });

                return result.ToString("0") + "%";
            }
            catch
            {
                return "err";
            }
        }
        public string getusedram()
        {
            return totalram - ramCounter.NextValue() + "MB";
        }
        public string gettotalram()
        {
            return totalram + "MB";
        }
        private void button5_Click(object sender, EventArgs e)
        {
            status = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form settings = new settings();
            settings.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer2.Start();
            timer1.Stop();
        }

        private void send(object sender, EventArgs e)
        {
            if (status == true)
            {
                try
                {
                    if (gettotalram() == "0MB")
                    {
                        port.Write("CPU:" + getCurrentCpuUsage() + " GPU:" + GetGPUUsage() + " _" + "Free RAM:" + getAvailableRAM() + "\n");
                    }
                    else
                    {
                        port.Write("CPU:" + getCurrentCpuUsage() + " GPU:" + GetGPUUsage() + " _" + "RAM:" + getusedram() + "\n");
                    }
                }
                catch
                {

                }
            }
            timer1.Start();
            timer2.Stop();
        }

        private void button7_Click(object sender, EventArgs e)
        {

        }
    }
    class Globals
    {
        public static Form1 form;
    }

}

