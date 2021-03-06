﻿Imports System.IO

Public Class TestCPU
    Dim Mhz, Mhz1 As Integer
    Dim CPUname As String

    Private Sub TestCPU_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.Icon = gIcon
        F1 = Me
        CenterForm()
        ColorizeForm()

        Dim customCulture As Globalization.CultureInfo = CType(Threading.Thread.CurrentThread.CurrentCulture.Clone(), Globalization.CultureInfo)
        customCulture.NumberFormat.NumberDecimalSeparator = "."

        Dim MyOBJ As Object
        Dim cpu As Object

        MyOBJ = GetObject("WinMgmts:").instancesof("Win32_Processor")
        For Each cpu In MyOBJ
            Mhz1 = Val(cpu.CurrentClockSpeed.ToString)
            Mhz = Val(cpu.MaxClockSpeed.ToString)
            CPUname = cpu.Name.ToString & " (" & cpu.NumberOfLogicalProcessors.ToString & " CPUs)"
        Next

        Label1.Text = "CPU Name: " & CPUname.Trim
        Label3.Text = "MHz: " & Mhz1.ToString & "/" & Mhz.ToString
        Label12.Text = "Mednafen " & x864
        Label14.Text = "OS: " & My.Computer.Info.OSFullName
        Label15.Text = "Platform: " & c_os.ToString & " bit"

        If "x" & Replace(c_os.ToString, "32", "86") <> x864 Then Label12.ForeColor = Color.DarkRed

        SetTheMessage()

        If x864 <> "x64" Then
            Label10.Text = "Not Supported"
            Label10.ForeColor = Color.DarkRed
        End If

        Process.Start("dxdiag.exe", "/t " & Path.Combine(Application.StartupPath, "PcSpecs.txt"))

        SoxStatus.Label1.Text = "Wait for CPU test..."
        SoxStatus.Text = "Wait..."
        SoxStatus.Show()
        Application.DoEvents()

        Dim codx As Integer = 1

tryagain:
        If File.Exists(Path.Combine(Application.StartupPath, "PcSpecs.txt")) Then
            SoxStatus.Close()
            ParsePcSpecs()
            codx = 0
        End If
        If codx >= 1 And codx < 500000 Then
            codx += 1
            GoTo tryagain
        End If

        SoxStatus.Close()
        BringToFront()
    End Sub

    Private Sub ParsePcSpecs()
        Try
            Dim reader As StreamReader = My.Computer.FileSystem.OpenTextFileReader(Path.Combine(Application.StartupPath, "PcSpecs.txt"))
            Dim a As String
            Dim SplitA() As String
            Dim Tmhz As Double

            Do
                a = reader.ReadLine
                If a.Contains("Processor: ") Then
                    Dim TSpeed As String
                    If a.Contains("GHz") Then
                        SplitA = Split(a, "~")
                        TSpeed = Replace(SplitA(1).Trim, "GHz", "")
                        Tmhz = Double.Parse(TSpeed, New Globalization.CultureInfo("en-US"))
                    ElseIf a.Contains("MHz") Then
                        SplitA = Split(a.Trim, "MHz")
                        Dim CSplit As Integer = SplitA.Length - 2
                        TSpeed = SplitA(CSplit).Substring(SplitA(CSplit).Length - 4, 4)
                        Tmhz = Val(TSpeed.Trim)
                    End If

                    If Tmhz < 10 Then Tmhz = Tmhz * 1000
                    If Val(Mhz) < Tmhz Then
                        Mhz = Tmhz
                        Label3.Text = "MHz: " & Mhz1.ToString & "/" & Mhz.ToString
                    End If
                    Exit Do
                End If
            Loop Until a Is Nothing
            reader.Close()

            File.Delete(Path.Combine(Application.StartupPath, "PcSpecs.txt"))
        Catch
        End Try
    End Sub

    Private Sub SetTheMessage()
        If Mhz <= 1500 Then
            Label6.Text = "Faust"
            Label6.ForeColor = Color.DarkViolet
            Label7.Text = "Fast"
            Label7.ForeColor = Color.DarkViolet
            Label9.Text = "Your CPU is CRAP"
            Label9.ForeColor = Color.DarkRed
            Label10.Text = "Your CPU is CRAP"
            Label10.ForeColor = Color.DarkRed
            Label11.Text = "You are using an obsolete or low-performance CPU.
This PC can start all Mednafen modules with dignity except PSX and Saturn.
The Snes can be emulated via the Faust module (except for games that use special chips) and the PC Engine will be emulated via the Fast module.
Forget any additional graphic effect, at the limit, enables the scanlines and / or the bilinear filter."
            Button1.Enabled = True
        ElseIf Mhz > 1500 And Mhz <= 2500 Then
            Label6.Text = "Faust"
            Label6.ForeColor = Color.DarkViolet
            Label7.Text = "Standard"
            Label7.ForeColor = Color.DarkGreen
            Label9.Text = "Your CPU is CRAP"
            Label9.ForeColor = Color.DarkRed
            Label10.Text = "Your CPU is CRAP"
            Label10.ForeColor = Color.DarkRed
            Label11.Text = "You are using a medium/low-end CPU.
This PC can start all Mednafen modules with dignity except PSX and Saturn.
The Snes can be emulated through the Faust module (except for games that use special chips).
You can also moderately enable graphic filters like shaders.
You could also try emulating the PSX by setting the SPU = 0 option, and possibly testing the Snes emulation via bsnes, but don't expect miracles."
            Button1.Enabled = True
        ElseIf Mhz > 2500 And Mhz <= 3300 Then
            Label6.Text = "bsnes"
            Label6.ForeColor = Color.DarkGreen
            Label7.Text = "Standard"
            Label7.ForeColor = Color.DarkGreen
            Label9.Text = "Standard"
            Label9.ForeColor = Color.DarkGreen
            Label10.Text = "Your CPU is CRAP"
            Label10.ForeColor = Color.DarkRed
            Label11.Text = "You are using a medium/high-end CPU.
This PC will be able to start all the Mednafen modules with some doubts about the Saturn, setting the SCSP option = 0.
You will be able to enable graphic filters like shaders without restrictions."
        Else
            Label6.Text = "bsnes"
            Label6.ForeColor = Color.DarkGreen
            Label7.Text = "Standard"
            Label7.ForeColor = Color.DarkGreen
            Label9.Text = "Standard"
            Label9.ForeColor = Color.DarkGreen
            Label10.Text = "Standard"
            Label10.ForeColor = Color.DarkGreen
            Label11.Text = "You're using a high-end CPU (the beast).
This PC can start all Mednafen modules and graphic filters without restrictions."
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        If Label7.Text = "Fast" Then
            MedGuiR.CheckBox1.Checked = True
        Else
            MedGuiR.CheckBox1.Checked = False
        End If

        If Label6.Text = "Faust" Then
            MedGuiR.CheckBox15.Checked = True
        Else
            MedGuiR.CheckBox15.Checked = False
        End If

    End Sub

    Private Sub Label12_MouseEnter(sender As Object, e As EventArgs) Handles Label12.MouseEnter
        If Label12.ForeColor = Color.DarkRed Then
            Label11.Text = "You are using Mednafen 32bit into a x64 OS Windows.
Using the 64-bit build is recommended, for better performance and functionality,
also Mednafen 32bit do not support Sega Saturn module."
        End If
    End Sub

    Private Sub Label12_MouseLeave(sender As Object, e As EventArgs) Handles Label12.MouseLeave
        If Label12.ForeColor = Color.DarkRed Then
            SetTheMessage()
        End If
    End Sub

End Class