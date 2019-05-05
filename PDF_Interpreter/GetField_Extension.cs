using System.Reflection;

/// <summary>
/// To deal with the Java interface hiding necessary properties! ~mwr
/// </summary>
public static class GetField_Extension
{   
    public static object GetField(this object randomPDFboxObject, string sFieldName)
    {
        FieldInfo itemInfo = randomPDFboxObject.GetType().GetField(sFieldName, BindingFlags.NonPublic | BindingFlags.Instance);
        return itemInfo.GetValue(randomPDFboxObject);
    }

}
