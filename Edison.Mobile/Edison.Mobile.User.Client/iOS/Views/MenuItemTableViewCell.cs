using System;
using Edison.Mobile.iOS.Common.Shared;
using Edison.Mobile.User.Client.iOS.Shared;
using UIKit;

namespace Edison.Mobile.User.Client.iOS.Views
{
    public class MenuItemTableViewCell : BaseMenuTableViewCell
    {
        public MenuItemTableViewCell(IntPtr handle) : base(handle) { }

        public void Initialize(string title, float fontSize = 16)
        {
            if (!isInitialized)
            {
                titleLabel = new UILabel
                {
                    TranslatesAutoresizingMaskIntoConstraints = false,
                    TextColor = PlatformConstants.Color.White,
                    Font = Constants.Fonts.RubikOfSize(fontSize),
                };

                ContentView.AddSubview(titleLabel);

                titleLabel.LeftAnchor.ConstraintEqualTo(ContentView.LeftAnchor, (ContentView.Bounds.Width / 2) + Constants.Padding).Active = true;
                titleLabel.RightAnchor.ConstraintEqualTo(ContentView.RightAnchor).Active = true;
                titleLabel.CenterYAnchor.ConstraintEqualTo(ContentView.CenterYAnchor).Active = true;

                isInitialized = true;
            }

            titleLabel.Text = title;
        }
    }
}
