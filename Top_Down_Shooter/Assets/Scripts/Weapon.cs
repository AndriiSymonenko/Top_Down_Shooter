
public enum WeaponType
{
    Pistol, 
    Revolver,
    Rifle,
    Shotgun,
    SniperRifle
}

[System.Serializable]
public class Weapon
{
    public WeaponType weaponType;
    public int ammo;
    public int maxAmmo;
}
