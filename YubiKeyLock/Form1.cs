using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using USBClassLibrary;

namespace YubiKeyLock
{
    public partial class Form1 : Form
    {
        [DllImport("user32")]
        public static extern void LockWorkStation();

        private USBClass USBPort;
        private List<USBClass.DeviceProperties> ListOfUSBDeviceProperties;
        public Form1()
        {
            InitializeComponent();
            WindowState = FormWindowState.Minimized;
            USBPort = new USBClass();
            ListOfUSBDeviceProperties = new List<USBClass.DeviceProperties>();
            USBPort.RegisterForDeviceChange(true, Handle);
            USBPort.USBDeviceRemoved += new USBClass.USBDeviceEventHandler(USBPort_USBDeviceRemoved);
        }
        protected override void WndProc(ref Message m)
        {
            bool IsHandled = false;
            USBPort.ProcessWindowsMessage(m.Msg, m.WParam, m.LParam, ref IsHandled);
            base.WndProc(ref m);
        }

        private void USBPort_USBDeviceRemoved(object sender,
                     USBClass.USBDeviceEventArgs e)
        {
            if (!USBClass.GetUSBDevice(1050, 0403, ref ListOfUSBDeviceProperties, false))
                LockWorkStation();
        }
    }

}