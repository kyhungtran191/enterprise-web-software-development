using System.ComponentModel;

namespace Server.Domain.Common.Constants
{
    public static class Permissions
    {
        public static class Dashboard
        {
            [Description("View dashboard")]
            public const string View = "Permissions.Dashboard.View";
        }

        public static class Roles
        {
            [Description("View Roles")]
            public const string View = "Permissions.Roles.View";

            [Description("Create New Role")]
            public const string Create = "Permissions.Roles.Create";

            [Description("Edit Role")]
            public const string Edit = "Permissions.Roles.Edit";

            [Description("Delete Role")]
            public const string Delete = "Permissions.Roles.Delete";
        }

        public static class Users
        {
            [Description("View Users")]
            public const string View = "Permissions.Users.View";

            [Description("Create New User")]
            public const string Create = "Permissions.Users.Create";

            [Description("Edit User")]
            public const string Edit = "Permissions.Users.Edit";

            [Description("Delete User")]
            public const string Delete = "Permissions.Users.Delete";
        }

          public static class Faculties
        {
            [Description("View Faculties")]
            public const string View = "Permissions.Faculties.View";

            [Description("Create New Faculty")]
            public const string Create = "Permissions.Faculties.Create";

            [Description("Edit Faculty")]
            public const string Edit = "Permissions.Faculties.Edit";

            [Description("Delete Faculty")]
            public const string Delete = "Permissions.Faculties.Delete";
        }



        public static class Tags
        {
            [Description("View Tags")]
            public const string View = "Permissions.Tags.View";

            [Description("Create Tags")]
            public const string Create = "Permissions.Tags.Create";

            [Description("Edit Tags")]
            public const string Edit = "Permissions.Tags.Edit";

            [Description("Delete Tags")]
            public const string Delete = "Permissions.Tags.Delete";
        }
        public static class AcademicYears
        {
            [Description("View AcademicYears")]
            public const string View = "Permissions.AcademicYears.View";

            [Description("Create AcademicYears")]
            public const string Create = "Permissions.AcademicYears.Create";

            [Description("Edit AcademicYears")]
            public const string Edit = "Permissions.AcademicYears.Edit";

            [Description("Delete AcademicYears")]
            public const string Delete = "Permissions.AcademicYears.Delete";
        }

        // public static class Posts
        // {
        //     [Description("Xem bài viết")]
        //     public const string View = "Permissions.Posts.View";

        //     [Description("Tạo bài viết")]
        //     public const string Create = "Permissions.Posts.Create";

        //     [Description("Sửa bài viết")]
        //     public const string Edit = "Permissions.Posts.Edit";

        //     [Description("Xóa bài viết")]
        //     public const string Delete = "Permissions.Posts.Delete";

        //     [Description("Duyệt bài viết")]
        //     public const string Approve = "Permissions.Posts.Approve";
        // }



        // public static class Royalty
        // {
        //     [Description("Xem nhuận bút")]
        //     public const string View = "Permissions.Royalty.View";

        //     [Description("Thanh toán nhuận bút")]
        //     public const string Pay = "Permissions.Royalty.Pay";
        // }
    }
}