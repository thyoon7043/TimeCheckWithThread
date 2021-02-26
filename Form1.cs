using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TimeTest
{
    public partial class Form1 : Form
    {
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        public static extern uint timeBeginPeriod(uint uMilliseconds);

        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        public static extern uint timeEndPeriod(uint uMilliseconds);

        int _timeInterval = 8;
        bool m_bThreading = false;

        public Form1()
        {
            InitializeComponent();

            timeBeginPeriod(1);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            initThread();
        }

        private void initThread() 
        { 
            Thread thread = new Thread(new ThreadStart(getTimeInterval));
            thread.Priority = System.Threading.ThreadPriority.Highest;
            thread.IsBackground = true;
            thread.Start();

            m_bThreading = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_bThreading = false;

            timeEndPeriod(1);
        }


        void getTimeInterval()
        {
            DateTime _lastCountTime = DateTime.Now;

            while (m_bThreading)
            {
                DateTime now = DateTime.Now;
                double totalMilSec = now.Subtract(_lastCountTime).TotalMilliseconds;
                //double totalMilSec = _lastCountTime.Subtract(now).TotalMilliseconds;

                if (totalMilSec >= _timeInterval)
                {
                    Console.WriteLine("시간 간격 : " + totalMilSec);
                    _lastCountTime = now;
                }
            }
        }
    }
}
