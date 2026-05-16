using System;
using System.Collections.Generic;
using System.Linq;

namespace AtMycelia
{
    public static class UniqueKeyGenerator
    {
        /// <summary>
        /// Generates a unique key for any keyed type.
        /// </summary>
        public static string GetUniqueKeyFor<T>(string suggestedKey, IList<T> keyedGroup,
            T ignoreItem = null, string defaultKey = null)
            where T : class, IHasKey
        {
            string baseKey = WithNamingRulesApplied(suggestedKey, defaultKey);
            string resultKey = baseKey;
            int duplicateKeySuffix = 0;

            while (true)
            {
                bool collision = false;

                for (int i = 0; i < keyedGroup.Count; i++)
                {
                    T registeredItem = keyedGroup[i];
                    bool shouldSkipThisOne = registeredItem == null ||
                        registeredItem == ignoreItem ||
                        registeredItem.Key == null;

                    if (shouldSkipThisOne)
                    {
                        continue;
                    }

                    bool keyAlreadyTaken = registeredItem.Key.Equals(resultKey,
                        StringComparison.CurrentCultureIgnoreCase);

                    if (keyAlreadyTaken)
                    {
                        collision = true;
                        duplicateKeySuffix++;
                        resultKey = baseKey + duplicateKeySuffix;
                        // It's possible that a previous element's key matches the 
                        // new resultKey, meaning we'll need to check them all
                        // again. Thus...
                        i = 0;
                    }
                }

                if (!collision)
                {
                    return resultKey;
                }
            }

        }

        /// <summary>
        /// This returns a version of the key that meets all of the following criteria: 
        /// <br></br>
        /// - Contains only letters, underscores or digits <br></br>
        /// - Doesn't start with a digit <br></br>
        /// - Isn't empty (if what this is given is empty, the fallback val gets returned)
        /// </summary>
        private static string WithNamingRulesApplied(string key, string fallback = null)
        {
            fallback ??= _defaultGenericKey;
            string candidate = key ?? string.Empty;

            // Only letters, underscores and digits allowed
            char[] chars = candidate.Where(c => char.IsLetterOrDigit(c) || c == '_').ToArray();
            candidate = new string(chars);

            // No leading digits
            candidate = candidate.TrimStart('0', '1', '2', '3', '4', '5', '6', '7', '8', '9');

            // No empty keys allowed
            if (candidate.Length == 0)
            {
                candidate = string.IsNullOrEmpty(fallback) ? 
                    _defaultGenericKey : 
                    fallback;
            }

            return candidate;
        }

        private static readonly string _defaultGenericKey = "Key";
        
    }
}