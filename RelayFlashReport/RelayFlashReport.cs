using System;

using Xamarin.Forms;

namespace RelayFlashReport
{
    public class App : Application
    {
        public App()
        {
            MainPage = new ReportPage();
            //MainPage = new ReportPageAbsolute();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}

