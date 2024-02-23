using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName ="BuildingList",menuName ="Building/BuildingTypes")]
public class BuildingList : ScriptableObject
{
    [SerializeField] private List<Building> buildingTypes;

    public List<Building> GetBuildingTypes() { return buildingTypes; }
}
