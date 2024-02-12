from Behaviour.Config import Config
from Behaviour.SpeechRecognition import SpeechRecognition
from Behaviour.Connector import Connector
from Behaviour.BehaviourController import BehaviourController
from Behaviour.Utils import clean, Verboser
from time import sleep

config = Config(language_speech="en")
config.verbose = 2
config.logging = True
verboser = Verboser(config)

# clean previous data
verboser.print("cleaning...", 1)
clean(config)
verboser.print("cleaned", 1)

verboser.print("initializing...", 1)
behaviour_controller = BehaviourController(config)


# connect to Unity
def on_received(format_name: str, data) -> None:
    on_speech_recognized(data["command"])


verboser.print("connecting...", 1)
connector = Connector(on_received=on_received).singleton
verboser.print("connected", 1)


def on_speech_recognized(transcription: str):
    verboser.print(f"speech recognized: {transcription}", 2)

    # to behaviour json
    behaviour_json = behaviour_controller.command(transcription)

    # send this to unity
    verboser.print("sending...", 1)
    connector.send_branch(behaviour_json)
    verboser.print("sent", 1)


# connector handles everything
while True:
    sleep(1)
