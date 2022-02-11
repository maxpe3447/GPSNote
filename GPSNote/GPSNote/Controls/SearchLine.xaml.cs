﻿using GPSNote.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GPSNote.Controls
{



    public partial class SearchLine : ContentView
    {
        #region -- BindableProperty --
        public static readonly BindableProperty TextLineProperty = BindableProperty.Create(
            nameof(TextLine),
            typeof(string),
            typeof(SearchLine),
            defaultValue: string.Empty,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: TextLineChanged);

        public static readonly BindableProperty CommandSearchProperty = BindableProperty.Create(
            nameof(TextLine),
            typeof(ICommand),
            typeof(SearchLine),
            defaultValue: default(Command),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: ComandSearchChanged);

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
            nameof(ItemsSource),
            typeof(List<PinModel>),
            typeof(SearchLine),
            defaultValue: default(List<PinModel>),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: ItemsSourceChanged);

        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(
            nameof(SelectedItem),
            typeof(PinModel),
            typeof(SearchLine),
            defaultValue: default(PinModel),
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: SelectedItemChanged);

        public static readonly BindableProperty TextChangeCommandProperty = BindableProperty.Create(
            nameof(TextChangeCommand),
            typeof(ICommand),
            typeof(SearchLine),
            defaultValue: default(ICommand),
            defaultBindingMode: BindingMode.TwoWay);

        public static readonly BindableProperty ExidCommandProperty = BindableProperty.Create(
            nameof(ExidCommand),
            typeof(ICommand),
            typeof(SearchLine),
            defaultValue: default(ICommand),
            defaultBindingMode: BindingMode.TwoWay);
        #endregion

        #region -- Propirties --
        public string TextLine
        {
            get => base.GetValue(TextLineProperty)?.ToString();
            set => base.SetValue(TextLineProperty, value);
        }
        public ICommand CommandSearch
        {
            get => (ICommand)base.GetValue(TextLineProperty);
            set => base.SetValue(TextLineProperty, value);
        }

        public List<PinModel> ItemsSource
        {
            get => (List<PinModel>)base.GetValue(ItemsSourceProperty);
            set => base.SetValue(ItemsSourceProperty, value);
        }

        public PinModel SelectedItem
        {
            get => (PinModel)GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        public ICommand TextChangeCommand
        {
            get => (ICommand)GetValue(TextChangeCommandProperty);
            set=> SetValue(TextChangeCommandProperty, value);
        }

        public ICommand ExidCommand
        {
            get => (ICommand)GetValue(ExidCommandProperty);
            set => SetValue(ExidCommandProperty, value);
        }
        #endregion

        #region -- Changed --

        private static void TextLineChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (SearchLine)bindable;

            control.line.Text = newValue?.ToString();
        }

        private static void ComandSearchChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (SearchLine)bindable;

            control.searchButton.Command = (ICommand)newValue;
        }

        private static void ItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            const int maxHeightForScrollView = 250;
            var control = (SearchLine)bindable;

            var lst = (List<PinModel>)newValue;

            control.listView.ItemsSource = lst;
            int height = lst.Count * 40;
            control.listView.HeightRequest = height < maxHeightForScrollView ? height : maxHeightForScrollView;
        }

        private static void SelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (SearchLine)bindable;
            control.SelectedItem = (PinModel)newValue;
        }
        //private static void CollectionCountChanged(BindableObject bindable, object oldValue, object newValue)
        //{
        //    var control = (SearchLine)bindable;

        //    col;
        //}

        #endregion
        public SearchLine()
        {
            InitializeComponent();

            line.TextChangedEv += (s, e) =>
            {
                TextLine = e.NewTextValue;

                if (TextChangeCommand?.CanExecute(null) ?? false)
                {
                    TextChangeCommand.Execute(null);
                }
            };
            listView.ItemSelected += (s, e) =>
              {
                  SelectedItem = (PinModel)e.SelectedItem;
                  listView.HeightRequest = 0;
                  listView.ItemsSource = null;
                  ItemsSource.Clear();
                  line.Text = SelectedItem.Name;
              };

            line.FocusedEv += (s, e) =>
              {
                  const byte delta = 50;   
                  serchColumn.Width = Application.Current.MainPage.Width-delta;
              };
            line.UnFocusedEv += (s, e) =>
            {
                serchColumn.Width = GridLength.Star;
            };

            listView.HeightRequest = 0;

            searchButton.Clicked += (s, e) =>
              {
                  if (ExidCommand?.CanExecute(null) ?? false)
                  {
                      ExidCommand.Execute(null);
                  }
              };
        }
    }
}