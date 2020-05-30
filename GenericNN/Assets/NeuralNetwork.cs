using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NeuralNetwork
{
    public delegate float ActivationFunction(float input);

    //public enum activationFunctions { Sigmoid }
    private float learningrate;
    public float LearningRate
    {
        get { return learningrate; }
        set { learningrate = value; }
    }
    public enum NetworkType { Supervised, PolicyGradient }
    protected NetworkType type;
    public int inputs;
    public int[] hiddens;
    public int outputs;
    private int defaultAFunction = 2;
    public Matrix[] layers;
    protected Matrix[] biases;
    ActivationFunction[] ActivationFunctions;
    ActivationFunction[] ActivationFunctionDerivs;
    public int[][] aFunctions;
    public NeuralNetwork(int numI, int[] numH, int numO, float learningRateI, NetworkType typeI)
    {
        LearningRate = learningRateI;
        type = typeI;
        inputs = numI;
        hiddens = (int[])numH.Clone();
        outputs = numO;
        layers = new Matrix[1 + hiddens.Length];
        aFunctions = new int[layers.Length][];
        for (int i = 0; i < hiddens.Length; i++)
        {
            aFunctions[i] = new int[hiddens[i]];
        }
        aFunctions[aFunctions.Length - 1] = new int[outputs];
        biases = new Matrix[1 + hiddens.Length];
        layers[0] = new Matrix(hiddens[0], inputs);
        biases[0] = new Matrix(hiddens[0], 1);
        for (int i = 1; i < hiddens.Length; i++)
        {
            layers[i] = new Matrix(hiddens[i], hiddens[i - 1]);
            biases[i] = new Matrix(hiddens[i], 1);
        }
        layers[layers.Length - 1] = new Matrix(outputs, hiddens[hiddens.Length - 1]);
        biases[layers.Length - 1] = new Matrix(outputs, 1);
        ActivationFunctions = new ActivationFunction[3];
        ActivationFunctionDerivs = new ActivationFunction[3];
        ActivationFunctions[0] = Sigmoid;
        ActivationFunctionDerivs[0] = DSigmoid;
        ActivationFunctions[1] = ReLU;
        ActivationFunctionDerivs[1] = DReLU;
        ActivationFunctions[2] = Tanh;
        ActivationFunctionDerivs[2] = DTanh;
        for (int i = 0; i < aFunctions.Length; i++)
        {
            for (int j = 0; j < aFunctions[i].Length; j++)
            {
                aFunctions[i][j] = defaultAFunction;
            }
        }
        for (int l = 0; l < layers.Length; l++)
        {
            for (int i = 0; i < layers[l].Rows; i++)
            {
                for (int j = 0; j < layers[l].Columns; j++)
                {
                    layers[l][i, j] = Random.Range(0f, 0.1f);
                    Random.InitState((int)System.DateTime.Now.Ticks);

                }
            }
        }
        for (int l = 0; l < biases.Length; l++)
        {
            for (int i = 0; i < biases[l].Rows; i++)
            {
                biases[l][i, 0] = Random.Range(0f, 0.1f);
                Random.InitState((int)System.DateTime.Now.Ticks);

            }
        }
    }

    public NeuralNetwork(string NN)
    {
        int i = 0;
        int numStart = 0;
        while (!NN[i].Equals(';'))
        {
            i++;
        }
        LearningRate = float.Parse(NN.Substring(numStart, i - numStart));
        i++;
        numStart = i;
        while (!NN[i].Equals(';'))
        {
            i++;
        }
        type = (NeuralNetwork.NetworkType)int.Parse(NN.Substring(numStart, i - numStart));
        i++;
        numStart = i;
        while (!NN[i].Equals(';'))
        {
            i++;
        }
        inputs = int.Parse(NN.Substring(numStart, i - numStart));
        i++;
        numStart = i;
        while (!NN[i].Equals(';'))
        {
            i++;
        }
        outputs = int.Parse(NN.Substring(numStart, i - numStart));
        i++;
        numStart = i;
        while (!NN[i].Equals(';'))
        {
            i++;
        }
        int numHidden = int.Parse(NN.Substring(numStart, i - numStart));
        i++;
        numStart = i;
        hiddens = new int[numHidden];
        Debug.Log(numHidden);
        for (int h = 0; h < numHidden; h++)
        {
            while (!NN[i].Equals(';'))
            {
                i++;
            }
            hiddens[h] = int.Parse(NN.Substring(numStart, i - numStart));
            i++;
            numStart = i;
        }
        layers = new Matrix[1 + hiddens.Length];
        biases = new Matrix[1 + hiddens.Length];
        layers[0] = new Matrix(hiddens[0], inputs);
        biases[0] = new Matrix(hiddens[0], 1);
        Debug.Log(hiddens[0]);
        for (int j = 1; j < hiddens.Length; j++)
        {
            layers[j] = new Matrix(hiddens[j], hiddens[j - 1]);
            biases[j] = new Matrix(hiddens[j], 1);
        }
        layers[layers.Length - 1] = new Matrix(outputs, hiddens[hiddens.Length - 1]);
        biases[layers.Length - 1] = new Matrix(outputs, 1);
        layers[layers.Length - 1] = new Matrix(outputs, hiddens[hiddens.Length - 1]);
        biases[layers.Length - 1] = new Matrix(outputs, 1);
        for (int j = 0; j < layers.Length; j++)
        {
            //Debug.Log("loop1");
            for (int k = 0; k < layers[j].Rows; k++)
            {
                //Debug.Log("loop2");
                //Debug.Log(layers[j].Columns);
                for (int l = 0; l < layers[j].Columns; l++)
                {
                    //Debug.Log("loop3");
                    numStart = i;
                    while (!NN[i].Equals(';'))
                    {
                        i++;
                    }
                    //Debug.Log(NN.Substring(numStart, i - numStart));
                    layers[j][k, l] = float.Parse(NN.Substring(numStart, i - numStart));
                    i++;
                }
            }
        }
        for (int j = 0; j < biases.Length; j++)
        {
            for (int k = 0; k < biases[j].Rows; k++)
            {
                for (int l = 0; l < biases[j].Columns; l++)
                {
                    numStart = i;
                    while (!NN[i].Equals(';'))
                    {
                        i++;
                    }
                    //Debug.Log(NN.Substring(numStart, i - numStart));
                    biases[j][k, l] = float.Parse(NN.Substring(numStart, i - numStart));
                    i++;
                }
            }
        }
    }

    private float Sigmoid(float n)
    {
        return 1 / (1 + Mathf.Exp(-n));
    }
    private float DSigmoid(float n)
    {
        return Sigmoid(n) * (1 - Sigmoid(n));
    }
    private float ReLU(float n)
    {
        if (n > 0)
        {
            return n;
        }
        else
        {
            return 0;
        }
    }
    private float DReLU(float n)
    {
        if (n > 0)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
    private float Tanh(float n)
    {
        return (Mathf.Exp(n) - Mathf.Exp(-n)) / (Mathf.Exp(n) + Mathf.Exp(-n));
    }
    private float DTanh(float n)
    {
        return 1 - (Tanh(n) * Tanh(n));
    }
    public float[] feedForward(float[] fInput)
    {
        Matrix input = new Matrix(fInput);
        for (int l = 0; l < layers.Length; l++)
        {
            //Debug.Log(l);
            Matrix hidden = layers[l] * input;
            hidden = hidden + biases[l];
            for (int i = 0; i < hidden.Rows; i++)
            {
                for (int j = 0; j < hidden.Columns; j++)
                {
                    hidden[i, j] = ActivationFunctions[aFunctions[l][i]](hidden[i, j]);
                }
            }
            input = hidden;
        }
        return input.toArray();
    }

    public bool Train(float[] fInput, float[] fCorrect, float[] reward = default)
    {
        Matrix input = new Matrix(fInput);
        Matrix[] postActivationFPrime = new Matrix[layers.Length];
        Matrix[] postActivationF = new Matrix[layers.Length];
        int a = 0;
        for (int l = 0; l < layers.Length; l++)
        {
            Matrix hidden = layers[l] * input;
            hidden = hidden + biases[l];
            postActivationFPrime[l] = hidden.duplicate();
            for (int i = 0; i < hidden.Rows; i++)
            {
                for (int j = 0; j < hidden.Columns; j++)
                {
                    postActivationFPrime[l][i, j] = ActivationFunctionDerivs[aFunctions[l][i]](hidden[i, j]);
                    hidden[i, j] = ActivationFunctions[aFunctions[l][i]](hidden[i, j]);
                }
            }
            a += hidden.Rows;
            postActivationF[l] = hidden.duplicate();

            input = hidden;
        }
        Matrix error = new Matrix(outputs, 1);
        if (type.Equals(NetworkType.Supervised))
        {
            for (int i = 0; i < error.Rows; i++)
            {
                error[i, 0] = fCorrect[i] - postActivationF[postActivationF.Length - 1].toArray()[i];
            }
        }
        else if (type.Equals(NetworkType.PolicyGradient))
        {
            for (int i = 0; i < error.Rows; i++)
            {
                error[i, 0] = (float)reward[i];
            }
        }
        //Debug.Log(layers.Length);
        for (int l = layers.Length - 1; l >= 0; l--)
        {
            //
            Matrix deltaB = learningrate * error.eWiseMultiply(postActivationFPrime[l]);
            biases[l] += deltaB;
            if (l > 0)
            {
                deltaB *= postActivationF[l - 1].Transpose();
            }
            else
            {
                //needs sigmoid
                deltaB *= new Matrix(fInput).Transpose();
            }
            layers[l] += deltaB;

            error = layers[l].Transpose() * error;
        }
        //learningrate = Random.Range(0.001f, 1f);
        //Random.InitState((int)System.DateTime.Now.Ticks);
        return true;
    }


    public string WriteString()
    {
        string output = "";
        output = output + learningrate + ';';
        output = output + type + ';';
        output = output + inputs + ';';
        output = output + outputs + ';';
        output = output + hiddens.Length + ';';
        for (int i = 0; i < hiddens.Length; i++)
        {
            output = output + hiddens[i] + ';';
        }
        for (int i = 0; i < layers.Length; i++)
        {
            for (int j = 0; j < layers[i].Rows; j++)
            {
                for (int k = 0; k < layers[i].Columns; k++)
                {
                    output += layers[i][j, k];
                    output += ';';
                }
            }
        }
        for (int i = 0; i < biases.Length; i++)
        {
            for (int j = 0; j < biases[i].Rows; j++)
            {
                for (int k = 0; k < biases[i].Columns; k++)
                {
                    output += biases[i][j, k];
                    output += ';';
                }
            }
        }
        return output;
    }

    public void addLayer()
    {
        int[] newHiddens = new int[hiddens.Length + 1];
        for (int i = 0; i < hiddens.Length; i++)
        {
            newHiddens[i] = hiddens[i];
            if (i == hiddens.Length - 1)
            {
                newHiddens[i + 1] = hiddens[i];
            }
        }
        hiddens = newHiddens;
        Matrix[] newLayers = new Matrix[layers.Length + 1];
        Matrix[] newBiases = new Matrix[biases.Length + 1];
        int[][] newAFunctions = new int[aFunctions.Length + 1][];
        for (int i = 0; i < layers.Length; i++)
        {
            if (i == layers.Length - 1)
            {
                newAFunctions[i + 1] = aFunctions[i];
                newAFunctions[i] = new int[hiddens[i - 1]];
                for (int j = 0; j < newAFunctions[i].Length; j++)
                {
                    newAFunctions[i][j] = defaultAFunction;
                }
                newLayers[i + 1] = layers[i];
                newLayers[i] = new Matrix(hiddens[i], hiddens[i - 1]);
                newBiases[i + 1] = biases[i];
                newBiases[i] = new Matrix(hiddens[i], 1);
            }
            else
            {
                newAFunctions[i] = aFunctions[i];
                newLayers[i] = layers[i];
                newBiases[i] = biases[i];
            }
        }
        aFunctions = newAFunctions;
        biases = newBiases;
        layers = newLayers;
    }

    public void subtractLayer()
    {
        if (layers.Length > 3)
        {
            int[] newHiddens = new int[hiddens.Length - 1];
            Matrix[] newLayers = new Matrix[layers.Length - 1];
            Matrix[] newBiases = new Matrix[biases.Length - 1];
            int[][] newAFunctions = new int[aFunctions.Length - 1][];

            for (int i = 0; i < newHiddens.Length; i++)
            {
                if (i == newHiddens.Length - 1)
                {
                    newHiddens[i] = hiddens[i + 1];
                }
                else
                {
                    newHiddens[i] = hiddens[i];
                }
            }

            for (int i = 0; i < newLayers.Length; i++)
            {
                if (i == newLayers.Length - 1)
                {
                    newAFunctions[i] = aFunctions[i + 1];
                    newLayers[i] = layers[i + 1];
                    newBiases[i] = biases[i + 1];
                }
                else if (i == newLayers.Length - 2)
                {
                    //newLayers[i] = layers[i];
                    newLayers[i] = new Matrix(hiddens[i + 1], hiddens[i - 1]);
                    //newBiases[i] = biases[i];
                    newBiases[i] = new Matrix(hiddens[i + 1], 1);
                    newAFunctions[i] = aFunctions[i];
                    newAFunctions[i] = new int[hiddens[i + 1]];
                    for (int j = 0; j < newAFunctions[i].Length; j++)
                    {
                        newAFunctions[i][j] = defaultAFunction;
                    }
                }
                else
                {
                    newAFunctions[i] = aFunctions[i];
                    newLayers[i] = layers[i];
                    newBiases[i] = biases[i];
                }
            }
            aFunctions = newAFunctions;
            biases = newBiases;
            layers = newLayers;
            hiddens = newHiddens;
        }
    }

    public void addNeuron(int layer, int ActivationFunction)
    {
        if (!(layer == 0 || layer == layers.Length-1))
        {
            hiddens[layer - 1]++;
            Matrix newBiases = new Matrix(hiddens[layer - 1], 1);
            Matrix newLayerLeft = new Matrix(layers[layer - 1].Rows + 1, layers[layer - 1].Columns);
            Matrix newLayerRight = new Matrix(layers[layer].Rows, layers[layer].Columns + 1);
            int[] newAFunctionLayer = new int[aFunctions[layer - 1].Length + 1];
            for (int i = 0; i < aFunctions[layer - 1].Length; i++)
            {
                if (i == newAFunctionLayer.Length)
                {
                    newAFunctionLayer[i] = ActivationFunction;
                }
                else
                {

                    newAFunctionLayer[i] = aFunctions[layer - 1][i];
                }

            }
            for (int i = 0; i < newLayerLeft.Rows; i++)
            {
                for (int j = 0; j < newLayerLeft.Columns; j++)
                {
                    if (i == newLayerLeft.Rows - 1)
                    {
                        newLayerLeft[i, j] = Random.Range(0f, 0.1f);
                        Random.InitState((int)System.DateTime.Now.Ticks);
                    }
                    else
                    {
                        newLayerLeft[i, j] = layers[layer - 1][i, j];
                    }
                }
            }
            //newLayerRight = layers[layer];
            for (int i = 0; i < newLayerRight.Rows; i++)
            {
                for (int j = 0; j < newLayerRight.Columns; j++)
                {
                    if (j == newLayerRight.Columns - 1)
                    {
                        newLayerRight[i, j] = Random.Range(0f, 0.1f);
                        Random.InitState((int)System.DateTime.Now.Ticks);
                    }
                    else
                    {
                        newLayerRight[i, j] = layers[layer][i, j];
                    }
                }
                biases[layer - 1] = newBiases;
            }
            aFunctions[layer - 1] = newAFunctionLayer;
            layers[layer - 1] = newLayerLeft;
            layers[layer] = newLayerRight;
        }
    }

    public void subtractNeuron(int layer)
    {
        if (layer > 0 && hiddens[layer - 1] > 0)
        {

            if (!(layer == 0 || layer == layers.Length-1))
            {
                hiddens[layer - 1]--;
                Matrix newBiases = new Matrix(hiddens[layer - 1], 1);
                Matrix newLayerLeft = new Matrix(layers[layer - 1].Rows - 1, layers[layer - 1].Columns);
                Matrix newLayerRight = new Matrix(layers[layer].Rows, layers[layer].Columns - 1);
                int[] newAFunctionLayer = new int[aFunctions[layer - 1].Length - 1];
                for (int i = 0; i < newAFunctionLayer.Length; i++)
                {
                    newAFunctionLayer[i] = aFunctions[layer - 1][i];
                }
                for (int i = 0; i < newLayerLeft.Rows; i++)
                {
                    for (int j = 0; j < newLayerLeft.Columns; j++)
                    {
                        newLayerLeft[i, j] = layers[layer - 1][i, j];
                    }
                }
                for (int i = 0; i < newLayerRight.Rows; i++)
                {
                    for (int j = 0; j < newLayerRight.Columns; j++)
                    {
                        newLayerRight[i, j] = layers[layer][i, j];
                    }
                }
                biases[layer - 1] = newBiases;
                aFunctions[layer - 1] = newAFunctionLayer;
                layers[layer - 1] = newLayerLeft;
                layers[layer] = newLayerRight;
            }
        }
    }
    public NeuralNetwork Duplicate()
    {
        NeuralNetwork output = new NeuralNetwork(inputs, hiddens, outputs, LearningRate, type);
        for(int i = 0; i < layers.Length; i++) {
            output.layers[i] = layers[i].duplicate();
        }
        for(int i = 0; i < biases.Length; i++)
        {
            output.biases[i] = biases[i].duplicate();
        }
        output.aFunctions = (int[][])aFunctions.Clone();
        return output;
    }
}
