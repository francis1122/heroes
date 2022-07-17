using UnityEngine;
    
[CreateAssetMenu]
public class IntVariable : ScriptableObject
{
    public int value;
    
    public static implicit operator int(IntVariable variable)
    {
        return variable.value;
    }
    
}