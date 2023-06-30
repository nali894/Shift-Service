namespace ShiftService
{ 
    public class Token
    {   
        private string _strSignToken = "";
        public string SignToken
        {
            get { return _strSignToken; }   
            set { _strSignToken = value; } 
        }
    }
}
