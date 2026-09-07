using SharpBotz.Arenas;
using SharpBotz.Botz.BotModules.Thrusters;

namespace SharpBotz.Worlds.EffectResolving;

public static class MovementEffectResolver
{
    private const int CollisionDamage = 10;

    public static void Handle(
        Arena arena,
        BotState[] botStates,
        BotStateEffect[] botStateEffects,
        ICollection<WorldEvent> worldEvents)
    {
        var intents = botStateEffects
            .Select((botStateEffect, botIndex) => new ThrusterIntent(
                botIndex,
                checked((int)botStateEffect.Effects.ThrusterEffects
                    .OfType<ThrusterEffect>()
                    .Sum(effect => (long)effect.Speed))))
            .ToArray();
        var stopped = new HashSet<int>();
        var maximumSpeed = intents
            .Select(intent => intent.Speed)
            .DefaultIfEmpty()
            .Max();

        for (var step = 1; step <= maximumSpeed; step++)
        {
            MoveOneStep(
                arena,
                botStates,
                botStateEffects,
                intents,
                stopped,
                step,
                worldEvents);
        }

        foreach (var botStateEffect in botStateEffects)
        {
            foreach (var effect in botStateEffect.Effects.ThrusterEffects.OfType<ThrusterOverChargedEffect>())
            {
                DamageResolver.Handle(
                    botStateEffect.BotState.Bot,
                    effect.ExcessPower * 3,
                    new DamageCause.ThrusterOvercharged(effect.Id),
                    worldEvents);
            }
        }
    }

    private static void MoveOneStep(
        Arena arena,
        BotState[] botStates,
        BotStateEffect[] botStateEffects,
        ThrusterIntent[] intents,
        HashSet<int> stopped,
        int step,
        ICollection<WorldEvent> worldEvents)
    {
        var movers = intents
            .Where(intent =>
                botStateEffects[intent.BotIndex].BotState.Bot.IsAlive &&
                intent.Speed >= step &&
                !stopped.Contains(intent.BotIndex))
            .ToArray();
        var occupants = Enumerable.Range(0, botStateEffects.Length)
            .Where(index => botStateEffects[index].BotState.Bot.IsAlive)
            .ToLookup(index => botStateEffects[index].BotState.Position.ToCoordinates());
        var targets = movers.ToDictionary(
            intent => intent.BotIndex,
            intent =>
            {
                var state = botStateEffects[intent.BotIndex].BotState;
                return state.Position.Move(state.Facing);
            });
        var competingMovers = movers.ToLookup(
            intent => targets[intent.BotIndex].ToCoordinates());
        var collisionVictims = new HashSet<int>();

        foreach (var mover in movers)
        {
            var moverIndex = mover.BotIndex;
            var moverBot = botStateEffects[moverIndex].BotState.Bot;
            var target = targets[moverIndex];

            if (!arena.IsTraversable(target))
            {
                stopped.Add(moverIndex);
                collisionVictims.Add(moverIndex);
            }

            foreach (var occupantIndex in occupants[target.ToCoordinates()])
            {
                var occupantBot = botStateEffects[occupantIndex].BotState.Bot;
                if (ReferenceEquals(moverBot, occupantBot))
                {
                    continue;
                }

                stopped.Add(moverIndex);
                collisionVictims.Add(moverIndex);
                collisionVictims.Add(occupantIndex);
            }

            foreach (var competitor in competingMovers[target.ToCoordinates()])
            {
                var competitorIndex = competitor.BotIndex;
                var competitorBot = botStateEffects[competitorIndex].BotState.Bot;
                if (ReferenceEquals(moverBot, competitorBot))
                {
                    continue;
                }

                stopped.Add(moverIndex);
                collisionVictims.Add(moverIndex);
                collisionVictims.Add(competitorIndex);
            }
        }

        foreach (var mover in movers.Where(mover => !stopped.Contains(mover.BotIndex)))
        {
            var botIndex = mover.BotIndex;
            var botStateEffect = botStateEffects[botIndex];
            var nextState = botStateEffect.BotState with
            {
                Position = targets[botIndex],
            };
            botStateEffects[botIndex] = botStateEffect with { BotState = nextState };
            botStates[botIndex] = nextState;
        }

        foreach (var victimIndex in collisionVictims)
        {
            DamageResolver.Handle(
                botStateEffects[victimIndex].BotState.Bot,
                CollisionDamage,
                new DamageCause.Collision(),
                worldEvents);
        }
    }

    private record ThrusterIntent(int BotIndex, int Speed);
}
