using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace ZincFramework
{
    public class PhysicsUtility
    {
        #region …‰œﬂºÏ≤‚œ‡πÿ
        public static void RayCast(Ray ray, UnityAction<RaycastHit> callback, float maxDistance, int layerMask)
        {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxDistance, layerMask))
            {
                callback.Invoke(hit);
            }
        }

        public static void RayCast(Ray ray, UnityAction<RaycastHit> callback, float maxDistance)
        {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxDistance))
            {
                callback.Invoke(hit);
            }
        }

        public static void RayCast(Ray ray, UnityAction<RaycastHit> callback)
        {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                callback.Invoke(hit);
            }
        }

        public static void RayCast(Ray ray, UnityAction<GameObject> callback, float maxDistance, int layerMask)
        {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxDistance, layerMask))
            {
                callback.Invoke(hit.collider.gameObject);
            }
        }

        public static void RayCast(Ray ray, UnityAction<GameObject> callback, float maxDistance)
        {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxDistance))
            {
                callback.Invoke(hit.collider.gameObject);
            }
        }

        public static void RayCast(Ray ray, UnityAction<GameObject> callback)
        {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                callback.Invoke(hit.collider.gameObject);
            }
        }

        public static void RayCast<T>(Ray ray, UnityAction<T> callback, float maxDistance, int layerMask) where T : Component
        {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxDistance, layerMask))
            {
                if (hit.collider.gameObject.TryGetComponent<T>(out T component))
                {
                    callback.Invoke(component);
                }
            }
        }

        public static void RayCast<T>(Ray ray, UnityAction<T> callback, float maxDistance) where T : Component
        {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxDistance))
            {
                if (hit.collider.gameObject.TryGetComponent<T>(out T component))
                {
                    callback.Invoke(component);
                }
            }
        }

        public static void RayCast<T>(Ray ray, UnityAction<T> callback) where T : Component
        {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.TryGetComponent<T>(out T component))
                {
                    callback.Invoke(component);
                }
            }
        }

        public static void RayCastAll(Ray ray, UnityAction<RaycastHit> callback, float maxDistance, int layerMask)
        {
            RaycastHit[] hits = Physics.RaycastAll(ray, maxDistance, layerMask);
            for (int i = 0; i < hits.Length; i++)
            {
                callback.Invoke(hits[i]);
            }
        }

        public static void RayCastAll(Ray ray, UnityAction<RaycastHit> callback, float maxDistance)
        {
            RaycastHit[] hits = Physics.RaycastAll(ray, maxDistance);
            for (int i = 0; i < hits.Length; i++)
            {
                callback.Invoke(hits[i]);
            }
        }

        public static void RayCastAll(Ray ray, UnityAction<RaycastHit> callback)
        {
            RaycastHit[] hits = Physics.RaycastAll(ray);
            for (int i = 0; i < hits.Length; i++)
            {
                callback.Invoke(hits[i]);
            }
        }

        public static void RayCastAll(Ray ray, UnityAction<GameObject> callback, float maxDistance, int layerMask)
        {
            RaycastHit[] hits = Physics.RaycastAll(ray, maxDistance, layerMask);
            for (int i = 0; i < hits.Length; i++)
            {
                callback.Invoke(hits[i].collider.gameObject);
            }
        }

        public static void RayCastAll(Ray ray, UnityAction<GameObject> callback, float maxDistance)
        {
            RaycastHit[] hits = Physics.RaycastAll(ray, maxDistance);
            for (int i = 0; i < hits.Length; i++)
            {
                callback.Invoke(hits[i].collider.gameObject);
            }
        }

        public static void RayCastAll(Ray ray, UnityAction<GameObject> callback)
        {
            RaycastHit[] hits = Physics.RaycastAll(ray);
            for (int i = 0; i < hits.Length; i++)
            {
                callback.Invoke(hits[i].collider.gameObject);
            }
        }

        public static void RayCastAll<T>(Ray ray, UnityAction<T> callback, float maxDistance, int layerMask) where T : Component
        {
            RaycastHit[] hits = Physics.RaycastAll(ray, maxDistance, layerMask);
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.gameObject.TryGetComponent<T>(out T component))
                {
                    callback.Invoke(component);
                }
            }
        }

        public static void RayCastAll<T>(Ray ray, UnityAction<T> callback, float maxDistance) where T : Component
        {
            RaycastHit[] hits = Physics.RaycastAll(ray, maxDistance);
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.gameObject.TryGetComponent<T>(out T component))
                {
                    callback.Invoke(component);
                }
            }
        }

        public static void RayCastAll<T>(Ray ray, UnityAction<T> callback) where T : Component
        {
            RaycastHit[] hits = Physics.RaycastAll(ray);
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.gameObject.TryGetComponent<T>(out T component))
                {
                    callback.Invoke(component);
                }
            }
        }
        #endregion

        #region ∑∂ŒßºÏ≤‚
        public static void OverlapBox<T>(Vector3 centerPostion, Vector3 halfExtents, Quaternion rotation, UnityAction<T> callback, int layerMask) where T : class
        {
            Collider[] colliders = Physics.OverlapBox(centerPostion, halfExtents, rotation, layerMask, QueryTriggerInteraction.Collide);
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

        public static void OverlapBox<T>(Vector3 centerPostion, Vector3 halfExtents, Quaternion rotation, UnityAction<T> callback) where T : class
        {
            Collider[] colliders = Physics.OverlapBox(centerPostion, halfExtents, rotation);
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

        public static void OverlapBox<T>(Vector3 centerPostion, Vector3 halfExtents, UnityAction<T> callback) where T : class
        {
            Collider[] colliders = Physics.OverlapBox(centerPostion, halfExtents);
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

        public static void OverlapSphere<T>(Vector3 centerPostion, float radius, UnityAction<T> callback, int layerMask) where T : class
        {
            Collider[] colliders = Physics.OverlapSphere(centerPostion, radius, layerMask, QueryTriggerInteraction.Collide);
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

        public static void OverlapSphere<T>(Vector3 centerPostion, float radius, UnityAction<T> callback) where T : class
        {
            Collider[] colliders = Physics.OverlapSphere(centerPostion, radius);
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

        #endregion
    }
}
