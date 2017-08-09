using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datd_mining
{
    public static class CommandDataProvider
    {
        private const string REGEX_PATTERN = @"knn -k([0-9\s]*) -cv([0-9\s]*) -t([yn\s]*) -f(.+)";
        private const string NN_DEFAULT_VALUE = "1";
        private const string FOLDS_DEFAULT_VALUE = "10";
        private const string IS_KDTREE_DEFAULT_VALUE = "Y";

        /// <summary>
        /// Extract out of the user input command the relevant data.
        /// </summary>
        /// <param name="inputCommand">string input from user</param>
        /// <returns>Data the user provide or defualt values otherwise</returns>
        public static CommandOutput ExtractCommandData(string inputCommand)
        {
            var regexObj = new System.Text.RegularExpressions.Regex(REGEX_PATTERN);
            var matchedInput = regexObj.Match(inputCommand);

            if (!matchedInput.Success) 
            {
                return null;
            }

            var userValues = GetUserOutputData(matchedInput.Groups, new string[] { NN_DEFAULT_VALUE, FOLDS_DEFAULT_VALUE, IS_KDTREE_DEFAULT_VALUE}).ToList();

            var output = new CommandOutput();
            output.NN = int.Parse(userValues[0].Trim());
            output.Folds = int.Parse(userValues[1].Trim());
            output.YN = userValues[2].Trim() == "y";
            output.FileName = userValues[3].Trim();

            return output;
        }

        /// <summary>
        /// Collect user input parameters
        /// </summary>
        /// <param name="regexCollection"></param>
        /// <param name="defaultValues"></param>
        /// <returns>Returns one parameter per itaration</returns>
        private static IEnumerable<string> GetUserOutputData(System.Text.RegularExpressions.GroupCollection regexCollection, string[] defaultValues)
        {
            for (int i = 1; i < regexCollection.Count; i++)
            {
                if (string.IsNullOrEmpty(regexCollection[i].Value))
                    yield return defaultValues[i - 1];
                else
                    yield return regexCollection[i].Value;
            }
        }
    }

    /// <summary>
    /// This class holds the data that the user provide. (or defualt values in case the user didnt provide)
    /// </summary>
    public class CommandOutput
    {
        public int NN { get; set; }
        public int Folds { get; set; }
        public bool YN { get; set; }
        public string FileName { get; set; }
    }
}
