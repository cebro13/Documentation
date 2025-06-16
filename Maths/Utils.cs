using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public const string NO_BUS_STOP_FOUND = "You don't know anywhere to go...";

    public static float AngleFromVectors(Vector2 vector1, Vector2 vector2) //Order is important since it's Atan
    {
        float result = Mathf.Atan2((vector2.y - vector1.y),(vector2.x - vector1.x))*Mathf.Rad2Deg;
        if(result < 0)
        {
            result += 360;
        }
        return result;
    }

    public static float AngleZFromPosition(Vector2 vector1, Vector2 vector2)
    {
        float result = Mathf.Atan2((vector2.x - vector1.x),(vector1.y - vector2.y))*Mathf.Rad2Deg;
        if(result < 0)
        {
            result += 360;
        }
        return result;
    }

    public static Vector2 DirectionFromVectors(Vector2 origin, Vector2 destination)
    {
        Vector2 result = new Vector2(destination.x - origin.x, destination.y - origin.y);
        result.Normalize();
        return result;
    }

    public static bool AreColorsSameWithinThreshold(Color color1, Color color2, float threshold)
    {
        float r = color1.r - color2.r;
        float g = color1.g - color2.g;
        float b = color1.b - color2.b;
        return r*r + g*g + b*b < threshold;
    }

    public static Vector2 MoveTowardsInX(Vector2 current, Vector2 target, float speed)
    {
        float direction = Mathf.Sign(target.x - current.x);
        return new Vector2(current.x + direction * speed, current.y);
    }

    public static LinearEquation GetLinearParameters(float y2, float y1, float x2, float x1)
    {
        LinearEquation linearEquation = new LinearEquation();
        linearEquation.slope = (y2 - y1)/(x2 - x1);
        if(y2 != 0 && x2 !=0)
        {
            linearEquation.ordonne = y2 - x2 * linearEquation.slope;
        }
        else if(y1 != 0 && x1 != 0)
        {
            linearEquation.ordonne = y1 - x1 * linearEquation.slope;
        }
        else
        {
            Debug.LogError("Ce cas ne devrait pas arriver");
        }
        return linearEquation;
    }

    public static bool GetVolumeFromDistance(Transform objectTransform, Transform cameraTransform, float minDistanceToDetect, float maxVolumeAtDistance, float slope, float ordonne, float maxVolume, out float volume)
    {
        float distance = Vector2.Distance(objectTransform.position, cameraTransform.position);
        if(distance > minDistanceToDetect)
        {
            volume = 0f;
            return false;
        }
        else if(distance > maxVolumeAtDistance)
        {
            volume = slope*distance + ordonne;
        }
        else
        {
            volume = maxVolume;
        }
        return true;

    }

    // Method to convert polar coordinates (in degrees) to a Vector2
    public static Vector2 PolarToCartesian2D(float distance, float angleDegrees)
    {
        // Convert angle from degrees to radians
        float angleRadians = angleDegrees * Mathf.Deg2Rad;
        
        float x = distance * Mathf.Cos(angleRadians);
        float y = distance * Mathf.Sin(angleRadians);
        
        return new Vector2(x, y);
    }

    public static void PlayTriggerAnimation(Animator animator, string animationName, int layer = 0)
    {
        if (!animator.GetCurrentAnimatorStateInfo(layer).IsName(animationName))
        {
            animator.SetTrigger(animationName);
        }
    }

    public struct LinearEquation
    {
        public float slope;
        public float ordonne;
    }

    public enum Direction
    {
        Left = -1,
        Right = 1
    }

    public enum TriggerType
    {
        Switch,
        Interact,
        ColliderEnter,
        ColliderExit,
        ColliderEnterAndExit,
        Action,
        None
    }

    public enum GroundStable
    {
        GroundIsStable,
        GroundIsUnstable,
        NoGround
    }

    public enum ElectricalContext
    {
        CONTEXT_1,
        CONTEXT_2,
        CONTEXT_3
    }
}
