using System;
using LeadersOfDigital.DependencyServices;

namespace LeadersOfDigital.Services
{
    public class SpeechToTextService : ISpeechToTextService
    {
        private readonly IPlatformSpeechToTextService _platformSpeechToTextService;

        public SpeechToTextService(IPlatformSpeechToTextService platformSpeechToTextService)
        {
            _platformSpeechToTextService = platformSpeechToTextService;

            _platformSpeechToTextService.SpeechRecognitionFinished += (sender, e) => SpeechRecognitionFinished?.Invoke(this, e);
        }

        public event EventHandler<string> SpeechRecognitionFinished;

        public void StartSpeechToText()
        {
            _platformSpeechToTextService.StartSpeechToText();
        }

        public void StopSpeechToText()
        {
            _platformSpeechToTextService.StopSpeechToText();
        }
    }
}
