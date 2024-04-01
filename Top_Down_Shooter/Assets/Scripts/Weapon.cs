
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

    public bool CanShoot()
    {
        return HaveEnoughBullets();
    }

    private bool HaveEnoughBullets()
    {
        if (ammo > 0)
        {
            ammo--;
            return true;
        }

        return false;
    }
}
