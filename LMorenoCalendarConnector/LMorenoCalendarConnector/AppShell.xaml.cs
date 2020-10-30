using System;
using System.Collections.Generic;
using LMorenoCalendarConnector.ViewModels;
using LMorenoCalendarConnector.Views;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;

namespace LMorenoCalendarConnector
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            var _permissionsUtil = CrossPermissions.Current;// DependencyService.Get<IPermissions>();
            //await _permissionsUtil.RequestPermissionsAsync(new Permission[] { Permission.Calendar, Permission.Contacts, Permission.Storage });

        }

    }
}
