Imports DevExpress.XtraEditors
Imports Secure.Platform.Contracts.Dtos.Seguridad
Imports Secure.Platform.WinForms.Infrastructure
Imports System.Linq

Namespace Forms.Auth
    ''' <summary>
    ''' Pantalla para seleccionar empresa operativa despues de login/MFA.
    ''' Autor: Mario Gomez.
    ''' </summary>
    Partial Public Class FrmSeleccionEmpresa
        Inherits XtraForm

        Private ReadOnly _apiClient As ApiClient
        Private ReadOnly _idFlujoAutenticacion As Guid
        Private ReadOnly _empresas As IReadOnlyList(Of EmpresaAccesoDto)
        Private ReadOnly _tenantCodigo As String
        Private ReadOnly _usuarioMostrar As String

        ''' <summary>
        ''' Resultado de seleccion de empresa con token final.
        ''' </summary>
        Public Property SeleccionResponse As SeleccionarEmpresaResponseDto

        ''' <summary>
        ''' Constructor requerido por diseñador WinForms.
        ''' </summary>
        Public Sub New()
            Me.New(
                CreateDefaultApiClient(),
                Guid.Empty,
                New List(Of EmpresaAccesoDto)(),
                "SEED",
                "Usuario diseño")
        End Sub

        Public Sub New(
            ByVal apiClient As ApiClient,
            ByVal idFlujoAutenticacion As Guid,
            ByVal empresas As IReadOnlyList(Of EmpresaAccesoDto),
            ByVal tenantCodigo As String,
            ByVal usuarioMostrar As String)

            _apiClient = apiClient
            _idFlujoAutenticacion = idFlujoAutenticacion
            _empresas = If(empresas, New List(Of EmpresaAccesoDto)())
            _tenantCodigo = If(String.IsNullOrWhiteSpace(tenantCodigo), "N/A", tenantCodigo)
            _usuarioMostrar = If(String.IsNullOrWhiteSpace(usuarioMostrar), "Usuario", usuarioMostrar)

            InitializeComponent()
            BindData()
            WireEvents()
        End Sub

        Private Shared Function CreateDefaultApiClient() As ApiClient
            Dim baseUrl = AppSettingsProvider.GetApiBaseUrl()
            Return New ApiClient(baseUrl)
        End Function

        Private Sub BindData()
            _lblUsuario.Text = $"Usuario: {_usuarioMostrar}"
            _lblTenant.Text = $"Tenant: {_tenantCodigo}"
            _lblEstado.Text = $"Empresas disponibles: {_empresas.Count}"

            Dim source = _empresas.
                Select(Function(item) New EmpresaLookupItem With {
                    .IdEmpresa = item.IdEmpresa,
                    .NombreMostrado = BuildEmpresaDisplay(item),
                    .EsPredeterminada = item.EsPredeterminada
                }).
                OrderByDescending(Function(item) item.EsPredeterminada).
                ThenBy(Function(item) item.NombreMostrado).
                ToList()

            _lookEmpresa.Properties.DisplayMember = NameOf(EmpresaLookupItem.NombreMostrado)
            _lookEmpresa.Properties.ValueMember = NameOf(EmpresaLookupItem.IdEmpresa)
            _lookEmpresa.Properties.ShowHeader = False
            _lookEmpresa.Properties.DataSource = source

            Dim predeterminada = source.FirstOrDefault(Function(item) item.EsPredeterminada)
            If predeterminada IsNot Nothing Then
                _lookEmpresa.EditValue = predeterminada.IdEmpresa
            ElseIf source.Count > 0 Then
                _lookEmpresa.EditValue = source(0).IdEmpresa
            End If
        End Sub

        Private Sub WireEvents()
            AddHandler _btnContinuar.Click, AddressOf OnContinuarClickAsync
            AddHandler _btnCancelar.Click, AddressOf OnCancelarClick
        End Sub

        Private Async Sub OnContinuarClickAsync(ByVal sender As Object, ByVal e As EventArgs)
            If _idFlujoAutenticacion = Guid.Empty Then
                XtraMessageBox.Show(Me, "No existe flujo de autenticacion activo para seleccionar empresa.", "Seleccion de empresa", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            If _lookEmpresa.EditValue Is Nothing Then
                XtraMessageBox.Show(Me, "Debe seleccionar una empresa para continuar.", "Seleccion de empresa", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim idEmpresa As Long
            If Not Long.TryParse(_lookEmpresa.EditValue.ToString(), idEmpresa) OrElse idEmpresa <= 0 Then
                XtraMessageBox.Show(Me, "La empresa seleccionada no es valida.", "Seleccion de empresa", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            ToggleUi(False)
            _lblEstado.Text = "Aplicando empresa de trabajo..."

            Try
                Dim request As New SeleccionarEmpresaRequestDto With {
                    .IdFlujoAutenticacion = _idFlujoAutenticacion,
                    .IdEmpresa = idEmpresa,
                    .HuellaDispositivo = Environment.MachineName,
                    .SolicitudId = Guid.NewGuid().ToString("N")
                }

                Dim response = Await _apiClient.PostAsync(Of SeleccionarEmpresaRequestDto, SeleccionarEmpresaResponseDto)(
                    "api/v1/seguridad/flujo_autenticacion/seleccionar_empresa",
                    request).ConfigureAwait(True)

                If response Is Nothing OrElse Not response.SeleccionAplicada Then
                    Dim mensaje = If(response?.Mensaje, "No fue posible completar la seleccion de empresa.")
                    _lblEstado.Text = mensaje
                    _lblEstado.Appearance.ForeColor = Color.Firebrick
                    XtraMessageBox.Show(Me, mensaje, "Seleccion de empresa", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If

                SeleccionResponse = response
                DialogResult = DialogResult.OK
                Close()
            Catch apiEx As ApiClientException
                _lblEstado.Text = "No fue posible aplicar la empresa seleccionada."
                _lblEstado.Appearance.ForeColor = Color.Firebrick
                ApiErrorPresenter.Show(Me, apiEx, "seleccion de empresa")
            Catch ex As Exception
                _lblEstado.Text = ex.Message
                _lblEstado.Appearance.ForeColor = Color.Firebrick
                ApiErrorPresenter.ShowUnexpected(Me, ex, "seleccion de empresa")
            Finally
                ToggleUi(True)
            End Try
        End Sub

        Private Sub ToggleUi(ByVal enabled As Boolean)
            _lookEmpresa.Enabled = enabled
            _btnContinuar.Enabled = enabled
            _btnCancelar.Enabled = enabled
            Cursor = If(enabled, Cursors.Default, Cursors.WaitCursor)
        End Sub

        Private Sub OnCancelarClick(ByVal sender As Object, ByVal e As EventArgs)
            DialogResult = DialogResult.Cancel
            Close()
        End Sub

        Private Shared Function BuildEmpresaDisplay(ByVal empresa As EmpresaAccesoDto) As String
            If empresa Is Nothing Then Return String.Empty

            Dim codigo = If(empresa.CodigoEmpresa, String.Empty).Trim()
            Dim nombre = If(empresa.NombreEmpresa, String.Empty).Trim()

            If Not String.IsNullOrWhiteSpace(codigo) AndAlso Not String.IsNullOrWhiteSpace(nombre) Then
                Return $"{codigo} - {nombre}"
            End If

            If Not String.IsNullOrWhiteSpace(nombre) Then
                Return nombre
            End If

            Return $"Empresa {empresa.IdEmpresa}"
        End Function

        Private NotInheritable Class EmpresaLookupItem
            Public Property IdEmpresa As Long
            Public Property NombreMostrado As String = String.Empty
            Public Property EsPredeterminada As Boolean
        End Class
    End Class
End Namespace
