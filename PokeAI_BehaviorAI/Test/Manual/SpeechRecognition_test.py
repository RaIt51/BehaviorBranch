from time import sleep
from Behaviour.SpeechRecognition import SpeechRecognition
from Behaviour.Config import Config

config = Config()
config.verbose = 2


def dummy_callback(text):
    print("callback called")
    print(text)
    print()


instance = SpeechRecognition(config, dummy_callback)

instance.listen_to_speech()
