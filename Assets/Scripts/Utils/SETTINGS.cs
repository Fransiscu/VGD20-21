using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SETTINGS
{
    // coins
    public static readonly int level1MinCoinScore = 1;
    public static readonly int level1MaxCoinScore = 10;

    public static readonly int level2MinCoinScore = 2;
    public static readonly int level2MaxCoinScore = 15;

    public static readonly int level3MinCoinScore = 5;
    public static readonly int level3MaxCoinScore = 20;

    public static readonly int bonusLevelMinCoinScore = 5;
    public static readonly int bonusLevelMaxCoinScore = 10;

    // enemies
    public static readonly float slimeHitDamage = 0.5f;
    public static readonly float snailDamage = 0.5f;
    public static readonly float spikesDamage = 1.0f;
    public static readonly float empressDamage = 1.0f;
    
    public static readonly float knockBackStrength = 200.0f;
    public static readonly float knockBackDistance = 300.0f;

    // player
    public static readonly float invincibilityHitTime = 1.0f;
    public static readonly float startingLives = 3.0f;
    
    public static readonly float invincibilityFramesDurationSeconds = 1.5f;
    public static readonly float invincibilityFramesDeltaTime = 0.15f;

    public static readonly float playerHitInputFreezeDuration = .5f;
    public static readonly float playerHitInputFreezeFramesDeltaTime = .1f;

    // powerups
    public static readonly float speedBoost = 1.2f;
    public static readonly float doubleJumpBuffDuration = 3f;

    // powedowns
    public static readonly float speedDeficit = 1.2f;

    // levels

}
