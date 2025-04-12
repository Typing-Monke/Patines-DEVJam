using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleBehaviour : MonoBehaviour
{
    private ParticleSystem particle;
    void Start() {
        particle = GetComponent<ParticleSystem>();
    }

    void Update() {
        if (particle.isPlaying == false) { 
            Destroy(particle.gameObject);
        }
    }
}
