
using Windows.Foundation.Metadata;

namespace AppCore.SDK.Controls
{
    internal class ApiInformationHelper
    {
        private static bool? _isAniversaryUpdateOrAbove;
        private static bool? _isCreatorsUpdateOrAbove;
        private static bool? _isFallCreatorsUpdateOrAbove;

        public static bool IsAniversaryUpdateOrAbove => (bool)(_isAniversaryUpdateOrAbove ??
            (_isAniversaryUpdateOrAbove = ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 3)));

        public static bool IsCreatorsUpdateOrAbove => (bool)(_isCreatorsUpdateOrAbove ??
            (_isCreatorsUpdateOrAbove = ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 4)));

        public static bool IsFallCreatorsUpdateOrAbove => (bool)(_isFallCreatorsUpdateOrAbove ??
            (_isFallCreatorsUpdateOrAbove = ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 5)));
    }
}
