using FI.AtividadeEntrevista.DML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoBeneficiario
    {
        /// <summary>
        /// Inclui um novo beneficiário
        /// </summary>
        /// <param name="beneficiario">Objeto de beneficiário</param>
        public void Incluir(DML.Beneficiario beneficiario)
        {
            DAL.DaoCliente dao = new DAL.DaoCliente();
            dao.AdicionarBeneficiario(beneficiario);
        }

        /// <summary>
        /// Altera um beneficiário existente
        /// </summary>
        /// <param name="beneficiario">Objeto de beneficiário com as novas informações</param>
        public void Alterar(DML.Beneficiario beneficiario)
        {
            DAL.DaoCliente dao = new DAL.DaoCliente();
            dao.AlterarBeneficiario(beneficiario);
        }

        /// <summary>
        /// Consulta beneficiários associados a um cliente
        /// </summary>
        /// <param name="idCliente">ID do cliente para o qual os beneficiários devem ser consultados</param>
        /// <returns>Lista de beneficiários associados ao cliente</returns>
        public List<DML.Beneficiario> Consultar(long idCliente)
        {
            DAL.DaoCliente dao = new DAL.DaoCliente();
            return dao.ConsultarBeneficiarios(idCliente);
        }

        /// <summary>
        /// Adiciona uma lista de beneficiários ao banco de dados
        /// </summary>
        /// <param name="idCliente">ID do cliente associado</param>
        /// <param name="beneficiarios">Lista de beneficiários</param>
        public void AdicionarBeneficiarios(long idCliente, List<Beneficiario> beneficiarios)
        {
            DAL.DaoCliente dao = new DAL.DaoCliente();
            dao.AdicionarBeneficiarios(idCliente, beneficiarios);
        }

        /// <summary>
        /// Lista os beneficiários com paginação e ordenação para um cliente específico
        /// </summary>
        /// <param name="idCliente">ID do cliente para filtrar os beneficiários</param>
        /// <param name="iniciarEm">Índice de início para paginação</param>
        /// <param name="quantidade">Quantidade de registros a serem retornados</param>
        /// <param name="campoOrdenacao">Campo pelo qual os registros serão ordenados</param>
        /// <param name="crescente">Define a ordem crescente (true) ou decrescente (false)</param>
        /// <param name="qtd">Quantidade total de registros</param>
        /// <returns>Lista de beneficiários</returns>
        public List<DML.Beneficiario> PesquisaBeneficiarios(long idCliente, int iniciarEm, int quantidade, string campoOrdenacao, bool crescente, out int qtd)
        {
            DAL.DaoCliente dao = new DAL.DaoCliente();
            return dao.PesquisaBeneficiarios(idCliente, iniciarEm, quantidade, campoOrdenacao, crescente, out qtd);
        }


        /// <summary>
        /// Consulta o beneficiário pelo ID
        /// </summary>
        /// <param name="id">ID do beneficiário</param>
        /// <returns>Objeto do beneficiário</returns>
        public Beneficiario ConsultarBeneficiarioPorId(long id)
        {
            DAL.DaoCliente dao = new DAL.DaoCliente();
            return dao.ConsultarBeneficiarioPorId(id);
        }


    }
}
