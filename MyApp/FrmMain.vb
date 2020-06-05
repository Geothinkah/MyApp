Public Class FrmMain
    'Declare the DataTable
    Private ReadOnly TheDataTable As New DataTable
    'Set the Data Directory's path name into DirData
    Private ReadOnly DirData As String = My.Computer.FileSystem.SpecialDirectories.CurrentUserApplicationData
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Create the Columns in the table
        With TheDataTable
            .Columns.Add("Customer", Type.GetType("System.String"))
            .Columns.Add("Product", Type.GetType("System.String"))
            .Columns.Add("Date", Type.GetType("System.DateTime"))
            .Columns.Add("Value", Type.GetType("System.Double"))
        End With
    End Sub

    'Load the table
    Private Sub BtnLoad_Click(sender As Object, e As EventArgs) Handles BtnLoad.Click
        Try
            'Read the CSV file
            Dim thereader As New IO.StreamReader(DirData & "\TheTableData.csv", System.Text.Encoding.Default)
            'Declare a variable to hold each line in the CSV file
            Dim sline As String = ""
            Do
                'Read a line from the CSV file
                sline = thereader.ReadLine()
                'Exit Do Loop if the line has nothing in it
                If sline Is Nothing Then Exit Do
                'Split the line by values into the array "thecolumns"
                Dim thecolumns() As String = sline.Split(CType(",", Char()))
                'Create the new table row (record)
                Dim newrow As DataRow = TheDataTable.NewRow
                newrow("Customer") = thecolumns(0)
                newrow("Product") = thecolumns(1)
                newrow("Date") = thecolumns(2)
                newrow("Value") = thecolumns(3)
                TheDataTable.Rows.Add(newrow)
            Loop
            thereader.Close()
        Catch ex As Exception
            Dim unused = MsgBox("Couldn't Open: " & vbCrLf & DirData & "\TheTableData.csv")
        End Try
        'Tell TheDataGridView where to get it's data
        Grid1.DataSource = TheDataTable
    End Sub

    'Save the table
    Private Sub BtnSave_Click(sender As Object, e As EventArgs) Handles BtnSave.Click
        'Declare an empty string
        Dim TheCSVFile As String = String.Empty
        'Build the csv file
        For Each row As DataGridViewRow In Grid1.Rows
            'get the cells
            For Each cell As DataGridViewCell In row.Cells
                'Get rid of any commas in the cell then add it to the csv file string
                '***** The original statement doesn't work to get rid of the commas
                'TheCSVFile = TheCSVFile & cell.FormattedValue.replace(",", "") & ","
                TheCSVFile = TheCSVFile & CType(cell.Value, String) & ","
            Next
            'trim the last comma
            TheCSVFile = TheCSVFile.TrimEnd(CType(",", Char()))
            'Add the line to the output
            TheCSVFile &= vbCrLf
        Next
        'Remove the extra lines at the end of the csv file
        TheCSVFile = TheCSVFile.Substring(0, TheCSVFile.Length - 4)
        'write the file
        My.Computer.FileSystem.WriteAllText(DirData & "\TheTableData.csv", TheCSVFile, False)
        'display the path where the file has been saved
        Dim unused = MsgBox("The file has been saved in the following directory:" & vbCrLf & DirData)
    End Sub
End Class
