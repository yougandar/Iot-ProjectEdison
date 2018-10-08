using CoreGraphics;
using Edison.Mobile.iOS.Common.Shared;
using Edison.Mobile.iOS.Common.Views;
using Edison.Mobile.User.Client.Core.ViewModels;
using Edison.Mobile.User.Client.iOS.DataSources;
using Edison.Mobile.User.Client.iOS.Views;
using UIKit;

namespace Edison.Mobile.User.Client.iOS.ViewControllers
{
    public class MenuViewController : BaseViewController<MenuViewModel>
    {
        readonly float maxShadowOpacity = 0.45f;

        UITableView tableView;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = PlatformConstants.Color.White;
            View.Layer.MasksToBounds = false;
            View.Layer.ShadowColor = PlatformConstants.Color.Black.CGColor;
            View.Layer.ShadowOffset = new CGSize(2, 2);
            View.Layer.ShadowRadius = 5;

            View.BackgroundColor = PlatformConstants.Color.DarkBlue;

            tableView = new UITableView
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                AlwaysBounceVertical = true,
                Source = new MenuTableViewSource { ViewModel = ViewModel },
                SeparatorStyle = UITableViewCellSeparatorStyle.None,
                TableFooterView = new UIView(),
                BackgroundColor = UIColor.Clear,
                DelaysContentTouches = false,
            };

            tableView.RegisterClassForCellReuse(typeof(MenuProfileTableViewCell), typeof(MenuProfileTableViewCell).Name);
            tableView.RegisterClassForCellReuse(typeof(MenuItemTableViewCell), typeof(MenuItemTableViewCell).Name);
            tableView.RegisterClassForCellReuse(typeof(MenuSeparatorTableViewCell), typeof(MenuSeparatorTableViewCell).Name);

            View.AddSubview(tableView);

            tableView.RightAnchor.ConstraintEqualTo(View.RightAnchor).Active = true;
            tableView.TopAnchor.ConstraintEqualTo(View.TopAnchor).Active = true;
            tableView.BottomAnchor.ConstraintEqualTo(View.BottomAnchor).Active = true;
            tableView.LeftAnchor.ConstraintEqualTo(View.LeftAnchor).Active = true;
        }

        public void SetPercentMaximized(float percent)
        {
            View.Layer.ShadowOpacity = maxShadowOpacity * percent;
        }
    }
}
