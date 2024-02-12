from Behaviour.BehaviourController.BehaviourNodes.BehaviourNode import (
    BehaviourNode,
)


class ControlNode(BehaviourNode):
    def __init__(self, name_control: str, args: list = []):
        super().__init__("control")
        self.name_control = name_control
        self.args = args.copy()
