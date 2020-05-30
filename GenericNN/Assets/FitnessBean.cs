using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitnessBean
{
    private MutatableNeuralNetwork nn;
    private float fitness;
    private ISample sample;
    private GameObject target;
    public float Fitness { get { return fitness; } set { fitness = value; } }
    public MutatableNeuralNetwork NN { get { return nn; } set { nn = value; } }
    public ISample Sample { get { return sample; } set { sample = value; } }
    public GameObject Target { get { return target; } set { target = value; } }
}
