using Accord.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datd_mining
{
    class KDTree
    {
        private Node m_root;

        public Node Root
        {
            get { return m_root; }
            set { m_root = value; }
        }


        public class Node
        {
            private Node left;

            private Node right;

            private Data dataFiled;

            private int axis;

            private double midValue;

            public Node Left
            {
                get { return left; }
                set { left = value; }
            }

            public Node Right
            {
                get { return right; }
                set { right = value; }
            }

            public Data DataPoint
            {
                get { return dataFiled; }
                set { dataFiled = value; }
            }

            public int Axis
            {
                get { return axis; }
                set { axis = value; }
            }

            public double MidValue
            {
                get { return midValue; }
                set { midValue = value; }
            }

        }

        /// <summary>
        /// This function is helper for the search in the tree function, and tell us if to go right or left in the tree
        /// </summary>
        /// <param name="neighbors"></param>
        /// <param name="testPoint"></param>
        /// <param name="midSplitVal"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        bool SearchInLeftOrRightNode(Dictionary<Data, double> neighbors, double testPoint, double midSplitVal, string direction)
        {
            bool isRight = true;
            bool isLeft = true;
            if (direction == "Right")
            {
                foreach (Data data in neighbors.Keys)
                {
                    if (testPoint + neighbors[data] > midSplitVal)
                    {
                        return isRight;

                    }
                }
                return !isRight;
            }
            else //if (direction == "Left")
            {
                foreach (Data data in neighbors.Keys)
                {
                    if (testPoint - neighbors[data] <= midSplitVal)
                    {
                        return isLeft;
                    }
                }
                return !isLeft;
            }
        }

        /// <summary>
        // this is a recursive function that will search in the kd tree for the closest nighbors
        /// </summary>
        /// <param name="dataTest"></param>
        /// <param name="neighbors"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public Dictionary<Data, double> returnNighborsOfDataPoint(Data dataTest, Dictionary<Data, double> neighbors, int k)
        {
            return returnNighborsOfDataPointHelper(dataTest, m_root, neighbors, k);
        }

        /// <summary>
        /// This function is going over the kdtree and looks for the closest nighbors.
       ///  Ordering the data in a kd tree will allow us to culculate the distance of a 
        /// littel group and not of all the data like we did in the simple search the tree allow us to divide the data
        /// so we only going to culc the distance of element that are likely the closest one to the test data  
        /// </summary>
        /// <param name="dataTest"></param>
        /// <param name="node"></param>
        /// <param name="neighbors"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        private Dictionary<Data, double> returnNighborsOfDataPointHelper(Data dataTest, Node node, Dictionary<Data, double> neighbors, int k)
        {
            if (node.Left == null)
            {
                double distance = Services.Disatance(dataTest.Line, node.DataPoint.Line, dataTest.Line.Count);
                if (neighbors.Count < k) //if we need to add more neighbors and there is a place in the list
                {
                    neighbors.Add(node.DataPoint, distance);
                }
                else //if we need to replace one of the neighbors in the list because we found a closer new neighbor we will remove the element that his distance is the bigger in the list if we found closer one
                {
                    ReplaceNeighborAcoordingToDistance(distance, node.DataPoint, neighbors);
                }
                return neighbors;
            }
            else //this part is build from recursive call that look for the group of nighbors that is the closest to our datatest
            {
                if (neighbors.Count < k)
                {
                    if (dataTest.Line[node.Axis] <= node.MidValue) // the midvalue is the parameter that will indicat if we will look in the left side in the node or right side
                    {
                        neighbors = returnNighborsOfDataPointHelper(dataTest, node.Left, neighbors, k);

                        if (SearchInLeftOrRightNode(neighbors, dataTest.Line[node.Axis], node.MidValue, "Right"))
                        {
                            neighbors = returnNighborsOfDataPointHelper(dataTest, node.Right, neighbors, k);
                        }
                    }
                    else if (dataTest.Line[node.Axis] > node.MidValue) //if the value is bigger than the value of the mid value which is the boarder of the two groups we will look in the right side were  the value of the  data is closer to the data set we work on
                    {
                        neighbors = returnNighborsOfDataPointHelper(dataTest, node.Right, neighbors, k);

                        if (SearchInLeftOrRightNode(neighbors, dataTest.Line[node.Axis], node.MidValue, "Left"))
                        {
                            neighbors = returnNighborsOfDataPointHelper(dataTest, node.Left, neighbors, k);
                        }
                    }
                }
                else if (neighbors.Count >= k)// if the neighbors list is already full there is achance that we didnt look in all the tree. but only in a part so  we will continue 
                {
                    if (SearchInLeftOrRightNode(neighbors, dataTest.Line[node.Axis], node.MidValue, "Left"))
                    {
                        neighbors = returnNighborsOfDataPointHelper(dataTest, node.Left, neighbors, k);
                        if (SearchInLeftOrRightNode(neighbors, dataTest.Line[node.Axis], node.MidValue, "Right"))
                        {
                            neighbors = returnNighborsOfDataPointHelper(dataTest, node.Right, neighbors, k);
                        }
                    }
                }
                return neighbors;
            }
        }

        /// <summary>
        /// This function checks if the data poin that gives as a parameter is closest from at least one of the neighbors.
        /// if it is, replace this one neighbor in the data point that given as a parameter
        /// this function is called when the list of neighbors is of the size of k
        /// </summary>
        /// <param name="distance">the distance from dataPoint to the current tset point</param>
        /// <param name="dataPoint">the point to check if it closest then another in the list</param>
        /// <param name="neighbors">the current nearest neighbors</param>
        private void ReplaceNeighborAcoordingToDistance(double distance, Data dataPoint, Dictionary<Data, double> neighbors)
        {
            double maxDistance = 0;
            Data keydata = null;
            int indexToRemove = 0, loopIndex = 0;
            foreach (Data data in neighbors.Keys)
            {
                if (neighbors[data] > maxDistance)
                {
                    maxDistance = neighbors[data];
                    keydata = data;
                    indexToRemove = loopIndex;
                }
                loopIndex++;
            }
            if (keydata != null)
            {
                if (distance < maxDistance)
                {
                    removeItemFromDictionary(neighbors, keydata, indexToRemove);
                    neighbors.Add(dataPoint, distance);
                }
            }
        }

        /// <summary>
        /// This function remove item from dictionary
        /// </summary>
        /// <param name="neighbors"></param>
        /// <param name="keydataToRemove"></param>
        /// <param name="indexToRemove"></param>
        private void removeItemFromDictionary(Dictionary<Data, double> neighbors, Data keydataToRemove, int indexToRemove)
        {
            int loopIndex = 0;

            foreach(var item in neighbors.Keys.ToList())
            {
                if(loopIndex == indexToRemove)
                {
                    neighbors.Remove(item);
                    break;
                }
                loopIndex++;
            }
        }

        /// <summary>
        /// Creates kdTree according to training set
        /// </summary>
        /// <param name="unionTraningSet">all sets without the test set</param>
        public void createKdTree(List<Data> unionTraningSet)
        {
            int dimentionSize = unionTraningSet[0].Line.Count;
            m_root = createKdTreeHelper(unionTraningSet, 0, dimentionSize);
        }

        /// <summary>
        /// recursivly creates KDTree. 
        /// </summary>
        /// <param name="unionTraningSet">all sets without the test set</param>
        /// <param name="level">current level of the tree</param>
        /// <param name="dimentionSize">dimention Size (size of single line of data) </param>
        /// <returns></returns>
        private Node createKdTreeHelper(List<Data> unionTraningSet, int level, int dimentionSize)
        {
            Node node = new Node();
            int axis = level % dimentionSize; //culculate the index of the element of the array of  data that we work on
            unionTraningSet = unionTraningSet.OrderBy(x => x.Line[axis]).ToList(); //we sort all the data by size to look for the middle value that will be the line that we will divide according to its value
            int mid = unionTraningSet.Count / 2;
            node.Axis = axis;

            if (mid != 0)
            {
                node.MidValue = unionTraningSet[mid].Line[axis];
                node.Left = createKdTreeHelper(unionTraningSet.GetRange(0, mid), level++, dimentionSize); // element that are smaller that the middle value will be in the left side of the tree
                node.Right = createKdTreeHelper(unionTraningSet.GetRange(mid, unionTraningSet.Count - mid), level++, dimentionSize); // element that are bigger than the middle value will sort to the right side of the tree
            }
            else
            {
                node.Right = node.Left = null;
                node.DataPoint = unionTraningSet[mid];
            }
            return node;
        }
    }
}
