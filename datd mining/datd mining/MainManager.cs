using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datd_mining
{
    public class MainManager
    {
        int k;
        int folds;
        bool isKDTreeChosen;
        string fileName;
        ArffReader dataSet = new ArffReader();

        /// <summary>
        /// This function get input from user and run the knn classifier according to the user request
        /// </summary>
        public void Run()
        {
            CommandOutput userInputValues;
            try
            {
                do
                {
                    string userInputLine = getInputFromUser();
                    userInputValues = CommandDataProvider.ExtractCommandData(userInputLine);
                    if (userInputValues == null)
                    {
                        Console.WriteLine("Input Is Invalid, Please Try Again. ");
                    }
                } while (userInputValues == null);

                // extract iput from CommandDataProvider
                k = userInputValues.NN;
                folds = userInputValues.Folds;
                isKDTreeChosen = userInputValues.YN;
                fileName = userInputValues.FileName;

                runAlgorithmsAccordingToUserInput();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private string getInputFromUser()
        {
            Console.WriteLine("Please Enter A Line In The Following Pattern: ");
            Console.WriteLine("knn -k <nn> -cv <folds> -t <y/n> -f <filename>");
            string userInput = Console.ReadLine();

            return userInput;
        }

        /// <summary>
        /// This function is call after extreact the input from the user
        /// and actually run the suitable knn classifier in accordance to user request. 
        /// </summary>
        private void runAlgorithmsAccordingToUserInput()
        {
            var arffFiles = dataSet.ExtractArffFilesPaths(fileName);

            foreach (string currentArffFile in arffFiles)
            {
                dataSet.getDataFromFile(currentArffFile);
                knn knnClassifier = new knn(dataSet.TypesList, dataSet.m_SumOfData, currentArffFile, folds, k, dataSet);
                if (isKDTreeChosen) // if KDtree knn was chosen
                {
                    knnClassifier.KdTreeAlgorithm();
                }
                else // if simple knn was chosen
                {
                    knnClassifier.SimpleKnnAlgorithm();
                }

                knnClassifier.PresentData();
                knnClassifier.PresentMatrix();
            }
        }
    }
}
