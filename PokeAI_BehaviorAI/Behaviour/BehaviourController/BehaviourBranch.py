from __future__ import annotations
from Behaviour.BehaviourController.BehaviourNodes import BehaviourNode


class BehaviourBranch:
    """
    Behaviour Branch
    """

    def __init__(self, root_node: BehaviourNode) -> None:
        self.root_node = root_node

    def connect_next(self, next: BehaviourNode | BehaviourBranch) -> None:
        # if `next` is BehaviourNode...
        if isinstance(next, BehaviourBranch):
            # ...get the root node of the branch
            next = next.root_node

        # `next` is now a BehaviourNode

        # connect to the last node
        self.last_node.connect_next(next)

    def get_nth_node(self, n: int) -> BehaviourNode | None:
        node = self.root_node
        for _ in range(n):
            node = node.next

            if node is None:
                return None

        return node

    @property
    def last_node(self) -> BehaviourNode:
        node = self.root_node
        while node.has_next:
            node = node.next
        return node
