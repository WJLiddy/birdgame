using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// weapon spread pattern?
// inaccuracy?
public abstract class Weapon
{
    public abstract string getName();
    public abstract int getWeight();
    public abstract Projectile.ProjType getProjectileType();
    public abstract float getCooldown();
    public abstract string getFilePath();
}

public class Crossbow : Weapon
{
    override public string getName() { return "XBOW"; }
    override public int getWeight() { return 10; }
    override public Projectile.ProjType getProjectileType() { return Projectile.ProjType.ARROW; }
    override public float getCooldown() { return 0.3f;  }
    override public string getFilePath() { return "crossbow";  }
}

public class Pistol : Weapon
{
    override public string getName() { return "F. LOCK"; }
    override public int getWeight() { return 5; }
    override public Projectile.ProjType getProjectileType() { return Projectile.ProjType.BULLET; }
    override public float getCooldown() { return 0.5f; }
    override public string getFilePath() { return "pistol"; }
}


public class Axe : Weapon
{
    override public string getName() { return "F. LOCK"; }
    override public int getWeight() { return 5; }
    override public Projectile.ProjType getProjectileType() { return Projectile.ProjType.AXE; }
    override public float getCooldown() { return 0.4f; }
    override public string getFilePath() { return "axe"; }
}

