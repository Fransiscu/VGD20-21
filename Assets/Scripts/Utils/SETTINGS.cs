public class SETTINGS
{
    // player
    public static readonly float startingLives = 5.0f;
    public static readonly float basePlayerSpeed = 7f;
    public static readonly int basePlayerJumpPower = 1450;

    // cygnus
    public static readonly float cygnusSpeed = 325f;
    public static readonly float cygnusDamage = 1.5f;

    public static readonly float cygnusKnockBackStrength = 500.0f;
    public static readonly float cygnusKnockBackDistance = 400.0f;

    // enemies
    public static readonly float level1EnemySpeed = 120f;
    public static readonly float level2EnemySpeed = 150f;
    public static readonly float level3EnemySpeed = 180f;

    public static readonly float level1EnemyDamage = 0.5f;
    public static readonly float level2EnemyDamage = 1f;
    public static readonly float level3EnemyDamage = 1.5f;

    public static readonly float spikesDamage = 1.0f;
    
    public static readonly float knockBackStrength = 700.0f;
    public static readonly float knockBackDistance = 450.0f;

    // status' duration
    public static readonly float invincibilityFramesDurationSeconds = 1.5f;
    public static readonly float invincibilityFramesDeltaTime = 0.15f;

    public static readonly float playerHitInputFreezeDuration = .5f;
    public static readonly float playerHitInputFreezeFramesDeltaTime = .1f;

    // levels
    public static readonly float soundVolume = 1f;
    public static readonly float musicVolume = .10f;

    // coins
    public static readonly int level1MinCoinScore = 1;
    public static readonly int level1MaxCoinScore = 10;

    public static readonly int level2MinCoinScore = 2;
    public static readonly int level2MaxCoinScore = 15;

    public static readonly int level3MinCoinScore = 5;
    public static readonly int level3MaxCoinScore = 20;

    public static readonly int bonusLevelMinCoinScore = 5;
    public static readonly int bonusLevelMaxCoinScore = 10;

    // powerups
    public static readonly float speedBoost = 1.3f;
    public static readonly float doubleJumpBuffDuration = 3f;
    public static readonly float speedBoostBuffDuration = 3f;

    // powedowns
    public static readonly float speedDeficit = .7f;
}
