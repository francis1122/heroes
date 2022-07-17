using UnityEngine;

[CreateAssetMenu(fileName = "IntVariableRuntime", menuName = "Variable/IntVariableRuntime")]
public class IntVariableRuntime : ScriptableObject
{
    public int DefaultValue;

    private int currentValue;

    public int CurrentValue
    {
        get { return currentValue; }
        set { currentValue = value; }
    }

    private void OnEnable()
    {
        currentValue = DefaultValue;
    }
    
    public static implicit operator int(IntVariableRuntime variable)
    {
        return variable.CurrentValue;
    }
    
}