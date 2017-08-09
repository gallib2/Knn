using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datd_mining
{
    /// <summary>
    /// this interface holds all the relevant data members for many classes that use it.
    /// </summary>
    public interface IDataInformation
    {
        int[,] m_confusunMatrix { get; set; }
        int counterFalseAnswer { get; set; }
        int counterCorrectAnswer { get; set; }
        List<string> m_typesList { get; set; }
        CrossValidation m_crossValidation { get; set; }
        int m_NumFolds { get; set; }
        int m_numOfAllDataToProcess { get; set; }
        string m_fileName { get; set; }
        int k { get; set; }
        ArffReader dataSet { get; set; }
    }
}
