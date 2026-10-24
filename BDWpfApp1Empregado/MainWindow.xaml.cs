using Microsoft.Data.SqlClient;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;

namespace BDWpfApp1Empregado
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection? conexao = null;

        BindingList<Empregado> objClasseListDataGridEmpregado = new();

        public MainWindow()
        {
            InitializeComponent();

            DataGridEmpregadoList.ItemsSource = objClasseListDataGridEmpregado;

            string URL = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DBEmpregadoGUI;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

            try
            {
                conexao = new(URL);
                conexao.Open();
                CriarTabelaEmpregado();
                LabelStatus.Content = "Conexão OK";
            }
            catch (Exception e)
            {
                // a conexao foi criada mas nao abriu, entao nao pode ser usada
                conexao?.Dispose();
                conexao = null;
                LabelStatus.Content = "Conexão NOT";
                Debug.WriteLine(e.Message);
            }
        }

        private void ButtonSalvar_Click(object sender, RoutedEventArgs e)
        {
            Empregado emp =  new()
            {
                CPF = TextBoxCPF.Text,
                Nome = TextBoxNome.Text,
                Endereco = TextBoxEndereco.Text
            };
            if (conexao == null)
            {
                LabelStatus.Content = "Sem conexão";
                return;
            }
            if (TextBoxMatricula.Text != "")
            {
                LabelStatus.Content = "Limpe a matrícula";
                return;
            }
            if (!VerificarCampos())
            {
                LabelStatus.Content = "Preencha os campos";
                return;
            }

            try
            {
                emp.Salvar(conexao);
                LabelStatus.Content = "Salvo com sucesso";
                LimparCampos();

                List<Empregado>? ret = emp.Pesquisar(0, conexao);
                if (ret != null)
                {
                    objClasseListDataGridEmpregado = new(ret);
                    DataGridEmpregadoList.ItemsSource = objClasseListDataGridEmpregado;
                }
            }
            catch (Exception ex)
            {
                LabelStatus.Content = "Erro ao salvar";
                Debug.WriteLine(ex.Message);
            }
        }

        // Cria a tabela Empregado no banco caso ela ainda nao exista
        private void CriarTabelaEmpregado()
        {
            var Cmd = conexao!.CreateCommand();
            Cmd.CommandText = @"IF OBJECT_ID('dbo.Empregado', 'U') IS NULL
                CREATE TABLE [dbo].[Empregado] (
                    [Matricula] INT            IDENTITY (1, 1) NOT NULL PRIMARY KEY,
                    [CPF]       NVARCHAR (MAX) NULL,
                    [Nome]      NVARCHAR (MAX) NULL,
                    [Endereco]  NVARCHAR (MAX) NULL
                );";
            Cmd.ExecuteNonQuery();
        }

        private bool VerificarCampos()
        {
            return TextBoxCPF.Text != "" &&
                   TextBoxNome.Text != "" &&
                   TextBoxEndereco.Text != "";
        }

        private void LimparCampos()
        {
            TextBoxMatricula.Text = ""; TextBoxCPF.Text = "";
            TextBoxNome.Text = ""; TextBoxEndereco.Text = "";
        }

        private void ButtonPesquisar_Click(object sender, RoutedEventArgs e)
        {
            Empregado emp = new();

            if (conexao == null)
            {
                LabelStatus.Content = "Sem conexão";
                return;
            }

            int matricula = 0;
            if (TextBoxMatricula.Text != "" && !int.TryParse(TextBoxMatricula.Text, out matricula))
            {
                LabelStatus.Content = "Matrícula inválida";
                return;
            }

            try
            {
                List<Empregado>? ret = emp.Pesquisar(matricula, conexao);

                if (ret != null)
                {
                    LabelStatus.Content = "Pesquisa OK";

                    objClasseListDataGridEmpregado = new(ret);
                    DataGridEmpregadoList.ItemsSource = objClasseListDataGridEmpregado;
                }
            }
            catch (Exception ex)
            {
                LabelStatus.Content = "Erro na pesquisa";
                Debug.WriteLine(ex.Message);
            }

            LimparCampos();
        }

        private void ButtonExcluir_Click(object sender, RoutedEventArgs e)
        {
            if (conexao == null)
            {
                LabelStatus.Content = "Sem conexão";
                return;
            }
            if (!int.TryParse(TextBoxMatricula.Text, out int matricula))
            {
                LabelStatus.Content = "Matrícula inválida";
                return;
            }
            if (MessageBox.Show($"Excluir a matrícula {matricula}?", "Confirmar",
                                MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                return;

            try
            {
                Empregado emp = new() { Matricula = matricula };

                if (emp.Excluir(conexao) == 0)
                {
                    LabelStatus.Content = "Não encontrado";
                    return;
                }

                LabelStatus.Content = "Excluído com sucesso";
                LimparCampos();

                List<Empregado>? ret = emp.Pesquisar(0, conexao);
                if (ret != null)
                {
                    objClasseListDataGridEmpregado = new(ret);
                    DataGridEmpregadoList.ItemsSource = objClasseListDataGridEmpregado;
                }
            }
            catch (Exception ex)
            {
                LabelStatus.Content = "Erro ao excluir";
                Debug.WriteLine(ex.Message);
            }
        }

        private void ButtonLimpar_Click(object sender, RoutedEventArgs e)
        {
            LimparCampos();
            LabelStatus.Content = "Status";
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            conexao?.Dispose();
        }
    }
}

