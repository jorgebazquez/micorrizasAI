// ==============================================================================
// INSTALADOR AUTÓNOMO SETUP EN C# (.NET FRAMEWORK)
// MicorrizasSoft AI Installer - Versión Autónoma con Recursos Embebidos
// Arquitectura Robusta: Gestión Inteligente de Permisos UAC y Accesos Directos
// ==============================================================================

using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;
using System.Security.Principal;

namespace _0x3D_InstallerPackage
{
    static class _0x99A1_SetupEntryPoint
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new _0x77B2_InstallerWizardForm());
        }
    }

    public class _0x77B2_InstallerWizardForm : Form
    {
        private int _0xPasoActual = 0;
        private Panel[] _0xPanelesPasos;
        private string _0xRutaDestino;

        private CheckBox _0xChkEscritorio;
        private CheckBox _0xChkInicio;
        private CheckBox _0xChkEjecutarAlFinalizar;
        private ProgressBar _0xPrgBarra;
        private TextBox _0xTxtLog;
        private Label _0xLblEstado;
        private Button _0xBtnAnt;
        private Button _0xBtnSig;
        private Button _0xBtnCan;
        private TextBox _0xTxtRutaDestino;
        private Button _0xBtnElevate;
        private Label _0xLblPrivilegios;

        private bool _0xEsAdmin;

        public _0x77B2_InstallerWizardForm()
        {
            this.Text = "Instalador Autónomo de MicorrizasSoft AI (C# Native Edition)";
            this.Size = new Size(720, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(245, 246, 250);

            _0xLoadInstallerIcon();
            _0xCheckUserPrivileges();
            _0xCalculateDefaultDestination();
            _0xBuildWizardInterface();
            _0xShowStep(0);
        }

        private void _0xLoadInstallerIcon()
        {
            try
            {
                string p = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app_icon.ico");
                if (File.Exists(p))
                {
                    this.Icon = new Icon(p);
                    return;
                }
                if (File.Exists("app_icon.ico"))
                {
                    this.Icon = new Icon("app_icon.ico");
                    return;
                }
                Stream res = Assembly.GetExecutingAssembly().GetManifestResourceStream("app_icon.ico");
                if (res != null)
                {
                    this.Icon = new Icon(res);
                    return;
                }
                this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            }
            catch { }
        }


        private void _0xCheckUserPrivileges()
        {
            try
            {
                WindowsIdentity id = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(id);
                _0xEsAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch
            {
                _0xEsAdmin = false;
            }
        }

        private void _0xCalculateDefaultDestination()
        {
            if (_0xEsAdmin)
            {
                string progFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                if (string.IsNullOrEmpty(progFiles)) progFiles = @"C:\Program Files";
                _0xRutaDestino = Path.Combine(progFiles, "MicorrizasSoft");
            }
            else
            {
                // Ruta libre de restricciones UAC para usuarios estándar
                _0xRutaDestino = @"C:\MicorrizasSoft";
            }
        }

        private void _0xBuildWizardInterface()
        {
            // Panel lateral institucional
            Panel pLateral = new Panel() { Dock = DockStyle.Left, Width = 210, BackColor = Color.FromArgb(44, 62, 80) };
            this.Controls.Add(pLateral);

            Label lblLogo = new Label()
            {
                Text = "🔬\r\nMicoSoft AI\r\n(C# Native)",
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.FromArgb(236, 240, 241),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 170
            };
            pLateral.Controls.Add(lblLogo);

            _0xLblPrivilegios = new Label()
            {
                Text = _0xEsAdmin ? "🛡 Privilegios:\r\nAdministrador (UAC Activo)" : "👤 Privilegios:\r\nUsuario Estándar",
                Font = new Font("Segoe UI", 8.5f),
                ForeColor = _0xEsAdmin ? Color.FromArgb(46, 204, 113) : Color.FromArgb(241, 196, 15),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Bottom,
                Height = 60
            };
            pLateral.Controls.Add(_0xLblPrivilegios);

            // Panel inferior de navegación
            Panel pBotones = new Panel() { Dock = DockStyle.Bottom, Height = 58, BackColor = Color.White };
            this.Controls.Add(pBotones);

            _0xBtnCan = new Button() { Text = "Cancelar", Location = new Point(600, 14), Size = new Size(90, 32), Font = new Font("Segoe UI", 9f) };
            _0xBtnCan.Click += (s, e) => this.Close();
            pBotones.Controls.Add(_0xBtnCan);

            _0xBtnSig = new Button()
            {
                Text = "Siguiente >",
                Location = new Point(490, 14),
                Size = new Size(100, 32),
                BackColor = Color.FromArgb(41, 128, 185),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold)
            };
            _0xBtnSig.Click += (s, e) => _0xAdvanceStep();
            pBotones.Controls.Add(_0xBtnSig);

            _0xBtnAnt = new Button() { Text = "< Anterior", Location = new Point(390, 14), Size = new Size(90, 32), Enabled = false, Font = new Font("Segoe UI", 9f) };
            _0xBtnAnt.Click += (s, e) => _0xBackStep();
            pBotones.Controls.Add(_0xBtnAnt);

            // Contenedor principal de pasos
            Panel pContenido = new Panel() { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(25, 20, 25, 10) };
            this.Controls.Add(pContenido);

            _0xPanelesPasos = new Panel[5];
            for (int i = 0; i < 5; i++)
            {
                _0xPanelesPasos[i] = new Panel() { Dock = DockStyle.Fill, BackColor = Color.White, Visible = false };
                pContenido.Controls.Add(_0xPanelesPasos[i]);
            }

            _0xBuildStep0(_0xPanelesPasos[0]);
            _0xBuildStep1(_0xPanelesPasos[1]);
            _0xBuildStep2(_0xPanelesPasos[2]);
            _0xBuildStep3(_0xPanelesPasos[3]);
            _0xBuildStep4(_0xPanelesPasos[4]);
        }

        private void _0xBuildStep0(Panel p)
        {
            Label t = new Label() { Text = "Bienvenido al Instalador de MicorrizasSoft AI", Font = new Font("Segoe UI", 13, FontStyle.Bold), ForeColor = Color.FromArgb(44, 62, 80), Location = new Point(5, 5), AutoSize = true };
            p.Controls.Add(t);

            Label m = new Label()
            {
                Text = "Este asistente configurará e instalará la suite científica MicorrizasSoft AI (C# Native Edition) en su equipo.\r\n\r\n" +
                       "Novedades de esta versión:\r\n" +
                       "• Paquete completamente autónomo con recursos y librerías pre-empaquetadas.\r\n" +
                       "• Interfaz ajustada a 1366x766 con barra lateral de bitácora expansible.\r\n" +
                       "• Acceso público directo para consulta de base de datos de microscopía.\r\n" +
                       "• Protección institucional con Contraseña Maestra para modificar o eliminar datos.\r\n" +
                       "• Módulo de captura en vivo para cámaras USB y microscopios ópticos.\r\n" +
                       "• Centinela en tiempo de ejecución contra ingeniería inversa y desensamblado.\r\n\r\n" +
                       "Haga clic en 'Siguiente' para seleccionar el destino de instalación.",
                Font = new Font("Segoe UI", 9.5f),
                ForeColor = Color.FromArgb(52, 73, 94),
                Location = new Point(5, 45),
                Size = new Size(465, 260)
            };
            p.Controls.Add(m);
        }

        private void _0xBuildStep1(Panel p)
        {
            Label t = new Label() { Text = "Carpeta de Instalación de Destino", Font = new Font("Segoe UI", 12.5f, FontStyle.Bold), ForeColor = Color.FromArgb(44, 62, 80), Location = new Point(5, 5), AutoSize = true };
            p.Controls.Add(t);

            Label m = new Label()
            {
                Text = "Seleccione la carpeta donde se desplegarán los binarios y archivos del sistema:",
                Font = new Font("Segoe UI", 9.2f),
                ForeColor = Color.FromArgb(127, 140, 141),
                Location = new Point(5, 38),
                AutoSize = true
            };
            p.Controls.Add(m);

            _0xTxtRutaDestino = new TextBox() { Text = _0xRutaDestino, Location = new Point(5, 75), Width = 340, Font = new Font("Segoe UI", 9.5f) };
            p.Controls.Add(_0xTxtRutaDestino);

            Button btnExam = new Button() { Text = "Examinar...", Location = new Point(355, 73), Size = new Size(100, 29), Font = new Font("Segoe UI", 9f) };
            btnExam.Click += delegate
            {
                using (FolderBrowserDialog fbd = new FolderBrowserDialog())
                {
                    fbd.SelectedPath = _0xTxtRutaDestino.Text;
                    if (fbd.ShowDialog(this) == DialogResult.OK)
                    {
                        _0xRutaDestino = Path.Combine(fbd.SelectedPath, "MicorrizasSoft");
                        _0xTxtRutaDestino.Text = _0xRutaDestino;
                    }
                }
            };
            p.Controls.Add(btnExam);

            // Información sobre permisos y elevación
            Panel pnlAviso = new Panel() { Location = new Point(5, 125), Size = new Size(450, 130), BackColor = Color.FromArgb(248, 249, 250), Padding = new Padding(12) };
            p.Controls.Add(pnlAviso);

            Label lblInfoPerm = new Label()
            {
                Text = _0xEsAdmin ?
                    "✔ El instalador cuenta con permisos de Administrador para instalar en Program Files." :
                    "ℹ Para instalar en 'C:\\Program Files' se requieren permisos elevados.\nSi prefiere evitar la elevación, puede instalar directamente en C:\\MicorrizasSoft.",
                Font = new Font("Segoe UI", 9f),
                ForeColor = _0xEsAdmin ? Color.FromArgb(39, 174, 96) : Color.FromArgb(44, 62, 80),
                Dock = DockStyle.Top,
                Height = 55
            };
            pnlAviso.Controls.Add(lblInfoPerm);

            if (!_0xEsAdmin)
            {
                _0xBtnElevate = new Button()
                {
                    Text = "🛡 Reejecutar como Administrador",
                    Dock = DockStyle.Bottom,
                    Height = 34,
                    BackColor = Color.FromArgb(52, 73, 94),
                    ForeColor = Color.White,
                    Font = new Font("Segoe UI", 9f, FontStyle.Bold)
                };
                _0xBtnElevate.Click += delegate
                {
                    _0xRelaunchAsAdministrator();
                };
                pnlAviso.Controls.Add(_0xBtnElevate);
            }
        }

        private void _0xRelaunchAsAdministrator()
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = Application.ExecutablePath;
                psi.Verb = "runas";
                psi.UseShellExecute = true;
                Process.Start(psi);
                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo elevar privilegios: " + ex.Message, "UAC", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void _0xBuildStep2(Panel p)
        {
            Label t = new Label() { Text = "Accesos Directos y Entorno", Font = new Font("Segoe UI", 12.5f, FontStyle.Bold), ForeColor = Color.FromArgb(44, 62, 80), Location = new Point(5, 5), AutoSize = true };
            p.Controls.Add(t);

            Label m = new Label() { Text = "Configure los accesos rápidos en su sistema operativo:", Font = new Font("Segoe UI", 9.2f), ForeColor = Color.FromArgb(127, 140, 141), Location = new Point(5, 38), AutoSize = true };
            p.Controls.Add(m);

            _0xChkEscritorio = new CheckBox() { Text = "Crear acceso directo en el Escritorio de Windows (con icono institucional)", Checked = true, Location = new Point(15, 80), AutoSize = true, Font = new Font("Segoe UI", 9.5f) };
            p.Controls.Add(_0xChkEscritorio);

            _0xChkInicio = new CheckBox() { Text = "Crear acceso directo en el Menú Inicio de Windows", Checked = true, Location = new Point(15, 120), AutoSize = true, Font = new Font("Segoe UI", 9.5f) };
            p.Controls.Add(_0xChkInicio);
        }

        private void _0xBuildStep3(Panel p)
        {
            Label t = new Label() { Text = "Instalando MicorrizasSoft AI...", Font = new Font("Segoe UI", 12.5f, FontStyle.Bold), ForeColor = Color.FromArgb(44, 62, 80), Location = new Point(5, 5), AutoSize = true };
            p.Controls.Add(t);

            _0xLblEstado = new Label() { Text = "Extrayendo paquetes de la carga útil autónoma...", Font = new Font("Segoe UI", 9f), ForeColor = Color.FromArgb(127, 140, 141), Location = new Point(5, 38), AutoSize = true };
            p.Controls.Add(_0xLblEstado);

            _0xPrgBarra = new ProgressBar() { Location = new Point(5, 70), Width = 460, Height = 25 };
            p.Controls.Add(_0xPrgBarra);

            _0xTxtLog = new TextBox()
            {
                Location = new Point(5, 110),
                Width = 460,
                Height = 190,
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Consolas", 8.8f),
                BackColor = Color.FromArgb(248, 249, 250)
            };
            p.Controls.Add(_0xTxtLog);
        }

        private void _0xBuildStep4(Panel p)
        {
            Label t = new Label() { Text = "¡Instalación Finalizada con Éxito!", Font = new Font("Segoe UI", 13.5f, FontStyle.Bold), ForeColor = Color.FromArgb(39, 174, 96), Location = new Point(5, 5), AutoSize = true };
            p.Controls.Add(t);

            Label m = new Label()
            {
                Text = "MicorrizasSoft AI (C# Native Edition) ha quedado desplegado y listo para su uso.\r\n\r\n" +
                       "Directorio de instalación:\r\n" + _0xRutaDestino + "\r\n\r\n" +
                       "Seguridad y Políticas del Laboratorio:\r\n" +
                       "• La base de datos es accesible en modo consulta abierta por defecto.\r\n" +
                       "• Para modificar o eliminar registros, ingrese la Contraseña Maestra.\r\n" +
                       "• La bitácora cuenta con resguardo criptográfico institucional.\r\n\r\n" +
                       "Haga clic en 'Finalizar' para cerrar el instalador.",
                Font = new Font("Segoe UI", 9.2f),
                ForeColor = Color.FromArgb(44, 62, 80),
                Location = new Point(5, 45),
                Size = new Size(465, 195)
            };
            p.Controls.Add(m);

            _0xChkEjecutarAlFinalizar = new CheckBox()
            {
                Text = "🚀 Iniciar MicorrizasSoft AI inmediatamente",
                Checked = true,
                Location = new Point(15, 248),
                AutoSize = true,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                ForeColor = Color.FromArgb(41, 128, 185)
            };
            p.Controls.Add(_0xChkEjecutarAlFinalizar);
        }

        private void _0xShowStep(int p)
        {
            for (int i = 0; i < 5; i++) _0xPanelesPasos[i].Visible = (i == p);
            _0xPasoActual = p;

            _0xBtnAnt.Enabled = (p > 0 && p < 3);
            if (p == 2)
            {
                _0xBtnSig.Text = "Instalar";
                _0xBtnSig.BackColor = Color.FromArgb(39, 174, 96);
            }
            else if (p == 3)
            {
                _0xBtnAnt.Enabled = false;
                _0xBtnSig.Enabled = false;
                _0xBtnCan.Enabled = false;
            }
            else if (p == 4)
            {
                _0xBtnAnt.Enabled = false;
                _0xBtnCan.Enabled = false;
                _0xBtnSig.Text = "Finalizar";
                _0xBtnSig.BackColor = Color.FromArgb(41, 128, 185);
                _0xBtnSig.Enabled = true;
            }
            else
            {
                _0xBtnSig.Text = "Siguiente >";
                _0xBtnSig.BackColor = Color.FromArgb(41, 128, 185);
                _0xBtnSig.Enabled = true;
            }
        }

        private void _0xAdvanceStep()
        {
            if (_0xPasoActual == 1)
            {
                _0xRutaDestino = _0xTxtRutaDestino.Text.Trim();
                _0xShowStep(2);
            }
            else if (_0xPasoActual == 2)
            {
                _0xShowStep(3);
                Thread t = new Thread(new ThreadStart(_0xExecuteInstallationWork));
                t.IsBackground = true;
                t.Start();
            }
            else if (_0xPasoActual == 4)
            {
                _0xCompleteAndExit();
            }
            else
            {
                _0xShowStep(_0xPasoActual + 1);
            }
        }

        private void _0xBackStep()
        {
            if (_0xPasoActual > 0) _0xShowStep(_0xPasoActual - 1);
        }

        private void _0xLog(string msg)
        {
            this.BeginInvoke(new Action(delegate
            {
                _0xLblEstado.Text = msg;
                _0xTxtLog.AppendText(msg + "\r\n");
            }));
        }

        private void _0xExecuteInstallationWork()
        {
            try
            {
                _0xLog("Verificando permisos en directorio: " + _0xRutaDestino);

                // Crear carpeta de destino de forma segura
                try
                {
                    if (!Directory.Exists(_0xRutaDestino)) Directory.CreateDirectory(_0xRutaDestino);
                }
                catch (UnauthorizedAccessException)
                {
                    // Fallback automático si no cuenta con privilegios en Program Files
                    string fallback = @"C:\MicorrizasSoft";
                    _0xLog("[!] Permiso denegado en Program Files. Reorientando automáticamente a: " + fallback);
                    _0xRutaDestino = fallback;
                    if (!Directory.Exists(_0xRutaDestino)) Directory.CreateDirectory(_0xRutaDestino);
                }

                this.BeginInvoke(new Action(delegate { _0xPrgBarra.Value = 20; }));

                string dirOrigen = AppDomain.CurrentDomain.BaseDirectory;
                string[] archivosPayload = new string[] {
                    "MicorrizasSoft.exe",
                    "config_seguridad.json",
                    "bd_microscopia.json",
                    "modelo_metadata.json",
                    "guia_cultivo.xml",
                    "app_icon.ico",
                    "app_icon.png"
                };

                Assembly assem = Assembly.GetExecutingAssembly();

                foreach (string arch in archivosPayload)
                {
                    string targetFile = Path.Combine(_0xRutaDestino, arch);
                    string localSource = Path.Combine(dirOrigen, arch);

                    if (File.Exists(localSource))
                    {
                        _0xLog("Desplegando " + arch + " (desde fuente local)...");
                        File.Copy(localSource, targetFile, true);
                    }
                    else
                    {
                        // Extraer desde recurso embebido autónomo
                        Stream resStream = assem.GetManifestResourceStream(arch);
                        if (resStream != null)
                        {
                            _0xLog("Extrayendo " + arch + " (desde recurso autónomo embebido)...");
                            using (FileStream fs = new FileStream(targetFile, FileMode.Create, FileAccess.Write))
                            {
                                resStream.CopyTo(fs);
                            }
                        }
                        else
                        {
                            _0xLog("Generando archivo predeterminado para " + arch + "...");
                            _0xWriteDefaultPayloadFile(targetFile, arch);
                        }
                    }
                }

                this.BeginInvoke(new Action(delegate { _0xPrgBarra.Value = 55; }));

                // Crear carpeta para resguardo de capturas de Petri
                string dirCaps = Path.Combine(_0xRutaDestino, "capturas_petri");
                if (!Directory.Exists(dirCaps)) Directory.CreateDirectory(dirCaps);

                // Generar script desinstalador limpio
                _0xLog("Generando script de desinstalación...");
                string uninstallerPath = Path.Combine(_0xRutaDestino, "Desinstalar_MicorrizasSoft.bat");
                string uninstallerCmd = string.Format(
                    "@echo off\r\n" +
                    "chcp 65001 > nul\r\n" +
                    "title Desinstalador MicorrizasSoft AI\r\n" +
                    "echo =====================================================\r\n" +
                    "echo           DESINSTALADOR DE MICORRIZASSOFT AI\r\n" +
                    "echo =====================================================\r\n" +
                    "echo.\r\n" +
                    "echo Eliminando accesos directos...\r\n" +
                    "del \"%USERPROFILE%\\Desktop\\MicorrizasSoft AI.lnk\" >nul 2>&1\r\n" +
                    "del \"%APPDATA%\\Microsoft\\Windows\\Start Menu\\Programs\\MicorrizasSoft\\*.lnk\" >nul 2>&1\r\n" +
                    "rd /s /q \"%APPDATA%\\Microsoft\\Windows\\Start Menu\\Programs\\MicorrizasSoft\" >nul 2>&1\r\n" +
                    "echo Eliminando archivos del sistema...\r\n" +
                    "cd ..\r\n" +
                    "rd /s /q \"{0}\"\r\n" +
                    "echo.\r\n" +
                    "echo [OK] Desinstalacion completada con exito.\r\n" +
                    "pause\r\n",
                    _0xRutaDestino
                );
                File.WriteAllText(uninstallerPath, uninstallerCmd, Encoding.UTF8);

                this.BeginInvoke(new Action(delegate { _0xPrgBarra.Value = 75; }));

                // Accesos directos con icono institucional
                string exeTarget = Path.Combine(_0xRutaDestino, "MicorrizasSoft.exe");
                string iconTarget = Path.Combine(_0xRutaDestino, "app_icon.ico");
                if (!File.Exists(iconTarget)) iconTarget = exeTarget;

                if (_0xChkEscritorio.Checked)
                {
                    _0xLog("Generando acceso directo en el Escritorio...");
                    string desktopDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                    _0xCreateDesktopShortcut(Path.Combine(desktopDir, "MicorrizasSoft AI.lnk"), exeTarget, _0xRutaDestino, iconTarget);
                }

                if (_0xChkInicio.Checked)
                {
                    _0xLog("Generando acceso directo en el Menú Inicio...");
                    string startMenu = Environment.GetFolderPath(Environment.SpecialFolder.Programs);
                    string smDir = Path.Combine(startMenu, "MicorrizasSoft");
                    if (!Directory.Exists(smDir)) Directory.CreateDirectory(smDir);
                    _0xCreateDesktopShortcut(Path.Combine(smDir, "MicorrizasSoft AI.lnk"), exeTarget, _0xRutaDestino, iconTarget);
                }

                this.BeginInvoke(new Action(delegate
                {
                    _0xPrgBarra.Value = 100;
                    _0xLog("¡Despliegue y configuración completados con éxito!");
                    _0xShowStep(4);
                }));
            }
            catch (Exception ex)
            {
                this.BeginInvoke(new Action(delegate
                {
                    MessageBox.Show("Error durante la instalación:\n" + ex.Message, "Error de Instalación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _0xBtnCan.Enabled = true;
                }));
            }
        }

        private void _0xWriteDefaultPayloadFile(string target, string arch)
        {
            try
            {
                if (arch == "config_seguridad.json")
                {
                    File.WriteAllText(target, "{\r\n  \"admin_hash\": \"b2d6d109f84194d39a0c1c64e21355869427e96370e48f0f340decd695db3540\",\r\n  \"api_key\": \"AIzaSyCjV1O6654oiUcuZXbbH4GwWtcg0nKOrYE\",\r\n  \"gemini_api_key\": \"AIzaSyCjV1O6654oiUcuZXbbH4GwWtcg0nKOrYE\"\r\n}", Encoding.UTF8);
                }
                else if (arch == "modelo_metadata.json")
                {
                    File.WriteAllText(target, "{\"accuracy\": 0.942, \"version\": \"2.5-ResNet-C#\"}", Encoding.UTF8);
                }
                else if (arch == "guia_cultivo.xml")
                {
                    File.WriteAllText(target, "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<GuiaCultivo>\r\n    <Texto>--- GUIA GENERAL DE CULTIVO DE MICORRIZAS ---\r\n1. Parametros Termicos: 20-25°C constante.\r\n2. Humedad: HR en camara 85% a 95%.\r\n3. Ausencia estricta de colonias burbujeantes o exudados bacterianos.\r\n4. Registrar evaluaciones en bitacora digital.\r\n</Texto>\r\n</GuiaCultivo>", Encoding.UTF8);
                }
                else if (arch == "bd_microscopia.json")
                {
                    File.WriteAllText(target, "[\r\n  {\r\n    \"id\": 0,\r\n    \"nombre\": \"Rhizophagus irregularis (Glomus)\",\r\n    \"dias_infeccion\": \"21 días\",\r\n    \"tratamiento\": \"Inoculación en sustrato vermiculita y arena estéril (1:1). Riego con solución nutritiva Long Ashton modificada con bajo fósforo (20 ppm). Temperatura diurna 24°C, nocturna 18°C. Humedad 85%.\",\r\n    \"respuesta_ia\": \"Morfología confirmada: Presencia de hifas cenocíticas ramificadas y vesículas intra-radicales globosas de 40-70 µm. Ausencia de colonias bacterianas burbujeantes. Cultivo viable y de alta pureza.\",\r\n    \"fecha\": \"2026-10-15\"\r\n  }\r\n]", Encoding.UTF8);
                }
            }
            catch { }
        }

        private void _0xCreateDesktopShortcut(string linkPath, string targetPath, string workDir, string iconPath)
        {
            try
            {
                string psCmd = string.Format(
                    "$w = New-Object -COM WScript.Shell; " +
                    "$s = $w.CreateShortcut('{0}'); " +
                    "$s.TargetPath = '{1}'; " +
                    "$s.WorkingDirectory = '{2}'; " +
                    "$s.IconLocation = '{3},0'; " +
                    "$s.Save()",
                    linkPath.Replace("'", "''"),
                    targetPath.Replace("'", "''"),
                    workDir.Replace("'", "''"),
                    iconPath.Replace("'", "''")
                );

                ProcessStartInfo psi = new ProcessStartInfo("powershell", "-NoProfile -ExecutionPolicy Bypass -Command \"" + psCmd + "\"");
                psi.CreateNoWindow = true;
                psi.UseShellExecute = false;
                Process p = Process.Start(psi);
                p.WaitForExit(3000);
            }
            catch { }
        }

        private void _0xCompleteAndExit()
        {
            if (_0xChkEjecutarAlFinalizar.Checked)
            {
                string exe = Path.Combine(_0xRutaDestino, "MicorrizasSoft.exe");
                if (File.Exists(exe))
                {
                    try
                    {
                        ProcessStartInfo psi = new ProcessStartInfo(exe);
                        psi.WorkingDirectory = _0xRutaDestino;
                        Process.Start(psi);
                    }
                    catch { }
                }
            }
            this.Close();
        }
    }
}
