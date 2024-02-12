from Behaviour.Config import Config
from Behaviour.BehaviourController import BehaviourController

config = Config(language_speech="en")
config.verbose = 2
config.logging = True

thinker = BehaviourController(config)

result = thinker.command("look right")

print(result)
