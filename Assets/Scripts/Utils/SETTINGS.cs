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
    public static readonly float level1EnemySpeed = 120f;
    public static readonly float level2EnemySpeed = 150f;
    public static readonly float level3EnemySpeed = 180f;

    public static readonly float level1EnemyDamage = 0.5f;
    public static readonly float level2EnemyDamage = 0.5f;
    public static readonly float level3EnemyDamage = 0.5f;

    public static readonly float spikesDamage = 1.0f;
    
    public static readonly float knockBackStrength = 450.0f;
    public static readonly float knockBackDistance = 300.0f;

    // cygnus
    public static readonly float cygnusSpeed = 300f;
    public static readonly float cygnusDamage = 1.0f;

    public static readonly float cygnusKnockBackStrength = 500.0f;
    public static readonly float cygnusKnockBackDistance = 400.0f;

    // player
    public static readonly float startingLives = 3.0f;
    public static readonly float basePlayerSpeed = 6.5f;
    public static readonly int basePlayerJumpPower = 1150;
    
    public static readonly float invincibilityFramesDurationSeconds = 1.5f;
    public static readonly float invincibilityFramesDeltaTime = 0.15f;

    public static readonly float playerHitInputFreezeDuration = .5f;
    public static readonly float playerHitInputFreezeFramesDeltaTime = .1f;

    // powerups
    public static readonly float speedBoost = 1.3f;
    public static readonly float doubleJumpBuffDuration = 3f;
    public static readonly float speedBoostBuffDuration = 3f;

    // powedowns
    public static readonly float speedDeficit = .2f;

    // levels
    public static readonly float soundVolume = 1f;
    public static readonly float musicVolume = .10f;
}
