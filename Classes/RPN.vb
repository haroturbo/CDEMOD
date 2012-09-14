Imports System.Text
Imports System
Imports System.IO
Imports System.Text.RegularExpressions

Class Node

    'http://smdn.jp/programming/tips/polish/　を改造

    Public Expression As String
    Public Left As Node = Nothing
    Public Right As Node = Nothing

    Public Sub New(ByVal expression As String)
        Me.Expression = expression
    End Sub

    Public Sub Parse()
        Dim posOperator As Integer = GetOperatorPos(Expression)

        If posOperator < 0 Then
            Left = Nothing
            Right = Nothing
            Return
        End If

        ' left-hand side
        Left = New Node(RemoveBracket(Me.Expression.Substring(0, posOperator)))
        Left.Parse()

        ' right-hand side
        Right = New Node(RemoveBracket(Me.Expression.Substring(posOperator + 1)))
        Right.Parse()

        ' operator
        Me.Expression = Me.Expression.Substring(posOperator, 1)
    End Sub

    Private Shared Function RemoveBracket(ByVal str As String) As String
        If Not (str.StartsWith("(") AndAlso str.EndsWith(")")) Then
            Return str
        End If

        Dim nest As Integer = 1

        For i As Integer = 1 To str.Length - 2 'C#版とくらべると-2になってなかった
            If str(i) = "(" Then
                nest += 1
            ElseIf str(i) = ")" Then
                nest -= 1
            End If

            If nest = 0 Then
                Return str
            End If
        Next

        If nest <> 1 Then Throw New Exception(String.Format("unbalanced bracket: {0}", str))

        str = str.Substring(1, str.Length - 2)

        If str.StartsWith("(") Then
            Return RemoveBracket(str)
        Else
            Return str
        End If
    End Function

    Private Shared Function GetOperatorPos(ByVal expression As String) As Integer
        If String.IsNullOrEmpty(expression) Then Return -1

        Dim pos As Integer = -1
        Dim nest As Integer = 0
        Dim priority As Integer = 0
        Dim lowestPriority As Integer = 5
        Dim bytes As Byte()
        Dim cp As Integer = 0

        '^-2 x^(-2)対策
        If (expression(0)) = "-" Then
            expression = "~" & expression.Remove(0, 1)
        End If

        For i As Integer = 0 To expression.Length - 1
            Select Case expression(i)
                Case CChar("=") : priority = 1
                Case CChar("+") : priority = 2
                Case CChar("-") : priority = 2
                Case CChar("*") : priority = 3
                Case CChar("/") : priority = 3
                Case CChar("^") : priority = 4
                Case CChar(",") : priority = 0
                    'Case "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"
                    '    priority = 4
                Case CChar("(") : nest += 1 : Continue For
                Case CChar(")") : nest -= 1 : Continue For
                Case Else
                    bytes = Encoding.GetEncoding(1200).GetBytes(expression(i))
                    cp = BitConverter.ToUInt16(bytes, 0)
                    If cp >= &HA1 Or (cp >= &H41 AndAlso cp <= &H5A) Then
                        priority = 4
                    Else
                        Continue For
                    End If
            End Select

            If nest = 0 AndAlso priority <= lowestPriority Then
                lowestPriority = priority
                pos = i
            End If
        Next

        Return pos
    End Function

    Public Function TraversePostorder(ByVal sb As StringBuilder, ByVal stack As Boolean) As StringBuilder
        If Left IsNot Nothing Then
            Left.TraversePostorder(sb, stack)
        End If
        If Right IsNot Nothing Then
            Right.TraversePostorder(sb, stack)
        End If


        If stack = False AndAlso ((Expression) = "^" Or ((Expression) >= "A" AndAlso Expression <= "V")) Then
            sb.Append("swap")
            sb.Append(",")
        ElseIf stack = False AndAlso (((Expression) >= "W" AndAlso Expression <= "Z")) Then
            sb.Append("swap3")
            sb.Append(",")
        End If
        sb.Append(Expression)
        sb.Append(",")

        Return sb

    End Function

End Class


Class Polish

    Dim rp2 As String() = {"logx", "pow", "xrt", "yrt", "logy", "logms",
                           "atan2_r", "atan2_g", "atan2_d", "atan2_",
                           "atan2ms_g", "atan2ms_r", "atan2ms_d", "atan2ms_",
                           "atanh2_d", "atanh2_g", "atanh2_r", "atanh2_",
                           "atanh2ms_d", "atanh2ms_g", "atanh2ms_r", "atanh2ms_",
                           "dms>deg", "hms>deg", "hms>h", "hms>d",
                           "dms2deg", "dms2d", "hms2deg", "hms2d", "hms2h",
                           "deg2rad", "deg2grad", "deg2r",
                           "grad2deg", "grad2rad", "grad2r",
                           "rad2deg", "rad2grad", "rad2r",
                           "r2deg", "r2rad", "r2grad",
                            "actanhd", "actanhr", "actanhg", "actanh",
                            "actang", "actand", "actanr", "actan",
                           "asechd", "asechr", "asechg", "asech",
                            "acschd", "acschr", "acschg", "acsch",
                            "acothd", "acothr", "acothg", "acoth",
                            "acotg", "acotd", "acotr", "acot",
                            "asecg", "asecd", "asecr", "asec",
                            "acscg", "acscd", "acscr", "acsc",
                            "sechg", "sechd", "sechr", "sech",
                            "cschg", "cschd", "cschr", "csch",
                            "ctanhg", "ctanhd", "ctanhr", "ctanh",
                            "cothg", "cothd", "cothr", "coth",
                            "ctang", "ctand", "ctanr", "ctan",
                            "cotg", "cotd", "cotr", "cot",
                            "cscg", "cscd", "cscr", "csc",
                            "secg", "secd", "secr", "sec",
                           "atanhg", "atanhd", "atanhr", "atanh",
                           "acoshng", "acoshnd", "acoshnr", "acoshn",
                           "acoshg", "acoshd", "acoshr", "acosh",
                           "asinhg", "asinhd", "asinhr", "asinh",
                           "tanhr", "coshr", "sinhr", _
                           "tanhg", "coshg", "sinhg", _
                           "tanhd", "coshd", "sinhd", _
                           "tanh", "cosh", "sinh", _
                           "atanr", "acosr", "asinr", "tanr", "cosr", "sinr", _
                           "atang", "acosg", "asing", "tang", "cosg", "sing", _
                           "atand", "acosd", "asind", "tand", "cosd", "sind", _
                            "hypot", "atan", "acos", "asin", "tan", "cos", "sin", _
                           "sqrt", "cbrt", "logten", "logtwo", "logthree", "ln", "reci", "√", "exp", "loge", "log", "abs", "chs",
                            "d2rad", "rad2d", "rad2g", "g2rad", "d2g", "g2d", "d2r", "g2r", "r2d", "r2g", _
                           ">>>", ">>", "<<", "\", "&", "|", "and", "xor", "ror", "rol", "sll", "srl", "or", "not", "sra", "mod"
                           }

    Dim kana As String() = {"ｵ", "ﾊ", "ｲ"}
    Dim mathrp As String() = {"goldenratio", "pi", "e"}

    Public Function Main(ByVal s As String, ByVal stack As Boolean, ByVal cr As Boolean, ByVal int As Boolean) As String
        s = s.Replace(" ", String.Empty).ToLower
        Dim rp As String() = Nothing
        Array.Resize(rp, rp2.Length)
        Dim c1 As Integer = &H41

        '1文字ダミー処理を作成
        For i = 0 To rp2.Length - 1
            c1 = &H41 + i
            If c1 >= &H5B Then
                c1 = (c1 + &H46)
            End If
            rp(i) = Encoding.GetEncoding(12000).GetString(BitConverter.GetBytes(c1))
        Next

        If int = True Then
            For i = rp2.Length - 16 To rp2.Length - 1
                s = s.Replace(rp2(i), rp(i))
            Next
        Else
            For i = 0 To rp2.Length - 17 '-1-16
                s = s.Replace(rp2(i), rp(i))
            Next
        End If


        If cr = True Then
            For i = 0 To kana.Length - 1
                s = s.Replace(mathrp(i), kana(i))
            Next

            Dim cross As New Regex("((ｵ|ﾊ|ｲ|\d)[A-Z\u00A1-\u0200\(]|\)\()")
            Dim hosei As Match = cross.Match(s)
            While hosei.Success
                s = s.Replace(hosei.Value, hosei.Value.Insert(1, "*"))
                hosei = hosei.NextMatch
            End While

            For i = 0 To kana.Length - 1
                s = s.Replace(kana(i), mathrp(i))
            Next
        End If


        Dim root As Node = New Node(s)
        Dim sb As New StringBuilder

        root.Parse()

        s = root.TraversePostorder(sb, stack).ToString
        'ダミー処理を復元
        For i = 0 To rp2.Length - 1
            s = s.Replace(rp(i), rp2(i))
        Next

        '空命令を除去
        While s.Contains(",,")
            s = s.Replace(",,", ",")
        End While
        If s(0) = "," Then
            s = s.Remove(0, 1)
        End If

        'x^(-3)とかの対策で~にしてたやつを戻す
        s = s.Replace("~", "-")

        Return s

    End Function
End Class