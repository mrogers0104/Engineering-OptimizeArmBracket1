using System;
using System.Collections.Generic;
using System.Linq;

using System.IO;

using System.Linq;

using MathNet.Numerics.LinearAlgebra;
using NeuroSharp;
using NeuroSharp.Data;
using NeuroSharp.Enumerations;
using NeuroSharp.Training;
//using Accord.Neuro;
//using System.Numerics;

namespace ArmBracketLibConsoleRun
{
    public class Trainer
    {
        internal static void MainTrainer(string[] args)
        {
            // Example usage: LetterIdentificationTraining(20);
            SentimentAnalysisTraining(15, trainingSize: 50000, testSize: 5000, maxWordCount: 1500, maxReviewLength: 20);
        }

        private static void LetterIdentificationTraining(int epochs)
        {
            // Prepare the training data
            string path = @"path";
            string possibleChars = "abcdefghijklmnopqrst";

            List<(Vector<double>, Vector<double>)> data = new List<(Vector<double>, Vector<double>)>();

            foreach (string file in Directory.EnumerateFiles(path, "*.txt"))
            {
                // Load the input data and label for each file
                Vector<double> x = Vector<double>.Build.DenseOfEnumerable(
                    File.ReadAllText(file).Split(",").Select(x => double.Parse(x))
                );

                string character = file.Replace(path + @"\", "")[0].ToString();
                Vector<double> y = Vector<double>.Build.Dense(possibleChars.Length);
                y[possibleChars.IndexOf(character)] = 1;

                data.Add((x, y));
            }

            // Shuffle the data randomly
            Random rand = new Random();
            data = data.OrderBy(x => rand.Next()).ToList();

            double trainSplit = 1;

            List<Vector<double>> xTrain =
                data.Take((int)Math.Round(trainSplit * data.Count)).Select(x => x.Item1).ToList();
            List<Vector<double>> yTrain =
                data.Take((int)Math.Round(trainSplit * data.Count)).Select(y => y.Item2).ToList();

            List<Vector<double>> xTest =
                data.Skip((int)Math.Round(trainSplit * data.Count)).Select(x => x.Item1).ToList();
            List<Vector<double>> yTest =
                data.Skip((int)Math.Round(trainSplit * data.Count)).Select(y => y.Item2).ToList();

            // Build the network architecture
            Network network = new Network(xTrain[0].Count);
            network.Add(new ConvolutionalLayer(kernel: 2, filters: 128, stride: 1, channels: 1));
            network.Add(new ActivationLayer(ActivationType.ReLu));
            network.Add(new MaxPoolingLayer(poolSize: 2));
            network.Add(new ConvolutionalLayer(kernel: 2, filters: 64, stride: 1, channels: 128));
            network.Add(new ActivationLayer(ActivationType.ReLu));
            network.Add(new MaxPoolingLayer(poolSize: 2));
            network.Add(new ConvolutionalLayer(kernel: 2, filters: 16, stride: 2, channels: 64));
            network.Add(new ActivationLayer(ActivationType.ReLu));
            network.Add(new MaxPoolingLayer(poolSize: 2));
            network.Add(new FullyConnectedLayer(512));
            network.Add(new ActivationLayer(ActivationType.Tanh));
            network.Add(new FullyConnectedLayer(256));
            network.Add(new ActivationLayer(ActivationType.Tanh));
            network.Add(new FullyConnectedLayer(128));
            network.Add(new ActivationLayer(ActivationType.Tanh));
            network.Add(new FullyConnectedLayer(64));
            network.Add(new ActivationLayer(ActivationType.Tanh));
            network.Add(new FullyConnectedLayer(yTrain[0].Count));
            network.Add(new SoftmaxActivationLayer());
            network.UseLoss(LossType.CategoricalCrossentropy);

            // Train the network
            network.Train(xTrain, yTrain, epochs: epochs, TrainingConfiguration.Minibatch, OptimizerType.Adam, batchSize: 64, learningRate: 0.002f);

            // Save the trained model
            string modelJson = network.SerializeToJSON();
            File.WriteAllText(@"path\characters_model.json", modelJson);

            if (trainSplit < 1)
            {
                // Evaluate the accuracy on the test set
                int i = 0;
                int wrongCount = 0;
                foreach (var test in xTest)
                {
                    var output = network.Predict(test);
                    int prediction = output.ToList().IndexOf(output.Max());
                    int actual = yTest[i].ToList().IndexOf(yTest[i].Max());
                    Console.WriteLine("Prediction: " + prediction);
                    Console.WriteLine("Actual: " + actual + "\n");

                    if (prediction != actual)
                        wrongCount++;

                    i++;
                }

                double acc = (1d - ((double)wrongCount) / i);
                Console.WriteLine("Accuracy: " + acc);
            }
        }

        private static void SentimentAnalysisTraining(int epochs, int trainingSize, int testSize, int maxWordCount, int maxReviewLength)
        {
            // TODO: Add code for sentiment analysis training
            LetterIdentificationTraining(20);
        }
    }
}