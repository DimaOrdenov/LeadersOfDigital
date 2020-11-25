using LeadersOfDigital.DependencyServices;

namespace LeadersOfDigital.Droid.DependencyServices
{
    public interface IAndroidSpeechToTextService : IPlatformSpeechToTextService
    {
        void InvokeSpeechRecognitionEvent(string speech);
    }
}
