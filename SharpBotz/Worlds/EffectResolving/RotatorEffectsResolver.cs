using SharpBotz.Botz;
using SharpBotz.Botz.BotModules.Rotators;

namespace SharpBotz.Worlds.EffectResolving;

public static class RotatorEffectsResolver
{
    public static void Handle(BotState[] botStates, BotStateEffect[] botStateEffects)
    {
        for (var botIndex = 0; botIndex < botStateEffects.Length; botIndex++)
        {
            HandleRotatorEffect(botIndex, botStates, botStateEffects);
        }
    }

    private static void HandleRotatorEffect(
        int botIndex,
        BotState[] botStates,
        BotStateEffect[] botStateEffects)
    {
        var botStateEffect = botStateEffects[botIndex];
        var effects = botStateEffect.Effects.RotatorEffects.OfType<RotateEffect>();
        var rotateLeft = effects
            .Where(effect => effect.Rotation == Rotation.Left)
            .Sum(effect => effect.Times);
        var rotateRight = effects
            .Where(effect => effect.Rotation == Rotation.Right)
            .Sum(effect => effect.Times);

        var turns = rotateRight - rotateLeft;
        if (turns == 0)
            return;

        bool turnRight = turns > 0;
        var times = Math.Abs(turns);
        var facing = botStateEffect.BotState.Facing;
        for (int i = 0; i < times; i++)
        {
            facing = turnRight
                ? facing.RotateRight()
                : facing.RotateLeft();
        }

        var nextState = botStateEffect.BotState with
        {
            Facing = facing,
        };
        botStateEffects[botIndex] = botStateEffect with { BotState = nextState };
        botStates[botIndex] = nextState;
    }
}
