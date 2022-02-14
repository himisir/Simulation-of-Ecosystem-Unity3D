
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NeuralNetwork : IComparable<NeuralNetwork>
{
    int[] layers;
    float[][] neurons;
    float[][][] weights;
    float[][] biases;
    float fitness;

    public NeuralNetwork(int[] layers)
    {
        this.layers = new int[layers.Length];
        for (int i = 0; i < layers.Length; i++)
        {
            this.layers[i] = layers[i];
        }
        InitNeurons();
        InitWeights();

    }

    void InitNeurons()
    {
        List<float[]> neuronsList = new List<float[]>();
        for (int i = 0; i < layers.Length; i++)
        {
            neuronsList.Add(new float[layers[i]]);
        }
        neurons = neuronsList.ToArray();

    }
    void InitWeights()
    {
        List<float[][]> weightsList = new List<float[][]>();
        for (int i = 0; i < layers.Length; i++)
        {
            List<float[]> layerWeightsList = new List<float[]>();
            int neuronsInPreviousLayer = layers[i - 1];
            for (int j = 0; j < neurons[i].Length; j++)
            {
                float[] neuronsWeights = new float[neuronsInPreviousLayer];
                for (int k = 0; k < neuronsInPreviousLayer; k++)
                {
                    neuronsWeights[k] = (float)UnityEngine.Random.Range(-0.5f, 0.5f);
                }
                layerWeightsList.Add(neuronsWeights);

            }

            weightsList.Add(layerWeightsList.ToArray());


        }
        weights = weightsList.ToArray();

    }
    public float[] FeedForward(float[] inputs)
    {

        for (int i = 0; i < inputs.Length; i++)
        {
            neurons[0][i] = inputs[i];
        }

        for (int i = 1; i < inputs.Length; i++)
        {
            for (int j = 0; j < neurons[i].Length; j++)
            {
                float value = 0.25f; //make if soft value later
                for (int k = 0; k < neurons[i - 1].Length; k++)
                {
                    value += weights[i - 1][j][k] * neurons[i - 1][k];
                }
                neurons[i][j] = (float)Math.Tanh(value);
            }
        }
        return neurons[neurons.Length - 1];

    }

    ///<summery>
    ///Mutation happense here
    ///Mutation of networks weights
    ///</summery> 

    public void Mutation()
    {

        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    float weight = weights[i][j][k];
                    float randomNumber = (float)UnityEngine.Random.Range(0, 10);

                    if (randomNumber <= 2.5f)
                    {
                        weight *= -1f;
                    }
                    if (randomNumber >= 5f)
                    {
                        weight = UnityEngine.Random.Range(-0.5f, 0.5f);

                    }
                    if (randomNumber >= 7f)
                    {
                        float factor = UnityEngine.Random.Range(0, 1f);
                        weight *= factor;
                    }
                    if (randomNumber >= 8.5f)
                    {
                        float factor = UnityEngine.Random.Range(0, .7f);
                        weight *= factor;
                    }
                    weights[i][j][k] = weight;
                }
            }
        }
    }

    public NeuralNetwork(NeuralNetwork copyNetwork)
    {
        this.layers = new int[copyNetwork.layers.Length];

        for (int i = 0; i < copyNetwork.layers.Length; i++)
        {
            this.layers[i] = copyNetwork.layers[i];
        }
        InitNeurons();
        InitWeights();
        CopyWeights(copyNetwork.weights);
    }
    public void CopyWeights(float[][][] copyWeights)
    {
        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    weights[i][j][k] = copyWeights[i][j][k];
                }
            }
        }
    }

    public void AddFitness(float fit)
    {
        fitness += fit;

    }
    public void AetFitness(float fit)
    {
        fitness = fit;
    }
    public float GetFitness()
    {
        return fitness;
    }


    public int CompareTo(NeuralNetwork other)
    {
        if (other == null) return 1;
        if (fitness > other.fitness) return 1;
        else if (fitness < other.fitness) return -1;
        else return 0;
    }

}
