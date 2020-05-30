using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBrain : MonoBehaviour, ISample
{
    private MutatableNeuralNetwork nn;
    Rigidbody rb;
    public GameObject targetB;
    bool active = true;
    float avgDist = 0;
    int trials = 0;
    public bool Active { get { return active; } set { active = value; } }

    public MutatableNeuralNetwork NN { get { return nn; } set { nn = value; } }

    //float[] reward;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        for(int i = 0; i < NN.aFunctions[0].Length; i++)
        {
            NN.aFunctions[0][i] = 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator Train()
    {
        float tarX = Random.Range(0f, 20f);
        float tarZ = Random.Range(0f, 20f);
        Random.InitState((int)System.DateTime.Now.Ticks);
        if (trials % 5 == 0)
        {
            targetB.transform.localPosition = new Vector3(tarX, 1, tarZ);
        }
        transform.localPosition = (new Vector3(0, 1, 0));
        rb.velocity = (new Vector3(0, 0, 0));
        float angle = Vector3.Angle(Vector3.forward, targetB.transform.position - this.transform.position);

        //print(angle);
        float[] inputs = { rb.mass, angle, rb.angularDrag, targetB.transform.position.x - transform.position.x, targetB.transform.position.z - transform.position.z };
        float[] output = NN.feedForward(inputs);
        Vector3 force = new Vector3(output[0], output[1], output[2]);
        //print(force);
        rb.AddForce(force * 600);
        yield return new WaitForSeconds(2.7f);
        avgDist += (targetB.transform.position - transform.position).magnitude;
        trials++;
        float[] reward = { ((targetB.transform.position.x - transform.position.x) / 100), (((targetB.transform.position.z - transform.position.z) / 100) + ((targetB.transform.position.x - transform.position.x) / 100)) / 50, ((targetB.transform.position.z - transform.position.z) / 100) };
        NN.Train(inputs, null, reward);
    }

    public float Fitness()
    {
        float ret = avgDist / trials;
        avgDist = 0;
        trials = 0;
        return ret;
    }

    IEnumerator ISample.Train()
    {
        StartCoroutine(Train());
        yield return null;
    }

    public void updateNN(MutatableNeuralNetwork m)
    {
        nn = m;
    }

    public void Reset()
    {

    }

}
