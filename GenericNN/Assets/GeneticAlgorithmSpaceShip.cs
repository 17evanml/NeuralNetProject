using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgorithmSpaceShip : MonoBehaviour
{
    public FitnessBean[] population;
    public GameObject[] instancesG;
    bool active = true;
    // Start is called before the first frame update
    void Awake()
    {
        int[] hiddenRange = { 1, 30 };
        int[] layerRange = { 1, 7};
        float[] learningRange = { 0.001f, 0.1f };
        population = new FitnessBean[instancesG.Length];
        for (int i = 0; i < population.Length; i++)
        {
            population[i] = new FitnessBean();
            population[i].Sample = instancesG[i].GetComponentInChildren<ISample>();
            population[i].NN = new MutatableNeuralNetwork(5, layerRange, hiddenRange, 4, learningRange, NeuralNetwork.NetworkType.PolicyGradient);
            Random.InitState((int)System.DateTime.Now.Ticks);
            population[i].Sample.NN = population[i].NN;
            population[i].Target = instancesG[i];
        }

    }
    private void Start()
    {
        //StartCoroutine(Train());
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            active = true;
            StartCoroutine(Train(10));
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            active = false;
            StartCoroutine(Train(5));
        }
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            Time.timeScale += 0.1f;
            print(Time.timeScale);
        }
        if (Input.GetKeyDown(KeyCode.RightBracket))
        {
            Time.timeScale -= 0.1f;
            print(Time.timeScale);
        }
    }

    void prune()
    {
        for (int i = 0; i < population.Length; i++)
        {
            population[i].Fitness = population[i].Sample.Fitness();
        }
        for (int i = 1; i < population.Length; i++)
        {
            for (int j = i; j < population.Length; j++)
            {
                if (population[i - 1].Fitness < population[i].Fitness)
                {
                    swap<FitnessBean>(population, i - 1, i);
                    swap<GameObject>(instancesG, i - 1, i);
                }
            }
        }
        for (int i = 0; i < population.Length / 2; i++)
        {
            population[i].Target.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.green;
            population[i + (population.Length / 2)].Target.transform.GetChild(0).GetComponent<Renderer>().material.color = Color.black;

            population[i + (population.Length / 2)].NN = population[i].NN.Duplicate();
            population[i + population.Length / 2].NN.Mutate();
            //population[i+(population.Length/2)].NN.layers[0].print();
            population[i + (population.Length / 2)].Sample.NN = population[i + (population.Length / 2)].NN;
        }
    }

    public static void swap<T>(T[] array, int aIndex, int bIndex)
    {
        T temp = array[aIndex];
        array[aIndex] = array[bIndex];
        array[bIndex] = temp;
    }
    IEnumerator Train(float timer)
    {
        for (int i = 0; i < population.Length; i++)
        {
            population[i].Sample.Active = true;
            StartCoroutine(population[i].Sample.Train());
        }
        yield return new WaitForSeconds(timer);
        for (int i = 0; i < population.Length; i++)
        {
            population[i].Sample.Active = false;
            population[i].Sample.Reset();
        }
        prune();
        yield return new WaitForSeconds(1);
        if(active)
        {
            StartCoroutine(Train(timer));
        }

    }
}


