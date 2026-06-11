using System.Linq;
using UnityEditor.PackageManager;
using StrCompare = System.StringComparison;

namespace AtMycelia.EditorExt
{
    public static class PackageUtils
    {
        /// <summary>
        /// Checks if a package with the specified name is installed in the project.
        /// By default, this performs a case-insensitive substring search, but you
        /// can specify an exact match and/or change the comparison type.
        /// </summary>
        /// <returns></returns>
        public static bool HasPackageWithName(string packageNameToFind, 
            StrCompare comparisonType = StrCompare.OrdinalIgnoreCase,
            bool exactMatch = false)
        {
            var packageNames = GetInstalledPackageNames();
            for (int i = 0; i < packageNames.Length; i++)
            {
                var currentName = packageNames[i];
                if (exactMatch)
                {
                    if (currentName.Equals(packageNameToFind, comparisonType))
                        return true;
                }
                else
                {
                    if (packageNames[i].Contains(packageNameToFind, comparisonType))
                        return true;
                }
            }

            return false;
        }

        public static string[] GetInstalledPackageNames()
        {
            var listRequest = Client.List(true); // true = include dependencies
            while (!listRequest.IsCompleted)
            {
                // spin until done (Editor-only, safe)
            }

            if (listRequest.Status == StatusCode.Failure)
                return new string[0];

            var packages = listRequest.Result.ToArray();
            int packageCount = packages.Count();
            string[] names = new string[packageCount];
            for (int i = 0; i < packages.Length; i++)
                names[i] = packages[i].name;

            return names;
        }
    }
}