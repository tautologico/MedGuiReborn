﻿Imports System.IO

Module RData
    Public percorso, base_file, riga, fileTXT, ext, romname, country, full_path, status As String
    Public OthersDat As Boolean
    Public RenameLikeDat As Boolean = False

    Sub LMain()

        Try
            'If stopscan = True Then Exit Sub
            If fileTXT <> MedExtra & "DATs\" & MedGuiR.ComboBox1.Text & "\CUE.dat" Then estensione()
            status = ""

            If Dir(fileTXT) = "" Or fileTXT = MedExtra & "DATs\" & MedGuiR.ComboBox1.Text & "\CUE.dat" Then
                status = ""
                If fileTXT.Contains("\CUE.dat") Then stopscan = True Else stopscan = False
                GoTo Boing
            End If

            Using reader As New StreamReader(fileTXT)
                'If stopscan = True Then Exit Sub
                OthersDat = True
                While Not reader.EndOfStream
                    riga = reader.ReadLine.Trim
                    If riga.Contains(">No-Intro<") Then OthersDat = False

                    If UCase(riga).Contains(base_file) Then
                        estrapola()
                        Exit While
                    End If
                End While
                reader.Dispose()
                reader.Close()
            End Using
Boing:

            If status <> "Ok" Then
                status = "?"
                country = "?"
                Dim indice3 As Integer
                indice3 = romname.IndexOf("(")

                If indice3 < 0 Or country = "" Then
                    country = "?"
                ElseIf indice3 >= 0 Then
                    country = Replace(romname.Substring(indice3, Len(romname) - indice3), ".", "")
                    romname = Replace(romname, country, "")
                End If

                'If MedGuiR.CheckBox22.Checked = True Then
                'If Len(romname) > 50 Then
                'Dim splitromname() As String
                'splitromname = romname.Split(" - ")
                'romname = splitromname(0)
                'End If
                'End If

                If UCase(romname).Contains("[BIOS]") Or UCase(romname).Contains(" BIOS ") Or UCase(romname).Contains("Enhancement Chip") Then
                Else
                    If ext <> "" Then
                        Select Case LCase(ext)
                            Case ".cue", ".ccd"
                                RealcdIsoName()
                            Case ".psf", ".psf1", ".minipsf", ".gsf", ".minigsf", ".nsf", ".spc", ".vgm", ".gbs", ".ssf", ".minissf"
                                DetectChipmodule()
                                country = "(Soundtrack)"
                            Case ".bin"
                                If real_name = "Sega - 8/16 bit console - Music Module" Then
                                    DetectChipmodule()
                                    country = "(Soundtrack)"
                                End If
                        End Select
                        MedGuiR.DataGridView1.Rows.Add(RemoveAmpersand(romname.Trim), New Bitmap(icon_console), country, status, full_path, real_name, consoles, ext, base_file)
                    End If
                    Exit Sub
                End If

            End If
            'MedGuiR.remove_double()
            'stopscan = True
        Catch
        End Try

        'SoxStatus.Text = "Waiting for Rom Scan..."
        'SoxStatus.Label1.Text = romname
        'SoxStatus.Show()
    End Sub

    Public Sub estrapola()
        Dim indice = riga.IndexOf(Chr(34))
        Dim indice1 As Integer

        'If OthersDat = False Then
        'indice1 = riga.IndexOf(".")
        'Else
        If riga.Contains("].") Then
            indice1 = riga.IndexOf("].") + 1
        Else
            indice1 = riga.IndexOf(").") + 1
        End If
        'End If

        Dim indice2 = riga.IndexOf(" (")
        Dim rrom As String

        Try
            If indice >= 0 Then
                Dim indice3 As Integer
                'rrom = riga.Substring(indice + 1, indice1 - indice - 1)
                rrom = riga.Substring(indice + 1, indice1 - indice - 1)
                indice3 = rrom.IndexOf("(")
                country = rrom.Substring(indice3, Len(rrom) - indice3)
                status = "Ok"

                If RenameLikeDat = True Then RenameFile(rrom)

                'stopscan = True
                rrom = Replace(rrom, country, "")

                'If MedGuiR.CheckBox22.Checked = True Then
                'If Len(rrom) > 50 Then
                'Dim splitromname() As String
                'splitromname = rrom.Split(" - ")
                'rrom = splitromname(0)
                'End If
                'End If

                If UCase(romname).Contains("[BIOS]") Or UCase(romname).Contains(" BIOS ") Or UCase(romname).Contains("Enhancement Chip") Then
                Else
                    If ext <> "" Then
                        MedGuiR.DataGridView1.Rows.Add(RemoveAmpersand(rrom.Trim), New Bitmap(icon_console), country, status, full_path, real_name, consoles, ext, base_file)
                    End If
                End If
                'MedGuiR.remove_double()
                'If Counter = 0 Then stopscan = True
            End If
        Catch
        End Try

    End Sub

    Private Sub RenameFile(FixedRomName)
        Dim frpath As String = Path.GetDirectoryName(full_path)
        Dim GetFixedExt As String = Path.GetExtension(full_path)
        Dim newFile As String = Path.Combine(frpath, FixedRomName & GetFixedExt)

        If full_path <> newFile Then
            Rename(full_path, newFile)
            full_path = newFile
        End If
    End Sub

    Public Sub RealcdIsoName()
        'Try
        Select Case consoles
            Case "ss"
                If r_ss = "" Then r_ss = Path.GetFileNameWithoutExtension(percorso)
                'MedGuiR.DataGridView1.CurrentRow.Cells(0).Value() = r_ss
                romname = r_ss
                'MedGuiR.DataGridView1.CurrentRow.Cells(2).Value() = "(" & v_ss & ")"
                country = "(" & v_ss & ")"
            Case "psx"
                If r_psx = "" Then r_psx = Path.GetFileNameWithoutExtension(n_psx)
                'MedGuiR.DataGridView1.CurrentRow.Cells(0).Value() = r_psx
                rn = r_psx
                romname = r_psx
                psx_version()
        End Select
        'Catch
        'stopscan = False
        'End Try
    End Sub

End Module