from __future__ import annotations
from Behaviour.Config import Config
from Behaviour.BehaviourController.BehaviourBranch import BehaviourBranch
from Behaviour.BehaviourController.BehaviourNodes import (
    BehaviourNode,
    ActionNode,
    ConditionNode,
    DummyNode,
    ControlNode,
)
from Behaviour.BehaviourController.BehaviourThinker import BehaviourThinker


class BehaviourController:
    """Control the behaviour branch"""

    def __init__(self, config: Config) -> None:
        self.config = config

        # modules
        self.thinker = BehaviourThinker(config)

    def command(self, command: str) -> list[dict]:
        """Execute the command"""

        if self.config.verbose >= 2:
            print(f"command: {command}")

            from time import time

            time_start = time()

        # make new branch
        branch = self.thinker.think(command)
        
        # none detected
        if (branch == None) or (branch.root_node is None):
            return []

        # convert branch to JSON-able format (list of dicts)
        branch_json = []
        self._convert_node_to_json(branch.root_node, branch_json)

        if self.config.verbose >= 2:
            print(
                f"BehaviourController.command executed in {time() - time_start} seconds"
            )
            print(f"branch_json: {branch_json}")

        return branch_json

    def _convert_node_to_json(
        self, node: BehaviourNode, current_list: list[dict]
    ) -> int | None:
        # this node's id
        id_this = len(current_list)

        # convert this node to dict
        if isinstance(node, ActionNode):
            node_dict = {
                "id": id_this,
                "type": "action",
                "action": node.name_action,
            }

            # args to string list
            args = []
            for arg in node.args:
                args.append(str(arg))

            node_dict["args"] = args

        elif isinstance(node, ControlNode):
            node_dict = {
                "id": id_this,
                "type": "control",
                "control": node.name_control,
            }

            args = []
            for arg in node.args:
                args.append(str(arg))

            node_dict["args"] = args

        elif isinstance(node, ConditionNode):
            node_dict = {
                "id": id_this,
                "type": "condition",
                "condition": str(node.condition),
                "args": [str(node.variable0), str(node.variable1)],
            }

        # add this node to the list
        current_list.append(node_dict)

        # add next node
        if node.has_next:
            id_next = self._convert_node_to_json(node.next, current_list)
            node_dict["node_next"] = id_next
        else:
            node_dict["node_next"] = -1

        # if condition...
        if isinstance(node, ConditionNode):
            if node.has_true:
                # ...add true node
                id_true = self._convert_node_to_json(
                    node.node_true, current_list
                )
                node_dict["node_true"] = id_true
            else:
                node_dict["node_true"] = -1

        return id_this
