using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Edison.Mobile.User.Client.Core.CollectionItemViewModels;
using Edison.Mobile.User.Client.Core.ViewModels;
using Edison.Mobile.User.Client.iOS.Views;
using Foundation;
using UIKit;

namespace Edison.Mobile.User.Client.iOS.DataSources
{
    public class ResponsesCollectionViewSource : UICollectionViewSource, IUICollectionViewDataSourcePrefetching
    {
        readonly ObservableRangeCollection<ResponseCollectionItemViewModel> responses;

        public ResponsesCollectionViewSource(ObservableRangeCollection<ResponseCollectionItemViewModel> responses) 
        {
            this.responses = responses;
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = collectionView.DequeueReusableCell(typeof(ResponseCollectionViewCell).Name, indexPath) as ResponseCollectionViewCell;
            cell.ViewModel = responses[(int)indexPath.Item];
            cell.Initialize();
            return cell;
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            return responses.Count;
        }

        public void PrefetchItems(UICollectionView collectionView, NSIndexPath[] indexPaths)
        {
            foreach (var indexPath in indexPaths) 
            {
                var cell = collectionView.DequeueReusableCell(typeof(ResponseCollectionViewCell).Name, indexPath) as ResponseCollectionViewCell;
                var viewModel = responses[(int)indexPath.Item];
                Task.Run(async () => await viewModel.GetPrimaryEventCluster());
            }
        }
    }
}
