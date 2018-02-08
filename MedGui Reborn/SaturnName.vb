﻿Imports System.IO
Imports DiscTools

Module SaturnName
    Public SnSaturn, clearregionsaturn, r_ss, v_ss, cdn As String
    Dim path_ss, n_ss As String

    Public Function cleansaturn(ByVal cleanstring As String) As String
        Dim i1, i2, i3 As Integer

        i3 = cleanstring.IndexOf("-1/")
        If i3 < 0 Then i3 = cleanstring.IndexOf("-1\")
        clearregionsaturn = Mid(cleanstring, i3 + 5, 10).Trim
        i1 = cleanstring.IndexOf("V") - 10
        cleanstring = cleanstring.Remove(0, i1).Trim
        i2 = cleanstring.IndexOf(" ")
        cleanstring = cleanstring.Remove(i2, Len(cleanstring) - i2).Trim
        cleansaturn = cleanstring
    End Function

    Public Sub ReadIsoSaturn()
        Try
            DetectIsoSaturn()
            Dim buffer As String
            Using reader As New StreamReader(path_ss)
                reader.BaseStream.Seek(0, SeekOrigin.Begin)
                buffer = reader.ReadLine
            End Using
            SnSaturn = (cleansaturn(buffer))
            If SnSaturn.Length > 11 Then
                Dim i1 As String
                i1 = SnSaturn.IndexOf("V")
                SnSaturn = SnSaturn.Remove(i1, Len(SnSaturn) - i1)
            End If

            'Dim CDinspector = DiscInspector.ScanSaturn(percorso)
            'SnSaturn = CDinspector.Data.SerialNumber
            'clearregionsaturn = CDinspector.Data.AreaCodes

            ReadDbSaturn()
        Catch
            If r_ss = "" Then r_ss = Path.GetFileNameWithoutExtension(n_ss)
            'MsgBox(Path.GetFileNameWithoutExtension(path_ss))
        End Try
    End Sub

    Private Sub ReadDbSaturn()
        cdn = ""
        SaturnUndetected()

        Using reader As New StreamReader(MedExtra & "\Plugins\db\Sega - Saturn.txt")
            While Not reader.EndOfStream
                Dim riga As String = reader.ReadLine
                If riga.Contains(SnSaturn) And riga.Contains(cdn) Then
                    r_ss = Trim((Replace(riga, SnSaturn, "")))

                    Select Case True
                        Case SnSaturn.Contains("H") And clearregionsaturn.Contains("E")
                            SegaCountry()
                        Case SnSaturn.Contains("H") And clearregionsaturn.Contains("U")
                            v_ss = "USA"
                        Case SnSaturn.Contains("MK") And clearregionsaturn.Contains("U")
                            v_ss = "USA"
                        Case SnSaturn.Contains("MK") And clearregionsaturn.Contains("E")
                            SegaCountry()
                        Case SnSaturn.Contains("G") And clearregionsaturn.Contains("J")
                            v_ss = "JAP"
                        Case Else
                            'SaturnUndetected()
                            'v_ss = clearregionsaturn
                    End Select

                    r_ss = r_ss & " [" & SnSaturn & "]"
                    'MsgBox(r_ss)
                    Exit While
                End If
            End While
            reader.Dispose()
            reader.Close()
        End Using
    End Sub

    Private Sub SaturnUndetected()
        Try
            Dim CDinspector = DiscInspector.ScanSaturn(percorso)
            cdn = " " & CDinspector.Data.DeviceInformation
            Select Case cdn.Trim
                Case "CD-1/1", "CD-1\1"
                    cdn = ""
                Case Else
                    cdn = Replace(Left(cdn.Trim, 4), "-", "")
            End Select
        Catch
            cdn = ""
        End Try
        '��������
    End Sub

    Private Sub DetectIsoSaturn()

        n_ss = Path.GetFileName(percorso)
        Select Case LCase(Path.GetExtension(n_ss))
            'If LCase(Path.GetExtension(n_psx)) = ".cue" Then
            Case ".cue" ', ".toc"
                Dim righe As String() = File.ReadAllLines(percorso)
                Dim result As String

                For i = 0 To 10
                    If righe(i).Contains("FILE ") Then
                        result = righe(i)
                        Exit For
                    End If
                Next

                Dim startPosition As Integer = result.IndexOf(Chr(34)) + 1
                Dim word2 As String = result.Substring(startPosition,
                                                         result.IndexOf(Chr(34), startPosition) - startPosition)
                path_ss = Replace(percorso, n_ss, "") & word2
            Case ".ccd"
                path_ss = Replace(percorso, ".ccd", "") & ".img"
            Case Else
                path_ss = percorso
        End Select
    End Sub

    Private Sub SegaCountry()
        Select Case True
            Case SnSaturn.EndsWith("-50")
                v_ss = "GENERAL"
            Case SnSaturn.EndsWith("-09")
                v_ss = "French"
            Case SnSaturn.EndsWith("-15")
                v_ss = "Germany"
            Case SnSaturn.EndsWith("-05")
                v_ss = "U.K."
            Case SnSaturn.EndsWith("-06")
                v_ss = "Spain"
            Case SnSaturn.EndsWith("-51")
                v_ss = "Italian"
            Case SnSaturn.EndsWith("-45")
                v_ss = "Brazilian"
            Case SnSaturn.EndsWith("-10")
                v_ss = "China"
            Case SnSaturn.EndsWith("-11")
                v_ss = "Hong Kong"
            Case SnSaturn.EndsWith("-16")
                v_ss = "Taiwan"
            Case SnSaturn.EndsWith("-08")
                v_ss = "Korean"
            Case Else
                v_ss = "EUR"
        End Select
    End Sub

End Module