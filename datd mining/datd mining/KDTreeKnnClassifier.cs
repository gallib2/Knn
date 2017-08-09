using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datd_mining
{
    public class KDTreeKnnClassifier: IKnnClassifier
    {
        public IDataInformation dataInformation;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="dataInformation">IDataInformation: The relevent data information for this class, e.g, the number of nearest neighbors to find, number of folds, etc.</param>
        public KDTreeKnnClassifier(IDataInformation dataInformation)
        {
            this.dataInformation = dataInformation;
        }

        /// <summary>
        /// This function pass every set (as the number of folds)
        /// and for all set create kdtree and search the 'k' nearst neighbors
        /// </summary>
        public void RunKnnClassifier()
        {
            for (int indexOfTestSet = 0; indexOfTestSet < dataInformation.m_NumFolds; indexOfTestSet++)
            {
                List<Data> unionTraningSet = new List<Data>();
                KDTree kdTree = new KDTree();
                unionTraningSet = createUnionTrennigSet(indexOfTestSet); 
                kdTree.createKdTree(unionTraningSet);
                getNeighborsWithKdTree(kdTree, dataInformation.m_crossValidation.StratifiedGrops[indexOfTestSet]);
            }
        }

        /// <summary>
        /// serch for the k nearst neighbors form one single point of test data to training set (as the number of test set).
        /// for every result get the common type and check if it match to the single point of test data
        /// </summary>
        /// <param name="kdTree">kdtree without the test set</param>
        /// <param name="testSet">current test set</param>
        private void getNeighborsWithKdTree(KDTree kdTree, List<Data> testSet)
        {
            foreach (Data singelData in testSet)
            {
                Dictionary<Data, double> nighborsDictionary = new Dictionary<Data, double>();
                nighborsDictionary = kdTree.returnNighborsOfDataPoint(singelData, nighborsDictionary, dataInformation.k);

                string commonType = Services.ReturnMostCommenType(nighborsDictionary, dataInformation.m_typesList);

                if (singelData.Type == commonType)
                {
                    dataInformation.counterCorrectAnswer++;
                }
                else
                {
                    dataInformation.counterFalseAnswer++;
                }
                dataInformation.m_confusunMatrix[dataInformation.m_typesList.IndexOf(singelData.Type), dataInformation.m_typesList.IndexOf(commonType)]++;
            }
        }

        /// <summary>
        /// creates the training set, i.e all sets without the current test set
        /// </summary>
        /// <param name="i_indexOfTestSet">the index of current test set so we wont includ it in the training set</param>
        /// <returns>training set</returns>
        List<Data> createUnionTrennigSet(int i_indexOfTestSet)
        {
            List<Data> unionTraningSet = new List<Data>();

            foreach (int dataList in dataInformation.m_crossValidation.StratifiedGrops.Keys)
            {
                if (dataList != i_indexOfTestSet)
                {
                    unionTraningSet.AddRange(dataInformation.m_crossValidation.StratifiedGrops[dataList]);
                }

            }
            return unionTraningSet;
        }
    }
}
