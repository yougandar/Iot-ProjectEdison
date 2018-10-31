using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Edison.Core.Common.Models;
using Edison.Mobile.Common.Shared;

namespace Edison.Mobile.User.Client.Core.ViewModels
{
    public class ChatViewModel : BaseViewModel
    {
        public ObservableRangeCollection<ReportLogModel> ChatMessages { get; } = new ObservableRangeCollection<ReportLogModel>();

        public override void ViewAppeared()
        {
            base.ViewAppeared();

            CreateChatMessages();
        }

        void CreateChatMessages()
        {
            Task.Run(async () =>
            {
                await Task.Delay(1000);

                var user = new ChatUserModel
                {
                    Id = "0",
                    Name = "John Doe",
                    Role = ChatUserRole.Consumer,
                };

                var admin = new ChatUserModel
                {
                    Id = "1",
                    Name = "Admin",
                    Role = ChatUserRole.Admin,
                };

                ChatMessages.AddRange(new ReportLogModel[]
                {
                    new ReportLogModel
                    {
                        Message = "Hi.",
                        From = user,
                    },
                    new ReportLogModel
                    {
                        Message = "Helo, how can I help?",
                        From = admin,
                    },
                    new ReportLogModel
                    {
                        Message = "This is merely a test message, meant to help develop the UX for this chat screen. Please feel free to ignore.",
                        From = user,
                    },
                    new ReportLogModel
                    {
                        Message = "Thank you. In that case, I will respond with a longer paragraph to help the developer test scenarios in which chat bubbles have multiple lines. Let's make sure things look proper.",
                        From = admin,
                    },
                });
            });
        }
    }
}
