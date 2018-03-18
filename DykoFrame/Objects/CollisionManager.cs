using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DykoFrame 
{
    public class CollisionManager : MonoBehaviour
    {
        private List<CollisionEntry> collisionsEnt = new List<CollisionEntry>();
        private List<CollisionEntry> collisionsExit = new List<CollisionEntry>();

        public virtual void OnTriggerEnter(Collider trigger)
        {
            CollisionTask(collisionsEnt, trigger.gameObject);
        }

        public virtual void OnTriggerExit(Collider trigger)
        {
            CollisionTask(collisionsExit, trigger.gameObject);
        }

        public void RegisterOnTriggerEnter(CollisionMatch type, string name, CollisionTrigger callback, bool inclusive = false)
        {
            if (type == CollisionMatch.Instantion)
                throw new System.InvalidOperationException("you called bad func");

            CollisionEntry en;
            en.type = type;
            en.name = name;
            en.inclusive = inclusive;
            en.obj = null;
            en.callback = callback;
            collisionsEnt.Add(en);
        }

        public void RegisterOnTriggerEnter(CollisionMatch type, GameObject obj, CollisionTrigger callback, bool inclusive = false)
        {
            if (type != CollisionMatch.Instantion)
                throw new System.InvalidOperationException("you called bad func");

            CollisionEntry en;
            en.type = type;
            en.inclusive = inclusive;
            en.name = null;
            en.obj = obj;
            en.callback = callback;
            collisionsEnt.Add(en);
        }

        public void RegisterOnTriggerExit(CollisionMatch type, string name, CollisionTrigger callback, bool inclusive = false)
        {
            if (type == CollisionMatch.Instantion)
                throw new System.InvalidOperationException("you called bad func");

            CollisionEntry en;
            en.type = type;
            en.name = name;
            en.inclusive = inclusive;
            en.obj = null;
            en.callback = callback;
            collisionsExit.Add(en);
        }

        public void RegisterOnTriggerExit(CollisionMatch type, GameObject obj, CollisionTrigger callback, bool inclusive = false)
        {
            if (type != CollisionMatch.Instantion)
                throw new System.InvalidOperationException("you called bad func");

            CollisionEntry en;
            en.type = type;
            en.inclusive = inclusive;
            en.name = null;
            en.obj = obj;
            en.callback = callback;
            collisionsExit.Add(en);
        }

        protected void CollisionTask(List<CollisionEntry> l, GameObject obj)
        {
            foreach(CollisionEntry e in l)
            {
               switch (e.type)
               {
                    case CollisionMatch.Tag:
                        if (obj.tag == e.name && !e.inclusive)
                            e.callback(obj);
                        else if (obj.tag != e.name && e.inclusive)
                            e.callback(obj);
                        break;
                    case CollisionMatch.Name:
                        if (obj.name == e.name && !e.inclusive)
                            e.callback(obj);
                        else if (obj.name != e.name && e.inclusive)
                            e.callback(obj);
                        break;
                    case CollisionMatch.Instantion:
                        if (Object.ReferenceEquals(obj, e) && !e.inclusive)
                            e.callback(obj);
                        else if (!Object.ReferenceEquals(obj, e) && e.inclusive)
                            e.callback(obj);
                        break;
                }
            }
        }

    }

}