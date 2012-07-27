Imports System
Imports System.Text
Imports System.IO

Public Class customtable

    Dim vstable As String() = {"table\sjisvsutf8", "table\eucvsutf8", "table\big5vsutf8", "table\eucmsvsutf8"}

    Public Function custom_pasrse(bs As Byte(), mode As Integer) As String

        Dim sel As Integer = 0

        If mode = 512132004 Then
            sel = 1
        ElseIf mode = 951 Then
            sel = 2
        ElseIf mode = 21220932 Then
            sel = 3
        End If

        Dim bss(CInt(bs.Length - 1) * 2) As Byte

            If File.Exists(vstable(sel)) Then
                Dim tfs As New FileStream(vstable(sel), FileMode.Open, FileAccess.Read)
                Dim tbl(CInt(tfs.Length - 1)) As Byte
                tfs.Read(tbl, 0, tbl.Length)
                tfs.Close()
                Dim tofu As Byte() = {&HE2, &H96, &HA1}
                Dim maru As Byte() = {&HE3, &H82, &H9A}
                Dim ac As Byte() = {&HCC, &H80}
                Dim ac2 As Byte() = {&HCC, &H81}
                Dim gousei As Byte() = {&HCB, &HA9, &HCB, &HA5}
                Dim jis201 As Byte() = {0, 0, 0}
                Dim c1 As Integer = 0
                Dim c2 As Integer = 0
                Dim c3 As Integer = 0
                Dim ct As Integer = 0
                Dim i As Integer = 0
                Dim k As Integer = 0
            Dim pos As Integer = 0
            Dim len As Integer = bs.Length

                If mode = 2132004 Then
                While i < len
                    c1 = bs(i)
                    If c1 < 128 Then
                        '制御コード排除
                        If (c1 < 32 AndAlso c1 <> &H9 AndAlso c1 <> &HD AndAlso c1 <> &HA) Or c1 = &H7F Then
                            c1 = 32
                        End If
                        bss(k) = CByte(c1)
                        '改行CRLF化
                        If c1 = &HA Then
                            If k >= 0 Then
                                c2 = bss(k - 1)
                            Else
                                c2 = 0
                            End If
                            If c2 <> &HD Then
                                bss(k) = &HD
                                bss(k + 1) = CByte(c1)
                                k += 1
                            End If
                        End If
                        k += 1
                        i += 1
                    ElseIf ((c1 + &H5F) And &HFF) < &H3F Then
                        c1 = c1 + &HFF60 - &HA0
                        'uuuu zzzz yyyy xxxx
                        'E1..EF 1110uuuu
                        '80..BF 10zzzzyy
                        '80..BF 10yyxxxx
                        jis201(0) = CByte((c1 >> 12) Or &HE0)
                        jis201(1) = CByte(((c1 >> 6) And &H3F) Or &H80)
                        jis201(2) = CByte((c1 And &H3F) Or &H80)
                        Array.Copy(jis201, 0, bss, k, 3)
                        k += 3
                        i += 1
                    ElseIf ((((c1 Xor &H20) + &H5F) And &HFF) < &H3C) AndAlso i + 2 < len Then
                        c2 = bs(i + 1)
                        If c2 >= &H40 AndAlso c2 <= &HFC AndAlso c2 <> &H7F Then
                            pos = ((c1 Xor &H20) - &HA1) * 192 + c2 - &H40
                            If pos * 4 < tbl.Length Then
                                Array.Copy(tbl, pos * 4, bss, k, 4)
                                ct = bss(k)
                                If ct = 0 Then
                                    Array.Copy(tofu, 0, bss, k, 3)
                                    k += 3
                                Else
                                    If ct < &H80 Then
                                        k += 1
                                    ElseIf ct < &HC2 Then
                                    ElseIf ct < &HE0 Then
                                        k += 2
                                        If c1 = &H86 AndAlso (c2 And 1) = 1 AndAlso c2 >= &H67 AndAlso c2 <= &H6D Then
                                            Array.Copy(ac, 0, bss, k, 2)
                                            k += 2
                                        ElseIf c1 = &H86 AndAlso (c2 And 1) = 0 AndAlso c2 >= &H68 AndAlso c2 <= &H6E Then
                                            Array.Copy(ac2, 0, bss, k, 2)
                                            k += 2
                                        ElseIf c1 = &H86 AndAlso (c2 = &H85 Or c2 = &H86) Then
                                            Array.Copy(gousei, (c2 And 1) * 2, bss, k, 2)
                                            k += 2
                                        ElseIf c1 = &H86 AndAlso c2 = &H63 Then
                                            Array.Copy(ac, 0, bss, k, 2)
                                            k += 2
                                        End If
                                    ElseIf ct < &HF0 Then
                                        k += 3
                                        If c1 = &H82 AndAlso c2 >= &HF5 AndAlso c2 <= &HF9 Then
                                            Array.Copy(maru, 0, bss, k, 3)
                                            k += 3
                                        ElseIf c1 = &H83 AndAlso c2 >= &H97 AndAlso c2 <= &H9E Then
                                            Array.Copy(maru, 0, bss, k, 3)
                                            k += 3
                                        ElseIf c1 = &H83 AndAlso c2 = &HF6 Then
                                            Array.Copy(maru, 0, bss, k, 3)
                                            k += 3
                                        End If


                                    ElseIf ct < &HF8 Then
                                        k += 4
                                    End If
                                End If
                            End If
                        Else
                            Array.Copy(tofu, 0, bss, k, 3)
                            k += 3
                        End If
                        i += 2
                    Else
                        i += 1
                    End If

                End While

            ElseIf mode = 512132004 Or mode = 21220932 Then
                While i < len
                    c1 = bs(i)
                    If c1 < 128 Then
                        '制御コード排除
                        If (c1 < 32 AndAlso c1 <> &H9 AndAlso c1 <> &HD AndAlso c1 <> &HA) Or c1 = &H7F Then
                            c1 = 32
                        End If
                        bss(k) = CByte(c1)
                        '改行CRLF化
                        If c1 = &HA Then
                            If k >= 0 Then
                                c2 = bss(k - 1)
                            Else
                                c2 = 0
                            End If
                            If c2 <> &HD Then
                                bss(k) = &HD
                                bss(k + 1) = CByte(c1)
                                k += 1
                            End If
                        End If
                        k += 1
                        i += 1
                    ElseIf c1 = &H8E AndAlso i + 2 < len Then
                        c2 = bs(i + 1)
                        If ((c2 + &H5F) And &HFF) < &H3F Then
                            c2 = c2 + &HFF60 - &HA0
                            'uuuu zzzz yyyy xxxx
                            'E1..EF 1110uuuu
                            '80..BF 10zzzzyy
                            '80..BF 10yyxxxx
                            jis201(0) = CByte((c2 >> 12) Or &HE0)
                            jis201(1) = CByte(((c2 >> 6) And &H3F) Or &H80)
                            jis201(2) = CByte((c2 And &H3F) Or &H80)
                            Array.Copy(jis201, 0, bss, k, 3)
                            k += 3
                        End If
                        i += 2

                    ElseIf ((((c1 + &H5F) And &HFF) < &H5E) Or c1 = &H8F) AndAlso i + 2 < len Then
                        c2 = bs(i + 1)
                        c3 = 0

                        If c1 = &H8F AndAlso i + 3 < len Then
                            c3 = 1
                            c1 = c2
                            c2 = bs(i + 2)
                        End If

                        If c2 >= &HA1 AndAlso c2 <= &HFE Then
                            pos = (c3 * 94 * 94) + (c1 - &HA1) * 94 + c2 - &HA1
                            If pos * 4 < tbl.Length Then
                                Array.Copy(tbl, pos * 4, bss, k, 4)
                                ct = bss(k)
                                If ct = 0 Then
                                    Array.Copy(tofu, 0, bss, k, 3)
                                    k += 3
                                Else
                                    If ct < &H80 Then
                                        k += 1
                                    ElseIf ct < &HC2 Then
                                    ElseIf ct < &HE0 Then
                                        k += 2
                                        If c3 = 0 AndAlso mode = 512132004 Then
                                            If c1 = &HAB AndAlso (c2 And 1) = 0 AndAlso c2 >= &HC8 AndAlso c2 <= &HCF Then
                                                Array.Copy(ac, 0, bss, k, 2)
                                                k += 2
                                            ElseIf c1 = &HAB AndAlso (c2 And 1) = 1 AndAlso c2 >= &HC8 AndAlso c2 <= &HCF Then
                                                Array.Copy(ac2, 0, bss, k, 2)
                                                k += 2
                                            ElseIf c1 = &HAB AndAlso (c2 = &HE5 Or c2 = &HE6) Then
                                                Array.Copy(gousei, (c2 And 1) * 2, bss, k, 2)
                                                k += 2
                                            ElseIf c1 = &HAB AndAlso c2 = &HC4 Then
                                                Array.Copy(ac, 0, bss, k, 2)
                                                k += 2
                                            End If
                                        End If

                                    ElseIf ct < &HF0 Then
                                        k += 3
                                        If c3 = 0 AndAlso mode = 512132004 Then
                                            If c1 = &HA4 AndAlso c2 >= &HF7 AndAlso c2 <= &HFB Then
                                                Array.Copy(maru, 0, bss, k, 3)
                                                k += 3
                                            ElseIf c1 = &HA5 AndAlso c2 >= &HF7 AndAlso c2 <= &HFE Then
                                                Array.Copy(maru, 0, bss, k, 3)
                                                k += 3
                                            ElseIf c1 = &HA6 AndAlso c2 = &HF8 Then
                                                Array.Copy(maru, 0, bss, k, 3)
                                                k += 3
                                            End If
                                        End If

                                    ElseIf ct < &HF8 Then
                                        k += 4
                                    End If
                                End If
                            End If
                        Else
                            Array.Copy(tofu, 0, bss, k, 3)
                            k += 3
                        End If


                        If c3 = 1 Then
                            i += 3
                        Else
                            i += 2

                        End If

                    Else
                        i += 1
                    End If

                End While

                'gb5hkscs
                ElseIf mode = 951 Then
                While i < len
                    c1 = bs(i)
                    If c1 < 128 Then
                        '制御コード排除
                        If (c1 < 32 AndAlso c1 <> &H9 AndAlso c1 <> &HD AndAlso c1 <> &HA) Or c1 = &H7F Then
                            c1 = 32
                        End If
                        bss(k) = CByte(c1)
                        '改行CRLF化
                        If c1 = &HA Then
                            If k >= 0 Then
                                c2 = bss(k - 1)
                            Else
                                c2 = 0
                            End If
                            If c2 <> &HD Then
                                bss(k) = &HD
                                bss(k + 1) = CByte(c1)
                                k += 1
                            End If
                        End If
                        k += 1
                        i += 1
                        'BIG5/GBK
                    ElseIf (((c1 + &H7F) And &HFF) < &H7E) AndAlso i + 2 < len Then
                        c2 = bs(i + 1)
                        If c2 >= &H40 AndAlso c2 <= &HFE Then
                            pos = (c1 - &H81) * 192 + c2 - &H40
                            If pos * 4 < tbl.Length Then
                                Array.Copy(tbl, pos * 4, bss, k, 4)
                                ct = bss(k)
                                If ct = 0 Then
                                    Array.Copy(tofu, 0, bss, k, 3)
                                    k += 3
                                Else
                                    If ct < &H80 Then
                                        k += 1
                                    ElseIf ct < &HC2 Then
                                    ElseIf ct < &HE0 Then
                                        k += 2
                                    ElseIf ct < &HF0 Then
                                        k += 3
                                    ElseIf ct < &HF8 Then
                                        k += 4
                                    End If
                                End If
                            End If
                        Else
                            Array.Copy(tofu, 0, bss, k, 3)
                            k += 3
                        End If
                        i += 2
                    Else
                        i += 1
                    End If

                End While
                End If

                Array.Resize(bss, k)

                Return Encoding.GetEncoding(65001).GetString(bss)



            Else
                MessageBox.Show(vstable(sel) & "がありません,GOOGLECODEからダウンロードして下さい")

            End If


            Return ""

    End Function

    Dim maru As Integer() = {&H304B, &H304D, &H304F, &H3051, &H3053, &H30AB, &H30AD, &H30AF, &H30B1, &H30B3, &H30BB, &H30C4, &H30C8, &H31F7}

    Function gouseimaru(ByVal pos As Integer) As Integer
        For i = 0 To 13
            If pos = maru(i) Then
                Return i
            End If
        Next
        Return -1
    End Function

    Dim maru2 As Integer() = {&H254, &H28C, &H259, &H25A}

    Function gouseiac(ByVal pos As Integer) As Integer
        For i = 0 To 3
            If pos = maru2(i) Then
                Return i
            End If
        Next
        Return -1
    End Function

    Dim hmaru As Integer() = {&HF582, &H9783, &HF683}
    Dim ha As Integer = &H6386
    Dim hac As Integer = &H6786
    Dim koe As Integer = &H8586
    Dim hmaru_euc As Integer() = {&HF7A4, &HF7A5, &HF8A6}
    Dim ha_euc As Integer = &HC4AB
    Dim hac_euc As Integer = &HC8AB
    Dim koe_euc As Integer = &HE5AB

    Dim euctofu As Byte() = {&HA2, &HA2}
    Dim bigtofu As Byte() = {&HA1, &HBC}
    Dim gbktofu As Byte() = {&HA1, &H5}
    Dim sjistofu As Byte() = {&H8A, &HA0}

    Public Function unicode2custom(str As String, tbl As Byte(), mode As Integer) As Byte()

        Dim bs As Byte() = Encoding.GetEncoding(1200).GetBytes(str)
        Dim bss(CInt(bs.Length + 100)) As Byte

            Dim code As Byte()

            Dim jis201 As Byte() = {0, 0, 0}
            Dim c1 As Integer = 0
            Dim c2 As Integer = 0
            Dim c3 As Integer = 0
            Dim ct As Integer = 0
            Dim i As Integer = 0
            Dim k As Integer = 0
            Dim l As Integer = 0
            Dim m As Integer = 0
            Dim hex As UInt16 = 0
            Dim hex2 As UInt16 = 0
            Dim pos As Int32 = 0
            'SJIS
        If mode = 0 Then

            While i < bs.Length
                hex = BitConverter.ToUInt16(bs, i)
                If hex >= &HD800 AndAlso hex <= &HDBFF Then
                    i += 2
                    hex2 = BitConverter.ToUInt16(bs, i)
                    If hex2 >= &HDC00 AndAlso hex2 <= &HDFFF Then
                        pos = (hex And &H3FF) * 1024 + (hex2 And &H3FF)
                        pos += &H10000
                    Else
                        pos = tbl.Length
                    End If
                Else
                    pos = Convert.ToInt32(hex)
                End If
                If pos * 4 < tbl.Length Then
                    Array.Copy(tbl, pos * 4, bss, k, 4)
                    c1 = bss(k)
                    c2 = bss(k + 1)
                    c3 = bss(k + 2)
                    If i + 4 <= bs.Length Then
                        hex2 = BitConverter.ToUInt16(bs, i + 2)
                    Else
                        hex2 = 0
                    End If

                    If hex2 = &H309A AndAlso gouseimaru(pos) >= 0 Then
                        m = gouseimaru(pos)
                        l = 0
                        If m > 12 Then
                            l = 2
                            m = 0
                        ElseIf m > 4 Then
                            l = 1
                            m -= 5
                        End If
                        code = BitConverter.GetBytes(hmaru(l) + &H100 * m)
                        Array.Copy(code, 0, bss, k, 4)
                        k += 2
                        i += 2

                    ElseIf hex2 = &H300 AndAlso pos = &HE6 Then
                        code = BitConverter.GetBytes(ha)
                        Array.Copy(code, 0, bss, k, 4)
                        k += 2
                        i += 2

                    ElseIf hex2 = &H300 AndAlso gouseiac(pos) >= 0 Then
                        code = BitConverter.GetBytes(hac + (2 * gouseiac(pos) * &H100))
                        Array.Copy(code, 0, bss, k, 4)
                        k += 2
                        i += 2

                    ElseIf hex2 = &H301 AndAlso gouseiac(pos) >= 0 Then
                        code = BitConverter.GetBytes(hac + ((2 * gouseiac(pos) + 1) * &H100))
                        Array.Copy(code, 0, bss, k, 4)
                        k += 2
                        i += 2

                    ElseIf hex2 = &H2E9 AndAlso pos = &H2E5 Then
                        code = BitConverter.GetBytes(koe + &H100)
                        Array.Copy(code, 0, bss, k, 4)
                        k += 2
                        i += 2

                    ElseIf hex2 = &H2E5 AndAlso pos = &H2E9 Then
                        code = BitConverter.GetBytes(koe)
                        Array.Copy(code, 0, bss, k, 4)
                        k += 2
                        i += 2
                    Else
                        If pos < 128 Then
                            If pos < 32 AndAlso pos <> &HA AndAlso pos <> &HD AndAlso pos <> &H9 Then
                                pos = 32
                            End If
                            bss(k) = CByte(pos)
                            k += 1
                        ElseIf c1 = 0 AndAlso c2 = 2 Then
                            Array.Copy(sjistofu, 0, bss, k, 2)
                            k += 2
                            'skip
                        ElseIf c2 = 0 AndAlso c1 >= &HA1 AndAlso c1 <= &HDF Then
                            k += 1
                        ElseIf (((c1 Xor &H20) + &H5F) And &HFF) < &H3C AndAlso c2 >= &H40 Then
                            k += 2
                        End If
                    End If
                End If
                i += 2
            End While

            'EUC
        ElseIf mode = 1 Or mode = 3 Then
            While i < bs.Length
                hex = BitConverter.ToUInt16(bs, i)
                If hex >= &HD800 AndAlso hex <= &HDBFF Then
                    i += 2
                    hex2 = BitConverter.ToUInt16(bs, i)
                    If hex2 >= &HDC00 AndAlso hex2 <= &HDFFF Then
                        pos = (hex And &H3FF) * 1024 + (hex2 And &H3FF)
                        pos += &H10000
                    Else
                        pos = tbl.Length
                    End If
                Else
                    pos = Convert.ToInt32(hex)
                End If
                If pos * 4 < tbl.Length Then
                    Array.Copy(tbl, pos * 4, bss, k, 4)
                    c1 = bss(k)
                    c2 = bss(k + 1)
                    c3 = bss(k + 2)
                    
                    If mode = 1 AndAlso i + 4 <= bs.Length Then
                        hex2 = BitConverter.ToUInt16(bs, i + 2)
                    Else
                        hex2 = 0
                    End If


                    If hex2 = &H309A AndAlso gouseimaru(pos) >= 0 Then
                        m = gouseimaru(pos)
                        l = 0
                        If m > 12 Then
                            l = 2
                            m = 0
                        ElseIf m > 4 Then
                            l = 1
                            m -= 5
                        End If
                        code = BitConverter.GetBytes(hmaru_euc(l) + &H100 * m)
                        Array.Copy(code, 0, bss, k, 4)
                        k += 2
                        i += 2

                    ElseIf hex2 = &H300 AndAlso pos = &HE6 Then
                        code = BitConverter.GetBytes(ha_euc)
                        Array.Copy(code, 0, bss, k, 4)
                        k += 2
                        i += 2

                    ElseIf hex2 = &H300 AndAlso gouseiac(pos) >= 0 Then
                        code = BitConverter.GetBytes(hac_euc + ((2 * gouseiac(pos)) * &H100))
                        Array.Copy(code, 0, bss, k, 4)
                        k += 2
                        i += 2

                    ElseIf hex2 = &H301 AndAlso gouseiac(pos) >= 0 Then
                        code = BitConverter.GetBytes(hac_euc + ((2 * gouseiac(pos) + 1) * &H100))
                        Array.Copy(code, 0, bss, k, 4)
                        k += 2
                        i += 2

                    ElseIf hex2 = &H2E9 AndAlso pos = &H2E5 Then
                        code = BitConverter.GetBytes(koe_euc + &H100)
                        Array.Copy(code, 0, bss, k, 4)
                        k += 2
                        i += 2

                    ElseIf hex2 = &H2E5 AndAlso pos = &H2E9 Then
                        code = BitConverter.GetBytes(koe_euc)
                        Array.Copy(code, 0, bss, k, 4)
                        k += 2
                        i += 2
                    Else
                        If pos < 128 Then
                            If pos < 32 AndAlso pos <> &HA AndAlso pos <> &HD AndAlso pos <> &H9 Then
                                pos = 32
                            End If
                            bss(k) = CByte(pos)
                            k += 1
                        ElseIf c1 = 0 AndAlso c2 = 0 Then
                            Array.Copy(euctofu, 0, bss, k, 2)
                            k += 2
                            'skip
                        ElseIf c1 = &H8E Then
                            k += 2
                        ElseIf c1 = &H8F Then
                            k += 3
                        ElseIf ((c1 + &H5F) And &HFF) < &H5E AndAlso c2 >= &HA1 Then
                            k += 2
                        End If
                    End If
                End If
                i += 2
            End While

        ElseIf mode = 2 Then
            While i < bs.Length
                hex = BitConverter.ToUInt16(bs, i)
                If hex >= &HD800 AndAlso hex <= &HDBFF Then
                    i += 2
                    hex2 = BitConverter.ToUInt16(bs, i)
                    If hex2 >= &HDC00 AndAlso hex2 <= &HDFFF Then
                        pos = (hex And &H3FF) * 1024 + (hex2 And &H3FF)
                        pos += &H10000
                    Else
                        pos = tbl.Length
                    End If
                Else
                    pos = Convert.ToInt32(hex)
                End If
                If pos * 4 < tbl.Length Then
                    Array.Copy(tbl, pos * 4, bss, k, 4)
                    c1 = bss(k)
                    c2 = bss(k + 1)
                    c3 = bss(k + 2)

                    If pos < 128 Then
                        If pos < 32 AndAlso pos <> &HA AndAlso pos <> &HD AndAlso pos <> &H9 Then
                            pos = 32
                        End If
                        bss(k) = CByte(pos)
                        k += 1
                    ElseIf c1 = 0 AndAlso c2 = 0 Then
                        Array.Copy(bigtofu, 0, bss, k, 2)
                        k += 2
                        'skip
                    ElseIf ((c1 + &H7F) And &HFF) < &H7E AndAlso c2 >= &H40 Then
                        k += 2
                    End If
                End If
                i += 2
            End While
        End If


            Array.Resize(bss, k)
            Return bss

    End Function

End Class
