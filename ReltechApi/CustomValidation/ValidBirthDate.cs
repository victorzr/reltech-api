using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReltechApi.CustomValidation
{
    public class ValidBirthDate : ValidationAttribute
    {
        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }

        public ValidBirthDate(string minDate)
        {
            MinDate = DateTime.Parse(minDate);
            MaxDate = DateTime.Now;
        }

        public override bool IsValid(object value)
        {
            var date = (DateTime)value;

            if (date > MaxDate || date < MinDate)
            {
                return false;
            }

            return true;
        }
    }
}
