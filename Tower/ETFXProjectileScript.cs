using UnityEngine;
using System.Collections;

namespace EpicToonFX
{
    public class ETFXProjectileScript : MonoBehaviour
    {
        public GameObject impactParticle; // Effect spawned when projectile hits a collider
        public GameObject projectileParticle; // Effect attached to the gameobject as child
        public GameObject muzzleParticle; // Effect instantly spawned when gameobject is spawned
        [Header("Adjust if not using Sphere Collider")]
        public float colliderRadius = 1f;
        [Range(0f, 1f)] // This is an offset that moves the impact effect slightly away from the point of impact to reduce clipping of the impact effect
        public float collideOffset = 0.15f;
        public MonsterManager monsterManager;
        public float damage = 0f;

        void Start()
        {
            projectileParticle = Instantiate(projectileParticle, transform.position, transform.rotation) as GameObject;
            projectileParticle.transform.parent = transform;
            if (muzzleParticle)
            {
                muzzleParticle = Instantiate(muzzleParticle, transform.position, transform.rotation) as GameObject;
                Destroy(muzzleParticle, 1.5f); // 2nd parameter is lifetime of effect in seconds
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Monster"))
            {
                Debug.Log("몬스터 검출");
                other.GetComponent<MonsterManager>().DamageByTower(damage);
            }
        }

        void FixedUpdate()
        {
            if (GetComponent<Rigidbody>().velocity.magnitude != 0)
            {
                transform.rotation = Quaternion.LookRotation(GetComponent<Rigidbody>().velocity); // Sets rotation to look at direction of movement
            }

            RaycastHit hit;

            float radius; // Sets the radius of the collision detection
            if (transform.GetComponent<SphereCollider>())
                radius = transform.GetComponent<SphereCollider>().radius;
            else
                radius = colliderRadius;

            Vector3 direction = transform.GetComponent<Rigidbody>().velocity; // Gets the direction of the projectile, used for collision detection
            if (transform.GetComponent<Rigidbody>().useGravity)
                direction = direction.normalized;

            float detectionDistance = transform.GetComponent<Rigidbody>().velocity.magnitude * Time.deltaTime; // Distance of collision detection for this frame

            if (Physics.SphereCast(transform.position, radius, direction, out hit, detectionDistance)) // Checks if collision will happen
            {
                if (hit.collider.CompareTag("Monster")) // Check if the collided object has the "Monster" tag
                {
                    transform.position = hit.point + (hit.normal * collideOffset); // Move projectile to point of collision

                    GameObject impactP = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, hit.normal)) as GameObject; // Spawns impact effect

                    ParticleSystem[] trails = GetComponentsInChildren<ParticleSystem>(); // Gets a list of particle systems, as we need to detach the trails
                                                                                         //Component at [0] is that of the parent i.e. this object (if there is any)
                    for (int i = 1; i < trails.Length; i++) // Loop to cycle through found particle systems
                    {
                        ParticleSystem trail = trails[i];

                        if (trail.gameObject.name.Contains("Trail"))
                        {
                            trail.transform.SetParent(null); // Detaches the trail from the projectile
                            Destroy(trail.gameObject, 2f); // Removes the trail after seconds
                        }
                    }

                    Destroy(projectileParticle, 2f); // Removes particle effect after delay
                    Destroy(impactP, 3.5f); // Removes impact effect after delay

                    Destroy(gameObject); // Removes the projectile
                }
            }
            else
            {
                // 만약 몬스터에 도달하지 못하고 3초가 지나면 게임 오브젝트 파괴
                Invoke("DestroyProjectile", 3f);
            }
        }

        // 게임 오브젝트 파괴 함수
        void DestroyProjectile()
        {
            Destroy(gameObject);
        }
    }

}