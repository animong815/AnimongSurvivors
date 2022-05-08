using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitObject : MonoBehaviour
{
    public enum TYPE
    {
        Player,
        Enemy
    }

    public TYPE CurrentType;
    public Player player;
    public Enemy enemy;
    
}
