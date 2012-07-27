Public Class gnote
    Friend rowcount As Integer

    Private Sub gnote_load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim ss As String() = TextBox1.Text.Split(CChar(vbLf))
        Dim head As String = ""
        Dim line, counter As Integer
        Dim note As String = ""
        Dim freecheat As String = ""
        Dim mask As String = "_(L|M|N) 0x[0-9a-zA-Z]{8} 0x[0-9a-zA-Z]{8}"
        Dim mask2 As String = "\$[^\$]+\$[0-2][^\$]+\$\([0-9A-Za-z\x20]+\)"
        Dim mask3 As String = "[0-9a-zA-Z]{8}"
        Dim q As New System.Text.RegularExpressions.Regex(mask, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Dim dg_comment As System.Text.RegularExpressions.Match
        Dim r As New System.Text.RegularExpressions.Regex(mask2, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Dim dg_comment2 As System.Text.RegularExpressions.Match
        Dim t As New System.Text.RegularExpressions.Regex(mask3, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        Dim dg_comment3 As System.Text.RegularExpressions.Match
        Dim doll As Integer
        For Each s In ss
            If s.Length >= 3 Then
                s = s.Trim
                head = s.Substring(0, 3)
                If head = "_N0" Or head = "_N1" Or head = "_N2" Then
                    freecheat = s.Substring(3, s.Length - 3).Trim
                ElseIf head.Substring(0, 1) = "$" Then
                    dg_comment2 = r.Match(s)
                    If dg_comment2.Success Then
                        s = s.Remove(0, 1)
                        doll = s.IndexOf("$")
                        If doll > 0 Then
                            freecheat = s.Substring(0, doll - 1).Trim
                            dg_comment3 = t.Match(s)
                            While dg_comment3.Success
                                counter += 1
                                If (counter And 1) = 0 Then
                                    note &= "<DGLINE" & Convert.ToString(line + 1) & "='" & freecheat.Trim & "'>" & vbCrLf
                                    line += 1
                                End If
                                dg_comment3 = dg_comment3.NextMatch
                            End While
                            Counter = 0
                        End If
                        freecheat = ""
                    End If

                ElseIf head = "_L " Or head = "_M" Or head = "_N " Then
                    dg_comment = q.Match(s)
                    If dg_comment.Success Then
                        note &= "<DGLINE" & Convert.ToString(line + 1) & "='" & freecheat.Trim & s.Substring(24, s.Length - 24).Trim & "'>" & vbCrLf
                    End If
                    line += 1
                    freecheat = ""
                    End If
            End If
        Next
        TextBox2.Text = note
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

End Class