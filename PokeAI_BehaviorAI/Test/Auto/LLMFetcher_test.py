from Behaviour.BehaviourController.BehaviourThinker.LLMFetcher import (
    LLMFetcher,
    Config,
)


def test_cleaner0():
    config = Config()
    instance = LLMFetcher(config)
    texts_testing = [
        """
    #後ろへ走る
    add_action_behaviour("RunTowardsEnemy", 180)
]

#待って
action4 = [
    add_action_behaviour("Wait")
]

#敵に近づいてアイアンテール
""",
        """
    #近づく
    add_action_behaviour("RunTowardsEnemy", 0),
    
    #敵に近づいたら、アイアンテール
    [
        add_condition_behaviour("distanceFrom", "<", "distanceFromEnemyObject"),
        add_action_behaviour("IronTail")
    ]
]
""",
        """
    #近づく
    add_action_behaviour("RunTowardsEnemy", 0),
    
    #敵に近づいたら、アイアンテール
    [
        add_condition
""",
    ]

    texts_cleaned = instance._clean_output(texts_testing)

    assert len(texts_cleaned) == 2

    assert texts_cleaned[0].startswith("[")
    assert texts_cleaned[0].endswith("]")
