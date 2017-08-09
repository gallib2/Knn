using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datd_mining
{
    public class ArffReader
    {
        private List<Data> m_listAttributes = new List<Data>();
        private Dictionary<string, List<Data>> m_SortDataByType = new Dictionary<string, List<Data>>();
        private string[] typesList;
        private Dictionary<string, int> m_typesCounter = new Dictionary<string, int>();
         
        public int m_SumOfData { get; set; }

        public Dictionary<string, List<Data>> SortDataByType
        {
            get { return m_SortDataByType; }
            set { m_SortDataByType = value; }
        }

        public Dictionary<string, int> TypesDictionary
        {
            get { return m_typesCounter; }
            set { m_typesCounter = value; }
        }

        public List<Data> DataSet
        {
            get { return m_listAttributes; }


            set { m_listAttributes = value; }
        }

        public string[] TypesList
        {
            get { return typesList; }


            set { typesList = value; }
        }

        public void SortAllDataByType()
        {
            foreach (Data curr in m_listAttributes)
            {
                m_SortDataByType[curr.Type].Add(curr);
            }
        }

        /// <summary>
        /// Extract the Arff files Paths from text file
        /// </summary>
        /// <param name="txtFilePath"></param>
        /// <returns></returns>
        public IEnumerable<string> ExtractArffFilesPaths(string txtFilePath)
        {
            foreach (string arffFilePath in File.ReadLines(txtFilePath))
                yield return arffFilePath;
        }


        /// <summary>
        /// Extract the types and the data from the given file
        /// </summary>
        /// <param name="filePath"></param>
        public void getDataFromFile(string filePath)
        {
            clearAllMembers();
            using (var fs = File.OpenRead(filePath))
            using (var reader = new StreamReader(fs))
            {
                double data;
                List<double> parseLine = new List<double>();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (line.Contains("@attribute") && (line.Contains("'Type'") || line.Contains("class")))
                    {
                        var split = line.Split('{', '}');

                        typesList = split[1].Split(',');

                        for (int i = 0; i < TypesList.Length; i++)
                        {
                            typesList[i] = typesList[i].TrimStart(' ');
                            m_typesCounter.Add(typesList[i], 0);
                            m_SortDataByType.Add(typesList[i].TrimStart(' '), new List<Data>());
                        }
                    }
                    if (line.Contains("@data"))
                    {
                        while (!reader.EndOfStream)
                        {
                            var singelLineData = reader.ReadLine();
                            if (singelLineData != string.Empty)
                            {
                                var splitOneLine = singelLineData.Split(',');
                                Data oneLineOfdataSet = new Data();
                                int sizeOfAttribute = splitOneLine.Count() - 1;
                                int index = 0;
                                foreach (string curr in splitOneLine)
                                {
                                    bool isParse = double.TryParse(curr, out data);
                                    if (index == sizeOfAttribute)
                                    {
                                        string trimCurr = curr.TrimStart(' ');
                                        oneLineOfdataSet.Type = trimCurr;
                                        m_typesCounter[trimCurr] += 1;

                                        break;
                                    }
                                    parseLine.Add(data);
                                    index++;

                                }
                                oneLineOfdataSet.Line = parseLine;

                                m_listAttributes.Add(oneLineOfdataSet);
                                parseLine.Clear();
                            }
                            m_SumOfData = m_listAttributes.Count;
                        }
                    }
                }
            }
            SortAllDataByType();
        }

        /// <summary>
        /// clear all the lists member (for every use of this class)
        /// </summary>
        private void clearAllMembers()
        {
            m_listAttributes.Clear();
            m_SortDataByType.Clear();
            m_typesCounter.Clear();

        }
    }
}
