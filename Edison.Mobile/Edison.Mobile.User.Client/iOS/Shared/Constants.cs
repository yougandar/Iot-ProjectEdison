using System;
using UIKit;

namespace Edison.Mobile.User.Client.iOS.Shared
{
    public static class Constants
    {
        public static readonly string ClientId = "fc3bf201-80f6-404d-9a70-8c19b8774a8b"; // edison ios user 
        //public static readonly string ClientId = "1133966b-9c18-4edb-9efc-0d0c01494e6b"; // web app

        public static readonly nfloat PulloutTopMargin = 140;
        public static readonly nfloat PulloutBottomMargin = 220;
        public static readonly nfloat PulloutVelocityThreshold = 200;
        public static readonly nfloat PulloutTopBarHeight = 30;
        public static readonly nfloat CornerRadius = 20;

        public static nfloat MenuRightMargin { get; set; } = 100;

        public static readonly nfloat Padding = 16;

        public static nfloat MenuCellHeight = 44f;

        public static class Assets 
        {
            public static UIImage Menu => UIImage.FromBundle("Menu");
            public static UIImage Brightness => UIImage.FromBundle("Brightness");
            public static UIImage Emergency => UIImage.FromBundle("Emergency");
            public static UIImage Chat => UIImage.FromBundle("Chat");
            public static UIImage Person => UIImage.FromBundle("Person");
            public static UIImage Logo => UIImage.FromBundle("Logo");
        }

        public static class Fonts 
        {
            public static UIFont RubikOfSize(float size) => UIFont.FromName("Rubik-Regular", size);
            public static UIFont RubikMediumOfSize(float size) => UIFont.FromName("Rubik-Medium", size);

            public static class Size 
            {
                public static float Eight = 8;
                public static float Ten = 10;
                public static float Twelve = 12;
                public static float Fourteen = 14;
                public static float Eighteen = 18;
                public static float TwentyFour = 24;
                public static float SeventyTwo = 72;
            }
        }
    }
}
