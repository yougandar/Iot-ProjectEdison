using System;
using System.Collections.ObjectModel;
using Edison.Core.Common.Models;
using Edison.Mobile.User.Client.iOS.Shared;
using Edison.Mobile.User.Client.iOS.Views;
using Foundation;
using UIKit;

namespace Edison.Mobile.User.Client.iOS.DataSources
{
    public class ChatCollectionViewSource : UICollectionViewSource
    {
        public ObservableRangeCollection<ReportLogModel> Messages { get; set; }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = collectionView.DequeueReusableCell(typeof(ChatMessageCollectionViewCell).Name, indexPath) as ChatMessageCollectionViewCell;
            var message = Messages[(int)indexPath.Item];
            cell.Initialize(message, indexPath.Item == 0 ? new ChatMessageType 
            {
                SelectedIconImage = Constants.Assets.EmergencyWhite,
                SelectionColor = Constants.Color.Red,
                Title = "EMERGENCY",
            } : null);

            return cell;
        }

        public override nint NumberOfSections(UICollectionView collectionView)
        {
            return 1;
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            return Messages?.Count ?? 0;
        }
    }
}
