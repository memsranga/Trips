using System;
using CoreAnimation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace Trips.iOS.Renderers
{
    public class CustomMarker : UIView
    {
        private UIView _pulseView;
        public CustomMarker()
        {
            Frame = new CoreGraphics.CGRect(0, 0, 50, 50);
            _pulseView = new UIView(Frame)
            {
                BackgroundColor = Color.FromHex("#80808080").ToUIColor()
            };
            Add(_pulseView);
            _pulseView.Layer.CornerRadius = 25;

            var animation = new CATransition();
            // animation.Delegate = this;
            animation.Duration = 3;
            animation.TimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseInEaseOut);
            animation.Type = CAAnimation.TransitionFade;

            _pulseView.Layer.AddAnimation(animation, null);
        }
    }
}
