using System.Collections.Generic;
using UnityEngine;

public class AffinityContainer : MonoBehaviour
{
    
    [SerializeField] public Dictionary<DamageTypes, AffinityTypes> AllWeak;
    [SerializeField] public Dictionary<DamageTypes, AffinityTypes> AllResist;
    [SerializeField] public Dictionary<DamageTypes, AffinityTypes> AllNull;
    [SerializeField] public Dictionary<DamageTypes, AffinityTypes> AllNormal;
}
