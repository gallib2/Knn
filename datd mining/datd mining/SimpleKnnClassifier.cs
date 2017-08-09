using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datd_mining
{
    public class SimpleKnnClassifier: IKnnClassifier
    {
        public IDataInformation dataInformation { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="dataInformation">IDataInformation: The relevent data information for this class, e.g, the number of nearest neighbors to find, number of folds, etc.</param>
        public SimpleKnnClassifier(IDataInformation dataInformation)
        {
            this.dataInformation = dataInformation;
        }

        /// <summary>
        /// This function pass every set (as the number of folds)
        /// and for all set finds the 'k' nearst neighbors
        /// </summary>
        public void RunKnnClassifier()
        {
            int indexOfTestGrop;

            for (indexOfTestGrop = 0; indexOfTestGrop < dataInformation.m_NumFolds; indexOfTestGrop++)
            {
                getNearestNeighbors(dataInformation.m_crossValidation.StratifiedGrops, indexOfTestGrop);
            }
        }

        /// <summary>
        /// this function pass for all test set of data
        /// for every single data of the test find the distance
        /// from the point of the current test data to all the rest sets without the test set.
        /// after find the distance, takse the first k's i.e, the k nearest neighbors. 
        /// for every result get the common type and check if it match to the single point of test data.
        /// </summary>
        /// <param name="allDataStartified"></param>
        /// <param name="indexOfTestGroup"></param>
        private void getNearestNeighbors(Dictionary<int, List<Data>> allDataStartified, int indexOfTestGroup )
        {
            List<KeyValuePair<double, Data>> distances;
            List<KeyValuePair<double, Data>> kNearestNeighbor;

            int kNighbers = dataInformation.k;

            foreach (Data singleData in allDataStartified[indexOfTestGroup])
            {
                distances = Services.CulcDistancesOnTrainingSet(singleData.Line, allDataStartified.Where(kvp => kvp.Key != indexOfTestGroup).ToDictionary(kvp => kvp.Key, kvp => kvp.Value));
                kNearestNeighbor = distances.OrderBy(n => n.Key).Take(kNighbers).ToList();
                var commenType = Services.ReturnMostCommenType(kNearestNeighbor, dataInformation.m_typesList);

                if (singleData.Type == commenType)
                {
                    dataInformation.counterCorrectAnswer++;
                }
                else
                {
                    dataInformation.counterFalseAnswer++;
                }
                dataInformation.m_confusunMatrix[dataInformation.m_typesList.IndexOf(singleData.Type), dataInformation.m_typesList.IndexOf(commenType)]++;
            }
        }
        
    }
}
