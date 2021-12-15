using System.Collections.Generic;

/// <summary>
/// Written by Quinn Nimmer & mike Schommer
/// </summary>
namespace MineralTester.Classes
{
    /// <summary>
    /// Interface for the business logic.
    /// </summary>
    public interface IBusinessLogic
    {
        /// <summary>
        /// Validates mineral related data to ensure it matches database column requirements.
        /// </summary>
        /// <param name="fieldsToValidate"> The fields being validated. </param>
        /// <returns> A list of booleans representing which fields are valid and which are not. </returns>
        List<bool> ValidateMineralData(List<object> fieldsToValidate);

        /// <summary>
        /// Validates user related data to ensure it matches database column requirements
        /// </summary>
        /// <param name="fieldsToValidate"> The fields being validated.</param>
        /// <returns></returns>
        List<bool> ValidateUserData(List<string> fieldsToValidate);

        /// <summary>
        /// Adds a mineral to the database.
        /// </summary>
        /// <param name="mineralToAdd"> The mineral to add to the database. </param>
        void AddMineral(Mineral mineralToAdd);

        /// <summary>
        /// Deletes a mineral from the database.
        /// </summary>
        /// <param name="mineralToDelete"> The mineral to delete. </param>
        void DeleteMineral(Mineral mineralToDelete);

        /// <summary>
        /// Updates a mineral in the database.
        /// </summary>
        /// <param name="mineralToUpdate"> The mineral to update. </param>
        void UpdateMineral(Mineral mineralToUpdate);

        /// <summary>
        /// Gets a specific mineral from the database.
        /// </summary>
        /// <param name="mineralName"> The name of the mineral to retrieve. </param>
        /// <returns> The mineral specified mineral. </returns>
        Mineral GetMineral(string mineralName);

        /// <summary>
        /// Gets a list of all minerals from the database.
        /// </summary>
        /// <returns> A list of minerals. </returns>
        List<Mineral> GetMinerals();
    }
}