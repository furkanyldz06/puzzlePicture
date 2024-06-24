using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Extensions
{
    public static void SetAlpha(this SpriteRenderer sp, float alpha){
        Color color = sp.color;
        color.a = alpha;
        sp.color = color;
    }

    public static void SetAlpha(this Text text, float alpha){
        Color color = text.color;
        color.a = alpha;
        text.color = color;
    }

    public static void SetAlpha(this Image image, float alpha){
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }

    private static System.Random rng = new System.Random();
    public static void Shuffle<T>(this IList<T> list, int seed = 0)
    {
        
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static void Shuffle<T>(this T[] array, int limit, int seed = 0)
    {
        rng = new System.Random();
        int n = limit;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = array[k];
            array[k] = array[n];
            array[n] = value;
        }
    }

    public static void Reverse<T>(this T[] array)
    {
        int n = array.Length;
        int m = array.Length;

        while (n > 1)
        {
            n -= 2;
            m --;

            T value = array[m];
            array[m] = array[(array.Length - 1) - m];
            array[(array.Length - 1) - m] = value;
        }
    }
 
    public static void AddExplosionForce(this Rigidbody2D body, float explosionForce, Vector3 explosionPosition, float explosionRadius)
    {
        Vector2 heading = body.transform.position - explosionPosition;
        float distance = Vector2.Distance(body.transform.position, explosionPosition);

        if(distance != 0){
            Vector2 direction = heading / distance;

            Vector2 force = (direction * explosionRadius - heading) * explosionForce;
    
            body.AddForce(Vector2.ClampMagnitude(force, 10f), ForceMode2D.Impulse);
        }
    }

    private static Vector3[] circlePath = new Vector3[9];
    public static Vector3[] GetCirclePath(Vector3 center, float radius)
    {
        circlePath[0] = center + (radius * Vector3.up);
        circlePath[1] = center + (radius * Mathf.Pow(2, 1/2) * (Vector3.up + Vector3.right).normalized);
        circlePath[2] = center + (radius * Vector3.right);
        circlePath[3] = center + (radius * Mathf.Pow(2, 1/2) * (Vector3.down + Vector3.right).normalized);
        circlePath[4] = center + (radius * Vector3.down);
        circlePath[5] = center + (radius * Mathf.Pow(2, 1/2) * (Vector3.down + Vector3.left).normalized);
        circlePath[6] = center + (radius * Vector3.left);
        circlePath[7] = center + (radius * Mathf.Pow(2, 1/2) * (Vector3.up + Vector3.left).normalized);
        circlePath[8] = center + (radius * Vector3.up);

        return circlePath;
    }

    private static Vector3[] curvedPath = new Vector3[5];
    public static Vector3[] GetCurvedPath(Vector3 firstPosition,Vector3 targetPosition)
    {
        curvedPath[0] = firstPosition;
        curvedPath[4] = targetPosition;

        Vector3 direction = (targetPosition - firstPosition);
        float distance = Vector3.Distance(firstPosition, targetPosition);
        distance /= 2.5f;
        distance = Mathf.Min(1.5f, distance);

        Vector3 v1 = firstPosition + (direction * (1 / 4f)); //nearPosition
        Vector3 v2 = firstPosition + (direction * (2 / 4f)); //middlePosition
        Vector3 v3 = firstPosition + (direction * (3 / 4f)); //farPosition

        Vector3 perpendicular = Vector3.Cross(direction, Vector3.forward);
        perpendicular.Normalize();

        if (Random.value < 0.5f)
        {
            distance = -distance;
        }
        curvedPath[1] = (distance*0.75f) * perpendicular + v1;
        curvedPath[2] = distance * perpendicular + v2;
        curvedPath[3] = (distance*0.75f) * perpendicular + v3;
        return curvedPath;
    }

    public static Vector3[] GetCurvedLocalPath(Vector3 firstPosition)
    {
        curvedPath[0] = firstPosition;
        curvedPath[4] = Vector3.zero;

        Vector3 direction = (Vector3.zero - firstPosition);
        float distance = Vector3.Distance(firstPosition, Vector3.zero);
        distance /= 2.5f;
        distance = Mathf.Min(1.5f, distance);

        Vector3 v1 = firstPosition + (direction * (1 / 4f)); //nearPosition
        Vector3 v2 = firstPosition + (direction * (2 / 4f)); //middlePosition
        Vector3 v3 = firstPosition + (direction * (3 / 4f)); //farPosition

        Vector3 perpendicular = Vector3.Cross(direction, Vector3.forward);
        perpendicular.Normalize();

        if (Random.value < 0.5f)
        {
            distance = -distance;
        }
        curvedPath[1] = (distance * 0.75f) * perpendicular + v1;
        curvedPath[2] = distance * perpendicular + v2;
        curvedPath[3] = (distance * 0.75f) * perpendicular + v3;
        return curvedPath;
    }

    public static bool IsInLayerMask(int layer, LayerMask layermask)
    {
        return layermask == (layermask | (1 << layer));
    }

    public static float EaseIn(float k)
    {
        return k * k * k;
    }

    public static bool IsIPhoneX()
    {
    
#if UNITY_IOS
        float aspect = (float)Screen.width / (float)Screen.height;
        if (aspect < 0.56f)
        {
            return true;
        }
#endif
        return false;
    }

    public static bool IsIPad()
    {
#if UNITY_IOS
        float aspect = (float)Screen.width / (float)Screen.height;
        if (aspect > 0.57f)
        {
            return true;
        }
#endif
        return false;
    }

}
