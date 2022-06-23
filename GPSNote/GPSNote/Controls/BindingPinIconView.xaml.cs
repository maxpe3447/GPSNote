using Xamarin.Forms;

namespace GPSNote.Controls
{
    public partial class BindingPinIconView : ContentView
    {
        public BindingPinIconView(ImageSource ico)
        {
            InitializeComponent();
           IconPin = ico;
            BindingContext = this;

            
        }
        private ImageSource _iconPin;
        public ImageSource IconPin
        {
            get { return _iconPin; }
            private set { _iconPin = value; }
        }
    }
}

