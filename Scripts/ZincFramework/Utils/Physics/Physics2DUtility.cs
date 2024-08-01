using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ZincFramework
{
    public class Physics2DUtility
    {
        public static void RayCast(Vector2 origin, Vector2 direction, UnityAction<RaycastHit2D> callback, float maxDistance, int layerMask)
        {
            RaycastHit2D hit = Physics2D.Raycast(direction, origin, maxDistance, layerMask);
            if (hit)
            {
                callback.Invoke(hit);
            }
        }

        public static void RayCast(Vector2 origin, Vector2 direction, UnityAction<RaycastHit2D> callback, float maxDistance)
        {
            RaycastHit2D hit = Physics2D.Raycast(direction, origin, maxDistance);
            if (hit)
            {
                callback.Invoke(hit);
            }
        }

        public static void RayCast(Vector2 origin, Vector2 direction, UnityAction<RaycastHit2D> callback)
        {
            RaycastHit2D hit = Physics2D.Raycast(direction, origin);
            if (hit)
            {
                callback.Invoke(hit);
            }
        }

        public static void RayCast(Vector2 origin, Vector2 direction, UnityAction<GameObject> callback, float maxDistance, int layerMask)
        {
            RaycastHit2D hit = Physics2D.Raycast(direction, origin, maxDistance, layerMask);
            if (hit)
            {
                callback.Invoke(hit.collider.gameObject);
            }
        }

        public static void RayCast(Vector2 origin, Vector2 direction, UnityAction<GameObject> callback, float maxDistance)
        {
            RaycastHit2D hit = Physics2D.Raycast(direction, origin, maxDistance);
            if (hit)
            {
                callback.Invoke(hit.collider.gameObject);
            }
        }

        public static void RayCast(Vector2 origin, Vector2 direction, UnityAction<GameObject> callback)
        {
            RaycastHit2D hit = Physics2D.Raycast(direction, origin);
            if (hit)
            {
                callback.Invoke(hit.collider.gameObject);
            }
        }

        public static void RayCast<T>(Vector2 origin, Vector2 direction, UnityAction<T> callback, float maxDistance, int layerMask) where T : Component
        {
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, maxDistance, layerMask);
            if (hit)
            {
                if (hit.collider.gameObject.TryGetComponent<T>(out T component))
                {
                    callback.Invoke(component);
                }
            }
        }

        public static void RayCast<T>(Vector2 origin, Vector2 direction, UnityAction<T> callback, float maxDistance) where T : Component
        {
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, maxDistance);
            if (hit)
            {
                if (hit.collider.gameObject.TryGetComponent<T>(out T component))
                {
                    callback.Invoke(component);
                }
            }
        }

        public static void RayCast<T>(Vector2 origin, Vector2 direction, UnityAction<T> callback) where T : Component
        {
            RaycastHit2D hit = Physics2D.Raycast(origin, direction);
            if (hit)
            {
                if (hit.collider.gameObject.TryGetComponent<T>(out T component))
                {
                    callback.Invoke(component);
                }
            }
        }

        public static void RayCastAll(Vector2 origin, Vector2 direction, UnityAction<RaycastHit2D> callback, float maxDistance, int layerMask)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(direction, origin, maxDistance, layerMask);
            for (int i = 0; i < hits.Length; i++)
            {
                callback.Invoke(hits[i]);
            }
        }

        public static void RayCastAll(Vector2 origin, Vector2 direction, UnityAction<RaycastHit2D> callback, float maxDistance)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(direction, origin, maxDistance);
            for (int i = 0; i < hits.Length; i++)
            {
                callback.Invoke(hits[i]);
            }
        }

        public static void RayCastAll2D(Vector2 origin, Vector2 direction, UnityAction<RaycastHit2D> callback)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(direction, origin);
            for (int i = 0; i < hits.Length; i++)
            {
                callback.Invoke(hits[i]);
            }
        }

        public static void RayCastAll(Vector2 origin, Vector2 direction, UnityAction<GameObject> callback, float maxDistance, int layerMask)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(direction, origin, maxDistance, layerMask);
            for (int i = 0; i < hits.Length; i++)
            {
                callback.Invoke(hits[i].collider.gameObject);
            }
        }

        public static void RayCastAll(Vector2 origin, Vector2 direction, UnityAction<GameObject> callback, float maxDistance)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(direction, origin, maxDistance);
            for (int i = 0; i < hits.Length; i++)
            {
                callback.Invoke(hits[i].collider.gameObject);
            }
        }

        public static void RayCastAll(Vector2 origin, Vector2 direction, UnityAction<GameObject> callback)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(direction, origin);
            for (int i = 0; i < hits.Length; i++)
            {
                callback.Invoke(hits[i].collider.gameObject);
            }
        }

        public static void RayCastAll<T>(Vector2 origin, Vector2 direction, UnityAction<T> callback, float maxDistance, int layerMask) where T : Component
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, maxDistance, layerMask);
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.gameObject.TryGetComponent<T>(out T component))
                {
                    callback.Invoke(component);
                }
            }
        }

        public static void RayCastAll<T>(Vector2 origin, Vector2 direction, UnityAction<T> callback, float maxDistance) where T : Component
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, maxDistance);
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.gameObject.TryGetComponent<T>(out T component))
                {
                    callback.Invoke(component);
                }
            }
        }

        public static void RayCastAll<T>(Vector2 origin, Vector2 direction, UnityAction<T> callback) where T : Component
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction);
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.gameObject.TryGetComponent<T>(out T component))
                {
                    callback.Invoke(component);
                }
            }
        }

        public static void OverlapBox<T>(Vector2 centerPostion, Vector2 size, float angle, UnityAction<T> callback, int layerMask) where T : class
        {
            Collider2D collider = Physics2D.OverlapBox(centerPostion, size, angle, layerMask);
            if (typeof(T) == typeof(GameObject))
            {
                callback.Invoke(collider.gameObject as T);
            }
            else if (typeof(T) == typeof(Collider))
            {
                callback.Invoke(collider as T);
            }
            else
            {
                if (collider.TryGetComponent<T>(out T component))
                {
                    callback.Invoke(component);
                }
            }
        }

        public static void OverlapBox<T>(Vector2 centerPostion, Vector2 size, float angle, UnityAction<T> callback) where T : class
        {
            Collider2D collider = Physics2D.OverlapBox(centerPostion, size, angle);
            if (typeof(T) == typeof(GameObject))
            {
                callback.Invoke(collider.gameObject as T);
            }
            else if (typeof(T) == typeof(Collider))
            {
                callback.Invoke(collider as T);
            }
            else
            {
                if (collider.TryGetComponent<T>(out T component))
                {
                    callback.Invoke(component);
                }
            }
        }

        public static void OverlapCircle<T>(Vector2 centerPostion, float radius, UnityAction<T> callback, int layerMask) where T : class
        {
            Collider2D collider = Physics2D.OverlapCircle(centerPostion, radius, layerMask);
            if (typeof(T) == typeof(GameObject))
            {
                callback.Invoke(collider.gameObject as T);
            }
            else if (typeof(T) == typeof(Collider))
            {
                callback.Invoke(collider as T);
            }
            else
            {
                if (collider.TryGetComponent<T>(out T component))
                {
                    callback.Invoke(component);
                }
            }
        }

        public static void OverlapCircle<T>(Vector2 centerPostion, float radius, UnityAction<T> callback) where T : class
        {
            Collider2D collider = Physics2D.OverlapCircle(centerPostion, radius);
            if (typeof(T) == typeof(GameObject))
            {
                callback.Invoke(collider.gameObject as T);
            }
            else if (typeof(T) == typeof(Collider))
            {
                callback.Invoke(collider as T);
            }
            else
            {
                if (collider.TryGetComponent<T>(out T component))
                {
                    callback.Invoke(component);
                }
            }
        }

        public static void OverlapBoxAll<T>(Vector2 centerPostion, Vector2 size, float angle, UnityAction<T> callback, int layerMask) where T : class
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(centerPostion, size, angle, layerMask);
            if (typeof(T) == typeof(GameObject))
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    callback.Invoke(colliders[i].gameObject as T);
                }
            }
            else if (typeof(T) == typeof(Collider))
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    callback.Invoke(colliders[i] as T);
                }
            }
            else
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].TryGetComponent<T>(out T component))
                    {
                        callback.Invoke(component);
                    }
                }
            }
        }

        public static void OverlapBoxAll<T>(Vector2 centerPostion, Vector2 size, float angle, UnityAction<T> callback) where T : class
        {
            Collider2D[] colliders = Physics2D.OverlapBoxAll(centerPostion, size, angle);
            if (typeof(T) == typeof(GameObject))
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    callback.Invoke(colliders[i].gameObject as T);
                }
            }
            else if (typeof(T) == typeof(Collider))
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    callback.Invoke(colliders[i] as T);
                }
            }
            else
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].TryGetComponent<T>(out T component))
                    {
                        callback.Invoke(component);
                    }
                }
            }
        }

        public static void OverlapCircleAll<T>(Vector2 centerPostion, float radius, UnityAction<T> callback, int layerMask) where T : class
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(centerPostion, radius, layerMask);
            if (typeof(T) == typeof(GameObject))
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    callback.Invoke(colliders[i].gameObject as T);
                }
            }
            else if (typeof(T) == typeof(Collider))
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    callback.Invoke(colliders[i] as T);
                }
            }
            else
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].TryGetComponent<T>(out T component))
                    {
                        callback.Invoke(component);
                    }
                }
            }
        }

        public static void OverlapCircleAll<T>(Vector2 centerPostion, float radius, UnityAction<T> callback) where T : class
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(centerPostion, radius);
            if (typeof(T) == typeof(GameObject))
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    callback.Invoke(colliders[i].gameObject as T);
                }
            }
            else if (typeof(T) == typeof(Collider))
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    callback.Invoke(colliders[i] as T);
                }
            }
            else
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].TryGetComponent<T>(out T component))
                    {
                        callback.Invoke(component);
                    }
                }
            }
        }
    }
}

