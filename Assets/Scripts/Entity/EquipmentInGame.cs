using UnityEngine;

public class EquipmentInGame : Equipment
{
    public EquipmentInGame(Equipment equipment) : base(
        Utils.Clone<Equipment>(equipment))
    {
    }
}
