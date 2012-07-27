Imports System.Text
Imports System.IO

'ICOMPAREはインデックスを破壊する?のでつかわない
Class sort
    'さんぷるどおりだとうごくがなぜかうまくいかないので代替関数
    Public Function sort_game(ByVal mode As Integer) As Boolean
        Dim m As MERGE = MERGE

        error_window.Visible = False
        m.codetree.BeginUpdate() ' This will stop the tree view from constantly drawing the changes while we sort the nodes

        Dim z As Integer = m.codetree.Nodes(0).Nodes.Count
        Dim i As Integer = 0
        Dim b1 As String = Nothing
        Dim b2 As String = Nothing
        Dim b3 As String = Nothing
        Dim s(z) As String
        Dim jp(z) As String
        Dim us(z) As String
        Dim eu(z) As String
        Dim hb(z) As String
        Dim c As Integer = 0
        Dim d As Integer = 0
        Dim e As Integer = 0
        Dim f As Integer = 0
        For Each n As TreeNode In m.codetree.Nodes(0).Nodes
            If (mode And 2) = 2 Then
                b1 = n.Text
            Else
                b1 = n.Tag.ToString
            End If
            Dim sb As New System.Text.StringBuilder()
            b2 = n.Index.ToString
            If mode = 8 Then
                b3 = n.Name
                sb.Append(b3)
            Else
                sb.Append(b1)
            End If
            sb.Append(" ,")
            sb.Append(b2)
            If mode >= 4 Then
                If b1.Contains("J") AndAlso m.PSX = False Then
                    jp(c) = sb.ToString
                    c += 1
                ElseIf b1.Contains("P") AndAlso m.PSX = True Then
                    jp(c) = sb.ToString
                    c += 1
                ElseIf b1.Contains("US") Then
                    us(d) = sb.ToString
                    d += 1
                ElseIf b1.Contains("ES") Then
                    eu(e) = sb.ToString
                    e += 1
                ElseIf b1.Contains("HB") Then
                    hb(f) = sb.ToString
                    f += 1
                Else
                    s(i) = sb.ToString
                    i += 1
                End If
            Else
                s(i) = sb.ToString
                i += 1
            End If
        Next
        If mode >= 4 Then
            Array.Resize(jp, c)
            Array.Resize(us, d)
            Array.Resize(eu, e)
            Array.Resize(hb, f)
            Array.Resize(s, i)
            Array.Sort(jp)
            Array.Sort(us)
            Array.Sort(eu)
            Array.Sort(hb)
            Array.Sort(s)
            Dim mergedArray As String() = jp.Union(us).ToArray()
            mergedArray = mergedArray.Union(eu).ToArray()
            mergedArray = mergedArray.Union(s).ToArray()
            mergedArray = mergedArray.Union(hb).ToArray()
            Array.Resize(s, z + 1)
            Array.Copy(mergedArray, 0, s, 1, z)
        Else
            Array.Sort(s)
        End If
        Dim j As Integer = 1
        Dim k As Integer = 0
        Dim y As Integer = 0
        Dim commaindex As Integer = 0
        Dim ss As String
        Dim dbtrim As String = Path.GetFileNameWithoutExtension(m.database)
        m.codetree.Nodes.Add(dbtrim & "_sort")
        If (mode And 1) = 0 Then
            While k < z
                commaindex = s(j).LastIndexOf(",") + 1
                ss = s(j).Substring(commaindex, s(j).Length - commaindex)
                y = CInt(ss)
                Dim cln As TreeNode = CType(m.codetree.Nodes(0).Nodes(y).Clone(), TreeNode)
                m.codetree.Nodes(1).Nodes.Add(cln)
                k += 1
                j += 1
            End While
        ElseIf (mode And 1) = 1 Then
            j = z
            While k < z
                commaindex = s(j).LastIndexOf(",") + 1
                ss = s(j).Substring(commaindex, s(j).Length - commaindex)
                y = CInt(ss)
                Dim cln As TreeNode = CType(m.codetree.Nodes(0).Nodes(y).Clone(), TreeNode)
                m.codetree.Nodes(1).Nodes.Add(cln)
                k += 1
                j -= 1
            End While
        End If
        m.codetree.Nodes(0).Remove()
        If m.codetree.Nodes.Count >= 1 Then
            m.codetree.Nodes(0).Expand()
        End If

        m.codetree.EndUpdate() ' Update the changes made to the tree view.

        If m.options_error.Checked = True Then
            error_window.Visible = True
        End If

        Return True
    End Function

End Class