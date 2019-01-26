﻿Imports System.IO

Public Class Mcheat
    Dim TypeCheat, CheatActive, LittleEndian, ByteLenght, CodeAdress, ByteValue, CheatName, CheatConsole As String
    Dim TWriteRAM As Boolean = True
    Dim ControlCheatPresence As Integer
    Dim DoesentPrepare As Boolean = False

    Private Sub NumericUpDown1_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown1.ValueChanged
        TextBox3.MaxLength = (NumericUpDown1.Value * 3) + 1
        UpdateValue()
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Dim searchcheatcode As String = ""
        Select Case CheatConsole
            Case "psx", "ss"
                searchcheatcode = "serial/" & GetSerial(rn)
            Case "pcfx"
                searchcheatcode = "search"
            Case "pce"
                If MedGuiR.DataGridView1.CurrentRow.Cells(5).Value() = "TurboGrafx 16 (CD)" Then
                    searchcheatcode = "search"
                Else
                    searchcheatcode = "crc32/" & base_file
                End If
            Case Else
                searchcheatcode = "crc32/" & base_file
        End Select

        _link = "https://gamehacking.org/" & searchcheatcode
        open_link()
    End Sub

    Private Function GetSerial(gamename As String) As String
        Dim splitrname() As String
        splitrname = gamename.Split("[")
        Return (Replace(splitrname(1), "]", ""))
    End Function

    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        _link = "http://bsfree.shadowflareindustries.com/index.php"
        open_link()
    End Sub

    Private Function AnalizeRAWCode(AdressCode As String) As String
        'RadioButton1.Checked = True
        Dim delimiterChars As Char() = {"?", ":", "-", " ", "+"}
        Dim SplitAdress() As String = AdressCode.Trim.Split(delimiterChars)

        If SplitAdress.Length < 2 Then Exit Function

        Select Case CheatConsole
            Case "gg", "sms"
                'RadioButton1.Checked = True
                If SplitAdress.Length > 2 Then
                    RadioButton5.Checked = True
                    TextBox1.Text = SplitAdress(0)
                    TextBox3.Text = SplitAdress(2) & " " & SplitAdress(1)
                Else
                    RadioButton1.Checked = True
                    If SplitAdress(1).Length = 4 Then
                        TextBox1.Text = SplitAdress(0).Substring(2, 2) & SplitAdress(1).Substring(0, 2)
                        TextBox3.Text = SplitAdress(1).Substring(2, 2)
                    Else
                        TextBox1.Text = SplitAdress(0)
                        TextBox3.Text = SplitAdress(1)
                    End If
                End If

            Case "nes"
                If SplitAdress.Length > 2 Then
                    RadioButton5.Checked = True
                    TextBox1.Text = SplitAdress(0)
                    TextBox3.Text = SplitAdress(2) & " " & SplitAdress(1)
                Else
                    RadioButton1.Checked = True
                    TextBox1.Text = SplitAdress(0)
                    TextBox3.Text = SplitAdress(1)
                End If

            Case "lynx"
                If SplitAdress.Length > 2 Then
                    'RadioButton5.Checked = True
                    TextBox1.Text = SplitAdress(0)
                    TextBox3.Text = SplitAdress(1) & " " & SplitAdress(2)
                Else
                    'RadioButton1.Checked = True
                    TextBox1.Text = SplitAdress(0)
                    TextBox3.Text = SplitAdress(1)
                End If

            Case "snes", "snes_faust"
                'RadioButton4.Checked = True
                TextBox1.Text = SplitAdress(0)
                TextBox3.Text = SplitAdress(1)

            Case Else
                'RadioButton1.Checked = True
                TextBox1.Text = SplitAdress(0)
                If SplitAdress.Length > 1 Then
                    NumericUpDown1.Value = Math.Ceiling(SplitAdress(1).Length / 2)
                    TextBox3.Text = SplitAdress(1)
                    UpdateValue()
                End If

        End Select
        TextBox1.Text = TextBox1.Text.PadLeft(8, "0")
    End Function

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        Dim Tlenght As Integer = 8
        If TWriteRAM = False Then Tlenght = 13 : Exit Sub
        TextBox1.Text = FormatText(TextBox1.Text, Tlenght)
        TextBox1.Select(TextBox1.Text.Length, 0)
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        PrepareCodeforMednafen()
    End Sub

    Private Sub PrepareCodeforMednafen()
        TypeWrite()
        If TWriteRAM = False Then AnalizeRAWCode(TextBox1.Text)

        SetCodeMode()
        Label11.Text = TypeCheat & CheatActive & ByteLenght & LittleEndian & FormatText(LCase(CodeAdress), 8) & FormatText(LCase(ByteValue), TextBox3.MaxLength) & CheatName
        Label11.Left = (Me.Width / 2) - (Label11.Width / 2)
    End Sub

    Private Function FormatText(TextValue As String, MTLenght As Integer) As String
        TextValue = TextValue.PadLeft(MTLenght, "0")
        If TextValue.Length > MTLenght Then TextValue = TextValue.Remove(0, 1)
        FormatText = TextValue
    End Function

    Private Sub RadioButton6_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton6.CheckedChanged
        TypeWrite()
    End Sub

    Private Sub RadioButton7_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton7.CheckedChanged
        TypeWrite()
    End Sub

    Private Sub TextBox3_TextChanged(sender As Object, e As EventArgs) Handles TextBox3.TextChanged
        If TWriteRAM = False Then TextBox3.MaxLength = 5 : Exit Sub
        UpdateValue()
    End Sub

    Private Sub UpdateValue()
        TextBox3.Text = FormatText(TextBox3.Text, TextBox3.MaxLength - 1)
        TextBox3.Select(TextBox3.Text.Length, 0)
    End Sub

    Private Sub TypeWrite()
        If RadioButton6.Checked = True Then
            TWriteRAM = True
            'TextBox1.MaxLength = 9
            TextBox3.Enabled = True
            NumericUpDown1.Enabled = True
        Else
            TWriteRAM = False
            'TextBox1.MaxLength = 14
            TextBox3.Enabled = False
            NumericUpDown1.Enabled = False
        End If

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If ListBox1.SelectedIndex < 0 Then Exit Sub
        WorkingWithCheat(ListBox1.SelectedItem.ToString, "", False)
        If ListBox1.Items.Count = 0 Then
            WorkingWithCheat("[" & ComboBox1.Text & "] " & Label7.Text.Trim, "", False)
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If ListBox1.SelectedIndex < 0 Then Exit Sub
        PrepareCodeforMednafen()
        WorkingWithCheat(ListBox1.SelectedItem.ToString, Label11.Text, False)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        TWriteRAM = True
        PrepareCodeforMednafen()

        Dim cheatpath As String = Path.Combine(MedGuiR.TextBox4.Text, "cheats\" & CheatConsole & ".cht")

        If File.Exists(cheatpath) = False Then
            File.Create(cheatpath).Dispose()
        End If

        ListBox1.Items.Clear()
        ParseCht()

        If ControlCheatPresence = 1 Then Exit Sub

        If ListBox1.Items.Count < 1 Then
            Dim ExNovo As String = "[" & ComboBox1.Text & "] " & Label7.Text.Trim & vbLf & Label11.Text.Trim
            WorkingWithCheat("", ExNovo, True)
        Else
            If ListBox1.SelectedIndex < 0 Then DoesentPrepare = True
            ListBox1.SelectedIndex = ListBox1.Items.Count - 1
            If ListBox1.SelectedItem.ToString.Trim = Label11.Text.Trim Then Exit Sub
            WorkingWithCheat(ListBox1.SelectedItem.ToString.Trim, ListBox1.SelectedItem.ToString.Trim & vbLf & vbLf & Label11.Text.Trim, False)
        End If
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        If ListBox1.Items.Count < 1 Or ListBox1.SelectedIndex < 0 Then Exit Sub

        'RadioButton6.Checked = True
        Dim RetrieveCheatValue() As String = ListBox1.SelectedItem.ToString.Split(" ")

        Select Case RetrieveCheatValue(0)
            Case "R"
                RadioButton1.Checked = True
            Case "A"
                RadioButton2.Checked = True
            Case "T"
                RadioButton3.Checked = True
            Case "S"
                RadioButton4.Checked = True
            Case "C"
                RadioButton5.Checked = True
        End Select

        Select Case RetrieveCheatValue(1)
            Case "A"
                CheckBox1.Checked = True
            Case "I"
                CheckBox1.Checked = False
        End Select

        NumericUpDown1.Value = Val(RetrieveCheatValue(2))

        Select Case RetrieveCheatValue(3)
            Case "B"
                CheckBox2.Checked = False
            Case "L"
                CheckBox2.Checked = True
        End Select

        'TextBox1.MaxLength = RetrieveCheatValue(5).Length + 1
        TextBox1.Text = RetrieveCheatValue(5)

        TextBox3.MaxLength = RetrieveCheatValue(6).Length + 1
        TextBox3.Text = RetrieveCheatValue(6)

        Dim cheatname As String
        For i = 7 To RetrieveCheatValue.Length - 1
            cheatname += RetrieveCheatValue(i) & " "
        Next

        If cheatname Is Nothing = False Then TextBox4.Text = cheatname.Trim

        If DoesentPrepare = False Then PrepareCodeforMednafen()
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        If File.Exists(Path.Combine(MedGuiR.TextBox4.Text, "cheats\" & CheatConsole & ".cht")) Then
            ListBox1.Items.Clear()
            ParseCht()
        End If
    End Sub

    Private Sub Mcheat_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Icon = gIcon
        F1 = Me
        CenterForm()

        ResetAll()
        CheatConsole = LCase(MedGuiR.DataGridView1.CurrentRow.Cells(6).Value)
        Select Case LCase(Path.GetExtension(percorso))
            Case ".zip", ".rar", ".7z"
                simple_extract()
            Case ".cue", ".toc", ".ccd", ".m3u"
                Dim savetype As String
                Select Case CheatConsole
                    Case "psx"
                        savetype = ".mcr"
                    Case "ss"
                        savetype = ".bkr"
                    Case Else
                        savetype = ".sav"
                End Select
                For Each foundFile As String In My.Computer.FileSystem.GetFiles(
  Path.Combine(MedGuiR.TextBox4.Text, "sav\"))
                    If foundFile.Contains(Path.GetFileNameWithoutExtension(percorso)) And
                        foundFile.Contains(savetype) Then
                        Dim Splitmcr() As String
                        Splitmcr = foundFile.Split(".")
                        'TextBox2.Text = Splitmcr(1)
                        Dim exist As Boolean = False
                        If ComboBox1.Items.Count > 0 Then
                            For i = 0 To ComboBox1.Items.Count - 1
                                If ComboBox1.Items(i) = Splitmcr(1) Then
                                    exist = True
                                    Exit For
                                End If
                            Next
                        End If
                        If exist = False Then ComboBox1.Items.Add(Splitmcr(1))
                    End If
                Next
                filepath = percorso
                GoTo skiphash
        End Select

        filepath = percorso

        Select Case CheatConsole
            Case "nes"
                RemoveHeader(16)
            Case "lynx"
                RemoveHeader(64)
        End Select

        MD5CalcFile()
        GetCRC32()

        MedGuiR.SetSpecialModule()
        If CheatConsole = "snes" And MedGuiR.tpce = "_faust" Then
            CheatConsole = "snes_faust"
            ComboBox1.Items.Add(r_sha.Substring(0, 32))
        Else
            ComboBox1.Items.Add(r_md5)
        End If

skiphash:

        Label7.Text = Path.GetFileNameWithoutExtension(filepath)

        If ComboBox1.Items.Count > 0 Then
            ComboBox1.SelectedIndex = 0
        End If

        If File.Exists(Path.Combine(MedGuiR.TextBox4.Text, "cheats\" & CheatConsole & ".cht")) Then
            ListBox1.Items.Clear()
            ParseCht()
        End If
    End Sub

    Public Function RemoveHeader(rembyte As Integer)

        Dim backPath As String = Path.Combine(MedExtra, "RomTemp\" & Path.GetFileName(filepath) & "_back")
        Dim WithHeader() As Byte = My.Computer.FileSystem.ReadAllBytes(filepath)

        Dim fs As FileStream
        fs = New FileStream(backPath, FileMode.Create)
        fs.Write(WithHeader, rembyte, WithHeader.Length - rembyte)
        fs.Close()
        filepath = backPath
    End Function

    Private Sub SetCodeMode()
        If RadioButton1.Checked = True Then
            TypeCheat = "R "
        ElseIf RadioButton2.Checked = True Then
            TypeCheat = "A "
        ElseIf RadioButton3.Checked = True Then
            TypeCheat = "T "
        ElseIf RadioButton4.Checked = True Then
            TypeCheat = "S "
        ElseIf RadioButton5.Checked = True Then
            TypeCheat = "C "
        End If

        If CheckBox1.Checked = True Then
            CheatActive = "A "
        Else
            CheatActive = "I "
        End If

        If CheckBox2.Checked = True Then
            LittleEndian = "L 0 "
        Else
            LittleEndian = "B 0 "
        End If

        ByteLenght = NumericUpDown1.Value.ToString.Trim & " "

        CodeAdress = TextBox1.Text.Trim & " "
        ByteValue = TextBox3.Text.Trim & " "
        CheatName = TextBox4.Text.Trim
    End Sub

    Private Sub Label11_DoubleClick(sender As Object, e As EventArgs) Handles Label11.DoubleClick
        If Label11.Text.Length > 0 And Label11.Text <> "Result Code" Then Clipboard.SetDataObject(Label11.Text)
    End Sub

    Private Sub ParseCht()
        ControlCheatPresence = 0
        Dim readText As String = File.ReadAllText(Path.Combine(MedGuiR.TextBox4.Text, "cheats\" & CheatConsole & ".cht"))
        Dim DeatilCheat() As String = readText.Split("[")

        Dim SplitCheat() As String
        
        For i = 0 To DeatilCheat.Length - 1
            If DeatilCheat(i).Contains(ComboBox1.Text) Or DeatilCheat(i).Contains(Label7.Text) Then
                If ComboBox1.Items.Count = 0 And DeatilCheat(i).Contains(cleanpsx(Label7.Text).Trim) Then
                    Dim SplitMd5() As String = DeatilCheat(i).Split("]")
                    ComboBox1.Text = SplitMd5(0)
                End If
                'If DeatilCheat(i).Contains(vbLf) = False Then i += 1
                If Len(DeatilCheat(i).Trim) <= Len(ComboBox1.Text & "] " & Label7.Text) Then Continue For
                SplitCheat = DeatilCheat(i).Split(vbLf)
                'Exit For
            End If
        Next

        If SplitCheat Is Nothing Then Exit Sub

        For i = 1 To SplitCheat.Length - 1
            If SplitCheat(i).Trim = "" Then Continue For
            ListBox1.Items.Add(SplitCheat(i).ToString)
            If SplitCheat(i).ToString.Trim = Label11.Text.Trim Then ControlCheatPresence = 1
        Next

    End Sub

    Private Sub ResetAll()
        ListBox1.Items.Clear()
        TextBox1.Text = ""
        'TextBox2.Text = ""
        ComboBox1.Items.Clear()
        TextBox3.Text = ""
        TextBox4.Text = ""
        Label7.Text = ""
        RadioButton7.Checked = True
        RadioButton1.Checked = True
        CheckBox1.Checked = True
        CheckBox2.Checked = True
        NumericUpDown1.Value = 1
        Label11.Text = "Result Code"
        Label11.Left = (Me.Width / 2) - (Label11.Width / 2)
    End Sub

    Private Function WorkingWithCheat(OriginalString As String, StringChange As String, AppendTxt As Boolean)
        'RadioButton1.Checked = True

        Dim txtcheat = My.Computer.FileSystem.ReadAllText(Path.Combine(MedGuiR.TextBox4.Text, "cheats\" & CheatConsole & ".cht"))

        If AppendTxt = False Then
            txtcheat = txtcheat.Replace(OriginalString, StringChange)
        Else
            txtcheat = StringChange
        End If
        My.Computer.FileSystem.WriteAllText(Path.Combine(MedGuiR.TextBox4.Text,
            "cheats\" & CheatConsole & ".cht"), vbLf & txtcheat.Trim & vbLf, AppendTxt)

        DoesentPrepare = False
        ListBox1.Items.Clear()
        ParseCht()
    End Function

End Class