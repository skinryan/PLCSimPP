using System;
using System.ComponentModel.DataAnnotations;

namespace PLCSimPP.PresentationControls.ValidationAttributes
{
    public class NumberRange : ValidationAttribute
    {
        private readonly int mMinNumber;
        private readonly int mMaxNumber;
        /// <summary>
        /// Set number range
        /// </summary>
        /// <param name="minNumber"></param>
        /// <param name="maxNumber"></param>
        public NumberRange(int minNumber, int maxNumber)
        {
            mMinNumber = minNumber;
            mMaxNumber = maxNumber;
        }
        /// <summary>
        /// Validation
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            if (value == null)
                return true;
            var stringValue = value.ToString();
            int intValue;
            if (!Int32.TryParse(stringValue, out intValue))
                return false;
            return intValue >= mMinNumber && intValue <= mMaxNumber;
        }
    }
}
