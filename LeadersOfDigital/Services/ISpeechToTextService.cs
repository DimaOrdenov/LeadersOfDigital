using System;

namespace LeadersOfDigital.Services
{
    public interface ISpeechToTextService
    {
        event EventHandler<string> SpeechRecognitionFinished;

        void StartSpeechToText();

        void StopSpeechToText();
    }
}
