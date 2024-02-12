from Behaviour.Config import Config
from Behaviour.SpeechRecognition.Microphone import Microphone
from Behaviour.SpeechRecognition.Transcriber import Transcriber
from Behaviour.Utils import Verboser


class SpeechRecognition:
    """
    listen to speech and transcribe it.
    This will block the thread for listening.

    :param callback: function to be called when speech is recognized. Speech(str) is passed as argument
    """

    def __init__(self, config: Config, callback: callable) -> None:
        self.config = config
        self.callback = callback
        self.verboser = Verboser(config)

        self.microphone = Microphone(config)
        self.transcriber = Transcriber(config)

    def listen_to_speech(self):
        self.microphone.start_listen_for_key(
            self.config.key_listen, self._on_recorded
        )

    def _on_recorded(self, path: str) -> str:
        self.verboser.start_timer()

        # transcribe
        transcription = self.transcriber.transcribe(path)

        # callback giving the transcription
        self.callback(transcription)

        self.verboser.finish_timer("I/O")
