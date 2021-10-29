using System;
using System.Collections.Generic;
using System.IO;


namespace MineralTester.Classes
{
    public class BusinessLogic : IBusinessLogic
    {
        public List<bool> ValidateMineralData(List<object> fields)
        {
            List<bool> validFields = new List<bool> { true, true};
            string name = (String)fields[0];
            float hardness = (float)fields[1];

            if (name.Length == 0 || name.Length > 30)
            {
                validFields[0] = false;
            }

            if (hardness == 0 || hardness > 10.0)
            {
                validFields[1] = false;
            }
            return validFields;
        }
    }
}
