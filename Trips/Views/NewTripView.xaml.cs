using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Trips.Views
{
    public partial class NewTripView : ContentPage
    {
        private bool isDrawerShown;
        public NewTripView()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            detailsDrawer.TranslationY = Height - 55;
            detailsDrawer.IsVisible = true;
        }

        public async void DetailsDrawerTapped(object sender, EventArgs ea)
        {
            if (isDrawerShown)
            {
                await detailsDrawer.TranslateTo(0, Height - 55);
            }
            else
            {
                await detailsDrawer.TranslateTo(0, Height - 150);
            }

            isDrawerShown = !isDrawerShown;
        }
    }
}
