using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipBrain : MonoBehaviour, ISample
{
    private bool active;
    private MutatableNeuralNetwork nn;
    public bool Active { get { return active; } set { active = value; } }
    public MutatableNeuralNetwork NN { get { return nn; } set { nn = value; } }
    float fitness = 0;
    Rigidbody rb;
    public Transform[] thrusters;
    Vector3 defaultPos;
    public GameObject food;
    public GameObject[] currentFood;
    float[] force;
    Vector3[] foodPos;
    int count;
    // Start is called before the first frame update
    void Start()
    {
        force = new float[thrusters.Length];
        foodPos = new Vector3[currentFood.Length];
        for (int i = 0; i < currentFood.Length; i++)
        {
            foodPos[i] = currentFood[i].transform.position;
        }
        rb = gameObject.GetComponent<Rigidbody>();
        defaultPos = transform.position;
    }
    // Update is called once per frame
    void Update()
    {


    }
    public float Fitness()
    {
        float ret = fitness;
        fitness = 0;
        return ret;
    }

    public IEnumerator Train()
    {
        if (active)
        {
            float angle = Vector3.Angle(Vector3.up, transform.up);
            //print(angle);
            rb.isKinematic = false;
            float[] input = { transform.position.x, transform.position.y, transform.position.z, rb.velocity.magnitude, angle };
            float[] output = NN.feedForward(input);
            for (int i = 0; i < output.Length; i++)
            {
                rb.AddForceAtPosition(transform.up * force[i] * 30, thrusters[i].position);
            }
            yield return new WaitForSeconds(0.1f);
            float avg = averageThrust(output);
            float[] reward = new float[thrusters.Length];
            for (int i = 0; i < thrusters.Length; i++)
            {
                if (angle > 8)
                {
                    reward[i] += avg - output[i];
                }
                if (transform.localPosition.y > 3)
                {
                    reward[i] += (3 - transform.position.y)/50;
                }
                else if (transform.localPosition.y < 2)
                {
                    reward[i] += (2 - transform.position.y)/50;

                }
                else
                {
                    reward[i] = -rb.velocity.y/10;
                }
            }
            bool move = NN.Train(input, null, reward);
            StartCoroutine(Train());
        }
        else
        {
            yield return null;
        }
    }


    public void Reset()
    {
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.position = defaultPos;
        rb.rotation = Quaternion.identity;
        for(int i = 0; i < currentFood.Length; i++)
        {
            if (currentFood[i] == null)
            {
                Instantiate(food, foodPos[i], Quaternion.identity);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print("enter");
        if (other.tag.Equals("Food"))
        {
            Destroy(other.gameObject);
            fitness+= 100;
        }
    }

    private float averageThrust(float[] output)
    {
        float sum = 0;
        for(int i = 0; i < output.Length; i++)
        {
            sum += output[i];
        }
        return sum / output.Length;
    }

    private float edit()
    {
        return 0;
    }
    private void OnCollisionEnter(Collision collision)
    {
        print("oof");
        fitness = -100;
    }


}
