using System;
using Android.Content;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Editor), typeof(LeadersOfDigital.Droid.CustomRenderers.EditorRenderer))]
namespace LeadersOfDigital.Droid.CustomRenderers
{
    public class EditorRenderer : Xamarin.Forms.Platform.Android.EditorRenderer
    {
        public EditorRenderer(Context context)
            : base(context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement == null || Control == null)
            {
                return;
            }

            // Remove underline
            Control.SetBackgroundColor(Android.Graphics.Color.Transparent);

            // Enable scroll for editor inside ScrollView
            Control.OverScrollMode = OverScrollMode.Always;
            Control.ScrollBarStyle = ScrollbarStyles.InsideInset;
            Control.SetOnTouchListener(new EditorTouchListener());
        }

        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();

            Control.Enabled = false;
            Control.Enabled = true;
        }

        private class EditorTouchListener : Java.Lang.Object, IOnTouchListener
        {
            /// <inheritdoc/>
            public bool OnTouch(Android.Views.View v, MotionEvent e)
            {
                v.Parent?.RequestDisallowInterceptTouchEvent(true);

                if ((e.Action & MotionEventActions.Up) != 0 && (e.ActionMasked & MotionEventActions.Up) != 0)
                {
                    v.Parent?.RequestDisallowInterceptTouchEvent(false);
                }

                return false;
            }
        }
    }
}
