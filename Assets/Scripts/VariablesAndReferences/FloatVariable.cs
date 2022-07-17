using UnityEngine;

[CreateAssetMenu]
public class FloatVariable : ScriptableObject
{
    public float value;
    
    public static implicit operator float(FloatVariable variable)
    {
        return variable.value;
    }
}