using FI.AtividadeEntrevista.BLL;
using WebAtividadeEntrevista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FI.AtividadeEntrevista.DML;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace WebAtividadeEntrevista.Controllers
{
    public class ClienteController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Incluir()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Incluir(ClienteModel clienteModel, List<BeneficiarioModel> beneficiarios)
        {
            BoCliente boCliente = new BoCliente();
            BoBeneficiario boBeneficiario = new BoBeneficiario();

            // Verificar se o CPF já existe
            if (boCliente.VerificarExistencia(clienteModel.CPF))
            {
                Response.StatusCode = 400;
                return Json("CPF já cadastrado, cadastre outro CPF.");
            }

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                // Adiciona o novo cliente e obtém o ID
                long idCliente = boCliente.Incluir(new Cliente()
                {
                    CEP = clienteModel.CEP,
                    Cidade = clienteModel.Cidade,
                    Email = clienteModel.Email,
                    Estado = clienteModel.Estado,
                    Logradouro = clienteModel.Logradouro,
                    Nacionalidade = clienteModel.Nacionalidade,
                    Nome = clienteModel.Nome,
                    Sobrenome = clienteModel.Sobrenome,
                    Telefone = clienteModel.Telefone,
                    CPF = clienteModel.CPF
                });

                // Prepara a lista de beneficiários
                List<Beneficiario> objBeneficiarios = new List<Beneficiario>();
                if (beneficiarios != null)
                {
                    foreach (var itemBeneficiario in beneficiarios)
                    {
                        objBeneficiarios.Add(new Beneficiario()
                        {
                            Nome = itemBeneficiario.Nome,
                            CPF = itemBeneficiario.CPF,
                            IdCliente = idCliente // Associar ao ID do cliente recém-adicionado
                        });
                    }
                }

                // Adiciona os beneficiários ao cliente
                if (objBeneficiarios.Any())
                {
                    boBeneficiario.AdicionarBeneficiarios(idCliente, objBeneficiarios);
                }

                return Json("Cadastro efetuado com sucesso");
            }
        }

        [HttpPost]
        public JsonResult Alterar(ClienteModel clienteModel, List<BeneficiarioModel> beneficiarios)
        {
            BoCliente boCliente = new BoCliente();
            BoBeneficiario boBeneficiario = new BoBeneficiario();

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                // Consulta o cliente existente para obter o CPF atual
                var clienteExistente = boCliente.Consultar(clienteModel.Id);
                if (clienteExistente == null)
                {
                    Response.StatusCode = 404;
                    return Json("Cliente não encontrado.");
                }

                // Verifica se o novo CPF é diferente do CPF atual e se já está cadastrado
                if (clienteModel.CPF != clienteExistente.CPF && boCliente.VerificarExistencia(clienteModel.CPF))
                {
                    Response.StatusCode = 400;
                    return Json("CPF já cadastrado, cadastre outro CPF.");
                }

                // Atualiza os dados do cliente
                boCliente.Alterar(new Cliente()
                {
                    Id = clienteModel.Id,
                    CEP = clienteModel.CEP,
                    Cidade = clienteModel.Cidade,
                    Email = clienteModel.Email,
                    Estado = clienteModel.Estado,
                    Logradouro = clienteModel.Logradouro,
                    Nacionalidade = clienteModel.Nacionalidade,
                    Nome = clienteModel.Nome,
                    Sobrenome = clienteModel.Sobrenome,
                    Telefone = clienteModel.Telefone,
                    CPF = clienteModel.CPF
                });

                List<Beneficiario> objBeneficiarios = new List<Beneficiario>();
                foreach (var itemBeneficiario in beneficiarios)
                {
                    objBeneficiarios.Add(new Beneficiario(){Id = itemBeneficiario.Id, Nome = itemBeneficiario.Nome, CPF = itemBeneficiario.CPF, IdCliente = itemBeneficiario.IdCliente });
                }
               
                // Atualiza os beneficiários
                if (beneficiarios != null && beneficiarios.Any())
                {
                    boBeneficiario.AdicionarBeneficiarios(clienteModel.Id, objBeneficiarios);
                }

                return Json("Cadastro alterado com sucesso");
            }
        }

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            BoCliente bo = new BoCliente();
            Cliente cliente = bo.Consultar(id);
            Models.ClienteModel model = null;

            if (cliente != null)
            {
                model = new ClienteModel()
                {
                    Id = cliente.Id,
                    CEP = cliente.CEP,
                    Cidade = cliente.Cidade,
                    Email = cliente.Email,
                    Estado = cliente.Estado,
                    Logradouro = cliente.Logradouro,
                    Nacionalidade = cliente.Nacionalidade,
                    Nome = cliente.Nome,
                    Sobrenome = cliente.Sobrenome,
                    Telefone = cliente.Telefone,
                    CPF = cliente.CPF
                };

            
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                int qtd = 0;
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = jtSorting.Split(' ');

                if (array.Length > 0)
                    campo = array[0];

                if (array.Length > 1)
                    crescente = array[1];

                List<Cliente> clientes = new BoCliente().Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

                //Return result to jTable
                return Json(new { Result = "OK", Records = clientes, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult IncluirBeneficiario(BeneficiarioModel model)
        {
            try
            {
                BoBeneficiario bo = new BoBeneficiario();
                bo.Incluir(new Beneficiario()
                {
                    Nome = model.Nome,
                    CPF = model.CPF,
                    IdCliente = model.IdCliente
                });

                return Json("Beneficiário incluído com sucesso");
            }
            catch (SqlException ex)
            {
                // Verifica se o erro é o erro de CPF já existente
                if (ex.Message.Contains("O CPF já consta na base."))
                {
                    Response.StatusCode = 400;
                    return Json("O CPF já consta na base.");
                }
                else
                {
                    Response.StatusCode = 500;
                    return Json("Erro interno do servidor.");
                }
            }
        }


        [HttpPost]
        public JsonResult AlterarBeneficiario(BeneficiarioModel model)
        {
            BoBeneficiario bo = new BoBeneficiario();

            if (!this.ModelState.IsValid)
            {
                List<string> erros = (from item in ModelState.Values
                                      from error in item.Errors
                                      select error.ErrorMessage).ToList();

                Response.StatusCode = 400;
                return Json(string.Join(Environment.NewLine, erros));
            }
            else
            {
                bo.Alterar(new Beneficiario()
                {
                    Id = model.Id,
                    IdCliente = model.IdCliente,
                    CPF = model.CPF,
                    Nome = model.Nome
                });

                return Json("Cadastro de beneficiário alterado com sucesso");
            }
        }

        [HttpGet]
        public ActionResult AlterarBeneficiario(long id)
        {
            BoBeneficiario bo = new BoBeneficiario();
            Beneficiario beneficiario = bo.ConsultarBeneficiarioPorId(id);
            BeneficiarioModel model = null;

            if (beneficiario != null)
            {
                model = new BeneficiarioModel()
                {
                    Id = beneficiario.Id,
                    IdCliente = beneficiario.IdCliente,
                    CPF = beneficiario.CPF,
                    Nome = beneficiario.Nome
                };
            }

            return View(model);
        }

        [HttpPost]
        public JsonResult BeneficiarioList(long idCliente, int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                int qtd = 0;
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = jtSorting.Split(' ');

                if (array.Length > 0)
                    campo = array[0];

                if (array.Length > 1)
                    crescente = array[1];

                // Passa o idCliente para o método de pesquisa
                List<Beneficiario> beneficiarios = new BoBeneficiario().PesquisaBeneficiarios(idCliente, jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

                // Retorna o resultado para o jTable
                return Json(new { Result = "OK", Records = beneficiarios, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }


    }
}