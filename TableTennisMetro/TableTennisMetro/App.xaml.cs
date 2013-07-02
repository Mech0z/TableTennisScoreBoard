using System;
using Microsoft.AspNet.SignalR.Client;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace TableTennisMetro
{

    sealed partial class App : Application
    {
        private Connection conn;
        private const string conUrl = "http://madsskipper.dk/hub";

        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            conn = new Connection(conUrl);
            conn.Received += Message_Recived;
            conn.Start();

            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new Frame();

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                if (!rootFrame.Navigate(typeof(MainPage), args.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }
            Window.Current.Activate();
        }

        private void Message_Recived(string s)
        {
            if(string.IsNullOrEmpty(s)) return;

            var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText02);
            var elements = toastXml.GetElementsByTagName("text");
            elements[0].AppendChild(toastXml.CreateTextNode("Match update"));
            elements[1].AppendChild(toastXml.CreateTextNode(s));

            var toast = new ToastNotification(toastXml);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {

            if (conn != null)
            {
                conn.Received -= Message_Recived;
                conn.Stop();
            }

            var deferral = e.SuspendingOperation.GetDeferral();
            deferral.Complete();
        }
    }
}
