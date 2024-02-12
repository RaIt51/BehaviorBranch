from Behaviour.BehaviourController.BehaviourThinker.BehaviourCodeReader import (
    read_behaviour_code,
)


def test_reader():
    test_behaviour_code = """
[
    #近づく
    add_action_behaviour("RunTowardsEnemy", 0),
    
    #敵に近づいたら、アイアンテール
    [
        add_condition_behaviour("distanceFrom", "<", "distanceFromEnemyObject"),
        add_action_behaviour("IronTail")
    ]
]
"""

    result = read_behaviour_code(test_behaviour_code)

    assert result.root_node._type == "action"
    assert result.root_node.name_action == "RunTowardsEnemy"
    assert result.root_node.next._type == "condition"
    assert result.root_node.next.next == None
    assert result.root_node.next.node_true._type == "action"
