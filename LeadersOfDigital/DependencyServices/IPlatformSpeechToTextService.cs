using System;

namespace LeadersOfDigital.DependencyServices
{
    public interface IPlatformSpeechToTextService
    {
        event EventHandler<string> SpeechRecognitionFinished;

        void StartSpeechToText();

        void StopSpeechToText();
    }
}
