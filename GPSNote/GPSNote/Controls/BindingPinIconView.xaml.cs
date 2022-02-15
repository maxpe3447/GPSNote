using Xamarin.Forms;

namespace GPSNote.Controls
{
    public partial class BindingPinIconView : StackLayout
    {
        public BindingPinIconView(string ico)
        {
            InitializeComponent();
           IconPin = ImageSource.FromFile(ico);
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

