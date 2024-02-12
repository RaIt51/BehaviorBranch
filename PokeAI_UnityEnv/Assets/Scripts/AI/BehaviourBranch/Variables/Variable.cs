using System;
using UnityEngine;
using Simulation.Objects.Fighters;

namespace AI.BehaviourBranch.Variables
{
    public abstract class Variable
    {
        public const float TRUE = 100f;
        public const float FALSE = -100f;

        protected FighterWatcher fighterWatcher;

        public Variable(FighterWatcher fighterWatcher)
        {
            this.fighterWatcher = fighterWatcher;
        }

        public abstract float Get();

        public static float ConvertVariable(string variableName, FighterWatcher fighterWatcher)
        {
            // if the variable is a float...
            if (float.TryParse(variableName, out float result))
            {
                // ...return it
                return result;
            }

            switch (variableName)
            {
                case "distanceFromTarget":
                    return new DistanceFromTarget(fighterWatcher).Get();
                case "irontailRange":
                    return new IronTailRange(fighterWatcher).Get();
                case "oppositeAngleFromAttack":
                    return new OppositeAngleFromAttack(fighterWatcher).Get();
                case "headingAngle":
                    return new HeadingAngle(fighterWatcher).Get();
                case "isIdle":
                    return new IsIdle(fighterWatcher).Get();
                case "true":
                    return TRUE;
                case "false":
                    return FALSE;
                case "angleFromTarget":
                    return new AngleFromTarget(fighterWatcher).Get();
                case "playersDirection":
                    return new PlayersDirection(fighterWatcher).Get();
                case "stopwatch":
                    return new Stopwatch(fighterWatcher).Get();
                default:
                    Debug.Log("Unknown variable: " + variableName);
                    return 0f;
            }
        }

        protected float EvaluateBool(bool condition)
        {
            if (condition)
            {
                return TRUE;
            }
            else
            {
                return FALSE;
            }
        }
    }
}
