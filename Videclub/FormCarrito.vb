Imports System.Data.OleDb

Public Class FormCarrito
    Private Sub FormCarrito_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DataGridView2.Columns.Add(0, "Piezas")
        DataGridView2.Columns.Add(0, "CveProducto")
        DataGridView2.Columns.Add(0, "Descripcion")
        DataGridView2.Columns.Add(0, "Existencias")
        DataGridView2.Columns.Add(0, "Precio")

        lblSubtotal.Text = "$0.00"
        lblIva.Text = "$0.00"
        lblTotal.Text = "$0.00"

    End Sub

    'Mostramos los datos de las peliculas en un datagridview'
    Private Sub CargarPeliculas(ByRef pPelicula As String)
        Dim oda As New OleDbDataAdapter
        Dim ods As New DataSet
        Dim condicion1 As String = TextBox1.Text
        Dim connectionString As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\chich\OneDrive\Escritorio\VHS2.mdb;"
        Dim query As String
        Dim connection As New OleDbConnection(connectionString)
        If (Len(pPelicula) = 0) Then
            query = "Select * from Productos"
        Else
            query = "Select * from Productos where Descripcion LIKE " + "'" + "%" + condicion1 + "%" + "'"
        End If
        oda = New OleDbDataAdapter(query, connection)
        ods.Tables.Add("Productos")
        oda.Fill(ods.Tables("Productos"))
        DataGridView1.DataSource = ods.Tables("Productos")
        DataGridView1.Visible = True
    End Sub

    'Evento key press'
    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TextBox1.KeyPress
        If (Asc(e.KeyChar) = 13) Then   'Con este codigo habilitamos una tecla para buscar'
            If (Len(TextBox1.Text) = 0) Then
                CargarPeliculas("")
            Else
                CargarPeliculas(TextBox1.Text)
            End If
        End If
    End Sub
    'Boton de regreso'
    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Form1.Show()
        Me.Hide()
    End Sub
    'Con lo siguiente movemos datos al datagridview2'
    Private Sub DataGridView1_CellMouseClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridView1.CellMouseClick
        Dim fila As Integer
        'Mover a la lista'
        fila = DataGridView1.CurrentRow.Index
        'Mover a otro grid si existe'
        Dim i
        Dim existe As Boolean
        existe = False
        i = 0
        While ((i < DataGridView2.Rows.Count)) And (Not (existe))
            If (DataGridView2.Rows(i).Cells(1).Value = DataGridView1.Rows(fila).Cells(0).Value) Then
                existe = True
            End If
            i = i + 1
        End While
        'Si no existe'
        If Not (existe) Then
            DataGridView2.Rows.Add(0, DataGridView1.Rows(fila).Cells(0).Value, DataGridView1.Rows(fila).Cells(1).Value, DataGridView1.Rows(fila).Cells(2).Value, DataGridView1.Rows(fila).Cells(3).Value)
        End If
        DataGridView2.Visible = True
    End Sub

    'Mensaje de error cuando excede la cantidad de piezas'
    Private Sub DataGridView2_KeyDown(sender As Object, e As KeyEventArgs) Handles DataGridView2.KeyDown
        If e.KeyCode = Keys.Enter Then
            Dim rowIndex As Integer = DataGridView2.CurrentCell.RowIndex
            Dim piezas As Integer = DataGridView2.Rows(rowIndex).Cells(0).Value
            Dim existencias As Integer = DataGridView2.Rows(rowIndex).Cells(3).Value
            If piezas > existencias Then
                MessageBox.Show("No hay suficientes existencias para esta cantidad de piezas", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                DataGridView2.Rows(rowIndex).Cells(0).Value = existencias ' Restaurar valor anterior
            End If
        End If
    End Sub

    'Mover los datos a los label'
    Private Sub btnVenta_Click(sender As Object, e As EventArgs) Handles btnVenta.Click
        Dim rowIndex As Integer = DataGridView2.CurrentCell.RowIndex
        Dim piezas2 As Integer = DataGridView2.Rows(rowIndex).Cells(0).Value
        Dim existencias As Integer = DataGridView2.Rows(rowIndex).Cells(3).Value
        If piezas2 > existencias Then
            MessageBox.Show("No hay suficientes existencias para esta cantidad de piezas", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            LimpiarVenta()
        End If

        Dim subtotal As Decimal = 0
        For Each row As DataGridViewRow In DataGridView2.Rows
            Dim piezas As Integer = row.Cells(0).Value
            Dim precio As Decimal = row.Cells(4).Value

            subtotal += piezas * precio
        Next
        Dim iva As Decimal = subtotal * 0.1
        lblSubtotal.Text = subtotal.ToString("C")
        lblIva.Text = iva.ToString("C")
        lblTotal.Text = (subtotal + iva).ToString("C")


    End Sub

    Private Sub btnRemision_Click(sender As Object, e As EventArgs) Handles btnRemision.Click
        Dim remision As String = ""
        remision += "Remisión del Cliente" & vbCrLf
        remision += "======================" & vbCrLf
        For Each row As DataGridViewRow In DataGridView2.Rows
            Dim piezas As Integer = row.Cells(0).Value
            Dim cveProducto As String = row.Cells(1).Value
            Dim descripcion As String = row.Cells(2).Value
            Dim precio As Decimal = row.Cells(3).Value
            Dim total As Decimal = piezas * precio
            remision += cveProducto & " - " & descripcion & vbCrLf
        Next
        remision += "Subtotal: " & lblSubtotal.Text & vbCrLf
        remision += "Iva: " & lblIva.Text & vbCrLf
        remision += "Total: " & lblTotal.Text & vbCrLf
        MessageBox.Show(remision)

    End Sub

    'Cancelar venta'
    Private Sub LimpiarVenta()
        DataGridView2.Rows.Clear()
        lblSubtotal.Text = "$0.00"
        lblIva.Text = "$0.00"
        lblTotal.Text = "$0.00"
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        LimpiarVenta()
    End Sub
End Class