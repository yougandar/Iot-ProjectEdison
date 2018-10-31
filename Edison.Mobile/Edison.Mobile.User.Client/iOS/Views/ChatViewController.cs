using System;
using CoreGraphics;
using Edison.Mobile.iOS.Common.Views;
using Edison.Mobile.User.Client.Core.ViewModels;
using Edison.Mobile.User.Client.iOS.DataSources;
using Edison.Mobile.User.Client.iOS.Shared;
using Foundation;
using UIKit;

namespace Edison.Mobile.User.Client.iOS.Views
{
    public class ChatViewController : BaseViewController<ChatViewModel>
    {
        readonly float inputHeight = 52;

        UILabel sendMessageLabel;
        UITextView inputTextView;
        UIView borderView;
        UICollectionView messageTypeCollectionView;
        UICollectionView chatCollectionView;
        ChatCollectionViewSource chatCollectionViewSource;

        NSLayoutConstraint bottomInputTextViewConstraint;

        NSObject keyboardWillShowNotificationToken;
        NSObject keyboardWillHideNotificationToken;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            sendMessageLabel = new UILabel
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                Font = Constants.Fonts.RubikOfSize(Constants.Fonts.Size.TwentyFour),
                TextColor = Constants.Color.DarkGray,
                Text = "Send Message",
            };

            View.AddSubview(sendMessageLabel);

            sendMessageLabel.TopAnchor.ConstraintEqualTo(View.TopAnchor).Active = true;
            sendMessageLabel.LeftAnchor.ConstraintEqualTo(View.LeftAnchor, Constants.Padding).Active = true;
            sendMessageLabel.RightAnchor.ConstraintEqualTo(View.RightAnchor, -Constants.Padding).Active = true;

            inputTextView = new UITextView
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                Font = Constants.Fonts.RubikOfSize(Constants.Fonts.Size.Fourteen),
                TextColor = Constants.Color.DarkGray,
            };

            View.AddSubview(inputTextView);
            inputTextView.LeftAnchor.ConstraintEqualTo(View.LeftAnchor).Active = true;
            inputTextView.RightAnchor.ConstraintEqualTo(View.RightAnchor).Active = true;
            bottomInputTextViewConstraint = inputTextView.BottomAnchor.ConstraintEqualTo(View.BottomAnchor);
            bottomInputTextViewConstraint.Active = true;
            inputTextView.HeightAnchor.ConstraintEqualTo(inputHeight).Active = true;

            borderView = new UIView
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                BackgroundColor = Constants.Color.BackgroundGray,
            };

            View.AddSubview(borderView);
            borderView.LeftAnchor.ConstraintEqualTo(View.LeftAnchor).Active = true;
            borderView.RightAnchor.ConstraintEqualTo(View.RightAnchor).Active = true;
            borderView.BottomAnchor.ConstraintEqualTo(inputTextView.TopAnchor).Active = true;
            borderView.HeightAnchor.ConstraintEqualTo(1).Active = true;

            var messageTypeCollectionViewFlowLayout = new UICollectionViewFlowLayout
            {
                MinimumLineSpacing = 0,
                MinimumInteritemSpacing = 0,
                EstimatedItemSize = new CGSize(100, Constants.ChatMessageTypeHeight),
                ScrollDirection = UICollectionViewScrollDirection.Horizontal,
            };

            messageTypeCollectionView = new UICollectionView(CGRect.Empty, messageTypeCollectionViewFlowLayout)
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                BackgroundColor = UIColor.Clear,
                Source = new ChatMessageTypeCollectionViewSource(),
                AlwaysBounceHorizontal = true,
            };

            messageTypeCollectionView.RegisterClassForCell(typeof(ChatMessageTypeCollectionViewCell), typeof(ChatMessageTypeCollectionViewCell).Name);

            View.AddSubview(messageTypeCollectionView);
            messageTypeCollectionView.LeftAnchor.ConstraintEqualTo(View.LeftAnchor).Active = true;
            messageTypeCollectionView.RightAnchor.ConstraintEqualTo(View.RightAnchor).Active = true;
            messageTypeCollectionView.BottomAnchor.ConstraintEqualTo(borderView.TopAnchor).Active = true;
            messageTypeCollectionView.HeightAnchor.ConstraintEqualTo(Constants.ChatMessageTypeHeight).Active = true;

            var chatCollectionViewFlowLayout = new UICollectionViewFlowLayout
            {
                MinimumLineSpacing = 0,
                MinimumInteritemSpacing = 0,
                EstimatedItemSize = new CGSize(UIScreen.MainScreen.Bounds.Width, 200),
                ScrollDirection = UICollectionViewScrollDirection.Vertical,
            };

            chatCollectionViewSource = new ChatCollectionViewSource();

            chatCollectionView = new UICollectionView(CGRect.Empty, chatCollectionViewFlowLayout)
            {
                TranslatesAutoresizingMaskIntoConstraints = false,
                Source = chatCollectionViewSource,
                BackgroundView = null,
                BackgroundColor = UIColor.Clear,
                AlwaysBounceVertical = true,
            };

            chatCollectionView.RegisterClassForCell(typeof(ChatMessageCollectionViewCell), typeof(ChatMessageCollectionViewCell).Name);

            View.AddSubview(chatCollectionView);
            chatCollectionView.TopAnchor.ConstraintEqualTo(sendMessageLabel.BottomAnchor).Active = true;
            chatCollectionView.LeftAnchor.ConstraintEqualTo(View.LeftAnchor).Active = true;
            chatCollectionView.RightAnchor.ConstraintEqualTo(View.RightAnchor).Active = true;
            chatCollectionView.BottomAnchor.ConstraintEqualTo(messageTypeCollectionView.TopAnchor).Active = true;
        }

        protected override void BindEventHandlers()
        {
            base.BindEventHandlers();
            keyboardWillShowNotificationToken = UIKeyboard.Notifications.ObserveWillShow(HandleKeyboardWillShow);
            keyboardWillHideNotificationToken = UIKeyboard.Notifications.ObserveWillHide(HandleKeyboardWillHide);
            chatCollectionViewSource.Messages = ViewModel.ChatMessages;
            ViewModel.ChatMessages.CollectionChanged += HandleChatMessagesCollectionChanged;
        }

        protected override void UnBindEventHandlers()
        {
            base.UnBindEventHandlers();

            if (keyboardWillShowNotificationToken != null) NSNotificationCenter.DefaultCenter.RemoveObserver(keyboardWillShowNotificationToken);
            if (keyboardWillHideNotificationToken != null) NSNotificationCenter.DefaultCenter.RemoveObserver(keyboardWillHideNotificationToken);
            chatCollectionViewSource.Messages = null;
            ViewModel.ChatMessages.CollectionChanged -= HandleChatMessagesCollectionChanged;
        }

        public void ChatSummoned()
        {
            inputTextView.BecomeFirstResponder();
            Console.WriteLine(View.LayoutMarginsGuide.BottomAnchor);
            Console.WriteLine(View.SafeAreaInsets.Bottom);
            Console.WriteLine(View.LayoutMargins.Bottom);
        }

        public void ChatDismissing()
        {
            inputTextView.ResignFirstResponder();
        }

        public void ChatDismissed()
        {

        }

        void HandleKeyboardWillShow(object sender, UIKeyboardEventArgs e)
        {
            bottomInputTextViewConstraint.Constant = -e.FrameEnd.Height;

            UIView.BeginAnimations(null);
            UIView.SetAnimationDuration(e.AnimationDuration);
            UIView.SetAnimationCurve(e.AnimationCurve);
            UIView.SetAnimationBeginsFromCurrentState(true);

            View.LayoutIfNeeded();

            UIView.CommitAnimations();
        }

        void HandleKeyboardWillHide(object sender, UIKeyboardEventArgs e)
        {
            bottomInputTextViewConstraint.Constant = 0;

            UIView.BeginAnimations(null);
            UIView.SetAnimationDuration(e.AnimationDuration);
            UIView.SetAnimationCurve(e.AnimationCurve);
            UIView.SetAnimationBeginsFromCurrentState(true);

            View.LayoutIfNeeded();

            UIView.CommitAnimations();
        }

        void HandleChatMessagesCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            InvokeOnMainThread(() =>
            {
                // TODO: insert cells rather than reload collection
                chatCollectionView.ReloadData();
            });
        }
    }
}
