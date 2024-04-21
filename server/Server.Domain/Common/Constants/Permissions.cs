using System.ComponentModel;
using System.Data.Common;

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
            [Description("Active AcademicYears")]
            public const string Activate = "Permissions.AcademicYears.Activate";
            [Description("Inactive AcademicYears")]
            public const string Inactivate = "Permissions.AcademicYears.Inactivate";
        }

        public static class Contributions
        {
            [Description("View Contributions")]
            public const string View = "Permissions.Contributions.View";

            [Description("Create Contributions")]
            public const string Create = "Permissions.Contributions.Create";

            [Description("Edit Contributions")]
            public const string Edit = "Permissions.Contributions.Edit";

            [Description("Delete Contributions")]
            public const string Delete = "Permissions.Contributions.Delete";

            [Description("Approve Contributions")]
            public const string Approve = "Permissions.Contributions.Approve";

            [Description("Download file")]
            public const string Download = "Permissions.Contributions.Download";
        }

        public static class StudentDashBoard
        {
            [Description("View Student Dashboard")]
            public const string View = "Permissions.StudentDashboard.View";
        }

        public static class StudentContribution
        {
            [Description("View Student Contribution")]
            public const string View = "Permissions.StudentContribution.View";
        }

        public static class AddContribution
        {
            [Description("View Add Contribution")]
            public const string View = "Permissions.AddContribution.View";
        }

        public static class EditContribution
        {
            [Description("View Edit Contribution")]
            public const string View = "Permissions.EditContribution.View";
        }

        public static class FavoriteContribution
        {
            [Description("View Favorite Contribution")]
            public const string View = "Permissions.FavoriteContribution.View";
        }

        public static class ReadLaterContribution
        {
            [Description("View Read Later Contribution")]
            public const string View = "Permissions.ReadLaterContribution.View";
        }

        public static class ManageContribution
        {
            [Description("View Manage Contribution")]
            public const string View = "Permissions.ManageContributions.View";
        }

        public static class SettingGAC
        {
            [Description("View SettingGAC")] 
            public const string View = "Permissions.SettingGAC.View";
        }

        public static class PreviewContribution
        {
            [Description("View Preview Contribution")]
            public const string View = "Permissions.PreviewContribution.View";
        }

        public static class ActivityLogs
        {
            [Description("View Activity Logs")] public const string View = "Permissions.ActivityLogs.View";
        }

        public static class NotCommentContribution
        {
            [Description("View Not Comment Contribution")]
            public const string View = "Permissions.NotCommentContribution.View";
        }
        // public static class Royalty
        // {
        //     [Description("Xem nhuận bút")]
        //     public const string View = "Permissions.Royalty.View";

        //     [Description("Thanh toán nhuận bút")]
        //     public const string Pay = "Permissions.Royalty.Pay";
        // }
    }
}