from __future__ import annotations


class BehaviourNode:
    def __init__(self, _type: str) -> None:
        self._type = _type
        self.next: BehaviourNode = None
        self.previous: BehaviourNode = None

    def connect_next(self, next: BehaviourNode) -> None:
        self.next = next
        next.previous = self

    @property
    def has_next(self) -> bool:
        return self.next is not None
