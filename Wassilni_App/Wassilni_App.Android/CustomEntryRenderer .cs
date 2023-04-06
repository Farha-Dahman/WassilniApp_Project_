using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Wassilni_App.Droid;

[assembly: ExportRenderer(typeof(Entry), typeof(CustomEntryRenderer))]
namespace Wassilni_App.Droid
{
    public class CustomEntryRenderer : EntryRenderer
    {
        public CustomEntryRenderer(Context context) : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.Background = null;
            }
        }
    }
}