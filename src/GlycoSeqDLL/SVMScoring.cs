using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Runtime.CompilerServices;
using LibSVMsharp;
using LibSVMsharp.Helpers;
using LibSVMsharp.Extensions;

//ccerhan/LibSVMsharp
//https://github.com/ccerhan/LibSVMsharp/blob/master/LibSVMsharp.Examples.Classification/Program.cs
namespace COL.GlycoSequence
{
    public static class SVMScoring
    {
        public static void Training(string argDataSet)
        {
            SVMProblem trainingSet = SVMProblemHelper.Load(argDataSet);
            SVMProblem testSet = SVMProblemHelper.Load(argDataSet);

            // Select the parameter set
            SVMParameter parameter = new SVMParameter();
            parameter.Type = SVMType.C_SVC;
            parameter.Kernel = SVMKernelType.RBF;
            parameter.C = 32;
            parameter.Gamma = 3.05176e-005;
            parameter.Probability = true;

            // Do cross validation to check this parameter set is correct for the dataset or not
            double[] crossValidationResults; // output labels
            int nFold = 5;
            trainingSet.CrossValidation(parameter, nFold, out crossValidationResults);

            // Evaluate the cross validation result
            // If it is not good enough, select the parameter set again
            double crossValidationAccuracy = trainingSet.EvaluateClassificationProblem(crossValidationResults);

            // Train the model, If your parameter set gives good result on cross validation
            SVMModel model = trainingSet.Train(parameter);

            // Save the model
            SVM.SaveModel(model, Environment.CurrentDirectory+"\\model.txt");

             // Predict the instances in the test set
            List<double[]> lstProbList = new List<double[]>();
            double[] testResults = testSet.PredictProbability(model, out lstProbList);
            StreamWriter sw = new StreamWriter(Environment.CurrentDirectory + "\\TestResult.txt");

            for (int i = 0; i < lstProbList.Count; i++)
            {
                sw.WriteLine(testResults[i] + " " + lstProbList[i][0] + " " + lstProbList[i][1] +" "+ lstProbList[i][2]);
            }
            sw.Close();
        }

        public static List<Tuple<int, List<double>>> GetSVMScore(List<List<double>> argFeatures)
        {
             return GetSVMScore( argFeatures, Environment.CurrentDirectory + "\\Resources\\model.txt");
        }

        public static List<Tuple<int, List<double>>> GetSVMScore(List<List<double>> argFeatures, string argModel)
        {
            string modelFile = argModel;

            List<Tuple<int, List<double>>> lstResult = new List<Tuple<int, List<double>>>(); //Category, Prob1, Prob2,Prob3;
            
            SVMModel model = SVM.LoadModel(modelFile);

            foreach (List<double> feature in argFeatures)
            {
                List<SVMNode> nodes = new List<SVMNode>();
                for (int i = 0; i < feature.Count; i++)
                {
                    nodes.Add(new SVMNode(i + 1, feature[i]));
                }
                double[] outResult = new double[3]; //Labeling order = 2 3 1;
                double result = model.PredictProbability(nodes.ToArray(), out outResult);

                lstResult.Add(new Tuple<int, List<double>>(Convert.ToInt32(result),
                    new List<double>() {outResult[model.Labels.ToList().IndexOf(1)],
                outResult[model.Labels.ToList().IndexOf(2)],
                outResult[model.Labels.ToList().IndexOf(3)]}));

            }
            return lstResult;
        }
    }
}
