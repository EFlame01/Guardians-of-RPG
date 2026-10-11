
/// <summary>
/// TurnStatus is an enum that defines
/// the order the characters are in 
/// during battle. Depending on their 
/// moves/circumstances, their status 
/// will also reflect that and allow 
/// them to move in that order.
/// </summary>
public enum TurnStatus
{
    INITIATIVE = 1,
    MOVE_FIRST = 2,
    RUN = 3,
    ITEM = 4,
    PROTECT = 5,
    COUNTER = 6,
    PRIORITY_1 = 7,
    PRIORITY_2 = 8,
    PRIORITY_3 = 9,
    FIGHT = 10,
    MOVE_LAST = 11,
    SKIP = 12,
    CANNOT_MOVE = 13,
    NOTHING = 14
}