using System;
using Edison.Mobile.iOS.Common.Shared;
using Edison.Mobile.User.Client.Core.ViewModels;
using Edison.Mobile.User.Client.iOS.Shared;
using Edison.Mobile.User.Client.iOS.Views;
using Foundation;
using UIKit;

namespace Edison.Mobile.User.Client.iOS.DataSources
{
    public class MenuTableViewSource : UITableViewSource
    {
        WeakReference<MenuViewModel> viewModel;

        public MenuViewModel ViewModel
        {
            get
            {
                viewModel.TryGetTarget(out MenuViewModel vm);
                return vm;
            }
            set
            {
                viewModel = new WeakReference<MenuViewModel>(value);
            }
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            UITableViewCell cell;
            switch (indexPath.Row)
            {
                case 0:
                    {
                        cell = tableView.DequeueReusableCell(typeof(MenuProfileTableViewCell).Name, indexPath) as MenuProfileTableViewCell;
                        (cell as MenuProfileTableViewCell).Initialize(ViewModel?.ProfileName);
                    }
                    break;
                case 1:
                case 2:
                    {
                        cell = tableView.DequeueReusableCell(typeof(MenuItemTableViewCell).Name, indexPath) as MenuItemTableViewCell;
                        (cell as MenuItemTableViewCell).Initialize(indexPath.Row == 1 ? "My Info" : "Notifications");
                    }
                    break;
                case 3:
                    {
                        cell = tableView.DequeueReusableCell(typeof(MenuSeparatorTableViewCell).Name, indexPath) as MenuSeparatorTableViewCell;
                        (cell as MenuSeparatorTableViewCell).Initialize();
                    }
                    break;
                default:
                    {
                        cell = tableView.DequeueReusableCell(typeof(MenuItemTableViewCell).Name, indexPath) as MenuItemTableViewCell;
                        (cell as MenuItemTableViewCell).Initialize("Sign Out", 12);
                    }
                    break;
            }

            cell.ContentView.BackgroundColor = UIColor.Clear;
            cell.BackgroundColor = UIColor.Clear;
            cell.SelectionStyle = UITableViewCellSelectionStyle.None;

            return cell;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return 5;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            if (indexPath.Row == 0) return 112;
            if (indexPath.Row == 3) return Constants.MenuCellHeight / 2;
            return indexPath.Row == 0 ? 112 : Constants.MenuCellHeight;
        }

        public override void RowHighlighted(UITableView tableView, NSIndexPath rowIndexPath)
        {
            var cell = tableView.CellAt(rowIndexPath);
            cell.Alpha = 0.6f;
        }

        public override void RowUnhighlighted(UITableView tableView, NSIndexPath rowIndexPath)
        {
            var cell = tableView.CellAt(rowIndexPath);
            UIView.Animate(PlatformConstants.AnimationDuration, () => cell.Alpha = 1);
        }
    }
}
