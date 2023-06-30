namespace ShiftService
{
    public static class Utilities
    {
        public static bool isDate(string straDate)
        {
            bool breturn = false;

            if(string.IsNullOrEmpty(straDate))
            {
                return breturn;
            }

            DateTime temp;
            if (DateTime.TryParse(straDate, out temp))
            {
                breturn = true;
            }          

            return breturn;
        }

        public static bool isNumber(string strNumber)
        {
            bool breturn = false;

            if (string.IsNullOrEmpty(strNumber))
            {
                return breturn;
            }

            int temp;
            if (int.TryParse(strNumber, out temp))
            {
                breturn = true;
            }

            return breturn;
        }
    }
}
