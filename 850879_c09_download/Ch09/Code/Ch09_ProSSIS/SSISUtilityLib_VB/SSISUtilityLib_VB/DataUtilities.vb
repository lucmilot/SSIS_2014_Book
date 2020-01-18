Imports System.Text.RegularExpressions

Public Class DataUtilities
    Public Shared Function isValidUSPostalCode(ByVal PostalCode As String) As Boolean
        isValidUSPostalCode = Regex.IsMatch(PostalCode, "^[0-9]{5}(-[0-9]{4})?$")
    End Function
End Class
