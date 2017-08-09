using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datd_mining
{
    public class CrossValidation
    {
        private int m_numOfFolds;
        private int m_totalNumOfData;
        Dictionary<int, List<Data>> m_stratifiedGrops = new Dictionary<int, List<Data>>();

        public CrossValidation(int numOfFolds, int sizeOfData)
        {
            m_numOfFolds = numOfFolds;
            m_totalNumOfData = sizeOfData;
        }

        public Dictionary<int, List<Data>> StratifiedGrops
        {
            get { return m_stratifiedGrops; }
            set { m_stratifiedGrops = value; }
        }

        /// <summary>
        /// This function create stratified groups by:
        /// 1. calculate the number of element in a single group
        /// 2. create dictionary as size of number of folds. 
        /// 3. for every type of data, calculate the members to select (so the grups will be stratified).
        /// 4. add to the dictionary the new members.
        /// 5. after this, there is still data that not divided:
        ///     so, random divide among all. 
        /// </summary>
        /// <param name="dataSet"></param>
        public void CreateStratifiedGrops(ArffReader dataSet)
        {
            int numOfElementInSingelGroup = m_totalNumOfData / m_numOfFolds;
            for (int i = 0; i < m_numOfFolds; i++)
            {
                m_stratifiedGrops.Add(i, new List<Data>());
                foreach (string typeOfDataSet in dataSet.TypesDictionary.Keys)
                {
                    int numOfElementByType = dataSet.TypesDictionary[typeOfDataSet];
                    int numOfMembersToSelect = getNumberOfMemberToSelect(numOfElementInSingelGroup, numOfElementByType);
                    m_stratifiedGrops[i].AddRange(returnListOfElementsFromSingelType(dataSet.SortDataByType[typeOfDataSet], numOfElementByType, numOfMembersToSelect));
                }
            }

            // divede randomly the remaining ones:

            List<Data> notSelectedItems = new List<Data>();
            foreach (string type in dataSet.SortDataByType.Keys)
            {
                foreach (var data in dataSet.SortDataByType[type])
                {
                    if (!data.IsSelectedToGroup)
                    {
                        notSelectedItems.Add(data);
                    }
                }
            }

            int num = notSelectedItems.Count;
            int m = 0, j = 0;
            for (j = 0; j < num; j++)
            {
                if (m == m_stratifiedGrops.Count)
                {
                    m = 0;
                }
                m_stratifiedGrops[m].Add(notSelectedItems[j]);

                m++;
            }
        }


        /// <summary>
        /// This function is used to get list of data from the same type to add to the strtified groups dictionary
        /// </summary>
        /// <param name="sameTypeList">List of data from the same type</param>
        /// <param name="numOfElementInType"></param>
        /// <param name="numOfMembersToSelect">how many to rerturn</param>
        /// <returns></returns>
        private List<Data> returnListOfElementsFromSingelType(List<Data> sameTypeList, int numOfElementInType, int numOfMembersToSelect)
        {
            List<Data> chooseElemList = new List<Data>();
            chooseElemList = sameTypeList.GetRange(0, numOfMembersToSelect);
            sameTypeList.RemoveRange(0, numOfMembersToSelect);

            return (chooseElemList);
        }

        /// <summary>
        /// this function calculate how many members from type to select for the strtified groups dictionary, so it will be stratifed.
        /// </summary>
        /// <param name="numOfElementInSingelGroup"></param>
        /// <param name="numOfElementInType"></param>
        /// <returns></returns>
        private int getNumberOfMemberToSelect(int numOfElementInSingelGroup, int numOfElementInType)
        {
            double dev = (double)numOfElementInSingelGroup / m_totalNumOfData;
            int numOfMembersToSelect = (int)((dev * numOfElementInType));
            return numOfMembersToSelect;
        }
    }
}
