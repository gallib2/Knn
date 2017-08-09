using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace datd_mining
{
    public class Data
    {
       private List<double> m_line=new List<double>();
       private string m_type;
       private bool m_isSelectedToGroup=false;

        /// <summary>
        /// flag to know if this data has been chosen to the stritified groups
        /// </summary>
        public bool IsSelectedToGroup
        {
            get { return m_isSelectedToGroup; }
            set { m_isSelectedToGroup = value; }
        }

        /// <summary>
        /// single line of data (i.e. a single point)
        /// </summary>
        public List<double> Line
        {
            get
            {
                return (m_line);
            }
            set
            {

                m_line.AddRange( value);
            }
        }

        /// <summary>
        /// The type of this line of data
        /// </summary>
        public string Type
        {
            get
            {
                return (m_type);
            }
            set
            {
                m_type = value;
            }
        }
    }
}
