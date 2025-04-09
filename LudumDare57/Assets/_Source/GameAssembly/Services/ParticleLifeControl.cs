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
            yield return new WaitForSeconds(lifetime);

            if (particles.gameObject != null)
                Object.Destroy(particles.gameObject);
        }
    }
}