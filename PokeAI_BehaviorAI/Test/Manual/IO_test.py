from Behaviour.Config import Config
from Behaviour.SpeechRecognition import SpeechRecognition
from Behaviour.KeyboardRecognition import KeyboardRecognition
from Behaviour.BehaviourController import BehaviourController
from Behaviour.Utils import clean, Verboser


config = Config()
config.verbose = 2
config.logging = True
verboser = Verboser(config)

# clean previous data
verboser.print("cleaning...", 1)
clean(config)
verboser.print("cleaned", 1)

verboser.print("initializing...", 1)
behaviour_controller = BehaviourController(config)


def on_speech_recognized(transcription: str):
    verboser.print(f"speech recognized: {transcription}", 2)

    # to behaviour json
    behaviour_json = behaviour_controller.command(transcription)

    print(behaviour_json)


# start listening to speech (This will block this thread)
# speech_recognition = SpeechRecognition(config, on_speech_recognized)
speech_recognition = KeyboardRecognition(config, on_speech_recognized)
speech_recognition.listen_to_speech()
