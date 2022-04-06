using GPSNote.Models;
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
        public static readonly BindableProperty TextLineProperty = 
            BindableProperty.Create(nameof(TextLine),
                                    typeof(string),
                                    typeof(SearchLine),
                                    defaultValue: string.Empty,
                                    defaultBindingMode: BindingMode.TwoWay,
                                    propertyChanged: TextLineChanged);

        //public static readonly BindableProperty CommandSearchProperty = 
        //    BindableProperty.Create(nameof(CommandSearch),
        //                            typeof(ICommand),
        //                            typeof(SearchLine),
        //                            defaultValue: default(Command),
        //                            defaultBindingMode: BindingMode.TwoWay,
        //                            propertyChanged: CommandSearchChanged);

        public static readonly BindableProperty ItemsSourceProperty = 
            BindableProperty.Create(nameof(ItemsSource),
                                    typeof(List<PinViewModel>),
                                    typeof(SearchLine),
                                    defaultValue: default(List<PinViewModel>),
                                    defaultBindingMode: BindingMode.TwoWay,
                                    propertyChanged: ItemsSourceChanged);

        public static readonly BindableProperty SelectedItemProperty = 
            BindableProperty.Create(nameof(SelectedItem),
                                    typeof(PinViewModel),
                                    typeof(SearchLine),
                                    defaultValue: default(PinViewModel),
                                    defaultBindingMode: BindingMode.TwoWay,
                                    propertyChanged: SelectedItemChanged);

        public static readonly BindableProperty TextChangeCommandProperty = 
            BindableProperty.Create(nameof(TextChangeCommand),
                                    typeof(ICommand),
                                    typeof(SearchLine),
                                    defaultValue: default(ICommand),
                                    defaultBindingMode: BindingMode.TwoWay);

        public static readonly BindableProperty ExidCommandProperty = 
            BindableProperty.Create(nameof(ExidCommand),
                                    typeof(ICommand),
                                    typeof(SearchLine),
                                    defaultValue: default(ICommand),
                                    defaultBindingMode: BindingMode.TwoWay);

        public static readonly BindableProperty SettingsCommandProperty =
            BindableProperty.Create(nameof(SettingsCommand),
                                    typeof(ICommand),
                                    typeof(SearchLine),
                                    defaultValue: default(ICommand),
                                    defaultBindingMode: BindingMode.TwoWay,
                                    propertyChanged: OnSettingsCommandChanged);

        public static readonly BindableProperty KeyBoardProperty =
            BindableProperty.Create(nameof(KeyBoard),
                                    typeof(Keyboard),
                                    typeof(SearchLine),
                                    default(Keyboard),
                                    defaultBindingMode: BindingMode.TwoWay,
                                    propertyChanged: OnKeyBoardChanged);

        public static readonly BindableProperty FontFamilyProperty =
            BindableProperty.Create(nameof(FontFamily),
                                    typeof(string),
                                    typeof(SearchLine),
                                    string.Empty,
                                    defaultBindingMode: BindingMode.TwoWay,
                                    propertyChanged: OnFontFamilyChanged);

        #endregion

        #region -- Propirties --
        public string FontFamily
        {
            get { return (string)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }
        public string TextLine
        {
            get => base.GetValue(TextLineProperty)?.ToString();
            set => base.SetValue(TextLineProperty, value);
        }

        public List<PinViewModel> ItemsSource
        {
            get => (List<PinViewModel>)base.GetValue(ItemsSourceProperty);
            set => base.SetValue(ItemsSourceProperty, value);
        }

        public PinViewModel SelectedItem
        {
            get => (PinViewModel)GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        public Keyboard KeyBoard
        {
            get { return (Keyboard)GetValue(KeyBoardProperty); }
            set { SetValue(KeyBoardProperty, value); }
        }

        //public ICommand CommandSearch
        //{
        //    get => (ICommand)base.GetValue(CommandSearchProperty);
        //    set => base.SetValue(CommandSearchProperty, value);
        //}

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

        public ICommand SettingsCommand
        {
            get => (ICommand)GetValue(SettingsCommandProperty);
            set => SetValue(SettingsCommandProperty, value);
        }
        #endregion

        #region -- Changed --

        private static void TextLineChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (SearchLine)bindable;

            control.line.Text = newValue?.ToString();
        }

        private static void OnSettingsCommandChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (SearchLine)bindable;

            control.bSettingOrBack.Command = (ICommand)newValue;
        }

        private static void ItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            const int maxHeightForScrollView = 250;
            var control = (SearchLine)bindable;

            var lst = (List<PinViewModel>)newValue;

            control.listView.ItemsSource = lst;
            int height = lst.Count * 50;
            control.listView.HeightRequest = height < maxHeightForScrollView ? height : maxHeightForScrollView;
        }

        private static void SelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (SearchLine)bindable;
            control.SelectedItem = (PinViewModel)newValue;
        }
        
        private static void OnKeyBoardChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var searckLine = (SearchLine)bindable;
            searckLine.KeyBoard = (Keyboard)newValue;

        }
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
                  SelectedItem = (PinViewModel)e.SelectedItem;
                  listView.HeightRequest = 0;
                  listView.ItemsSource = null;
                  ItemsSource.Clear();
                  line.Text = SelectedItem.Name;
              };

            line.FocusedEv += (s, e) =>
              {
                  const byte delta = 80;   
                  serchColumn.Width = Application.Current.MainPage.Width-delta;
                  bExit.IsEnabled = false;
              };
            line.UnFocusedEv += (s, e) =>
            {
                serchColumn.Width = GridLength.Star;
                bExit.IsEnabled = true;

            };

            listView.HeightRequest = 0;

            bExit.Clicked += (s, e) =>
              {
                  if (ExidCommand?.CanExecute(null) ?? false)
                  {
                      ExidCommand.Execute(null);
                  }
              };

            
        }
        
        private static void OnFontFamilyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var passLine = (SearchLine)bindable;
            passLine.line.FontFamily = newValue.ToString();
        }
    }
}