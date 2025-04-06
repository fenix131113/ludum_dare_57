using Services;
using UnityEngine;

namespace ItemsSystem.Objects.Breakable
{
    public class ParticleBreakableObject : ABreakableItemObject
    {
        [SerializeField] protected ParticleSystem breakParticles;
        [SerializeField] protected float breakParticlesLifetime;
        [SerializeField] private bool resetParticlesRotation = true;

        protected override void OnBreak()
        {
            breakParticles.transform.parent = null;
            breakParticles.Play();

            if (resetParticlesRotation)
                breakParticles.transform.rotation = Quaternion.Euler(Vector3.zero);

            ParticleLifeControl.RegisterParticleLifeWithTime(breakParticles, breakParticlesLifetime);
        }
    }
}