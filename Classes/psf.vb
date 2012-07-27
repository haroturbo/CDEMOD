Imports System.IO
Imports System.Text
Imports System.Security.Cryptography

Public Class psf


    Public Function video(ByVal filename As String) As String

        Dim fs As New FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read)
        Dim bs(2047) As Byte
        Dim result As String = ""
        Dim MODE As String = ""

        If fs.Length > 2048 + 40 Then
            fs.Read(bs, 0, 2048)
            'PBP
            If bs(0) = &H0 AndAlso bs(1) = &H50 AndAlso bs(2) = &H42 AndAlso bs(3) = &H50 Then
                fs.Close()
                Return "PBP"
            ElseIf bs(0) = &H43 AndAlso bs(1) = &H49 AndAlso bs(2) = &H53 AndAlso bs(3) = &H4F Then
                fs.Close()
                Return "CISO"
            End If
        End If


        If fs.Length > &H8060 Then
            fs.Seek(&H8000, SeekOrigin.Begin)
            fs.Read(bs, 0, 2048)
            '.CD001
            If bs(0) = &H1 AndAlso bs(1) = &H43 AndAlso bs(2) = &H44 AndAlso bs(3) = &H30 _
                AndAlso bs(4) = &H30 AndAlso bs(5) = &H31 Then
                'PSP GAME
                If bs(8) = &H50 AndAlso bs(9) = &H53 AndAlso bs(10) = &H50 AndAlso bs(11) = &H20 _
                    AndAlso bs(12) = &H47 AndAlso bs(13) = &H41 AndAlso bs(14) = &H4D AndAlso bs(15) = &H45 Then

                    fs.Close()
                    Return "PSP"

                ElseIf bs(8) = &H55 AndAlso bs(9) = &H4D AndAlso bs(10) = &H44 AndAlso bs(11) = &H20 _
                        AndAlso bs(12) = &H56 AndAlso bs(13) = &H49 AndAlso bs(14) = &H44 AndAlso bs(15) = &H45 AndAlso bs(16) = &H4F Then

                    fs.Close()
                    Return "VIDEO"
                End If
            End If
        End If

        fs.Close()
        Return ""

    End Function

    Public Function GETID(ByVal filename As String) As String

        Dim fs As New FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read)
        Dim bs(2047) As Byte
        Dim result As String = ""
        Dim m As MERGE = MERGE
        Dim NOID As Boolean = False

        If fs.Length > 2048 + 40 Then
            fs.Read(bs, 0, 2048)
            'PBP
            If bs(0) = &H0 AndAlso bs(1) = &H50 AndAlso bs(2) = &H42 AndAlso bs(3) = &H50 Then

                fs.Seek(40, SeekOrigin.Begin)
                fs.Read(bs, 0, 2048)

                'PSF
                If bs(0) = &H0 AndAlso bs(1) = &H50 AndAlso bs(2) = &H53 AndAlso bs(3) = &H46 Then

                    Dim offset(3) As Byte
                    Array.ConstrainedCopy(bs, 8, offset, 0, 4)
                    Dim i As Integer = BitConverter.ToInt32(offset, 0)
                    Array.ConstrainedCopy(bs, 12, offset, 0, 4)
                    Dim k As Integer = BitConverter.ToInt32(offset, 0)
                    Array.ConstrainedCopy(bs, 16, offset, 0, 4)
                    Dim z As Integer = BitConverter.ToInt32(offset, 0)
                    Dim psfbyte(200) As Byte
                    Array.ConstrainedCopy(bs, i, psfbyte, 0, 200)
                    Dim pname As String = Encoding.GetEncoding(65001).GetString(psfbyte)
                    pname = pname.Substring(0, pname.IndexOf(Chr(0) & Chr(0)))
                    Dim psfst As String() = pname.Split(Chr(0))
                    For i = 0 To z - 1
                        If psfst(i) = "DISC_ID" Then
                            Exit For
                        End If
                    Next
                    If i = z Then
                        NOID = True
                    End If

                    Dim gidpsf(8) As Byte
                    k += CInt(bs(32 + 16 * i))
                    Array.ConstrainedCopy(bs, k, gidpsf, 0, 9)
                        result = Encoding.GetEncoding(0).GetString(gidpsf)

                    If result = "UCJS10041" Or NOID = True Or My.Settings.hbhash = True Then
                        fs.Seek(0, SeekOrigin.Begin)
                        fs.Read(bs, 0, 2048)
                        Dim md5 As MD5 = md5.Create()
                        'ハッシュ値を計算する 
                        Dim b As Byte() = md5.ComputeHash(bs)

                        'ファイルを閉じる 
                        fs.Close()

                        Dim hex(4) As UInteger
                        Dim code(3) As Byte
                        For i = 0 To 3
                            Array.ConstrainedCopy(b, i << 2, code, 0, 4)
                            hex(i) = BitConverter.ToUInt32(code, 0)
                        Next
                        hex(4) = hex(0) Xor hex(1)
                        hex(4) = hex(4) Xor hex(2)
                        hex(4) = hex(4) Xor hex(3)

                        result = "HB" & hex(4).ToString("X")

                        Return result
                    Else
                        If m.PSX = True Then
                            result = result.Insert(4, "_")
                        Else
                            result = result.Insert(4, "-")
                        End If
                        fs.Close()
                        Return result
                    End If
                End If
                'DAX
            ElseIf bs(0) = &H44 AndAlso bs(1) = &H41 AndAlso bs(2) = &H58 AndAlso bs(3) = &H0 Then
                result = "DAX"
                'CISO
            ElseIf bs(0) = &H43 AndAlso bs(1) = &H49 AndAlso bs(2) = &H53 AndAlso bs(3) = &H4F Then

                Dim offset(7) As Byte
                Dim source(2047) As Byte
                Dim seek As Integer = 0
                Array.ConstrainedCopy(bs, 8, offset, 0, 4)
                Dim size As Long = BitConverter.ToInt64(offset, 0)
                Array.ConstrainedCopy(bs, 16, offset, 0, 4)
                Dim sec As Integer = BitConverter.ToInt32(offset, 0) >> 8
                Array.ConstrainedCopy(bs, 20, offset, 0, 4)
                Dim align As Integer = BitConverter.ToInt32(offset, 0) >> 8
                Dim counter As Integer = CInt(size / sec)

                fs.Seek(24 + 4 * 16, System.IO.SeekOrigin.Begin)
                fs.Read(offset, 0, 4)
                seek = BitConverter.ToInt32(offset, 0) << align

                fs.Seek(seek, System.IO.SeekOrigin.Begin)
                fs.Read(source, 0, 2048)
                If (seek And &H8000000) <> 0 Then
                Else
                    Dim ms As New MemoryStream()
                    ms.Write(source, 0, 2048)
                    ms.Position = 0
                    Dim zipStream As New System.IO.Compression.DeflateStream(ms, System.IO.Compression.CompressionMode.Decompress)
                    zipStream.Read(source, 0, 2048)
                    zipStream.Close()
                    ms.Close()
                End If

                Dim gid(9) As Byte

                Array.ConstrainedCopy(source, &H373, gid, 0, 10)
                result = Encoding.GetEncoding(0).GetString(gid)

                fs.Close()
                Return result
                'result = "CSO"
                'JISO
            ElseIf bs(0) = &H4A AndAlso bs(1) = &H49 AndAlso bs(2) = &H53 AndAlso bs(3) = &H4F Then
                result = "JSO"
            End If

            If fs.Length > &H8060 AndAlso m.PSX = False AndAlso result = "" Then
                fs.Seek(&H8000, SeekOrigin.Begin)
                fs.Read(bs, 0, 2048)
                '.CD001
                If bs(0) = &H1 AndAlso bs(1) = &H43 AndAlso bs(2) = &H44 AndAlso bs(3) = &H30 _
                    AndAlso bs(4) = &H30 AndAlso bs(5) = &H31 Then
                    'PSP GAME
                    If bs(8) = &H50 AndAlso bs(9) = &H53 AndAlso bs(10) = &H50 AndAlso bs(11) = &H20 _
                        AndAlso bs(12) = &H47 AndAlso bs(13) = &H41 AndAlso bs(14) = &H4D AndAlso bs(15) = &H45 Then

                        Dim gid(9) As Byte

                        Array.ConstrainedCopy(bs, &H373, gid, 0, 10)
                        result = Encoding.GetEncoding(0).GetString(gid)

                        fs.Close()
                        Return result

                    ElseIf bs(8) = &H55 AndAlso bs(9) = &H4D AndAlso bs(10) = &H44 AndAlso bs(11) = &H20 _
                            AndAlso bs(12) = &H56 AndAlso bs(13) = &H49 AndAlso bs(14) = &H44 AndAlso bs(15) = &H45 AndAlso bs(16) = &H4F Then
                        fs.Close()
                        Return "UMDVIDEO"
                    End If
                End If
            End If
        End If

        fs.Close()
        Return result

    End Function


    Public Function GETNAME(ByVal filename As String) As String

        Dim fs As New FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read)
        Dim bs(2047) As Byte
        Dim result As String = ""
        Dim m As MERGE = MERGE

        If fs.Length > 2048 + 40 Then

            fs.Read(bs, 0, 2048)
            'PBP
            If bs(0) = &H0 AndAlso bs(1) = &H50 AndAlso bs(2) = &H42 AndAlso bs(3) = &H50 Then

                fs.Seek(40, SeekOrigin.Begin)
                fs.Read(bs, 0, 2048)

                'PSF
                If bs(0) = &H0 AndAlso bs(1) = &H50 AndAlso bs(2) = &H53 AndAlso bs(3) = &H46 Then
                    Dim offset(3) As Byte
                    Array.ConstrainedCopy(bs, 8, offset, 0, 4)
                    Dim i As Integer = BitConverter.ToInt32(offset, 0)
                    Array.ConstrainedCopy(bs, 12, offset, 0, 4)
                    Dim k As Integer = BitConverter.ToInt32(offset, 0)
                    Array.ConstrainedCopy(bs, 16, offset, 0, 4)
                    Dim z As Integer = BitConverter.ToInt32(offset, 0)
                    Dim psfbyte(200) As Byte
                    Array.ConstrainedCopy(bs, i, psfbyte, 0, 200)
                    Dim pname As String = Encoding.GetEncoding(65001).GetString(psfbyte)
                    pname = pname.Substring(0, pname.IndexOf(Chr(0) & Chr(0)))
                    Dim psfst As String() = pname.Split(Chr(0))
                    i=0
                    while true
                        If psfst(i) = "TITLE" Then
                            Exit While
                        End If
                    If i = z Then
                        fs.Close()
                        Return "NOTITLE"
                    End If
                    i+=1
                    end while

                    Array.ConstrainedCopy(bs, 32 + i * 16, offset, 0, 4)
                    i = BitConverter.ToInt32(offset, 0)
                    k += i
                    Dim name(128) As Byte
                    Array.ConstrainedCopy(bs, k, name, 0, 128)
                    result = Encoding.GetEncoding(65001).GetString(name)
                    i = result.IndexOf(vbNullChar)
                    result = result.Substring(0, i)
                    If result = "" Then
                        result = "NULL"
                    End If
                    fs.Close()
                    Return result
                Else
                    fs.Close()
                    Return ""
                End If
                'DAX
            ElseIf bs(0) = &H44 AndAlso bs(1) = &H41 AndAlso bs(2) = &H58 AndAlso bs(3) = &H0 Then
                result = "DAX"
                'CISO
            ElseIf bs(0) = &H43 AndAlso bs(1) = &H49 AndAlso bs(2) = &H53 AndAlso bs(3) = &H4F Then

                Dim offset(7) As Byte
                Dim source(2047) As Byte
                Dim seek As Long = 0
                Dim i As Integer = 0

                Array.ConstrainedCopy(bs, 8, offset, 0, 4)
                Dim size As Long = BitConverter.ToInt64(offset, 0)
                Array.ConstrainedCopy(bs, 16, offset, 0, 4)
                Dim sec As Integer = BitConverter.ToInt32(offset, 0)
                Array.ConstrainedCopy(bs, 20, offset, 0, 4)
                Dim align As Integer = BitConverter.ToInt32(offset, 0) >> 8

                fs.Seek(24 + 4 * 16, System.IO.SeekOrigin.Begin)
                fs.Read(offset, 0, 4)
                seek = BitConverter.ToInt64(offset, 0) << align

                fs.Seek(seek, System.IO.SeekOrigin.Begin)
                fs.Read(source, 0, 2048)
                If (seek And &H8000000) <> 0 Then
                Else
                    Dim ms As New MemoryStream()
                    ms.Write(source, 0, 2048)
                    ms.Position = 0
                    Dim zipStream As New System.IO.Compression.DeflateStream(ms, System.IO.Compression.CompressionMode.Decompress)
                    zipStream.Read(source, 0, 2048)
                    zipStream.Close()
                    ms.Close()
                End If

                Dim str(8) As Byte
                Array.ConstrainedCopy(source, &H8, str, 0, 9)
                Dim iso As String = Encoding.GetEncoding(0).GetString(str)

                Array.ConstrainedCopy(source, &H50, offset, 0, 5)
                seek = BitConverter.ToInt64(offset, 0)

                fs.Seek(24 + 4 * 16, System.IO.SeekOrigin.Begin)
                fs.Read(offset, 0, 4)
                seek = BitConverter.ToInt64(offset, 0) << align

                fs.Seek(seek, System.IO.SeekOrigin.Begin)
                fs.Read(source, 0, 2048)

                If (seek And &H8000000) <> 0 Then
                Else
                    Dim ms As New MemoryStream()
                    ms.Write(source, 0, 2048)
                    ms.Position = 0
                    Dim zipStream As New System.IO.Compression.DeflateStream(ms, System.IO.Compression.CompressionMode.Decompress)
                    zipStream.Read(source, 0, 2048)
                    zipStream.Close()
                    ms.Close()
                End If

                If (iso.Contains("PSP GAME")) Then 'Or iso.Contains("UMD VIDEO")) Then

                    'PSP_GAME,UMD_VIDEO,LPATHTABLE
                    Array.ConstrainedCopy(source, &H8C, offset, 0, 4)
                    seek = BitConverter.ToInt32(offset, 0)
                    If seek * 2048 > size Then
                        fs.Close()
                        Return ""
                    End If

                    fs.Seek(24 + 4 * seek, System.IO.SeekOrigin.Begin)
                    fs.Read(offset, 0, 4)
                    seek = BitConverter.ToInt64(offset, 0) << align

                    fs.Seek(seek, System.IO.SeekOrigin.Begin)
                    fs.Read(source, 0, 2048)

                    If (seek And &H8000000) <> 0 Then
                    Else
                        Dim ms As New MemoryStream()
                        ms.Write(source, 0, 2048)
                        ms.Position = 0
                        Dim zipStream As New System.IO.Compression.DeflateStream(ms, System.IO.Compression.CompressionMode.Decompress)
                        zipStream.Read(source, 0, 2048)
                        zipStream.Close()
                        ms.Close()
                    End If

                    i = 6
                    While True
                        'PSP_GAME
                        If iso.Contains("PSP GAME") = True AndAlso source(i) = &H50 AndAlso source(i + 1) = &H53 AndAlso source(i + 2) = &H50 Then
                            Exit While
                            'ElseIf iso.Contains("UMD VIDEO") = True AndAlso source(i) = &H55 AndAlso source(i + 1) = &H4D AndAlso source(i + 2) = &H44 AndAlso source(i + 3) = &H5F AndAlso source(i + 4) = &H56 Then
                            '    Exit While
                        ElseIf i > 2038 Then
                            fs.Close()
                            Return ""
                        End If
                        i += 1
                    End While

                    Array.ConstrainedCopy(source, i - 6, offset, 0, 4)
                    seek = BitConverter.ToInt32(offset, 0)
                    If seek * 2048 > size Then
                        fs.Close()
                        Return ""
                    End If

                    fs.Seek(24 + 4 * seek, System.IO.SeekOrigin.Begin)
                    fs.Read(offset, 0, 4)
                    seek = BitConverter.ToInt64(offset, 0) << align

                    fs.Seek(seek, System.IO.SeekOrigin.Begin)
                    fs.Read(source, 0, 2048)

                    If (seek And &H8000000) <> 0 Then
                    Else
                        Dim ms As New MemoryStream()
                        ms.Write(source, 0, 2048)
                        ms.Position = 0
                        Dim zipStream As New System.IO.Compression.DeflateStream(ms, System.IO.Compression.CompressionMode.Decompress)
                        zipStream.Read(source, 0, 2048)
                        zipStream.Close()
                        ms.Close()
                    End If

                    i = 31
                    'PARAM.SFO
                    While True
                        If source(i) = &H50 AndAlso source(i + 1) = &H41 AndAlso source(i + 2) = &H52 AndAlso source(i + 3) = &H41 Then
                            Exit While
                        ElseIf i > 2038 Then
                            fs.Close()
                            Return ""
                        End If
                        i += 1
                    End While
                    Array.ConstrainedCopy(source, i - 31, offset, 0, 4)
                    seek = BitConverter.ToInt64(offset, 0)
                    If seek * 2048 > size Then
                        fs.Close()
                        Return ""
                    End If

                    fs.Seek(24 + 4 * seek, System.IO.SeekOrigin.Begin)
                    fs.Read(offset, 0, 4)
                    seek = BitConverter.ToInt64(offset, 0) << align

                    fs.Seek(seek, System.IO.SeekOrigin.Begin)
                    fs.Read(source, 0, 2048)

                    If (seek And &H8000000) <> 0 Then
                    Else
                        Dim ms As New MemoryStream()
                        ms.Write(source, 0, 2048)
                        ms.Position = 0
                        Dim zipStream As New System.IO.Compression.DeflateStream(ms, System.IO.Compression.CompressionMode.Decompress)
                        zipStream.Read(source, 0, 2048)
                        zipStream.Close()
                        ms.Close()
                    End If

                    Array.ConstrainedCopy(source, 0, bs, 0, 2048)

                    'PSF
                    If bs(0) = &H0 AndAlso bs(1) = &H50 AndAlso bs(2) = &H53 AndAlso bs(3) = &H46 Then
                        Array.ConstrainedCopy(bs, 8, offset, 0, 4)
                        i = BitConverter.ToInt32(offset, 0)
                        Array.ConstrainedCopy(bs, 12, offset, 0, 4)
                        Dim k As Integer = BitConverter.ToInt32(offset, 0)
                        Array.ConstrainedCopy(bs, 16, offset, 0, 4)
                        Dim z As Integer = BitConverter.ToInt32(offset, 0)
                        Dim psfbyte(200) As Byte
                        Array.ConstrainedCopy(bs, i, psfbyte, 0, 200)
                        Dim pname As String = Encoding.GetEncoding(65001).GetString(psfbyte)
                        pname = pname.Substring(0, pname.IndexOf(Chr(0) & Chr(0)))
                        Dim psfst As String() = pname.Split(Chr(0))
                        i = 0
                        While True
                            If psfst(i) = "TITLE" Then
                                Exit While
                            End If
                            If i = z - 1 Then
                                fs.Close()
                                Return ""
                            End If
                            i += 1
                        End While

                        Array.ConstrainedCopy(bs, 32 + i * 16, offset, 0, 4)
                        i = BitConverter.ToInt32(offset, 0)
                        k += i

                        Dim name(128) As Byte
                        Array.ConstrainedCopy(bs, k, name, 0, 128)
                        result = Encoding.GetEncoding(65001).GetString(name)
                        i = result.IndexOf(vbNullChar)
                        result = result.Substring(0, i)


                        fs.Close()
                        Return result
                    End If
                End If
                'result = "CSO"
                'JISO
            ElseIf bs(0) = &H4A AndAlso bs(1) = &H49 AndAlso bs(2) = &H53 AndAlso bs(3) = &H4F Then
                result = "JSO"
            End If

            If fs.Length > &H8060 AndAlso m.PSX = False AndAlso result = "" Then
                fs.Seek(&H8000, SeekOrigin.Begin)
                fs.Read(bs, 0, 2048)
                '.CD001
                If bs(0) = &H1 AndAlso bs(1) = &H43 AndAlso bs(2) = &H44 AndAlso bs(3) = &H30 _
                    AndAlso bs(4) = &H30 AndAlso bs(5) = &H31 Then
                    'PSP GAME
                    If bs(8) = &H50 AndAlso bs(9) = &H53 AndAlso bs(10) = &H50 AndAlso bs(11) = &H20 _
                        AndAlso bs(12) = &H47 AndAlso bs(13) = &H41 AndAlso bs(14) = &H4D AndAlso bs(15) = &H45 Then

                        Dim lba(3) As Byte
                        Dim i As Integer = 0
                        Dim k As Integer = 0
                        Dim z As Integer = 0
                        Dim size(7) As Byte

                        fs.Seek(&H8050, SeekOrigin.Begin)
                        fs.Read(size, 0, 5)
                        Dim lbatotal As Int64 = BitConverter.ToInt64(size, 0)
                        lbatotal *= 2048
                        If lbatotal - fs.Length <= 2048 Then

                    'PSP_GAME,UMD_VIDEO,LPATHTABLE
                    'http://euc.jp/periphs/iso9660.ja.html#preface
                            fs.Seek(&H808C, SeekOrigin.Begin) '0x809E rootdir
                            fs.Read(lba, 0, 2)
                            z = BitConverter.ToInt32(lba, 0)
                            If z * 2048 > fs.Length Then
                                fs.Close()
                                Return ""
                            End If
                            fs.Seek(z * 2048, SeekOrigin.Begin)
                            fs.Read(bs, 0, 2048)
                            i=6
                            'PSP_GAME
                            While True
                                If bs(i) = &H50 AndAlso bs(i + 1) = &H53 AndAlso bs(i + 2) = &H50 Then
                                    Exit While
                                ElseIf i > 2038 Then
                                    fs.Close()
                                    Return ""
                                End If
                                i += 1
                            End While
                            'Array.ConstrainedCopy(bs, i - 31, lba, 0, 2) 'rootdir for 0x809E
                            Array.ConstrainedCopy(bs, i - 6, lba, 0, 2)
                            z = BitConverter.ToInt32(lba, 0)
                            If z * 2048 > fs.Length Then
                                fs.Close()
                                Return ""
                            End If
                            fs.Seek(z * 2048, SeekOrigin.Begin)
                            fs.Read(bs, 0, 2048)
                            i = 31
                            'PARAM.SFO
                            While True
                                If bs(i) = &H50 AndAlso bs(i + 1) = &H41 AndAlso bs(i + 2) = &H52 AndAlso bs(i + 3) = &H41 Then
                                    Exit While
                                ElseIf i > 2038 Then
                                    fs.Close()
                                    Return ""
                                End If
                                i += 1
                            End While
                            Array.ConstrainedCopy(bs, i - 31, lba, 0, 3)
                            z = BitConverter.ToInt32(lba, 0)
                            If z * 2048 > fs.Length Then
                                fs.Close()
                                Return ""
                            End If
                            fs.Seek(z * 2048, SeekOrigin.Begin)
                            fs.Read(bs, 0, 2048)
                            'PSF
                            If bs(0) = &H0 AndAlso bs(1) = &H50 AndAlso bs(2) = &H53 AndAlso bs(3) = &H46 Then
                                Dim offset(3) As Byte
                                Array.ConstrainedCopy(bs, 8, offset, 0, 4)
                                i = BitConverter.ToInt32(offset, 0)
                                Array.ConstrainedCopy(bs, 12, offset, 0, 4)
                                k = BitConverter.ToInt32(offset, 0)
                                Array.ConstrainedCopy(bs, 16, offset, 0, 4)
                                z = BitConverter.ToInt32(offset, 0)
                                Dim psfbyte(200) As Byte
                                Array.ConstrainedCopy(bs, i, psfbyte, 0, 200)
                                Dim pname As String = Encoding.GetEncoding(65001).GetString(psfbyte)
                                pname = pname.Substring(0, pname.IndexOf(Chr(0) & Chr(0)))
                                Dim psfst As String() = pname.Split(Chr(0))
                                i=0
                                while True
                                    If psfst(i) = "TITLE" Then
                                        Exit While
                                    End If
                                If i = z Then
                                    fs.Close()
                                    Return "NOTITLE"
                                End If
                                    i+=1
                                end while

                                Array.ConstrainedCopy(bs, 32 + i * 16, offset, 0, 4)
                                i = BitConverter.ToInt32(offset, 0)
                                k += i

                                Dim name(128) As Byte
                                Array.ConstrainedCopy(bs, k, name, 0, 128)
                                result = Encoding.GetEncoding(65001).GetString(name)
                                i = result.IndexOf(vbNullChar)
                                result = result.Substring(0, i)

                                If result = "" Then
                                    result = "NULL"
                                End If

                                fs.Close()
                                Return result
                            Else
                                fs.Close()
                                Return ""
                            End If
                        End If

                    ElseIf bs(8) = &H55 AndAlso bs(9) = &H4D AndAlso bs(10) = &H44 AndAlso bs(11) = &H20 _
                            AndAlso bs(12) = &H56 AndAlso bs(13) = &H49 AndAlso bs(14) = &H44 AndAlso bs(15) = &H45 AndAlso bs(16) = &H4F Then
                        fs.Close()
                        Return "UMDVIDEO"
                    End If
                End If
            End If
        End If

        fs.Close()
        Return result

    End Function

End Class
