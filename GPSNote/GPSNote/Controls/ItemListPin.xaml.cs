using GPSNote.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace GPSNote.Controls
{
    public partial class ItemListPin : ViewCell
    {
        public ItemListPin()
        {
            InitializeComponent();

            bDelete.IsVisible = bEdit.IsVisible = _isMenuOpen;

            bMenu.Clicked += (s, e) =>
             {

                 if (!_isMenuOpen)
                 {
                     _isMenuOpen = true;
                     ShowMenu();
                 }
                 else
                 {
                     _isMenuOpen = false;
                     UnShowMenu();

                 }
             };
        }


        #region -- Public property -- 
        public static readonly BindableProperty TextNameProperty =
            BindableProperty.Create(nameof(TextName),
                                    typeof(string),
                                    typeof(ItemListPin),
                                    string.Empty,
                                    defaultBindingMode: BindingMode.TwoWay,
                                    propertyChanged: OnTextNameChanged);
        public string TextName
        {
            get { return (string)GetValue(TextNameProperty); }
            set { SetValue(TextNameProperty, value); }
        }


        public static readonly BindableProperty TextCoordProperty =
            BindableProperty.Create(nameof(TextCoord),
                                    typeof(string),
                                    typeof(ItemListPin),
                                    string.Empty,
                                    defaultBindingMode: BindingMode.TwoWay,
                                    propertyChanged: OnTextCoordChanged);
        public string TextCoord
        {
            get { return (string)GetValue(TextCoordProperty); }
            set { SetValue(TextCoordProperty, value); }
        }


        public static readonly BindableProperty LikeCommandProperty =
            BindableProperty.Create(nameof(LikeCommand),
                                    typeof(ICommand),
                                    typeof(ItemListPin),
                                    default(Command),
                                    defaultBindingMode: BindingMode.TwoWay,
                                    propertyChanged: OnLikeCommandChanged);
        public ICommand LikeCommand
        {
            get { return (ICommand)GetValue(LikeCommandProperty); }
            set { SetValue(LikeCommandProperty, value); }
        }

        public static readonly BindableProperty EditCommandProperty =
            BindableProperty.Create(nameof(EditCommand),
                                    typeof(ICommand),
                                    typeof(ItemListPin),
                                    default(Command),
                                    defaultBindingMode: BindingMode.TwoWay,
                                    propertyChanged: EditCommandChanged);
        public ICommand EditCommand
        {
            get { return (ICommand)GetValue(EditCommandProperty); }
            set { SetValue(EditCommandProperty, value); }
        }

        public static readonly BindableProperty DeleteCommandProperty =
            BindableProperty.Create(nameof(DeleteCommand),
                                    typeof(ICommand),
                                    typeof(ItemListPin),
                                    default(Command),
                                    defaultBindingMode: BindingMode.TwoWay,
                                    propertyChanged: DeleteCommandChanged);
        public ICommand DeleteCommand
        {
            get { return (ICommand)GetValue(DeleteCommandProperty); }
            set { SetValue(DeleteCommandProperty, value); }
        }

        public static readonly BindableProperty IsFavoritProperty =
            BindableProperty.Create(nameof(IsFavorit),
                                    typeof(bool),
                                    typeof(ItemListPin),
                                    default(bool),
                                    defaultBindingMode: BindingMode.TwoWay,
                                    propertyChanged: OnIsFavoritChanged);
        public bool IsFavorit
        {
            get { return (bool)GetValue(IsFavoritProperty); }
            set { SetValue(IsFavoritProperty, value); }
        }
        #endregion

        #region -- Private --
        private static void OnTextNameChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var itemListPin = bindable as ItemListPin;
            itemListPin.lName.Text = newValue.ToString();

        }
        private static void OnTextCoordChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var itemListPin = bindable as ItemListPin;
            itemListPin.lCoordinate.Text = newValue.ToString();

        }
        private static void OnLikeCommandChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var itemListPin = bindable as ItemListPin;

            if (newValue is ICommand command) {

                itemListPin.bLike.Command = command;
                itemListPin.bLike.CommandParameter = itemListPin.TextCoord;
            }

        }
        private static void EditCommandChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var itemListPin = bindable as ItemListPin;

            if (newValue is ICommand command)
            {

                itemListPin.bEdit.Command = command;
                itemListPin.bEdit.CommandParameter = itemListPin.TextCoord;
            }

        }

        private static void DeleteCommandChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var itemListPin = bindable as ItemListPin;

            if (newValue is ICommand command)
            {

                itemListPin.bDelete.Command = command;
                itemListPin.bDelete.CommandParameter = itemListPin.TextCoord;
            }

        }
        private static void OnIsFavoritChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var itemListPin = bindable as ItemListPin;

            if ((bool)newValue)
            {
                itemListPin.bLike.ImageSource = (ImageSource)App.Current.Resources[ImageNames.ic_like_blue];
            }
            else
            {
                itemListPin.bLike.ImageSource = (ImageSource)App.Current.Resources[ImageNames.ic_like_gray];
            }

        }

        private async void ShowMenu()
        {
            bDelete.IsVisible = bEdit.IsVisible = _isMenuOpen;
            delta = 0;
            for (int i = 0; i <= _buttonMenuWidth; i += _animationStep)
            {
                grid.Margin = new Thickness(grid.Margin.Left - (delta++), 
                                            grid.Margin.Top, 
                                            grid.Margin.Right, 
                                            grid.Margin.Bottom);
                cdDelete.Width = i;
                cdEdit.Width = i;
                await Task.Delay(1);
            }
            
        }
        private async void UnShowMenu()
        {
            
            for (int i = _buttonMenuWidth; i >= 0; i -= _animationStep)
            {
                grid.Margin = new Thickness(grid.Margin.Left + (--delta), 
                                            grid.Margin.Top, 
                                            grid.Margin.Right, 
                                            grid.Margin.Bottom);
                cdDelete.Width = i;
                cdEdit.Width = i;
                await Task.Delay(1);
            }
            bDelete.IsVisible = bEdit.IsVisible = _isMenuOpen;
        }
        private const int _buttonMenuWidth = 56;
        private bool _isMenuOpen = false;
        private const int _animationStep = 4;
        private int delta = 0;
        #endregion
    }
}