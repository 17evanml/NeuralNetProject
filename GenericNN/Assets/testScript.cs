using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class testScript : MonoBehaviour
{
    public GameObject[] neurons;
    //public NeuralNetworkVisualizer nn;
    public MutatableNeuralNetwork nn;
    public Color test;
    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        int[] hiddens = { 100, 100, 100, 100,100,100,100 };
        nn = new MutatableNeuralNetwork(2, hiddens, 1, 0.2f, NeuralNetwork.NetworkType.Supervised);
        float[] input1 = { 0, 0 };
        float[] target1 = { 0.1f };
        float[] input2 = { 1, 1 };
        float[] target2 = { 0.1f };
        float[] input3 = { 1, 0 };
        float[] target3 = { 0.9f };
        float[] input4 = { 0, 1 };
        float[] target4 = { 0.9f };
        float[][] inputs = { input1, input2, input3, input4 };
        float[][] targets = {target1, target2, target3, target4};
        //int[] hiddens = { 2 };
        //nn = new NeuralNetwork(2, hiddens, 1);
        //print(nn.WriteString());
        //NeuralNetwork nn2 = new NeuralNetwork(nn.WriteString());
        //print(nn2.WriteString());
        //for (int i = 0; i < 10000; i++)
        //{
        //    int data = (int)Random.Range(0, 3.99999f);
        //    nn.Train(inputs[data], targets[data]);
        //    Random.InitState((int)System.DateTime.Now.Ticks);

        //}
        //StartCoroutine(train(60000));
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.M))
        {
            Time.timeScale = 100;
            float[] input1 = { 0, 0 };
            float[] target3 = { 0.9f };
            nn.Mutate();
            //print(nn.layers.Length);
            //print(nn.inputs);
            //for(int i = 0; i < nn.hiddens.Length; i++)
            //{
            //    print(nn.hiddens[i]);
            //}
            //print(nn.outputs);
            nn.feedForward(input1);

            nn.Train(input1, target3);
        }
    }

    //IEnumerator train(int times)
    //{
    //    float[] input1 = { 0, 0 };
    //    float[] target1 = { 0.1f };
    //    float[] input2 = { 1, 1 };
    //    float[] target2 = { 0.1f };
    //    float[] input3 = { 1, 0 };
    //    float[] target3 = { 0.9f };
    //    float[] input4 = { 0, 1 };
    //    float[] target4 = { 0.9f };
    //    float[][] inputs = { input1, input2, input3, input4 };
    //    float[][] targets = { target1, target2, target3, target4 };
    //    if (times >= 0)
    //    {
    //        for (int i = 0; i < 1; i++)
    //        {
    //            int data = (int)Random.Range(0, 3.99999f);
    //            nn.Train(inputs[data], targets[data]);
    //            Random.InitState((int)System.DateTime.Now.Ticks);
    //            yield return new WaitForSeconds(0.01f);
    //            StartCoroutine(train(times - 1));
    //            string tests = "";
    //            float[] guess = nn.feedForward(input1, true);
    //            tests += "Input: 0, 0, Output: " + guess[0] + "\n";
    //            guess = nn.feedForward(input2, false);
    //            tests += "Input: 1, 1, Output: " + guess[0] + "\n";
    //            guess = nn.feedForward(input3, false);
    //            tests += "Input: 1, 0, Output: " + guess[0] + "\n";
    //            guess = nn.feedForward(input4, false);
    //            tests += "Input: 0, 1" + "\n Output: " + guess[0] + "\n";
    //            text.text = tests;
    //        }
    //    }
    //}
}
