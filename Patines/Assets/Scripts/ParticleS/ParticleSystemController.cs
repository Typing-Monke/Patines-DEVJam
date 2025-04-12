using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemController : MonoBehaviour
{
    public ParticleSystem[] particles;
    public Transform[] particlePositions;

    //public bool trigger;
    public int particleIndex;
    void Start() {
        
    }
    void Update() {
        //if (trigger) {
        //    TriggerParticleSystem(particles[particleIndex], particlePositions[particleIndex]);
        //    trigger = false;
        //}
    }
    public void TriggerParticleSystem(int index) {
        ParticleSystem particleInstance = Instantiate(particles[index], particlePositions[index].position, particles[index].transform.rotation);        
    }
}
