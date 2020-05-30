using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutatableNeuralNetwork : NeuralNetwork
{

    public MutatableNeuralNetwork(int numI, int[] layerBounds, int[] hiddenBounds, int numO, float[] learningRateBounds, NetworkType typeI) : base(numI, HiddenInitialization(layerBounds, hiddenBounds), numO, LearningRateInitialization(learningRateBounds), typeI)
    {
        //Debug.Log(layers.Length);
        //for(int i = 0; i < base.hiddens.Length; i++)
        //{
        //    Debug.Log(base.hiddens[i]);
        //}
    }

    public MutatableNeuralNetwork(int numI, int[] numH, int numO, float learningRateI, NetworkType typeI) : base(numI, numH, numO, learningRateI, typeI)
    {

    }

    public int Mutate()
    {
        //MutateLayer();
        //for(int i = 0; i < layers.Length; i++)
        //{
        //    Debug.Log(layers[i].Columns + "x" + layers[i].Rows);
        //    Debug.Log(biases[i].Rows);
        //}
        int select= Random.Range(0, 100);
        if (select < 5)
        {
            MutateLayers();
        } else if (select > 5 && select < 25)
        {
            MutateLayer();
        }
        else if( select > 25 && select < 50)
        {
            MutateLearningRate(0, 0.5f);
        }
        return select;
    }


    public void MutateLayers()
    {
        int choice = Random.Range(0, 100);
        if (choice > 50)
        {
            base.addLayer();
        }
        else
        {
            base.subtractLayer();
        }
    }

    public void MutateLayer()
    {
        //for (int i = 0; i < layers.Length; i++)
        //{
        //    Debug.Log(layers[i].Columns + "x" + layers[i].Rows);
        //}
        int choice = Random.Range(0, 100);
        if (choice > 100)
        {
            int layer = Random.Range(1, layers.Length - 2);
            Random.InitState((int)System.DateTime.Now.Ticks);
            int activFunction = Random.Range(0, 2);
            base.addNeuron(layer, activFunction);
        }
        else
        {
            int layer = Random.Range(1, layers.Length - 2);
            Random.InitState((int)System.DateTime.Now.Ticks);
            base.subtractNeuron(layer);
        }
        //Debug.Log("______");
        //for (int i = 0; i < layers.Length; i++)
        //{
        //    Debug.Log(layers[i].Columns + "x" + layers[i].Rows);
        //}
    }

    public void MutateLearningRate(float lowerbound, float upperbound)
    {
        float lr = Random.Range(lowerbound, upperbound);
        Random.InitState((int)System.DateTime.Now.Ticks);
        LearningRate = lr;
    }

    public void MutateActivationFunction()
    {

    }

    public new MutatableNeuralNetwork Duplicate()
    {
        MutatableNeuralNetwork output = new MutatableNeuralNetwork(inputs, hiddens, outputs, LearningRate, type);
        for (int i = 0; i < layers.Length; i++)
        {
            output.layers[i] = layers[i].duplicate();
        }
        for (int i = 0; i < biases.Length; i++)
        {
            output.biases[i] = biases[i].duplicate();
        }
        output.aFunctions = (int[][])aFunctions.Clone();
        return output;
    }

    static int[] HiddenInitialization(int[] layerBounds, int[] hiddenBounds)
    {
        int numL = Random.Range(layerBounds[0], layerBounds[1]);
        Random.InitState((int)System.DateTime.Now.Ticks);
        int[] numH = new int[numL];
        for (int i = 0; i < numH.Length; i++)
        {
            numH[i] = Random.Range(hiddenBounds[0], hiddenBounds[1]);
            Random.InitState((int)System.DateTime.Now.Ticks);
        }

        return numH;
    }
    static float LearningRateInitialization(float[] learningRateBounds)
    {
        float lr = Random.Range(learningRateBounds[0], learningRateBounds[1]);
        Random.InitState((int)System.DateTime.Now.Ticks);
        return lr;
    }
}