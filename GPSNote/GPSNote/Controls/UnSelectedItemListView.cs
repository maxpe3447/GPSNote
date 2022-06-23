using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace GPSNote.Controls
{
    public class UnSelectedItemListView : ListView
    {
        public UnSelectedItemListView():base()
        {
            this.ItemSelected += OnUnSelectedItemListView_ItemSelected;
        }

        private void OnUnSelectedItemListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            SelectedItem = null;
        }
    }
}
