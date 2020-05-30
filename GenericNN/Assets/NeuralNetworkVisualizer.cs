using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NeuralNetworkVisualizer : MonoBehaviour
{
    NeuralNetwork nn;
    Canvas nnCanvas;
    public Camera camera;
    public int inputs;
    public int[] hiddens;
    public int outputs;
    public float x;
    public float y;
    public float width;
    public float height;
    List<GameObject[]> layers;
    List<GameObject[]> connections;
    float connectionScale = 10;
    bool animating = false;
    // Start is called before the first frame update
    void Awake()
    {
        nn = new NeuralNetwork(inputs, hiddens, outputs, 0.2f, NeuralNetwork.NetworkType.Supervised);
        layers = new List<GameObject[]>();
        connections = new List<GameObject[]>();
        nnCanvas = gameObject.AddComponent<Canvas>();
        RectTransform rt = gameObject.AddComponent<RectTransform>();
        rt = gameObject.GetComponent<RectTransform>();
        rt.position = new Vector3(x, y, 0);
        gameObject.AddComponent<CanvasRenderer>();
        nnCanvas.worldCamera = camera;
        if (GameObject.Find("EventSystem") == null)
        {
            GameObject eventsystem = new GameObject("EventSystem");
            eventsystem.AddComponent<EventSystem>();
        }

        generateNetwork();
        generateConnections();
        nn.layers[0][0, 0] = -0.5f;
        setConnections();
    }

    // Update is called once per frame
    void Update()
    {

    }

    GameObject generateObject(string name, float widthI, float heightI, float xI, float yI)
    {
        GameObject neuron = new GameObject(name);
        RectTransform rT = neuron.AddComponent<RectTransform>();
        rT.sizeDelta = new Vector2(widthI, heightI);
        rT.position = new Vector3(xI, yI + y, 0);
        neuron.AddComponent<Image>();
        neuron.transform.parent = gameObject.transform;
        return neuron;
    }

    void generateLayer(int layer, float xI)
    {
        float dist;
        int numNeurons;
        if (layer == 0)
        {
            dist = height / inputs;
            numNeurons = inputs;
        }
        else if (layer == hiddens.Length+1)
        {
            dist = height / outputs;
            numNeurons = outputs;
        }
        else
        {
                dist = height / hiddens[layer-1];
                numNeurons = hiddens[layer-1];
        }
        GameObject[] tempLayer = new GameObject[numNeurons];
        for (int i = 0; i < numNeurons; i++)
        {
            float scale = height / (numNeurons * 5);
            tempLayer[i] = generateObject("input" + i, scale, scale, xI + x, i * dist);
        }
        layers.Add(tempLayer);
    }

    void generateNetwork()
    {
        float dist = width / (hiddens.Length+2);
        for (int i = 0; i < (hiddens.Length + 2); i++)
        {
            print("generate");
            generateLayer(i, i * dist);
        }
    }

    void generateConnections()
    {
        for (int i = 0; i < layers.Count - 1; i++)
        {
            for (int j = 0; j < layers[i].Length; j++)
            {
                for (int k = 0; k < layers[i + 1].Length; k++)
                {
                    GameObject g = new GameObject("" + k);
                    g.transform.SetParent(layers[i][j].transform);
                    g.AddComponent<LineRenderer>();
                    LineRenderer lR = g.GetComponent<LineRenderer>();
                    lR.startWidth = 0.1f;
                    lR.endWidth = 0.1f;
                    lR.useWorldSpace = false;
                    lR.SetPosition(0, layers[i][j].transform.position);
                    Vector3 endpoint = layers[i + 1][k].transform.position;
                    //endpoint.x += x;
                    lR.SetPosition(1, endpoint);
                }
            }
        }
    }

    void setConnections()
    {
        for (int i = 0; i < layers.Count - 1; i++)
        {
            for (int j = 0; j < layers[i].Length; j++)
            {
                for (int k = 0; k < layers[i + 1].Length; k++)
                {
                    LineRenderer lR = layers[i][j].transform.GetChild(k).GetComponent<LineRenderer>();
                    float weight = nn.layers[i][k, j];
                    if (weight > connectionScale / 3)
                    {
                        connectionScale = 3 * weight;
                    }
                    lR.startWidth = Mathf.Abs(weight / connectionScale);
                    lR.endWidth = Mathf.Abs(weight / connectionScale);
                    if (weight >= 0)
                    {
                        lR.material.color = Color.black;
                        lR.material.color = Color.black;
                    }
                    else
                    {
                        //lR.startColor = Color.red;
                        //lR.endColor = Color.red;
                        lR.material.color = Color.red;
                        lR.material.color = Color.red;
                    }
                }
            }
        }
    }

    public void Train(float[] fInput, float[] fTarget)
    {
        print(fInput);
        print(fTarget);
        nn.Train(fInput, fTarget);
        setConnections();
    }

    public float[] feedForward(float[] fInput, bool animated)
    {
        for (int i = 0; i < fInput.Length; i++)
        {
            //if (fInput[i] >= 0)
            //{
            //    setColor(layers[0][i], Color.black);
            //}
            //else
            //{
            //    setColor(layers[0][i], Color.red);
            //}
        }
        float[] output = nn.feedForward(fInput);
        for (int i = 0; i < output.Length; i++)
        {
            //if (output[i] >= 0)
            //{
            //    setColor(layers[layers.Count - 1][i], Color.black);
            //}
            //else
            //{
            //    setColor(layers[layers.Count - 1][i], Color.red);
            //}
        }
        StartCoroutine(resetNeurons());
        return output;
    }

    private IEnumerator resetNeurons()
    {
        if (!animating)
        {
            yield return new WaitForSeconds(1);
            //for (int i = 0; i < inputs; i++)
            //{
            //    setColor(layers[0][i].getChild, Color.white);
            //}
            //for (int i = 0; i < outputs; i++)
            //{
            //    setColor(layers[layers.Count - 1][i], Color.white);
            //}
        }
    }

    private void setColor(GameObject g, Color c)
    {
        g.GetComponent<Renderer>().material.color = c;
    }
}
