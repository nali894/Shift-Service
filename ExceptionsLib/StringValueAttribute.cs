using System;

namespace ExceptionsLib
{

    public class StringValueAttribute : Attribute
    {

        #region Properties

        /// <summary>
        /// Stores the string value for a specific value within an enum.
        /// </summary>
        public string StringValue { get; protected set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor used to initialize a StringValue attribute.
        /// </summary>
        /// <param name="value"></param>
        public StringValueAttribute(string value)
        {
            this.StringValue = value;
        }

        #endregion

    }
}
