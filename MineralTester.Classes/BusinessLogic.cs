using System.Collections.Generic;

/// <summary>
/// Written by Quinn Nimmer & Mike Schommer
/// </summary>
namespace MineralTester.Classes
{
    public class BusinessLogic : IBusinessLogic
    {
        IDatabase db = new Database();

        /// <summary>
        /// Validates mineral related data to ensure it matches database column requirements.
        /// </summary>
        /// <param name="fieldsToValidate"> The fields being validated. </param>
        /// <returns> A list of booleans representing which fields are valid and which are not. </returns>
        public List<bool> ValidateMineralData(List<object> fieldsToValidate)
        {
            List<bool> validFields = new List<bool> { true, true, true };
            string name = (string)fieldsToValidate[0];
            float hardness = (float)fieldsToValidate[1];
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

        /// <summary>
        /// Validates user related data to ensure it matches database column requirements
        /// </summary>
        /// <param name="fieldsToValidate"> The fields being validated.</param>
        /// <returns></returns>
        public List<bool> ValidateUserData(List<string> fieldsToValidate)
        {
            List<bool> validFields = new List<bool> { true, true, true, true, true };
            string firstName = fieldsToValidate[0];
            string lastName = fieldsToValidate[1];
            string username = fieldsToValidate[2];
            string password = fieldsToValidate[3];
            if (firstName.Length == 0 || firstName.Length > 50)
            {
                validFields[0] = false;
            }
            if (lastName.Length == 0 || lastName.Length > 50)
            {
                validFields[1] = false;
            }
            if (username.Length == 0 || username.Length > 50)
            {
                validFields[2] = false;
            }
            if (password.Length < 8)
            {
                validFields[3] = false;
            }
            if (db.CheckUserExists(username))
            {
                validFields[4] = false;
            }
            return validFields;
        }

        /// <summary>
        /// Adds a mineral to the database.
        /// </summary>
        /// <param name="mineralToAdd"> The mineral to add to the database. </param>
        public void AddMineral(Mineral mineralToAdd)
        {
            db.AddMineral(mineralToAdd);
        }

        /// <summary>
        /// Deletes a mineral from the database.
        /// </summary>
        /// <param name="mineralToDelete"> The mineral to delete. </param>
        public void DeleteMineral(Mineral mineralToDelete)
        {
            db.DeleteMineral(mineralToDelete);
        }

        /// <summary>
        /// Gets a specific mineral from the database.
        /// </summary>
        /// <param name="mineralName"> The name of the mineral to retrieve. </param>
        /// <returns> The mineral specified mineral. </returns>
        public Mineral GetMineral(string mineralName)
        {
            return db.GetMineral(mineralName);
        }

        /// <summary>
        /// Updates a mineral in the database.
        /// </summary>
        /// <param name="mineralToUpdate"> The mineral to update. </param>
        public void UpdateMineral(Mineral mineralToUpdate)
        {
            db.UpdateMineral(mineralToUpdate);
        }

        /// <summary>
        /// Gets a list of all minerals from the database.
        /// </summary>
        /// <returns> A list of minerals. </returns>
        public List<Mineral> GetMinerals()
        {
            return db.GetMinerals();
        }
    }
}
