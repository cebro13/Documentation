using UnityEngine;

namespace ShowAttribute
{
    public class ShowDetailsAttribute : PropertyAttribute
    {
        public string GroupName;

        public ShowDetailsAttribute(string groupName)
        {
            GroupName = groupName;
        }
    }
}
