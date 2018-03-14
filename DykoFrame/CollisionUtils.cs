using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DykoFrame
{
    public delegate void CollisionTrigger(GameObject o);

    public enum CollisionMatch
    {
        Tag,
        Name,
        Instantion
    };

    public struct CollisionEntry
    {
        public CollisionMatch type;
        public bool inclusive;
        public string name;
        public GameObject obj;
        public CollisionTrigger callback;
    }
}
