Imports System.Globalization
Imports System.Numerics

Public Class HexDecimal
    Public Shared Function HexToDecimal(hexValue As String) As Decimal
        If hexValue Is Nothing Then
            Return 0D
        End If
        If hexValue.StartsWith("0x", StringComparison.OrdinalIgnoreCase) Then
            hexValue = String.Concat("0", hexValue.AsSpan(2))
        End If
        Return BigInteger.Parse(hexValue, NumberStyles.HexNumber)
    End Function

    Public Shared Function DecimalToHexDecimal(decValue As Decimal) As String
        Return "0x" + Hex(decValue).ToLower()
    End Function
End Class
