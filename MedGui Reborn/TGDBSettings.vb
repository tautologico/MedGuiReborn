﻿Imports System.IO
Imports Newtonsoft.Json

Public Class TGDBSettings

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = True Then
            Panel1.Enabled = True
            NewAPI = True
        Else
            Panel1.Enabled = False
            NewAPI = False
        End If
    End Sub

    Private Sub TGBSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Icon = gIcon
        F1 = Me
        CenterForm()
        CheckBox1.Checked = GlobalVar.NewAPI
    End Sub

    Public Function MakeTGDBList(ByVal APIvalue As String)
        Dim SplittedScrapedValue() As String
        Dim ResultScraped As String
        Dim JsonT As String = New Net.WebClient().DownloadString("https://api.thegamesdb.net/" & APIvalue & "?apikey=" & VSTripleDES.DecryptData("WGjcjnPpu1N7Cj92IF4kIiHtXEd5iNXtaZWpDkM5FVIwZuE2Kpo8R5KzvfENFA8kWmEGOig1hY30hlgoajQ+JjL+Zyv5rmRS+FPemgiaKTTMsGDF4jNR0W1rCBblcD9p6CrUK9MH7YKlexs1HkfrqTlyGKzDLlla1vBfFiI1gmp9haPCMNH84Q=="))

        If File.Exists(MedExtra & "\Plugins\db\TGDB") = False Then
            Directory.CreateDirectory(MedExtra & "\Plugins\db\TGDB")
        End If

        File.WriteAllText(MedExtra & "\Plugins\db\TGDB\" & APIvalue, JsonConvert.SerializeObject(JsonConvert.DeserializeXmlNode(JsonT, "Root"), Formatting.Indented))

        Dim oFile As System.IO.File
        Dim oRead As System.IO.StreamReader

        Try
            oRead = oFile.OpenText(MedExtra & "\Plugins\db\TGDB\" & APIvalue)

            While oRead.Peek <> -1
                SplittedScrapedValue = Split(oRead.ReadLine(), ":")

                Select Case SplittedScrapedValue(0).Trim
                    Case """id"""
                        ResultScraped += Replace(SplittedScrapedValue(1).Trim, ",", " | ")
                    Case """name"""
                        ResultScraped += SplittedScrapedValue(1).Trim
                        If ResultScraped.Substring(ResultScraped.Length - 1, 1) = "," Then
                            ResultScraped = ResultScraped.Substring(0, ResultScraped.Length - 1)
                        End If
                        ResultScraped += vbCrLf
                End Select

                ResultScraped = Replace(ResultScraped, """", "")
            End While

            File.WriteAllText(MedExtra & "\Plugins\db\TGDB\" & APIvalue & ".txt", ResultScraped)
        Catch ex As Exception
        Finally
            oRead.Close()
            File.Delete(MedExtra & "\Plugins\db\TGDB\" & APIvalue)
            MsgBox(APIvalue & " Updated!", MsgBoxStyle.Information + vbOKOnly, "Update Done...")
        End Try
    End Function

    Private Sub PictureBox3_Click(sender As Object, e As EventArgs) Handles PictureBox3.Click
        MakeTGDBList("Platforms")
    End Sub

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        MakeTGDBList("Developers")
    End Sub

    Private Sub PictureBox2_Click(sender As Object, e As EventArgs) Handles PictureBox2.Click
        MakeTGDBList("Genres")
    End Sub

    Private Sub PictureBox4_Click(sender As Object, e As EventArgs) Handles PictureBox4.Click
        MakeTGDBList("Publishers")
    End Sub

    Private Sub TGDBSettings_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        TGDBIni()
    End Sub

End Class