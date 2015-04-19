using UnityEngine;

namespace JamSuite {

    public static class Physics {

        public static void IgnoreCollision(GameObject object1, GameObject object2, bool ignore = true) {
            if(object1 == object2) return;

            Collider[] colliders1 = object1.GetComponentsInChildren<Collider>();
            Collider[] colliders2 = object2.GetComponentsInChildren<Collider>();

            foreach(Collider collider1 in colliders1) {
                foreach(Collider collider2 in colliders2) {
                    if(collider1 == collider2) continue;
                    UnityEngine.Physics.IgnoreCollision(collider1, collider2, ignore);
                }
            }
        }
        
        public static void IgnoreCollision(Rigidbody body1, Rigidbody body2, bool ignore = true) {
            if(body1 == body2) return;

            Collider[] colliders1 = body1.GetComponentsInChildren<Collider>();
            Collider[] colliders2 = body2.GetComponentsInChildren<Collider>();

            foreach(Collider collider1 in colliders1) {
                foreach(Collider collider2 in colliders2) {
                    if(collider1 == collider2) continue;
                    UnityEngine.Physics.IgnoreCollision(collider1, collider2, ignore);
                }
            }
        }
    }
}
