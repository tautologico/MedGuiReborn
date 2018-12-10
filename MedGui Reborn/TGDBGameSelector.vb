﻿Public Class TGDBGameSelector

    Private Sub TGDBGameSelector_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Icon = gIcon
        F1 = Me
        CenterForm()
    End Sub

    Private Sub DataGridView1_CellContentDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentDoubleClick
        Try
            '<TheGamesDb newapi>
            Dim Json1 As String = New Net.WebClient().DownloadString("https://api.thegamesdb.net/Games/ByGameID?apikey=" & VSTripleDES.DecryptData("sCIncJ8wu3H2kmUNaEd4r3oxxsji80o2gVZlp+LKd7Zwp4f4wq6P5f23EaIp9NQFVFwko+jbtvULpqijriaQapiPRCpNGjFCiOlRaxOggKCddRhcmQRC4B3et57yNohlyKuW1s5DvXoVm+iRRO2qEpzO4KnDAmADOxChXfGe7QCInElJHwS+qA==") _
         & "&id=" & DataGridView1.CurrentRow.Cells(0).Value() & "&fields=players%2Cpublishers%2Cgenres%2Coverview%2Ccoop&filter&include=boxart%2Cplatform")
            Threading.Thread.Sleep(1000)
            Dim str = Newtonsoft.Json.JsonConvert.DeserializeXmlNode(Json1, "Root")

            Dim File As IO.StreamWriter
            File = My.Computer.FileSystem.OpenTextFileWriter(MedExtra & "Scraped\" & MedGuiR.DataGridView1.CurrentRow.Cells(5).Value() & "\" & Trim(MedGuiR.DataGridView1.CurrentRow.Cells(0).Value()) & ".xml", False)
            Dim splitXml As String() = Split(str.OuterXml, "<pages>")
            File.WriteLine(str.OuterXml.Remove(splitXml(0).Length, str.OuterXml.Length - splitXml(0).Length - 7))
            File.Close()

            Scrape.ReadXml()
        Catch ex As System.Net.WebException
            MessageBox.Show(ex.Message)
            If (ex.Response IsNot Nothing) Then
                Dim hr As System.Net.HttpWebResponse = DirectCast(ex.Response, System.Net.HttpWebResponse)
            End If
        Finally
            Me.Close()
        End Try
    End Sub

End Class