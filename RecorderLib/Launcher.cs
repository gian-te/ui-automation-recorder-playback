using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;


namespace RecorderLib
{
    class Launcher
    {
        public static ChromeDriver LaunchChrome()
        {
            return new ChromeDriver(AppDomain.CurrentDomain.BaseDirectory);
        }

        public static void ExitBrowser(ChromeDriver driver)
        {
            driver.Close();
        }
    }
}
