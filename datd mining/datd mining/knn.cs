using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datd_mining
{
    public class knn: IDataInformation
    {
        public int[,] m_confusunMatrix { get; set; }
        public int counterFalseAnswer { get; set; }
        public int counterCorrectAnswer { get; set; }
        public List<string> m_typesList { get; set; }
        public CrossValidation m_crossValidation { get; set; }
        public int m_NumFolds { get; set; }
        public int m_numOfAllDataToProcess { get; set; }
        public string m_fileName { get; set; }
        public int k { get; set; }
        public ArffReader dataSet { get; set; }

        /// <summary>
        /// this class implemnts "IDataInformation" that holds all data that the algoritms need
        /// </summary>
        /// <param name="typesList">Type list from current file</param>
        /// <param name="numOfAllDataToProcess">Size of data</param>
        /// <param name="fileName"></param>
        /// <param name="numFolds">Number of folds</param>
        /// <param name="k">Numbers of nearest neighbors serch</param>
        /// <param name="dataSet"></param>

        public knn(string[] typesList, int numOfAllDataToProcess, string fileName, int numFolds, int k, ArffReader dataSet)
        {
            int sizeOfMatrix = typesList.Length;
            m_confusunMatrix = new int[sizeOfMatrix, sizeOfMatrix];
            m_typesList = typesList.ToList<string>();
            m_numOfAllDataToProcess = numOfAllDataToProcess;
            m_fileName = fileName;
            m_NumFolds = numFolds;
            this.k = k;
            this.dataSet = dataSet;
            m_crossValidation = new CrossValidation(numFolds, m_numOfAllDataToProcess);
            m_crossValidation.CreateStratifiedGrops(dataSet);
            counterFalseAnswer = 0;
            counterCorrectAnswer = 0;
        }

        public void KdTreeAlgorithm()
        {
            KDTreeKnnClassifier kdTreeKnnClassifier = new KDTreeKnnClassifier(this);
            kdTreeKnnClassifier.RunKnnClassifier();
        }

        public void SimpleKnnAlgorithm()
        {
            SimpleKnnClassifier simpleKnn = new SimpleKnnClassifier(this); // give only the intreface with the data 
            simpleKnn.RunKnnClassifier();
        }

        public void PresentData()
        {
            double CorrectAnswerPrecent = Math.Round(((double)counterCorrectAnswer / (double)m_numOfAllDataToProcess) * 100);
            double FalseAnswerPrecent = Math.Round(((double)counterFalseAnswer / (double)m_numOfAllDataToProcess) * 100);
            Console.WriteLine("Filename: " + m_fileName);
            Console.WriteLine( string.Format("Correctly Classified Instances: {0}, {1}% ",counterCorrectAnswer, CorrectAnswerPrecent));
            Console.WriteLine(string.Format("Inorrectly Classified Instances: {0}, {1}% ", counterFalseAnswer, FalseAnswerPrecent));
        }

        public void PresentMatrix()
        {
            double numcol , numRow =m_confusunMatrix.Length;
            numRow = Math.Sqrt(numRow) ;
            numcol = numRow;
            char typeItem = 'a';
            for (int i = 0; i < numRow; i++)
            {
                Console.Write(typeItem++ + "   ");
            }
            Console.WriteLine(" ");
            typeItem = 'a';
            for (int i=0; i < numRow; i++)
            {
                for (int j = 0; j < numcol; j++)
                {
                    if (m_confusunMatrix[i, j] <= 9)
                    {
                        Console.Write(m_confusunMatrix[i, j] + "   ");
                    }
                    else if(m_confusunMatrix[i, j] > 9 && m_confusunMatrix[i, j]<= 99)
                    {
                        Console.Write(m_confusunMatrix[i, j] + "  ");

                    }
                    else
                    {
                        Console.Write(m_confusunMatrix[i, j] + " ");
                    }
                }
                Console.Write(typeItem++);
                Console.Write(" = " + m_typesList[i]);
                
                Console.WriteLine(" ");
            }
        }


    }
}
