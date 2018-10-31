using System;
using Edison.Mobile.User.Client.iOS.Shared;
using UIKit;
namespace Edison.Mobile.User.Client.iOS.Views
{
    public class ResponseUpdateTableViewCell : UITableViewCell
    {
        readonly float dotSize = 12;
        readonly float cellPadding = 8;
        bool isInitialized;

        UILabel headerLabel;
        UILabel contentLabel;
        UIView dotView;
        UIView aboveDotLineView;
        UIView belowDotLineView;

        public ResponseUpdateTableViewCell(IntPtr handle) : base(handle) { }

        public void Initialize(string content, bool showTopLine = true)
        {
            if (!isInitialized)
            {
                BackgroundColor = Constants.Color.BackgroundDarkGray;

                headerLabel = new UILabel
                {
                    TranslatesAutoresizingMaskIntoConstraints = false,
                    Lines = 1,
                    Font = Constants.Fonts.RubikOfSize(Constants.Fonts.Size.Twelve),
                    TextColor = Constants.Color.LightGray,
                    BackgroundColor = UIColor.Clear,
                    Text = "MESSAGE - 9:41 PM",
                };

                ContentView.AddSubview(headerLabel);
                headerLabel.TopAnchor.ConstraintEqualTo(ContentView.TopAnchor, cellPadding * 2).Active = true;
                headerLabel.RightAnchor.ConstraintEqualTo(ContentView.RightAnchor, -cellPadding).Active = true;

                dotView = new UIView
                {
                    TranslatesAutoresizingMaskIntoConstraints = false,
                    BackgroundColor = Constants.Color.LightGray,
                };

                ContentView.AddSubview(dotView);
                dotView.LeftAnchor.ConstraintEqualTo(ContentView.LeftAnchor, cellPadding).Active = true;
                dotView.TopAnchor.ConstraintEqualTo(ContentView.TopAnchor, cellPadding * 2).Active = true;
                dotView.WidthAnchor.ConstraintEqualTo(dotSize).Active = true;
                dotView.HeightAnchor.ConstraintEqualTo(dotSize).Active = true;
                dotView.Layer.CornerRadius = dotSize / 2;

                headerLabel.LeftAnchor.ConstraintEqualTo(dotView.RightAnchor, cellPadding).Active = true;
                headerLabel.CenterYAnchor.ConstraintEqualTo(dotView.CenterYAnchor).Active = true;

                aboveDotLineView = new UIView
                {
                    TranslatesAutoresizingMaskIntoConstraints = false,
                    BackgroundColor = Constants.Color.DarkGray,
                    Alpha = showTopLine ? 1 : 0,
                };

                ContentView.InsertSubviewBelow(aboveDotLineView, dotView);
                aboveDotLineView.TopAnchor.ConstraintEqualTo(ContentView.TopAnchor).Active = true;
                aboveDotLineView.BottomAnchor.ConstraintEqualTo(dotView.CenterYAnchor).Active = true;
                aboveDotLineView.CenterXAnchor.ConstraintEqualTo(dotView.CenterXAnchor).Active = true;
                aboveDotLineView.WidthAnchor.ConstraintEqualTo(1).Active = true;

                belowDotLineView = new UIView
                {
                    TranslatesAutoresizingMaskIntoConstraints = false,
                    BackgroundColor = Constants.Color.DarkGray,
                };

                ContentView.InsertSubviewBelow(belowDotLineView, dotView);
                belowDotLineView.TopAnchor.ConstraintEqualTo(dotView.CenterYAnchor).Active = true;
                belowDotLineView.BottomAnchor.ConstraintEqualTo(ContentView.BottomAnchor).Active = true;
                belowDotLineView.CenterXAnchor.ConstraintEqualTo(dotView.CenterXAnchor).Active = true;
                belowDotLineView.WidthAnchor.ConstraintEqualTo(1).Active = true;

                contentLabel = new UILabel
                {
                    TranslatesAutoresizingMaskIntoConstraints = false,
                    Lines = 0,
                    LineBreakMode = UILineBreakMode.WordWrap,
                    Font = Constants.Fonts.RubikOfSize(Constants.Fonts.Size.Twelve),
                    TextColor = Constants.Color.White,
                    BackgroundColor = UIColor.Clear,
                };

                ContentView.AddSubview(contentLabel);

                contentLabel.TopAnchor.ConstraintEqualTo(headerLabel.BottomAnchor, cellPadding).Active = true;
                contentLabel.BottomAnchor.ConstraintEqualTo(ContentView.BottomAnchor).Active = true;
                contentLabel.LeftAnchor.ConstraintEqualTo(headerLabel.LeftAnchor).Active = true;
                contentLabel.RightAnchor.ConstraintEqualTo(ContentView.RightAnchor, -cellPadding).Active = true;

                SelectionStyle = UITableViewCellSelectionStyle.None;

                isInitialized = true;
            }

            contentLabel.Text = content;
        }
    }
}
