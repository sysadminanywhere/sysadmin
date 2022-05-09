using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Threading;

namespace UITests
{
    public class Tests
    {

        WindowsDriver<WindowsElement> desktopSession;

        [SetUp]
        public void Setup()
        {
            desktopSession = ClientSession.Setup();
        }

        [Test, Order(1)]
        public void LoginTest()
        {

            // LeftClick on TabItem "Advanced" at (87,26)
            Console.WriteLine("LeftClick on TabItem \"Advanced\" at (87,26)");
            string xpath_LeftClickTabItemAdvanced_87_26 = "/Window[@ClassName=\"WinUIDesktopWin32WindowClass\"][@Name=\"WinUI Desktop\"]/Pane[@ClassName=\"Microsoft.UI.Content.ContentWindowSiteBridge\"][@Name=\"DesktopChildSiteBridge\"]/Custom[@AutomationId=\"nvMain\"]/Tab[@AutomationId=\"pivotLogin\"]/TabItem[@Name=\"Advanced\"][@AutomationId=\"LoginPivotItemAdvanced\"]";
            var winElem_LeftClickTabItemAdvanced_87_26 = desktopSession.FindElementByAbsoluteXPath(xpath_LeftClickTabItemAdvanced_87_26);
            if (winElem_LeftClickTabItemAdvanced_87_26 != null)
            {
                winElem_LeftClickTabItemAdvanced_87_26.Click();
            }
            else
            {
                Console.WriteLine($"Failed to find element using xpath: {xpath_LeftClickTabItemAdvanced_87_26}");
                return;
            }


            // LeftClick on Edit "" at (213,20)
            Console.WriteLine("LeftClick on Edit \"\" at (213,20)");
            string xpath_LeftClickEdit_213_20 = "/Window[@ClassName=\"WinUIDesktopWin32WindowClass\"][@Name=\"WinUI Desktop\"]/Pane[@ClassName=\"Microsoft.UI.Content.ContentWindowSiteBridge\"][@Name=\"DesktopChildSiteBridge\"]/Custom[@AutomationId=\"nvMain\"]/Tab[@AutomationId=\"pivotLogin\"]/TabItem[@Name=\"Advanced\"][@AutomationId=\"LoginPivotItemAdvanced\"]/Edit[@AutomationId=\"serverName\"]";
            var winElem_LeftClickEdit_213_20 = desktopSession.FindElementByAbsoluteXPath(xpath_LeftClickEdit_213_20);
            if (winElem_LeftClickEdit_213_20 != null)
            {
                winElem_LeftClickEdit_213_20.Click();
            }
            else
            {
                Console.WriteLine($"Failed to find element using xpath: {xpath_LeftClickEdit_213_20}");
                return;
            }


            // KeyboardInput VirtualKeys="Keys.NumberPad1 + Keys.NumberPad1Keys.NumberPad9 + Keys.NumberPad9Keys.NumberPad2 + Keys.NumberPad2Keys.Decimal + Keys.DecimalKeys.NumberPad1 + Keys.NumberPad1Keys.NumberPad6 + Keys.NumberPad6Keys.NumberPad8 + Keys.NumberPad8Keys.Decimal + Keys.DecimalKeys.NumberPad0 + Keys.NumberPad0Keys.Decimal + Keys.DecimalKeys.NumberPad1 + Keys.NumberPad1Keys.NumberPad0 + Keys.NumberPad0Keys.NumberPad7 + Keys.NumberPad7" CapsLock=False NumLock=True ScrollLock=False
            Console.WriteLine("KeyboardInput VirtualKeys=\"Keys.NumberPad1 + Keys.NumberPad1Keys.NumberPad9 + Keys.NumberPad9Keys.NumberPad2 + Keys.NumberPad2Keys.Decimal + Keys.DecimalKeys.NumberPad1 + Keys.NumberPad1Keys.NumberPad6 + Keys.NumberPad6Keys.NumberPad8 + Keys.NumberPad8Keys.Decimal + Keys.DecimalKeys.NumberPad0 + Keys.NumberPad0Keys.Decimal + Keys.DecimalKeys.NumberPad1 + Keys.NumberPad1Keys.NumberPad0 + Keys.NumberPad0Keys.NumberPad7 + Keys.NumberPad7\" CapsLock=False NumLock=True ScrollLock=False");
            System.Threading.Thread.Sleep(100);
            winElem_LeftClickEdit_213_20.SendKeys(Keys.NumberPad1);
            winElem_LeftClickEdit_213_20.SendKeys(Keys.NumberPad9);
            winElem_LeftClickEdit_213_20.SendKeys(Keys.NumberPad2);
            winElem_LeftClickEdit_213_20.SendKeys(Keys.Decimal);
            winElem_LeftClickEdit_213_20.SendKeys(Keys.NumberPad1);
            winElem_LeftClickEdit_213_20.SendKeys(Keys.NumberPad6);
            winElem_LeftClickEdit_213_20.SendKeys(Keys.NumberPad8);
            winElem_LeftClickEdit_213_20.SendKeys(Keys.Decimal);
            winElem_LeftClickEdit_213_20.SendKeys(Keys.NumberPad0);
            winElem_LeftClickEdit_213_20.SendKeys(Keys.Decimal);
            winElem_LeftClickEdit_213_20.SendKeys(Keys.NumberPad1);
            winElem_LeftClickEdit_213_20.SendKeys(Keys.NumberPad0);
            winElem_LeftClickEdit_213_20.SendKeys(Keys.NumberPad7);


            // LeftClick on Edit "" at (153,19)
            Console.WriteLine("LeftClick on Edit \"\" at (153,19)");
            string xpath_LeftClickEdit_153_19 = "/Window[@ClassName=\"WinUIDesktopWin32WindowClass\"][@Name=\"WinUI Desktop\"]/Pane[@ClassName=\"Microsoft.UI.Content.ContentWindowSiteBridge\"][@Name=\"DesktopChildSiteBridge\"]/Custom[@AutomationId=\"nvMain\"]/Tab[@AutomationId=\"pivotLogin\"]/TabItem[@Name=\"Advanced\"][@AutomationId=\"LoginPivotItemAdvanced\"]/Edit[@AutomationId=\"userNameOther\"]";
            var winElem_LeftClickEdit_153_19 = desktopSession.FindElementByAbsoluteXPath(xpath_LeftClickEdit_153_19);
            if (winElem_LeftClickEdit_153_19 != null)
            {
                winElem_LeftClickEdit_153_19.Click();
            }
            else
            {
                Console.WriteLine($"Failed to find element using xpath: {xpath_LeftClickEdit_153_19}");
                return;
            }


            // KeyboardInput VirtualKeys=""admin"Keys.Tab + Keys.Tab" CapsLock=False NumLock=True ScrollLock=False
            Console.WriteLine("KeyboardInput VirtualKeys=\"\"admin\"Keys.Tab + Keys.Tab\" CapsLock=False NumLock=True ScrollLock=False");
            System.Threading.Thread.Sleep(100);
            winElem_LeftClickEdit_153_19.SendKeys("admin");
            winElem_LeftClickEdit_153_19.SendKeys(Keys.Tab + Keys.Tab);


            Console.WriteLine("LeftClick on TabItem \"Advanced\" at (89,385)");
            string xpath_LeftClickTabItemAdvanced_89_385 = "/Window[@ClassName=\"WinUIDesktopWin32WindowClass\"][@Name=\"WinUI Desktop\"]/Pane[@ClassName=\"Microsoft.UI.Content.ContentWindowSiteBridge\"][@Name=\"DesktopChildSiteBridge\"]/Custom[@AutomationId=\"nvMain\"]/Tab[@AutomationId=\"pivotLogin\"]/TabItem[@Name=\"Advanced\"][@AutomationId=\"LoginPivotItemAdvanced\"]/Edit[@AutomationId=\"passwordOther\"]";
            var winElem_LeftClickTabItemAdvanced_89_385 = desktopSession.FindElementByAbsoluteXPath(xpath_LeftClickTabItemAdvanced_89_385);
            if (winElem_LeftClickTabItemAdvanced_89_385 != null)
            {
                winElem_LeftClickTabItemAdvanced_89_385.Click();
            }
            else
            {
                Console.WriteLine($"Failed to find element using xpath: {xpath_LeftClickTabItemAdvanced_89_385}");
                return;
            }


            // KeyboardInput VirtualKeys="Keys.LeftShift + "s" + Keys.LeftShift"ecret2"Keys.LeftShift + "3" + Keys.LeftShift" CapsLock=False NumLock=True ScrollLock=False
            Console.WriteLine("KeyboardInput VirtualKeys=\"Keys.LeftShift + \"s\" + Keys.LeftShift\"ecret2\"Keys.LeftShift + \"3\" + Keys.LeftShift\" CapsLock=False NumLock=True ScrollLock=False");
            System.Threading.Thread.Sleep(100);
            winElem_LeftClickTabItemAdvanced_89_385.SendKeys(Keys.LeftShift + "s" + Keys.LeftShift);
            winElem_LeftClickTabItemAdvanced_89_385.SendKeys("ecret2");
            winElem_LeftClickTabItemAdvanced_89_385.SendKeys(Keys.LeftShift + "3" + Keys.LeftShift);

            // LeftClick on Button "Login" at (30,25)
            Console.WriteLine("LeftClick on Button \"Login\" at (30,25)");
            string xpath_LeftClickButtonLogin_30_25 = "/Window[@ClassName=\"WinUIDesktopWin32WindowClass\"][@Name=\"WinUI Desktop\"]/Pane[@ClassName=\"Microsoft.UI.Content.ContentWindowSiteBridge\"][@Name=\"DesktopChildSiteBridge\"]/Custom[@AutomationId=\"nvMain\"]/Button[@ClassName=\"Button\"][@Name=\"Login\"]";
            var winElem_LeftClickButtonLogin_30_25 = desktopSession.FindElementByAbsoluteXPath(xpath_LeftClickButtonLogin_30_25);
            if (winElem_LeftClickButtonLogin_30_25 != null)
            {
                winElem_LeftClickButtonLogin_30_25.Click();
            }
            else
            {
                Console.WriteLine($"Failed to find element using xpath: {xpath_LeftClickButtonLogin_30_25}");
                return;
            }

            System.Threading.Thread.Sleep(10000);
        }

        [TearDown]
        public void Cleanup()
        {
            ClientSession.Cleanup();
        }
    }
}