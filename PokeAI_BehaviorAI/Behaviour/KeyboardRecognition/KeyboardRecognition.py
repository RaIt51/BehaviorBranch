from Behaviour.Config import Config
from Behaviour.SpeechRecognition import SpeechRecognition
from Behaviour.Utils import Verboser


class KeyboardRecognition(SpeechRecognition):
    """
    listen to keyboard instead of speech
    """

    def __init__(self, config: Config, callback: callable) -> None:
        self.config = config
        self.callback = callback
        self.verboser = Verboser(config)

    def listen_to_speech(self):
        while True:
            text = input("Enter command >>")
            self.callback(text)
