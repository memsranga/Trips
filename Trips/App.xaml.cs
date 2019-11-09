﻿using System;
using Prism;
using Prism.Ioc;
using Prism.Unity;
using Trips.ViewModels;
using Trips.Views;
using Xamarin.Forms;

namespace Trips
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer platformInitializer) : base(platformInitializer) { }

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

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>("NavigationView");
            containerRegistry.RegisterForNavigation<TripsView, TripsViewModel>();
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();
            await NavigationService.NavigateAsync("NavigationView/TripsView");
        }
    }
}
