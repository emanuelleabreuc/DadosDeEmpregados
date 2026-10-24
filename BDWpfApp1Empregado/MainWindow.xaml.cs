using Microsoft.Data.SqlClient;
using System.Text;
using System.Windows;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
                LabelStatus.Content = "Conexão NOT";
                Console.WriteLine(e.Message);
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
            if (conexao != null && VerificarCampos())
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
            else
            {
                LabelStatus.Content = "Erro ao salvar";
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
            return TextBoxMatricula.Text == "" && // se a matricula estiver vazia, é um novo registro, então não precisa verificar
                   TextBoxCPF.Text != "" &&
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

            List<Empregado>? ret = null;

            if (conexao != null)
            {
                if (TextBoxMatricula.Text == "")
                    ret = emp.Pesquisar(0, conexao);
                else
                    ret = emp.Pesquisar(int.Parse(TextBoxMatricula.Text), conexao);
            }

            if (ret != null)
            {
                LabelStatus.Content = "Pesquisa OK";

                objClasseListDataGridEmpregado = new(ret);
                DataGridEmpregadoList.ItemsSource = objClasseListDataGridEmpregado;
            }

            LimparCampos();
        }
        }
    }
