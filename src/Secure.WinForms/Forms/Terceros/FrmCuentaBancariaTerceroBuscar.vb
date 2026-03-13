Imports System.Data
Imports DevExpress.XtraGrid.Views.Grid
Imports Secure.Platform.Contracts.Dtos.Tercero
Imports Secure.Platform.WinForms.Forms.Base
Imports Secure.Platform.WinForms.Infrastructure

Namespace Forms.Terceros
    ''' <summary>
    ''' Pantalla de busqueda para tercero.cuenta_bancaria_tercero.
    ''' Autor: Mario Gomez.
    ''' </summary>
    Public Class FrmCuentaBancariaTerceroBuscar
        Inherits BaseSearchForm

        Public Sub New()
            Me.New(Nothing, Nothing)
        End Sub

        Public Sub New(ByVal apiClient As ApiClient, ByVal sessionContext As UserSessionContext)
            MyBase.New(apiClient, sessionContext)
        End Sub

        Protected Overrides Function BuildEndpoint() As String
            Return "api/v1/tercero/cuenta_bancaria_tercero/listar"
        End Function

        Protected Overrides Function BuildFormTitle() As String
            Return "Cuentas Bancarias de Tercero - Busqueda"
        End Function

        Protected Overrides Sub ConfigureColumns(ByVal gridView As GridView)
            gridView.Columns.Clear()
            gridView.Columns.AddVisible("id_cuenta_bancaria_tercero", "Id").Width = 80
            gridView.Columns.AddVisible("id_tercero", "Id tercero").Width = 100
            gridView.Columns.AddVisible("id_banco", "Banco").Width = 90
            gridView.Columns.AddVisible("numero_cuenta", "Numero cuenta").Width = 230
            gridView.Columns.AddVisible("id_moneda", "Moneda").Width = 90
            gridView.Columns.AddVisible("principal", "Principal").Width = 90
        End Sub

        Protected Overrides Function BuildSampleData() As DataTable
            Dim table = CreateTable()

            Try
                If ApiClient IsNot Nothing Then
                    Dim items = ApiClient.GetAsync(Of List(Of CuentaBancariaTerceroDto))(BuildEndpoint()).GetAwaiter().GetResult()
                    If items IsNot Nothing Then
                        For Each item In items
                            table.Rows.Add(
                                item.IdCuentaBancariaTercero.GetValueOrDefault(0),
                                item.IdTercero.GetValueOrDefault(0),
                                item.IdBanco.GetValueOrDefault(0),
                                item.NumeroCuenta,
                                item.IdMoneda.GetValueOrDefault(0),
                                item.Principal.GetValueOrDefault(False))
                        Next
                        Return table
                    End If
                End If
            Catch
                ' Fallback a data local de diseno.
            End Try

            Return table
        End Function

        Private Shared Function CreateTable() As DataTable
            Dim table As New DataTable()
            table.Columns.Add("id_cuenta_bancaria_tercero", GetType(Long))
            table.Columns.Add("id_tercero", GetType(Long))
            table.Columns.Add("id_banco", GetType(Integer))
            table.Columns.Add("numero_cuenta", GetType(String))
            table.Columns.Add("id_moneda", GetType(Integer))
            table.Columns.Add("principal", GetType(Boolean))
            Return table
        End Function
    End Class
End Namespace

