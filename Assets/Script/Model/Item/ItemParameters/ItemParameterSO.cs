using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Game/ItemParameterSO")]
public class ItemParameterSO : ScriptableObject
{
    [field: SerializeField]
    public string ParameterName { get; set; }
}
