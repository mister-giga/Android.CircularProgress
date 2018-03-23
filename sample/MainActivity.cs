using Android.App;
using Android.Widget;
using Android.OS;

namespace sample
{
    [Activity(Label = "sample", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        float count = 0;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.myButton);

            var view = FindViewById<MPDC.Android.CircularProgress.CircularProgressView>(Resource.Id.donut_progress);
            view.setMax(1);
            button.Click += delegate
            {
                count += 0.1f;
                view.AnimateCircular(count);
            };


        }
    }
}

