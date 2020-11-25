using System;
using AVFoundation;
using Foundation;
using LeadersOfDigital.DependencyServices;
using Speech;

namespace LeadersOfDigital.iOS.DependencyServices
{
    public class PlatformSpeechToTextService : IPlatformSpeechToTextService
    {
        private readonly AVAudioEngine _audioEngine;
        private readonly SFSpeechRecognizer _speechRecognizer;
        private SFSpeechAudioBufferRecognitionRequest _recognitionRequest;
        private SFSpeechRecognitionTask _recognitionTask;
        private string _recognizedString;
        private NSTimer _timer;
        private bool isNotContinious;

        public event EventHandler<string> SpeechRecognitionFinished;

        public PlatformSpeechToTextService()
        {
            _audioEngine = new AVAudioEngine();
            _speechRecognizer = new SFSpeechRecognizer();
        }

        public void StartSpeechToText()
        {
            if (_audioEngine.Running)
            {
                StopRecordingAndRecognition();
            }

            StartRecordingAndRecognizing();
        }

        public void StopSpeechToText()
        {
            StopRecordingAndRecognition();
        }

        private void StartRecordingAndRecognizing()
        {
            _timer = NSTimer.CreateRepeatingScheduledTimer(5, delegate
            {
                DidFinishTalk();
            });

            _recognitionTask?.Cancel();
            _recognitionTask = null;

            var audioSession = AVAudioSession.SharedInstance();
            NSError nsError;

            nsError = audioSession.SetCategory(AVAudioSessionCategory.PlayAndRecord);

            audioSession.SetMode(AVAudioSession.ModeDefault, out nsError);

            nsError = audioSession.SetActive(true, AVAudioSessionSetActiveOptions.NotifyOthersOnDeactivation);

            audioSession.OverrideOutputAudioPort(AVAudioSessionPortOverride.Speaker, out nsError);

            _recognitionRequest = new SFSpeechAudioBufferRecognitionRequest();

            var inputNode = _audioEngine.InputNode;

            if (inputNode == null)
            {
                throw new Exception();
            }

            var recordingFormat = inputNode.GetBusOutputFormat(0);

            inputNode.InstallTapOnBus(0, 1024, recordingFormat, (buffer, when) =>
            {
                _recognitionRequest?.Append(buffer);
            });

            _audioEngine.Prepare();
            _audioEngine.StartAndReturnError(out nsError);

            _recognitionTask = _speechRecognizer.GetRecognitionTask(_recognitionRequest, (result, error) =>
            {
                var isFinal = false;

                if (result != null)
                {
                    _recognizedString = result.BestTranscription.FormattedString;

                    SpeechRecognitionFinished?.Invoke(this, _recognizedString);

                    _timer.Invalidate();
                    _timer = null;
                    _timer = NSTimer.CreateRepeatingScheduledTimer(2, delegate
                    {
                        DidFinishTalk();
                    });
                }

                if (error != null || isFinal)
                {
                    StopRecordingAndRecognition();
                }
            });
        }

        private void StopRecordingAndRecognition()
        {
            if (_audioEngine.Running)
            {
                _audioEngine.Stop();
                _audioEngine.InputNode.RemoveTapOnBus(0);
                _recognitionTask?.Cancel();
                _recognitionRequest.EndAudio();
                _recognitionRequest = null;
                _recognitionTask = null;
            }
        }

        private void DidFinishTalk()
        {
            if (_timer != null)
            {
                _timer.Invalidate();
                _timer = null;
            }

            if (_audioEngine.Running)
            {
                StopRecordingAndRecognition();
            }
        }
    }
}
