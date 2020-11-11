using System;
using UnityEngine;
[Serializable]
public class EquipmentInGame : Equipment
{
    public EquipmentInGame(Equipment equipment) : base(
        Utils.Clone<Equipment>(equipment))
    {
    }
}
