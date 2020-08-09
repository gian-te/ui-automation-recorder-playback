using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Chrome;
using System.Drawing;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Threading;


namespace RecorderLib
{
    public abstract class RecorderBase
    {
        public IWebDriver Driver { get; set; }
        public abstract void LaunchBrowser();
        public abstract Step AttachListener(int rowCount);
        public abstract void GoToUrl(string url);
    }

    public class Recorder : RecorderBase
    {
        public override Step AttachListener(int rowCount)
        {
            try
            {
                dynamic obj = ((IJavaScriptExecutor)Driver).ExecuteAsyncScript(
                "var callback = arguments[arguments.length-1];" +
                "var timeout = null; " +
                "var returnObj = null;" +
                "document.addEventListener(\"mousedown\", function(event) {returnObj = [\"click\", event.target.value, event.target.tagName, event.target.getAttribute(\"class\"),event.target.getAttribute(\"id\"),event.clientX, event.clientY];callback(returnObj); });" +
                "document.addEventListener(\"keyup\", function(event) {" +
                "clearTimeout(timeout); " +
                "if(event.target.value) {" +
                    "timeout = setTimeout(function (){" +
                    "typeTimer = setTimeout(callback([\"keystroke\", event.target.value, event.target.tagName,event.target.getAttribute(\"class\"),event.target.getAttribute(\"id\"),event.clientX, event.clientY]))}, 800);" +
                     "}}, false);"
                );
                // function(event) {var returnObj = [\"click\", event.target.value, event.target.tagName, event.target.getAttribute(\"class\"),event.target.getAttribute(\"id\"),event.clientX, event.clientY];callback(returnObj); }
                // function (event) {  var returnObj = [\"keystroke\", event.target.value, event.target.tagName, event.target.getAttribute(\"class\"),event.target.getAttribute(\"id\"),event.clientX, event.clientY];callback(returnObj); }
                string action = obj[0];
                string value = obj[1];
                string tag = obj[2];
                string className = obj[3];
                string id = obj[4];
                int Xcoord = obj[5] != null ? (int)obj[5] : 0;
                int Ycoord = obj[6] != null ? (int)obj[6] : 0;

                var step = new Step()
                {
                    Action = action,
                    Id = Guid.NewGuid(),
                    Value = value,
                    TagName = tag,
                    Class = className,
                    ElementId = id,
                    Selector = tag + (!string.IsNullOrEmpty(id) ? ("#" + id) : "") + (!string.IsNullOrEmpty(className) ? ("." + className) : ""),
                    DateCaptured = DateTime.Now,
                    ActionNumber = rowCount + 1
                };

                if (Xcoord > 0 && Ycoord > 0)
                {
                    step.Coordinates = new Point(Xcoord, Ycoord);
                }

                return step;
            }
            catch (Exception ex)
            {
                throw;
            }


        }

        public override void GoToUrl(string url)
        {
            Driver.Url = url;
        }

        public override void LaunchBrowser()
        {
            Driver = Launcher.LaunchChrome();
        }
    }

    public class Step
    {
        public Guid Id { get; set; }

        public string Action { get; set; }

        public string Selector { get; set; }

        public string Value { get; set; }

        public Point Coordinates { get; set; }

        public string Class { get; set; }

        public string ElementId { get; set; }

        public string TagName { get; set; }

        public DateTime DateCaptured { get; set; }

        public int ActionNumber { get; set; }

    }

}
