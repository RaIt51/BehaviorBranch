# Behaviour Design

## ActionNode

- RunTowardsTarget(float angle)

    Specify the direction by the angle from the enemy target.  
    0 = enemy side, 90 = left, 180 = back, 270 = right
    
- Stop()
    
    stop to idle state
    
- IronTail()
    Try Iron Tail until it once succeeds
    
- Tackle()
    Try Tackle until it once succeeds
    
- Thunderbolt()
    Try Thunderbolt until it once succeeds
    

## ConditionNode

- ==
    
    Approximation process is performed
    
- >
- <

## ControlNode

- Then()
    
    ConditionNodeが判定受付中ならば、そのNodeTrueの末尾に付加

    for action -> connect without interruption
    for condition -> connect to the true node of the condition
    for repitition -> activates after the repetition finished
    
- Repeat(int)
    Repeat next node
    -1 is infinite

- QuitRepeating()
    Quit Repeating

## Variable

- distanceFromTarget
- ironTailRange
- oppositeAngleFromAttack
- headingAngle
- isIdle
    is idle state or not -> evaluate by true/false
- true
- false