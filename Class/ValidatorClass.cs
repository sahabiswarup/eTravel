using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace SIBINUtility.ValidatorClass
{
    public static class ValidatorClass
    {
        public static bool ValidateText(string TextToValidate)
        {
            return true;
            //return Regex.IsMatch(TextToValidate, @"^[a-zA-Z0-9,\s\-&*(),.;@:/_+ ]+$");
        }
        public static bool IsValidEmail(string EmailToValid)
        {
            try
            {
                return Regex.IsMatch(EmailToValid, @"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$");
            }
            catch (FormatException)
            {
                return false;
            }
        }
        public static bool isValidURL(string URLtoValid)
        {
            try
            {
                return Regex.IsMatch(URLtoValid, "(http(s)?://)?([\\w-]+\\.)+[\\w-]+(/[\\w- ;,./?%&=]*)?");
               // return Regex.IsMatch(URLtoValid, @"^(ht|f)tp(s?)\:\/\/[0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*(:(0-9)*)*(\/?)([a-zA-Z0-   9\-\.\?\,\'\/\\\+&amp;%\$#_]*)?$");
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static bool IsValidMobileNo(string NumberToValidate)
        {
            return Regex.IsMatch(NumberToValidate, @"^[0-9]{1,10}$");
        }
        public static bool isValidCurrency(string CurrencyToValid)
        {
            return Regex.IsMatch(CurrencyToValid, @"^[0-9]+(\.[0-9][0-9])?$");//decimal part is optional
        }
        public static bool IsNumeric(this object value)
        {
            if (value == null || value is DateTime)
            {
                return false;
            }

            if (value is Int16 || value is Int32 || value is Int64 || value is Decimal || value is Single || value is Double || value is Boolean)
            {
                return true;
            }

            try
            {
                if (value is string)
                    Double.Parse(value as string);
                else
                    Double.Parse(value.ToString());
                return true;
            }
            catch { }
            return false;
        }
    }
}