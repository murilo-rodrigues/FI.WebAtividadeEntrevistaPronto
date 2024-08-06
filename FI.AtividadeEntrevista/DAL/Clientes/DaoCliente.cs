using FI.AtividadeEntrevista.DML;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace FI.AtividadeEntrevista.DAL
{
    /// <summary>
    /// Classe de acesso a dados de Cliente
    /// </summary>
    internal class DaoCliente : AcessoDados
    {
        /// <summary>
        /// Inclui um novo cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        internal long Incluir(DML.Cliente cliente)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("Nome", cliente.Nome));
            parametros.Add(new System.Data.SqlClient.SqlParameter("Sobrenome", cliente.Sobrenome));
            parametros.Add(new System.Data.SqlClient.SqlParameter("Nacionalidade", cliente.Nacionalidade));
            parametros.Add(new System.Data.SqlClient.SqlParameter("CEP", cliente.CEP));
            parametros.Add(new System.Data.SqlClient.SqlParameter("Estado", cliente.Estado));
            parametros.Add(new System.Data.SqlClient.SqlParameter("Cidade", cliente.Cidade));
            parametros.Add(new System.Data.SqlClient.SqlParameter("Logradouro", cliente.Logradouro));
            parametros.Add(new System.Data.SqlClient.SqlParameter("Email", cliente.Email));
            parametros.Add(new System.Data.SqlClient.SqlParameter("Telefone", cliente.Telefone));
            parametros.Add(new System.Data.SqlClient.SqlParameter("CPF", cliente.CPF));

            DataSet ds = base.Consultar("FI_SP_IncClienteV2", parametros);
            long ret = 0;
            if (ds.Tables[0].Rows.Count > 0)
                long.TryParse(ds.Tables[0].Rows[0][0].ToString(), out ret);
            return ret;
        }

        /// <summary>
        /// Inclui um novo cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        internal DML.Cliente Consultar(long Id)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("Id", Id));

            DataSet ds = base.Consultar("FI_SP_ConsCliente", parametros);
            List<DML.Cliente> cli = Converter(ds);

            return cli.FirstOrDefault();
        }

        internal bool VerificarExistencia(string CPF)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("CPF", CPF));

            DataSet ds = base.Consultar("FI_SP_VerificaCliente", parametros);

            return ds.Tables[0].Rows.Count > 0;
        }

        internal List<Cliente> Pesquisa(int iniciarEm, int quantidade, string campoOrdenacao, bool crescente, out int qtd)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("iniciarEm", iniciarEm));
            parametros.Add(new System.Data.SqlClient.SqlParameter("quantidade", quantidade));
            parametros.Add(new System.Data.SqlClient.SqlParameter("campoOrdenacao", campoOrdenacao));
            parametros.Add(new System.Data.SqlClient.SqlParameter("crescente", crescente));

            DataSet ds = base.Consultar("FI_SP_PesqCliente", parametros);
            List<DML.Cliente> cli = Converter(ds);

            int iQtd = 0;

            if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                int.TryParse(ds.Tables[1].Rows[0][0].ToString(), out iQtd);

            qtd = iQtd;

            return cli;
        }

        /// <summary>
        /// Lista todos os clientes
        /// </summary>
        internal List<DML.Cliente> Listar()
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("Id", 0));

            DataSet ds = base.Consultar("FI_SP_ConsCliente", parametros);
            List<DML.Cliente> cli = Converter(ds);

            return cli;
        }

        /// <summary>
        /// Inclui um novo cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        internal void Alterar(DML.Cliente cliente)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("Nome", cliente.Nome));
            parametros.Add(new System.Data.SqlClient.SqlParameter("Sobrenome", cliente.Sobrenome));
            parametros.Add(new System.Data.SqlClient.SqlParameter("Nacionalidade", cliente.Nacionalidade));
            parametros.Add(new System.Data.SqlClient.SqlParameter("CEP", cliente.CEP));
            parametros.Add(new System.Data.SqlClient.SqlParameter("Estado", cliente.Estado));
            parametros.Add(new System.Data.SqlClient.SqlParameter("Cidade", cliente.Cidade));
            parametros.Add(new System.Data.SqlClient.SqlParameter("Logradouro", cliente.Logradouro));
            parametros.Add(new System.Data.SqlClient.SqlParameter("Email", cliente.Email));
            parametros.Add(new System.Data.SqlClient.SqlParameter("Telefone", cliente.Telefone));
            parametros.Add(new System.Data.SqlClient.SqlParameter("CPF", cliente.CPF));
            parametros.Add(new System.Data.SqlClient.SqlParameter("ID", cliente.Id));

            base.Executar("FI_SP_AltCliente", parametros);
        }
        public List<DML.Beneficiario> PesquisaBeneficiarios(long idCliente, int iniciarEm, int quantidade, string campoOrdenacao, bool crescente, out int qtd)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("@IdCliente", idCliente));
            parametros.Add(new System.Data.SqlClient.SqlParameter("@iniciarEm", iniciarEm));
            parametros.Add(new System.Data.SqlClient.SqlParameter("@quantidade", quantidade));
            parametros.Add(new System.Data.SqlClient.SqlParameter("@campoOrdenacao", campoOrdenacao));
            parametros.Add(new System.Data.SqlClient.SqlParameter("@crescente", crescente));

            DataSet ds = base.Consultar("FI_SP_PesqBeneficiarios", parametros);
            List<DML.Beneficiario> beneficiarios = ConverterBeneficiario(ds);

            int iQtd = 0;

            if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                int.TryParse(ds.Tables[1].Rows[0][0].ToString(), out iQtd);

            qtd = iQtd;

            return beneficiarios;
        }


        /// <summary>
        /// Adiciona um novo beneficiário
        /// </summary>
        /// <param name="beneficiario">Objeto de beneficiário</param>
        public void AdicionarBeneficiario(Beneficiario beneficiario)
        {
            // Prepara os parâmetros
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("@IdCLIENTE", SqlDbType.BigInt) { Value = beneficiario.IdCliente },
                new SqlParameter("@CPF", SqlDbType.VarChar, 14) { Value = beneficiario.CPF },
                new SqlParameter("@Nome", SqlDbType.VarChar, 50) { Value = beneficiario.Nome }
            };

            // Chama a procedure usando o método Executar
            Executar("FI_SP_IncBeneficiario", parametros);
        }

        /// <summary>
        /// Consulta os beneficiários pelo id do cliente
        /// </summary>
        /// <param name="idCliente">id do cliente</param>
        /// <returns>Lista de beneficiários</returns>
        public List<Beneficiario> ConsultarBeneficiarios(long idCliente)
        {
            // Prepara os parâmetros
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("@IdCLIENTE", idCliente)
            };

            DataSet ds = Consultar("FI_SP_ConsBeneficiarios", parametros);
            return ConverterBeneficiario(ds);
        }

        /// <summary>
        /// Altera um beneficiário existente
        /// </summary>
        /// <param name="beneficiario">Objeto de beneficiário</param>
        public void AlterarBeneficiario(Beneficiario beneficiario)
        {
            // Prepara os parâmetros
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("@Id", SqlDbType.BigInt) { Value = beneficiario.Id },
                new SqlParameter("@IdCLIENTE", SqlDbType.BigInt) { Value = beneficiario.IdCliente },
                new SqlParameter("@CPF", SqlDbType.VarChar, 14) { Value = beneficiario.CPF },
                new SqlParameter("@Nome", SqlDbType.VarChar, 255) { Value = beneficiario.Nome }
            };

            // Chama a procedure usando o método Executar
            Executar("FI_SP_AltBeneficiario", parametros);
        }

        public Beneficiario ConsultarBeneficiarioPorId(long id)
        {
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("@Id", id)
            };

            DataSet ds = base.Consultar("FI_SP_ConsBeneficiarioPorId", parametros);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                Beneficiario beneficiario = new Beneficiario
                {
                    Id = Convert.ToInt64(row["Id"]),
                    IdCliente = Convert.ToInt64(row["IdCLIENTE"]),
                    CPF = Convert.ToString(row["CPF"]),
                    Nome = Convert.ToString(row["Nome"])
                };
                return beneficiario;
            }
            return null;
        }

        /// <summary>
        /// Adiciona uma lista de beneficiários ao banco de dados
        /// </summary>
        /// <param name="idCliente">ID do cliente associado</param>
        /// <param name="beneficiarios">Lista de beneficiários</param>
        public void AdicionarBeneficiarios(long idCliente, List<Beneficiario> beneficiarios)
        {
            // Cria um DataTable para armazenar a lista de beneficiários
            DataTable beneficiariosTable = CriarDataTableBeneficiarios(beneficiarios);

            // Prepara os parâmetros
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("@IdCLIENTE", SqlDbType.BigInt) { Value = idCliente },
                new SqlParameter("@Beneficiarios", SqlDbType.Structured)
                {
                    TypeName = "dbo.BeneficiariosType", // Nome do tipo de tabela definido no SQL Server
                    Value = beneficiariosTable
                }
            };

            // Chama a procedure usando o método Executar
            // A procedure sempre exclui todos os registros de beneficiarios deste cliente para adicionar novos, assim mantendo sempre atualizado
            // como este é um sistema de teste estou fazendo assim, porém em um sistema real seria necessario uma verificação para atualizar os registros que ja existem
            // e incluir apenas os que não existem para que as amarrações feitas com os ids dos beneficiarios não tenham problemas.
            Executar("FI_SP_IncBeneficiarios", parametros);
        }

        private DataTable CriarDataTableBeneficiarios(List<Beneficiario> beneficiarios)
        {
            DataTable table = new DataTable();
            table.Columns.Add("CPF", typeof(string));
            table.Columns.Add("Nome", typeof(string));

            foreach (var beneficiario in beneficiarios)
            {
                table.Rows.Add(beneficiario.CPF, beneficiario.Nome);
            }

            return table;
        }



        /// <summary>
        /// Excluir Cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        internal void Excluir(long Id)
        {
            List<System.Data.SqlClient.SqlParameter> parametros = new List<System.Data.SqlClient.SqlParameter>();

            parametros.Add(new System.Data.SqlClient.SqlParameter("Id", Id));

            base.Executar("FI_SP_DelCliente", parametros);
        }

        private List<DML.Cliente> Converter(DataSet ds)
        {
            List<DML.Cliente> lista = new List<DML.Cliente>();
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DML.Cliente cli = new DML.Cliente();
                    cli.Id = row.Field<long>("Id");
                    cli.CEP = row.Field<string>("CEP");
                    cli.Cidade = row.Field<string>("Cidade");
                    cli.Email = row.Field<string>("Email");
                    cli.Estado = row.Field<string>("Estado");
                    cli.Logradouro = row.Field<string>("Logradouro");
                    cli.Nacionalidade = row.Field<string>("Nacionalidade");
                    cli.Nome = row.Field<string>("Nome");
                    cli.Sobrenome = row.Field<string>("Sobrenome");
                    cli.Telefone = row.Field<string>("Telefone");
                    cli.CPF = row.Field<string>("CPF");
                    lista.Add(cli);
                }
            }

            return lista;
        }

        private List<Beneficiario> ConverterBeneficiario(DataSet ds)
        {
            List<Beneficiario> lista = new List<Beneficiario>();
            if (ds != null && ds.Tables != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    Beneficiario ben = new Beneficiario();
                    ben.Id = row.Field<long>("Id");
                    ben.CPF = row.Field<string>("CPF");
                    ben.Nome = row.Field<string>("Nome");
                    ben.IdCliente = row.Field<long>("IdCliente"); // Adapte conforme o nome da coluna na tabela de beneficiários
                    lista.Add(ben);
                }
            }

            return lista;
        }
    }
}
