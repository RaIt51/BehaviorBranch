from Behaviour.BehaviourController.BehaviourNodes.BehaviourNode import (
    BehaviourNode,
)


class ActionNode(BehaviourNode):
    def __init__(self, name_action: str, args: list = []):
        super().__init__("action")
        self.name_action = name_action
        self.args = args.copy()
