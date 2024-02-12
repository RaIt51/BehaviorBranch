from Behaviour.BehaviourController.BehaviourNodes.BehaviourNode import (
    BehaviourNode,
)


class ConditionNode(BehaviourNode):
    def __init__(self, variable0: str, condition: str, variable1: str):
        super().__init__("condition")
        self.variable0 = variable0
        self.condition = condition
        self.variable1 = variable1

        self.node_true: BehaviourNode = None

    def connect_true(self, node_next: BehaviourNode):
        self.node_true = node_next
        node_next.previous = self

    @property
    def has_true(self) -> bool:
        return self.node_true != None
