using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using System.Timers;

namespace HeartRateMonitor
{
    public partial class Form1 : Form
    {

        SerialPort currentPort;
        Boolean portFound;
        System.Timers.Timer portTimer = new System.Timers.Timer(); //periodically check for bytes at the currentPort

        public Form1()
        {
            InitializeComponent();
        }

        private void setComPorts()
        {
            try
            {
                string[] ports = SerialPort.GetPortNames();
                foreach (string port in ports)
                {
                    currentPort = new SerialPort(port, 9600);
                    if (detectArduino())
                    {
                        portFound = true;
                        break;
                    }
                    else
                        portFound = false;
                }

            }
            catch
            {

            }
        }

        Boolean detectArduino()
        {
            try
            {
                byte[] buffer = new byte[5];
                buffer[0] = Convert.ToByte(16);
                buffer[1] = Convert.ToByte(128);
                buffer[2] = Convert.ToByte(0);
                buffer[3] = Convert.ToByte(0);
                buffer[4] = Convert.ToByte(4);
                int returnAscii = 0;
                //char returnChar = (char)returnAscii;
                currentPort.Open();
                currentPort.Write(buffer, 0, 5);
                Thread.Sleep(1000);
                int count = currentPort.BytesToRead;
                string returnMessage = "";
                while (count > 0)
                {
                    returnAscii = currentPort.ReadByte();
                    returnMessage = returnMessage + Convert.ToChar(returnAscii);
                    count--;
                }
                currentPort.Close();
                if (returnMessage.Contains("Hello budie!"))
                    return true;
                else
                    return false;

            }
            catch
            {
                return false;
            }
        }

        private void connect_Click(object sender, EventArgs e)
        {
            setComPorts();
            if (portFound)
            {
                MessageBox.Show("Connected on " + currentPort.PortName);
                portTimer.Elapsed += new ElapsedEventHandler(readFromPort);
                portTimer.Interval = 500;
                portTimer.Enabled = true;
                currentPort.Open();
            }
            else
            {
                try
                {
                    currentPort.Close();
                }
                catch { }
                MessageBox.Show("Could not connect to arduino");
            }    
        }
        private void readFromPort(object e, ElapsedEventArgs args)
        {
            try
            {
                if (currentPort.IsOpen)
                {
                    if (currentPort.BytesToRead > 0)
                    {
                        int btr = currentPort.ReadByte();
                        Console.WriteLine(btr);
                    }
                }
                else
                {
                    portFound = false;
                    portTimer.Enabled = false;

                }
            }
            catch { }
        }
    }
}

