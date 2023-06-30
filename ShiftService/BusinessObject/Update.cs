namespace ShiftService
{
    public class Update
    {      
        public bool UpdateStatus(int intServiceID, string strStatus)
        {            
            Dictionary<string, dynamic> lstParameters = new Dictionary<string, dynamic>()
            {
                { "intServiceID",intServiceID },
                { "strStatus",strStatus}
            };

            return MySqlStartup.CallStoredProcedure_Update(lstParameters, "UptadeStatus");
           
        }


        public bool UpdateService_User(int intServiceID, string strUserCode)
        {
            Dictionary<string, dynamic> lstParameters = new Dictionary<string, dynamic>()
            {
                { "intServiceID",intServiceID },
                { "strUser",strUserCode}
            };

            return MySqlStartup.CallStoredProcedure_Update(lstParameters, "UpdateService_User");
        }
    }
}
