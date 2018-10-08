using System;
using System.Linq;
using CoreGraphics;
using Edison.Core.Common.Models;
using Edison.Mobile.iOS.Common.Shared;
using Edison.Mobile.User.Client.iOS.Shared;
using UIKit;

namespace Edison.Mobile.User.Client.iOS.Views
{
    public class ResponseDetailsView : UIView
    {
        readonly UILabel titleLabel;
        readonly UIImageView titleLogoImageView;
        readonly UILabel notificationLabel;
        readonly UILabel contentLabel;
        readonly UIButton moreInfoButton;

        EventClusterModel eventCluster;

        public EventClusterModel EventCluster
        {
            get => eventCluster;
            set
            {
                eventCluster = value;
                if (eventCluster != null) UpdateLabels();
            }
        }

        public ResponseDetailsView()
        {
            BackgroundColor = PlatformConstants.Color.White;



            var topBarVerticalPadding = 6f;
            var topBarView = new UIView { TranslatesAutoresizingMaskIntoConstraints = false };
            AddSubview(topBarView);
            topBarView.LeftAnchor.ConstraintEqualTo(LeftAnchor).Active = true;
            topBarView.RightAnchor.ConstraintEqualTo(RightAnchor).Active = true;
            topBarView.TopAnchor.ConstraintEqualTo(TopAnchor).Active = true;
            topBarView.HeightAnchor.ConstraintEqualTo(HeightAnchor, multiplier: 0.24f).Active = true;
            //topBarView.BackgroundColor = UIColor.Orange;

            titleLogoImageView = new UIImageView
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                BackgroundColor = PlatformConstants.Color.LightGray,
            };
            topBarView.AddSubview(titleLogoImageView);
            titleLogoImageView.LeftAnchor.ConstraintEqualTo(topBarView.LeftAnchor, Constants.Padding).Active = true;
            titleLogoImageView.CenterYAnchor.ConstraintEqualTo(topBarView.CenterYAnchor).Active = true;
            titleLogoImageView.HeightAnchor.ConstraintEqualTo(topBarView.HeightAnchor, multiplier: 0.723f).Active = true;
            titleLogoImageView.WidthAnchor.ConstraintEqualTo(titleLogoImageView.HeightAnchor).Active = true;

            titleLabel = new UILabel
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                Font = Constants.Fonts.RubikMediumOfSize(Constants.Fonts.Size.Fourteen),
                TextColor = PlatformConstants.Color.DarkGray,
            };
            topBarView.AddSubview(titleLabel);
            titleLabel.LeftAnchor.ConstraintEqualTo(titleLogoImageView.RightAnchor, 8f).Active = true;
            titleLabel.RightAnchor.ConstraintEqualTo(topBarView.RightAnchor, -Constants.Padding).Active = true;
            titleLabel.CenterYAnchor.ConstraintEqualTo(topBarView.CenterYAnchor).Active = true;

            notificationLabel = new UILabel
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                Font = Constants.Fonts.RubikOfSize(Constants.Fonts.Size.Eight),
                TextColor = PlatformConstants.Color.MidGray,
            };
            AddSubview(notificationLabel);
            notificationLabel.LeftAnchor.ConstraintEqualTo(LeftAnchor, Constants.Padding).Active = true;
            notificationLabel.TopAnchor.ConstraintEqualTo(topBarView.BottomAnchor, 3f).Active = true;
            notificationLabel.RightAnchor.ConstraintEqualTo(RightAnchor, -Constants.Padding).Active = true;
            notificationLabel.SetContentHuggingPriority(1000, UILayoutConstraintAxis.Vertical);
            //notificationLabel.BackgroundColor = UIColor.Blue;

            moreInfoButton = new UIButton { TranslatesAutoresizingMaskIntoConstraints = false, UserInteractionEnabled = false };
            moreInfoButton.SetTitle("MORE INFO", UIControlState.Normal);
            moreInfoButton.TitleLabel.Font = Constants.Fonts.RubikOfSize(Constants.Fonts.Size.Fourteen);
            moreInfoButton.SetTitleColor(PlatformConstants.Color.Blue, UIControlState.Normal);
            AddSubview(moreInfoButton);
            moreInfoButton.LeftAnchor.ConstraintEqualTo(LeftAnchor).Active = true;
            moreInfoButton.RightAnchor.ConstraintEqualTo(RightAnchor).Active = true;
            moreInfoButton.BottomAnchor.ConstraintEqualTo(BottomAnchor).Active = true;
            moreInfoButton.HeightAnchor.ConstraintEqualTo(34).Active = true;

            contentLabel = new UILabel
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                Lines = 0,
                LineBreakMode = UILineBreakMode.WordWrap,
                Font = Constants.Fonts.RubikMediumOfSize(Constants.Fonts.Size.Ten),
                TextColor = PlatformConstants.Color.DarkGray,
            };
            AddSubview(contentLabel);
            contentLabel.LeftAnchor.ConstraintEqualTo(LeftAnchor, Constants.Padding).Active = true;
            contentLabel.RightAnchor.ConstraintEqualTo(RightAnchor, -Constants.Padding).Active = true;
            contentLabel.TopAnchor.ConstraintEqualTo(notificationLabel.BottomAnchor, topBarVerticalPadding).Active = true;
            contentLabel.BottomAnchor.ConstraintLessThanOrEqualTo(moreInfoButton.TopAnchor, -topBarVerticalPadding).Active = true;
            //contentLabel.BackgroundColor = UIColor.Cyan;
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            titleLogoImageView.Layer.CornerRadius = titleLogoImageView.Bounds.Height / 2;
        }

        void UpdateLabels()
        {
            titleLabel.Text = eventCluster.EventType;

            if (eventCluster.Events.ToList().First() is EventModel firstEvent)
            {
                contentLabel.Text = "";
                foreach (var pair in firstEvent.Metadata) 
                {
                    contentLabel.Text += $"{pair.Key} - {pair.Value}\n";
                }
            }

            notificationLabel.Text = eventCluster.UpdateDate.ToString();
        }
    }
}
