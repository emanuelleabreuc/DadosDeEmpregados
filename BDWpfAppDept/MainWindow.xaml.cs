using Microsoft.Data.SqlClient;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;

namespace BDWpfAppDept
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection? conexao = null;

        BindingList<Dept> objClasseListDataGridDept = new();

        public MainWindow()
        {
            InitializeComponent();

            DataGridDeptList.ItemsSource = objClasseListDataGridDept;

            string URL = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ExerciciosDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

            try
            {
                conexao = new(URL);
                conexao.Open();
                CriarTabelaDept();
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

        // Cria a tabela DEPT no banco caso ela ainda nao exista
        private void CriarTabelaDept()
        {
            using var Cmd = conexao!.CreateCommand();
            Cmd.CommandText = @"IF OBJECT_ID('dbo.DEPT', 'U') IS NULL
                CREATE TABLE [dbo].[DEPT] (
                    [id]    INT          IDENTITY (1, 1) NOT NULL,
                    [name]  VARCHAR (30) NOT NULL,
                    [email] VARCHAR (30) NOT NULL,
                    CONSTRAINT [DEPT_PK] PRIMARY KEY ([id])
                );";
            Cmd.ExecuteNonQuery();
        }

        private void ButtonSalvar_Click(object sender, RoutedEventArgs e)
        {
            Dept dept = new()
            {
                Name = TextBoxName.Text,
                Email = TextBoxEmail.Text
            };
            if (conexao == null)
            {
                LabelStatus.Content = "Sem conexão";
                return;
            }
            if (TextBoxId.Text != "")
            {
                LabelStatus.Content = "Limpe o id";
                return;
            }
            if (!VerificarCampos())
            {
                LabelStatus.Content = "Preencha os campos";
                return;
            }

            try
            {
                dept.Salvar(conexao);
                LabelStatus.Content = "Salvo OK";
                LimparCampos();

                List<Dept>? ret = dept.Recuperar(0, conexao);
                if (ret != null)
                {
                    objClasseListDataGridDept = new(ret);
                    DataGridDeptList.ItemsSource = objClasseListDataGridDept;
                }
            }
            catch (Exception ex)
            {
                LabelStatus.Content = "Salvo NOT";
                Debug.WriteLine(ex.Message);
            }
        }

        private void ButtonRecuperar_Click(object sender, RoutedEventArgs e)
        {
            Dept dept = new();

            if (conexao == null)
            {
                LabelStatus.Content = "Sem conexão";
                return;
            }

            int id = 0;
            if (TextBoxId.Text != "" && !int.TryParse(TextBoxId.Text, out id))
            {
                LabelStatus.Content = "Id inválido";
                return;
            }

            try
            {
                List<Dept>? ret = dept.Recuperar(id, conexao);

                if (ret != null)
                {
                    LabelStatus.Content = "Recuperar OK";

                    objClasseListDataGridDept = new(ret);
                    DataGridDeptList.ItemsSource = objClasseListDataGridDept;
                }
            }
            catch (Exception ex)
            {
                LabelStatus.Content = "Recuperar NOT";
                Debug.WriteLine(ex.Message);
            }

            LimparCampos();
        }

        private bool VerificarCampos()
        {
            return TextBoxName.Text != "" &&
                   TextBoxEmail.Text != "";
        }

        private void LimparCampos()
        {
            TextBoxId.Text = ""; TextBoxName.Text = ""; TextBoxEmail.Text = "";
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
