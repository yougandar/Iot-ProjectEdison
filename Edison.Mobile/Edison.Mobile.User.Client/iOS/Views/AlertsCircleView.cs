using System;
using CoreAnimation;
using CoreGraphics;
using Edison.Mobile.iOS.Common.Shared;
using Edison.Mobile.User.Client.iOS.Shared;
using UIKit;

namespace Edison.Mobile.User.Client.iOS.Views
{
    public class AlertsCircleView : UIView
    {
        readonly UILabel alertsLabel;
        readonly UILabel alertCountLabel;
        readonly UIView innerCircleView;
        readonly AlertRingView ringView;
        readonly nfloat innerCircleMargin = 15;

        int alertCount;

        public UIColor InnerCircleBackgroundColor
        {
            get => innerCircleView.BackgroundColor;
            set
            {
                innerCircleView.BackgroundColor = value;
                ringView.RingColor = value;
            }
        }

        public int AlertCount 
        {
            get => alertCount;
            set 
            {
                alertCount = value;
                alertCountLabel.Text = alertCount.ToString();
                alertsLabel.Text = alertCount == 1 ? "ALERT" : "ALERTS";
            }
        }

        public AlertsCircleView()
        {
            innerCircleView = new UIView { TranslatesAutoresizingMaskIntoConstraints = false, BackgroundColor = UIColor.White };
            AddSubview(innerCircleView);
            innerCircleView.CenterXAnchor.ConstraintEqualTo(CenterXAnchor).Active = true; 
            innerCircleView.CenterYAnchor.ConstraintEqualTo(CenterYAnchor).Active = true;
            innerCircleView.WidthAnchor.ConstraintEqualTo(WidthAnchor).Active = true;
            innerCircleView.HeightAnchor.ConstraintEqualTo(innerCircleView.WidthAnchor).Active = true;

            alertCountLabel = new UILabel
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                TextColor = PlatformConstants.Color.White,
                Font = Constants.Fonts.RubikOfSize(Constants.Fonts.Size.SeventyTwo),
                TextAlignment = UITextAlignment.Center,
            };

            innerCircleView.AddSubview(alertCountLabel);
            alertCountLabel.CenterXAnchor.ConstraintEqualTo(innerCircleView.CenterXAnchor).Active = true;
            alertCountLabel.CenterYAnchor.ConstraintEqualTo(innerCircleView.CenterYAnchor, -(innerCircleView.Bounds.Height / 12)).Active = true;

            alertsLabel = new UILabel
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                TextColor = PlatformConstants.Color.White,
                Font = Constants.Fonts.RubikOfSize(Constants.Fonts.Size.Fourteen),
                Text = "ALERTS",
                TextAlignment = UITextAlignment.Center,
            };

            innerCircleView.AddSubview(alertsLabel);
            alertsLabel.TopAnchor.ConstraintEqualTo(alertCountLabel.BottomAnchor).Active = true;
            alertsLabel.CenterXAnchor.ConstraintEqualTo(innerCircleView.CenterXAnchor).Active = true;

            ringView = new AlertRingView
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                RingColor = InnerCircleBackgroundColor,
                RingThickness = innerCircleMargin / 2,
            };

            AddSubview(ringView);

            ringView.LeftAnchor.ConstraintEqualTo(LeftAnchor).Active = true;
            ringView.RightAnchor.ConstraintEqualTo(RightAnchor).Active = true;
            ringView.TopAnchor.ConstraintEqualTo(TopAnchor).Active = true;
            ringView.BottomAnchor.ConstraintEqualTo(BottomAnchor).Active = true;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            innerCircleView.Layer.CornerRadius = innerCircleView.Bounds.Height / 2;
            ringView.RingThickness = innerCircleMargin / 2;

            SetNeedsUpdateConstraints();
        }

        public override void UpdateConstraints()
        {
            base.UpdateConstraints();

            innerCircleView.WidthAnchor.ConstraintEqualTo(WidthAnchor, constant: -(2 * innerCircleMargin)).Active = true;
            alertCountLabel.CenterYAnchor.ConstraintEqualTo(innerCircleView.CenterYAnchor, -(innerCircleView.Bounds.Height / 12)).Active = true;
        }
    }
}
