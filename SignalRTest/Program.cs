using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;

namespace SignalRTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Connection conn = null;

            try
            {
                conn = new Connection("http://madsskipper.dk/hub");
                conn.Received += connection_Received;

                conn.Start().Wait();

                Console.ReadLine();

                // Windows 8
                //var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText02);
                //var elements = toastXml.GetElementsByTagName("text");
                //elements[0].AppendChild(toastXml.CreateTextNode("Match status"));
                //elements[1].AppendChild(toastXml.CreateTextNode(obj));

                //var toast = new ToastNotification(toastXml);
                //ToastNotificationManager.CreateToastNotifier().Show(toast);
            }
            finally
            {
                if (conn != null) conn.Received -= connection_Received;
            }
            
        }

        static void connection_Received(string obj)
        {
             Console.WriteLine(obj);
        }
    }
}
