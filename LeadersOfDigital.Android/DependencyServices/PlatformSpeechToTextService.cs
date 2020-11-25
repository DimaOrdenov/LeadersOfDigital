using System;
using Android.Content;
using Android.Speech;
using LeadersOfDigital.DependencyServices;
using Plugin.CurrentActivity;

namespace LeadersOfDigital.Droid.DependencyServices
{
    public class PlatformSpeechToTextService : IAndroidSpeechToTextService
    {
        public PlatformSpeechToTextService()
        {
        }

        public event EventHandler<string> SpeechRecognitionFinished;

        public void StartSpeechToText()
        {
            StartRecordingAndRecognizing();
        }

        public void StopSpeechToText()
        {
            throw new NotImplementedException();
        }

        public void InvokeSpeechRecognitionEvent(string speech)
        {
            SpeechRecognitionFinished?.Invoke(this, speech);
        }

        private void StartRecordingAndRecognizing()
        {
            string rec = Android.Content.PM.PackageManager.FeatureMicrophone;

            if (rec == "android.hardware.microphone")
            {
                var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);

                voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
                voiceIntent.PutExtra(RecognizerIntent.ExtraPrompt, "Speak now-w-w-w");
                voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 1500);
                voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 1500);
                voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 15000);
                voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);
                voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);

                CrossCurrentActivity.Current.Activity.StartActivityForResult(voiceIntent, 10);
            }
            else
            {
                throw new PlatformNotSupportedException("No mic found");
            }
        }
    }
}
