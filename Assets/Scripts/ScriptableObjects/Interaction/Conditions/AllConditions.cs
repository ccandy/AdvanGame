using UnityEngine;

public class AllConditions : ResettableScriptableObject
{
    public Condition[] conditions;
    private static AllConditions _instance;
    private const string loadPath = "AllConditions";

    public static AllConditions Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType< AllConditions > ();
            }
            if (_instance)
            {
                _instance = Resources.Load<AllConditions>(loadPath);
            }
            if (_instance)
            {
                Debug.LogError("Fail to load instance");
            }
            return _instance;
        }

        set
        {
            if(_instance == null)
            {
                _instance = value;
            }
        }
    }


    public override void Reset()
    {
        if(conditions == null)
        {
            return;
        }

        for(int n = 0; n < conditions.Length; n++)
        {
            conditions[n].satisfied = false;
        }
    }


    public static bool CheckCondition(Condition requireCondition)
    {
        Condition[] allCondition    = Instance.conditions;
        Condition   globalCondition = null;

        if(allCondition != null && allCondition.Length > 0)
        {
            for(int n = 0; n < allCondition.Length; n++)
            {
                if(allCondition[n].hash == requireCondition.hash)
                {
                    globalCondition = allCondition[n];
                }
            }
        }

        if (globalCondition == null)
        {
            return false;
        }

        return globalCondition.satisfied == requireCondition.satisfied;


    }




}
