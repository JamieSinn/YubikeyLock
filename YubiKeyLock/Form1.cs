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

/// <summary>
/// Lock your computer on Yubikey Disconnection. I use this for security against siblings due to NDA Sensitive items on it.
/// 
/// Copyright Jamie Sinn 2015
/// 
/// </summary>
namespace YubiKeyLock
{
    public partial class Form1 : Form
    {
        [DllImport("user32")]
        public static extern void LockWorkStation();

        private USBClass Yubikey;
        private List<USBClass.DeviceProperties> ListOfUSBDeviceProperties;
        public Form1()
        {
            InitializeComponent();
            WindowState = FormWindowState.Minimized;
            Yubikey = new USBClass();
            ListOfUSBDeviceProperties = new List<USBClass.DeviceProperties>();
            Yubikey.RegisterForDeviceChange(true, Handle);
            Yubikey.USBDeviceRemoved += new USBClass.USBDeviceEventHandler(DeviceRemoved);
        }
        protected override void WndProc(ref Message m)
        {
            bool IsHandled = false;
            Yubikey.ProcessWindowsMessage(m.Msg, m.WParam, m.LParam, ref IsHandled);
            base.WndProc(ref m);
        }

        private void DeviceRemoved(object sender,
                     USBClass.USBDeviceEventArgs e)
        {
            // The VID and PID need to be changed. This is so it works for my specific key. 
            if (!USBClass.GetUSBDevice(1050, 0403, ref ListOfUSBDeviceProperties, false))
                LockWorkStation();
        }
    }

}