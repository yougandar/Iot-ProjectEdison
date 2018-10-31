using System;
using CoreGraphics;
using Edison.Mobile.User.Client.iOS.Views;
using Foundation;
using UIKit;

namespace Edison.Mobile.User.Client.iOS.DataSources
{
    public class TableViewScrolledEventArgs : EventArgs 
    {
        public CGPoint ContentOffset { get; set; }
    }

    public class ResponseUpdatesTableViewSource : UITableViewSource
    {
        public event EventHandler<TableViewScrolledEventArgs> OnTableViewScrolled;

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(typeof(ResponseUpdateTableViewCell).Name, indexPath) as ResponseUpdateTableViewCell;
            cell.Initialize("testing stuff ou tand stuf f testing stuff ou tand stuf ftesting stuff ou tand stuf ftesting stuff ou tand stuf ftesting stuff ou tand stuf ftesting stuff ou tand stuf ftesting stuff ou tand stuf f,", indexPath.Row != 0);
            return cell;
        }

        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            return 20;
        }

        public override void Scrolled(UIScrollView scrollView)
        {
            OnTableViewScrolled?.Invoke(this, new TableViewScrolledEventArgs
            {
                ContentOffset = scrollView.ContentOffset,
            });
        }

        //public override UIView GetViewForHeader(UITableView tableView, nint section)
        //{
        //    var headerCell = tableView.DequeueReusableHeaderFooterView(typeof(ResponseUpdateTableViewHeaderCell).Name) as ResponseUpdateTableViewHeaderCell;
        //    headerCell.Initialize();
        //    return headerCell;
        //}
    }
}
