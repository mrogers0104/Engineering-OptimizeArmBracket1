using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Neuro;
using Accord.Neuro.Learning;
using Accord.Math;
using ArmBracketDesignLibrary.ArmBracketDesignEngine;
using System.Net;

using Keras;
using Keras.Layers;
using Keras.Models;
using Keras.Optimizers;
using Keras.Utils;
using Numpy;
using System.IO;

namespace ArmBracketLibConsoleRun
{
    internal class StructuralDesignNN
    {



        internal static void DesignNN(List<ArmBracketDesignResults> dataSet)
        {
            // Load your dataset (X: input features, Y: output weights)
            double[][] X = LoadInputFeatures(dataSet);
            double[][] Y = LoadOutputWeights(dataSet);

            // Create a neural network
            ActivationNetwork network = new ActivationNetwork(
                new SigmoidFunction(), // Activation function
                X[0].Length,           // Input layer size (features)
                10,                    // Hidden layer size (adjust as needed)
                1);                    // Output layer size (1 for weight prediction)

            // Initialize the network weights
            new NguyenWidrow(network).Randomize();

            // Train the network
            BackPropagationLearning teacher = new BackPropagationLearning(network)
            {
                LearningRate = 0.1,
                Momentum = 0.1
            };
            teacher.RunEpoch(X, Y);

            // Make predictions
            double[] newDesignFeatures = LoadNewDesignFeatures();
            double predictedWeight = network.Compute(newDesignFeatures)[0];

            Console.WriteLine($"Predicted weight for new design: {predictedWeight}");
        }

        private static double[] LoadNewDesignFeatures()
        {
            throw new NotImplementedException();
        }

        private static double[][] LoadOutputWeights(List<ArmBracketDesignResults> dataSet)
        {
            double[][] weights = new double[dataSet.Count][];

            return weights;
        }

        private static double[][] LoadInputFeatures(List<ArmBracketDesignResults> dataSet)
        {
            double[][] features = new double[dataSet.Count][];

            return features;
        }

        // Implement your data loading methods here
        // (LoadInputFeatures, LoadOutputWeights, LoadNewDesignFeatures)


        internal static void DesignKera(List<ArmBracketDesignResults> dataSet)
        {
            // Define input data
            NDarray x = np.array(new float[,] { { 0, 0 }, { 0, 1 }, { 1, 0 }, { 1, 1 } });
            NDarray y = np.array(new float[] { 0, 1, 1, 0 });

            // Create a sequential model
            var model = new Sequential();
            model.Add(new Dense(32, activation: "relu", input_shape: new Shape(2)));
            model.Add(new Dense(64, activation: "relu"));
            model.Add(new Dense(1, activation: "sigmoid"));
            model.Compile(optimizer: "sgd", loss: "binary_crossentropy", metrics: new string[] { "accuracy" });
            model.Fit(x, y, batch_size: 2, epochs: 1000, verbose: 1);

            // Save the model
            string json = model.ToJson();
            File.WriteAllText("model.json", json);
            model.SaveWeight("model.h5");

            // Load the saved model
            var loaded_model = Sequential.ModelFromJson(File.ReadAllText("model.json"));
            loaded_model.LoadWeight("model.h5");
        }
    }
}
