using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datd_mining
{
    static public class Services
    {
        /// <summary>
        /// calculate Euclidean distance from one point to another.
        /// </summary>
        /// <param name="dataTestLine"></param>
        /// <param name="dataLine"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static double Disatance(List<double> dataTestLine, List<double> dataLine, int length)
        {
            double distance = 0;

            for (int i = 0; i < length; i++)
            {
                distance += Math.Pow((dataTestLine[i] - dataLine[i]), 2);
            }

            return Math.Sqrt(distance);
        }

        /// <summary>
        /// This function creates list of disatnaces from the test point to training set
        /// </summary>
        /// <param name="testLine">test point</param>
        /// <param name="allDataStartified">all startified data without the test set</param>
        /// <returns>list of distances and the suitable data</returns>
        public static List<KeyValuePair<double, Data>> CulcDistancesOnTrainingSet(List<double> testLine, Dictionary<int, List<Data>> allDataStartified)
        {
            List<KeyValuePair<double, Data>> distances = new List<KeyValuePair<double, Data>>();
            double disatanceFromTestPoint;

            foreach (var currGroup in allDataStartified.Keys)
            {
                foreach (var currData in allDataStartified[currGroup])
                {
                    disatanceFromTestPoint = Services.Disatance(testLine, currData.Line, testLine.Count);
                    KeyValuePair<double, Data> distanceToTestAndData = new KeyValuePair<double, Data>(disatanceFromTestPoint, currData);
                    distances.Add(distanceToTestAndData);
                }
            }
            return distances;
        }

        /// <summary>
        /// get the commen type from the kNearestNeighbors dictionary
        /// </summary>
        /// <param name="kNearestNeighbors"></param>
        /// <param name="typesList"></param>
        /// <returns></returns>
        public static string ReturnMostCommenType(Dictionary<Data, double> kNearestNeighbors, List<string> typesList)
        {
            Dictionary<string, int> typesByRepeat = new Dictionary<string, int>();
            int numOfCummonType = 0;
            string cummonType;

            // initialize the dictionary
            foreach (string currType in typesList)
            {
                typesByRepeat.Add(currType, 0);
            }

            // insert values to the dictionary <type, count>
            foreach (var curr in kNearestNeighbors)
            {
                foreach (var data in kNearestNeighbors.Keys)
                {
                    typesByRepeat[data.Type]++;
                }
            }

            int i = 0;
            numOfCummonType = typesByRepeat[typesList[0]];
            cummonType = typesList[0];
            foreach (string currType in typesByRepeat.Keys)
            {
                if (numOfCummonType < typesByRepeat[typesList[i]])
                {
                    numOfCummonType = typesByRepeat[typesList[i]];
                    cummonType = typesList[i];
                }
                i++;
            }

            return cummonType;
        }

        /// <summary>
        /// get the commen type from the kNearestNeighbors dictionary
        /// </summary>
        /// <param name="kNearestNeighbors"></param>
        /// <param name="typesList"></param>
        /// <returns></returns>
        public static string ReturnMostCommenType(List<KeyValuePair<double, Data>> kNearestNeighbors, List<string> typesList)
        {
            //m_typesList
            Dictionary<string, int> typesByRepeat = new Dictionary<string, int>();
            int numOfCummonType = 0;
            string cummonType;

            // initialize the dictionary
            foreach (string currType in typesList)
            {
                typesByRepeat.Add(currType, 0);
            }

            // insert values to the dictionary <type, count>
            foreach (var curr in kNearestNeighbors)
            {
                typesByRepeat[curr.Value.Type]++;
            }

            int i = 0;
            numOfCummonType = typesByRepeat[typesList[0]];
            cummonType = typesList[0];
            foreach (string currType in typesByRepeat.Keys)
            {
                if (numOfCummonType < typesByRepeat[typesList[i]])
                {
                    numOfCummonType = typesByRepeat[typesList[i]];
                    cummonType = typesList[i];
                }
                i++;
            }

            return cummonType;
        }
    }
}
