namespace Taydogmus
{
    public enum GamePhase
    {
        Menu,
        Playing,
        Over
    }

    public enum LevelState
    {
        Ready,
        Playing,
        End
    }

    public enum UpgradeType
    {
        FireRate,
        BulletDamage,
        AttackFormation,
        Bounce
    }

    public enum WinState
    {
        OnGoing,
        Win,
        Lose
    }
    
    public enum BounceType
    {
        Default = 0,
        Twice = 1,
        Infinite = 2
    }

    public enum FormationType
    {
        Single,
        Triple,
        DoubleTriple
    }
    
    public static class Constants{}
}
