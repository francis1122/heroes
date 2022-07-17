using UnityEngine;

[CreateAssetMenu(fileName = "FloatVariableRuntime", menuName = "Variable/FloatVariableRuntime")]
public class FloatVariableRuntime : ScriptableObject
{
    public float DefaultValue;

    private float currentValue;

    public float CurrentValue
    {
        get { return currentValue; }
        set { currentValue = value; }
    }

    private void OnEnable()
    {
        currentValue = DefaultValue;
    }
    
    public static implicit operator float(FloatVariableRuntime variable)
    {
        return variable.CurrentValue;
    }
    
}
