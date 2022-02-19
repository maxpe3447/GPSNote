using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GPSNote.Controls
{
    public class UnSelectedItemListView : ListView
    {
        public UnSelectedItemListView():base()
        {
            this.ItemSelected += (s, e) =>
            {
                SelectedItem = null;
            };
        }
    }
}
