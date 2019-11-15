using System;
using CoreAnimation;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace Trips.iOS.Renderers
{
    public class PulsatingMarker : UIView
    {
        public PulsatingMarker()
        {
            Frame = new CoreGraphics.CGRect(0, 0, 50, 50);
            var fixedView = new UIView(new CoreGraphics.CGRect(0, 0, 20, 20))
            {
                Center = new CoreGraphics.CGPoint(25, 25),
                BackgroundColor = Color.FromHex("#201E45").ToUIColor(),
            };
            fixedView.Layer.CornerRadius = 10;
            Add(fixedView);

            var pulseView = new UIView(Frame)
            {
                BackgroundColor = Color.FromHex("#99201E45").ToUIColor()
            };
            Add(pulseView);
            pulseView.Layer.CornerRadius = 25;

            pulseView.Layer.AddAnimation(InitAnimation(), "PulseAnimation");
        }

        private CABasicAnimation InitAnimation()
        {
            var pulseAnimation = CABasicAnimation.FromKeyPath("transform.scale");
            pulseAnimation.Duration = 1;
            pulseAnimation.RepeatCount = float.MaxValue;
            pulseAnimation.AutoReverses = true;
            pulseAnimation.From = NSNumber.FromDouble(0.5);
            pulseAnimation.To = NSNumber.FromDouble(1);
            return pulseAnimation;
        }
    }
}
