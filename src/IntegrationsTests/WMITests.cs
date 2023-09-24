using NUnit.Framework;
using Sysadmin.WMI.Services;
using System.Collections.Generic;

namespace IntegrationsTests
{

    [NonParallelizable]
    public class WMITests
    {

        public string server;
        public ICredential credential;

        [SetUp]
        public void Setup()
        {
            server = TestsSetup.SERVER.ServerName;
            credential = new Credential()
            {
                UserName = TestsSetup.CREDENTIAL.UserName,
                Password = TestsSetup.CREDENTIAL.Password
            };
        }

        [Test, Order(1)]
        public void GetProcessesAsync()
        {
            using (var wmi = new WMIService(server, credential))
            {
                List<Dictionary<string, object>> result = wmi.Query("Select * From Win32_Process");
                Assert.Greater(result.Count, 0);
            }
        }

        [Test, Order(2)]
        public void GetServicesAsync()
        {
            using (var wmi = new WMIService(server, credential))
            {
                List<Dictionary<string, object>> result = wmi.Query("Select * From Win32_Service");
                Assert.Greater(result.Count, 0);
            }
        }

        [Test, Order(3)]
        public void GetLogEventsAsync()
        {
            using (var wmi = new WMIService(server, credential))
            {
                List<Dictionary<string, object>> result = wmi.Query("Select RecordNumber, EventType, EventCode, Type, TimeGenerated, SourceName, Category, Logfile, Message From Win32_NTLogEvent Where EventType = 1");
                Assert.Greater(result.Count, 0);
            }
        }

        [Test, Order(4)]
        public void GetProductsAsync()
        {
            using (var wmi = new WMIService(server, credential))
            {
                List<Dictionary<string, object>> result = wmi.Query("Select * From Win32_Product");
                Assert.Greater(result.Count, 0);
            }
        }

        [Test, Order(5)]
        public void StartServiceAsync()
        {
            using (var wmi = new WMIService(server, credential))
            {
                wmi.Invoke("Select * From Win32_Service Where Name='Audiosrv'", "StartService");

                List<Dictionary<string, object>> result = wmi.Query("Select * From Win32_Service Where Name='Audiosrv'");
                Assert.AreEqual(result.Count, 1);
                Assert.AreEqual(result[0]["State"], "Running");
            }
        }

        [Test, Order(6)]
        public void StopServiceAsync()
        {
            using (var wmi = new WMIService(server, credential))
            {
                wmi.Invoke("Select * From Win32_Service Where Name='Audiosrv'", "StopService");

                List<Dictionary<string, object>> result = wmi.Query("Select * From Win32_Service Where Name='Audiosrv'");
                Assert.AreEqual(result.Count, 1);
                Assert.AreEqual(result[0]["State"], "Stopped");
            }
        }

    }
}
