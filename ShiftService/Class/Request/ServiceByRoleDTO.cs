using System.Xml.Linq;

namespace ShiftService
{
    public class ServiceByRoleDTO
    {        
        private string _strRole = "";
        private string _strDateTime = DateTime.Now.ToString("yyyy/MM/dd");

        public string strRole
        {
            get { return _strRole; }   // get method
            set { _strRole = value; }  // set method


        }

        public string? strDateTime
        {
            get { return _strDateTime; }   // get method
            set { _strDateTime = value; }  // set method


        }
    }
}
