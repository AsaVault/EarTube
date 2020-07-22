using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EarTube.MyValidator
{
    [AttributeUsage(
        validOn: AttributeTargets.Field | AttributeTargets.Property,
        AllowMultiple = false,
        Inherited = true)]
    public class MyUploadFileSizeValidator: ValidationAttribute
    {
        public long SizeInBytes { get; private set; }
        public MyUploadFileSizeValidator(long sizeInBytes)
        {
            SizeInBytes = sizeInBytes;
        }

        public override bool IsValid(object value)
        {
            bool isValid = false;

            // NOTE: Use HttpPostedFileBase instead of IFormFile in ASP.NET MVC
            if (value is IFormFile file)
            {
                isValid = file.Length <= this.SizeInBytes;
            }

            return isValid;
        }
    }
}
