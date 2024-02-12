from Behaviour.BehaviourController.BehaviourNodes.BehaviourNode import (
    BehaviourNode,
)


class DummyNode(BehaviourNode):
    def __init__(self) -> None:
        super().__init__("dummy")
