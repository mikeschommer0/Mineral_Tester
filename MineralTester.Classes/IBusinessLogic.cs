using System.Collections.Generic;

namespace MineralTester.Classes
{
    public interface IBusinessLogic
    {
        List<bool> ValidateMineralData(List<object> fields);

        List<bool> ValidateUserData(List<string> fields);

        void AddMineral(Mineral toAdd);

        void DeleteMineral(Mineral toDelete);

        void UpdateMineral(Mineral toUpdate);

        Mineral GetMineral(string mineralName);
    }
}