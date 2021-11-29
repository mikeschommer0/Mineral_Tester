using System;
using System.Collections.Generic;
using System.IO;


namespace MineralTester.Classes
{
    public class BusinessLogic : IBusinessLogic
    {
        IDatabase db = new Database();
        public List<bool> ValidateMineralData(List<object> fields)
        {
            List<bool> validFields = new List<bool> { true, true, true};
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

            if (db.CheckMineralExists(name))
            {
                validFields[2] = false;
            }


            return validFields;
        }

        public List<bool> ValidateUserData(List<string> fields)
        {
            List<bool> validFields = new List<bool> { true, true, true, true, true};
            string firstName = fields[0];
            string lastName = fields[1];
            string username = fields[2];
            string password = fields[3];

            if (firstName.Length == 0 || firstName.Length > 50)
            {
                validFields[0] = false;
            }

            if (lastName.Length == 0 || lastName.Length > 50)
            {
                validFields[1] = false;
            }

            if (username.Length == 0 || username.Length > 8)
            {
                validFields[2] = false;
            }

            if (password.Length < 8)
            {
                validFields[3] = false;
            }

            //added to check if username is in use
            if (db.CheckUserExists(username))
            {
                validFields[4] = false;
            }

            return validFields;
        }

        /// <summary>
        /// Call to DB to add mineral.
        /// </summary>
        /// <param name="toAdd"> Mineral to add to db. </param>
        public void AddMineral(Mineral toAdd)
        {
            db.AddMineral(toAdd);
        }

        /// <summary>
        /// Call to DB to delete mineral.
        /// </summary>
        /// <param name="toDelete"> Mineral to delete from db. </param>
        public void DeleteMineral(Mineral toDelete)
        {
            db.DeleteMineral(toDelete);
        }

        public Mineral GetMineral(string mineralName)
        {
            return db.GetMineral(mineralName);
        }
    }
}
