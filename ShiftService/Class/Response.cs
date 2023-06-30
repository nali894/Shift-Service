using ExceptionsLib;
using Google.Protobuf.WellKnownTypes;

namespace ShiftService
{
    public class Response
    {
        private int _intCode;
        private dynamic _dynValue;
        private dynamic _strDescription;

        public int Code
        {
            get { return this._intCode; }
            set { this._intCode = value; }
        }

        public string Description
        {
            get { return this._strDescription; }
            set { this._strDescription= value; }
        }
        public dynamic Values
        {
            get { return this._dynValue; }
            set { this._dynValue = value; }
        }


        public Response(ExceptionCode enumCode,dynamic dynValue=null)
        {
            this._intCode = enumCode.GetHashCode();
            this._strDescription = enumCode.GetStringValue();
            this._dynValue = dynValue;
        }       
    }
       

}
