using UnityEngine;

public class HumanControlledFighter : Fighter
{
    public override AffinityTypes TakeAction(Fighter target)
    {
        bool useSkill = Random.Range(0, 100) > 49;

        if (!useSkill)
            return BasicAttack(target);

        return UseSkill(Skills[0], target);
    }
}
