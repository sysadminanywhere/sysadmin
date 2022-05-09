using NUnit.Framework;
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UITests
{
    public class ClientSession
    {

        protected const string AppId = "bf25fbfe-d6fe-473b-9b2d-b2dfa303d2e7_pyde9gwt8sqkr!App";
        protected const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";

        protected static WindowsDriver<WindowsElement> session;

        private static Process appProcess;

        public static WindowsDriver<WindowsElement> Setup()
        {

            StartDriver();

            if (session == null)
            {
                var appCapabilities = new DesiredCapabilities();
                appCapabilities.SetCapability("app", AppId);
                appCapabilities.SetCapability("deviceName", "WindowsPC");
                session = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appCapabilities);

                Assert.IsNotNull(session);
            }

            return session;
        }

        public static void Cleanup()
        {
            if (session != null)
            {
                session.Quit();
                session.Dispose();
                session = null;
            }

            StopDriver();
        }

        private static void StartDriver()
        {
            appProcess = new Process();
            appProcess.StartInfo.FileName = @"C:\Program Files\Windows Application Driver\WinAppDriver.exe";
            appProcess.Start();
        }

        private static void StopDriver()
        {
            appProcess.Kill();
        }

    }
}