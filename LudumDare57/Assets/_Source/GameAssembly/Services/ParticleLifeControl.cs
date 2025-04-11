using System.Collections;
using UnityEngine;

namespace Services
{
    public static class ParticleLifeControl
    {
        public static void RegisterParticleLifeWithTime(ParticleSystem particles, float lifetime) =>
            CoroutineRunner.Run(ParticleLifeCoroutine(particles, lifetime));

        private static IEnumerator ParticleLifeCoroutine(ParticleSystem particles, float lifetime)
        {
            var timer = 0f;
            while (timer < lifetime)
            {
                if (particles == null || particles.gameObject == null)
                    yield break;

                timer += Time.deltaTime;
                yield return null;
            }

            if (particles != null && particles.gameObject != null)
            {
                Object.Destroy(particles.gameObject);
            }
        }
    }
}