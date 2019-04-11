using UnityEngine;

public class ConditionCollection : ScriptableObject
{
    public string               description;
    public Condition[]          requiredConditions = new Condition[0];
    public ReactionCollection   reactionCollection;


    public bool CheckAndReact()
    {
        for (int n = 0; n < requiredConditions.Length; n++)
        {
            if (!AllConditions.CheckCondition(requiredConditions[n]))
            {
                return false;
            }
        }

        if (reactionCollection)
        {
            reactionCollection.React();
        }

        return true;
    }
}