using UnityEngine;

namespace EpicToonFX
{
    public class ETFXLightFade : MonoBehaviour
    {
        [Header("Seconds to dim the light")]
        public float life = 0.2f;
        public bool killAfterLife = true;

        private Light li;
        private float initIntensity;

        private bool fading = false;
        private float fadeStartTime;

        // Use this for initialization
        void Start()
        {
            li = GetComponent<Light>();
            if (li != null)
            {
                initIntensity = li.intensity;
                fading = true;
                fadeStartTime = Time.time;
            }
            else
            {
                Debug.LogError("No Light component found on " + gameObject.name);
            }
        }

        void Update()
        {
            if (fading)
            {
                float elapsedTime = Time.time - fadeStartTime;
                float fadeProgress = elapsedTime / life;
                li.intensity = Mathf.Lerp(initIntensity, 0f, fadeProgress);

                if (fadeProgress >= 1f)
                {
                    if (killAfterLife)
                    {
                        Destroy(gameObject);
                    }
                    else
                    {
                        fading = false;
                    }
                }
            }
        }
    }
}
