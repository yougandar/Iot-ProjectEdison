using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Edison.Mobile.User.Client.iOS.Layout
{
    public class ChatCollectionViewLayout : UICollectionViewLayout
    {
        readonly UICollectionViewLayoutAttributes[] cache = new UICollectionViewLayoutAttributes[] { };

        nfloat contentWidth;
        nfloat contentHeight;

        public override CGSize CollectionViewContentSize => new CGSize(contentWidth, contentHeight);

        public override void PrepareLayout()
        {
               
        }

        public override UICollectionViewLayoutAttributes[] LayoutAttributesForElementsInRect(CGRect rect)
        {
            return base.LayoutAttributesForElementsInRect(rect);
        }

        public override UICollectionViewLayoutAttributes LayoutAttributesForItem(NSIndexPath indexPath)
        {
            return base.LayoutAttributesForItem(indexPath);
        }
    }
}
