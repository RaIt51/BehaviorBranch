from Behaviour.Config import Config
from Behaviour.SpeechRecognition import SpeechRecognition
from Behaviour.Connector import Connector
from Behaviour.BehaviourController import BehaviourController
from Behaviour.Utils import clean, Verboser


config = Config()
verboser = Verboser(config)

# clean previous data
verboser.print("cleaning...", 1)
clean(config)
verboser.print("cleaned", 1)

verboser.print("initializing...", 1)
behaviour_controller = BehaviourController(config)

# connect to Unity
verboser.print("connecting...", 1)
connector = Connector().singleton
verboser.print("connected", 1)


def on_speech_recognized(transcription: str):
    verboser.print(f"speech recognized: {transcription}", 2)

    # to behaviour json
    behaviour_json = behaviour_controller.command(transcription)

    # send this to unity
    verboser.print("sending...", 1)
    connector.send_branch(behaviour_json)
    verboser.print("sent", 1)


# start listening to speech (This will block this thread)
speech_recognition = SpeechRecognition(config, on_speech_recognized)
speech_recognition.listen_to_speech()
