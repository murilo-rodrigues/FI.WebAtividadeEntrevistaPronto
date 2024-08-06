var IdCliente = 0;
let beneficiarios = [];
let currentPage = 1;
const pageSize = 5;
let isEditing = false; // Variável para distinguir entre edição e inclusão
$(document).ready(function () {

    loadBeneficiarios();   

    $('#beneficiariosModal').on('show.bs.modal', function (e) {
        loadBeneficiarios();
    });

    // Carregar beneficiários do servidor
    function loadBeneficiarios() {       
       renderBeneficiariosTable();        
    }

    // Adicionar beneficiário visualmente na tabela
    $('#btnAddBeneficiario').click(function () {
        let cpfField = $('#beneficiarioCPF');
        let cpf = cpfField.val(); // Mantém os caracteres não numéricos
        let nome = $('#beneficiarioNome').val();

        let cpfDuplicado = beneficiarios.some(beneficiario => beneficiario.CPF === cpf);
        if (cpfDuplicado) {
            cpfField.next(".text-danger").text("CPF já cadastrado.").show();
            return;
        }

        if (!isValidCPF(cpf)) {
            showInvalidCPFMessage(cpfField);
            return;
        } else {
            clearInvalidCPFMessage(cpfField);
        }

        let beneficiario = {
            CPF: cpf,
            Nome: nome
        };

        beneficiarios.push(beneficiario);
        renderBeneficiariosTable();

        // Limpar o formulário de beneficiário
        $('#formBeneficiario')[0].reset();
    });




    $('#prevPage').click(function (e) {
        e.preventDefault();
        if (currentPage > 1) {
            currentPage--;
            renderBeneficiariosTable();
        }
    });

    $('#nextPage').click(function (e) {
        e.preventDefault();
        if (currentPage * pageSize < beneficiarios.length) {
            currentPage++;
            renderBeneficiariosTable();
        }
    });





    // Máscara de CPF para os campos de CPF
    $('#beneficiarioCPF').on('input', function () {
        clearInvalidCPFMessage($(this));
        $(this).val(formatCPF($(this).val()));
    });

    // Função para formatar o CPF enquanto o usuário digita
    $('#CPF').on('input', function () {
        clearInvalidCPFMessage($(this));
        $(this).val(formatCPF($(this).val()));
    });
    $('#formCadastro').submit(function (e) {
        e.preventDefault();

        var cpfField = $(this).find("#CPF");
        var cpf = cpfField.val();

        if (!isValidCPF(cpf)) {
            showInvalidCPFMessage(cpfField);
            return;
        } else {
            clearInvalidCPFMessage(cpfField);
        }

        // Obtém os dados do cliente
        var clienteData = {
            Nome: $(this).find("#Nome").val(),
            Sobrenome: $(this).find("#Sobrenome").val(),
            Email: $(this).find("#Email").val(),
            Telefone: $(this).find("#Telefone").val(),
            CPF: cpf,
            CEP: $(this).find("#CEP").val(),
            Nacionalidade: $(this).find("#Nacionalidade").val(),
            Estado: $(this).find("#Estado").val(),
            Cidade: $(this).find("#Cidade").val(),
            Logradouro: $(this).find("#Logradouro").val()
        };

        // Envia os dados do cliente e a lista de beneficiários para o servidor
        $.ajax({
            url: urlPost,
            method: "POST",
            data: JSON.stringify({
                clienteModel: clienteData,
                beneficiarios: beneficiarios // Assumindo que beneficiarios é uma variável global contendo a lista atualizada
            }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            error: function (r) {
                if (r.status == 400)
                    ModalDialog("Ocorreu um erro", r.responseJSON);
                else if (r.status == 500)
                    ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
            },
            success: function (r) {
                ModalDialog("Sucesso!", r);
                $("#formCadastro")[0].reset();
                beneficiarios = [];
                renderBeneficiariosTable();
                window.location.href = urlRetorno;
            }
        });
    });



    // Função para validar o CPF
    function isValidCPF(cpf) {
        cpf = cpf.replace(/[^\d]+/g, '');
        if (cpf.length !== 11 || /^(\d)\1+$/.test(cpf)) return false;
        var soma = 0;
        for (var i = 0; i < 9; i++) {
            soma += parseInt(cpf.charAt(i)) * (10 - i);
        }
        var resto = 11 - (soma % 11);
        if (resto === 10 || resto === 11) resto = 0;
        if (resto !== parseInt(cpf.charAt(9))) return false;
        soma = 0;
        for (i = 0; i < 10; i++) {
            soma += parseInt(cpf.charAt(i)) * (11 - i);
        }
        resto = 11 - (soma % 11);
        if (resto === 10 || resto === 11) resto = 0;
        if (resto !== parseInt(cpf.charAt(10))) return false;
        return true;
    }

    // Função para mostrar mensagem de CPF inválido
    function showInvalidCPFMessage(field) {
        cpfField.next(".text-danger").text("CPF Invalido.").show();
        field.focus(); // Define o foco no campo de CPF
    }

    // Função para limpar mensagem de CPF inválido
    function clearInvalidCPFMessage(field) {
        field.next(".text-danger").hide();
    }




    // Função para formatar o CPF
    function formatCPF(cpfValue) {
        cpfValue = cpfValue.replace(/\D/g, ''); // Remove todos os caracteres não numéricos
        cpfValue = cpfValue.replace(/(\d{3})(\d)/, '$1.$2'); // Adiciona o primeiro ponto
        cpfValue = cpfValue.replace(/(\d{3})(\d)/, '$1.$2'); // Adiciona o segundo ponto
        cpfValue = cpfValue.replace(/(\d{3})(\d{1,2})$/, '$1-$2'); // Adiciona o traço
        return cpfValue;
    }
    
});

function updatePaginationControls() {
    $('#prevPage').toggleClass('disabled', currentPage === 1);
    $('#nextPage').toggleClass('disabled', currentPage * pageSize >= beneficiarios.length);
}

function renderBeneficiariosTable() {
    let start = (currentPage - 1) * pageSize;
    let end = start + pageSize;
    let paginatedBeneficiarios = beneficiarios.slice(start, end);

    $('#gridBeneficiarios tbody').empty();

    if (paginatedBeneficiarios.length === 0) {
        // Adiciona uma linha indicando que não há registros
        let noRecordsRow = `
            <tr>
                <td colspan="3" class="text-center">Nenhum beneficiário encontrado.</td>
            </tr>
        `;
        $('#gridBeneficiarios tbody').append(noRecordsRow);
    } else {
        paginatedBeneficiarios.forEach(function (beneficiario) {
            let row = `
                <tr>
                    <td>${beneficiario.CPF}</td>
                    <td>${beneficiario.Nome}</td>
                    <td>
                        <button class="btn btn-primary btn-sm" onclick="editarBeneficiario('${beneficiario.CPF}', '${beneficiario.Nome}')">Alterar</button>
                        <button class="btn btn-danger btn-sm" onclick="excluirBeneficiario('${beneficiario.CPF}')">Excluir</button>
                    </td>
                </tr>
            `;
            $('#gridBeneficiarios tbody').append(row);
        });
    }

    updatePaginationControls();
}
function editarBeneficiario(cpf, nome) {
    $('#beneficiarioCPF').val(cpf).prop('readonly', true);
    $('#beneficiarioNome').val(nome);
    isEditing = true; // Indica que está editando um registro existente
    $('#btnAddBeneficiario').text('Salvar').off('click').on('click', function () {
        salvarEdicaoBeneficiario(cpf);
    });
}

function salvarEdicaoBeneficiario(cpfOriginal) {
    let nome = $('#beneficiarioNome').val();
    let cpfField = $('#beneficiarioCPF');
    let cpf = cpfField.val().trim(); // Manter o formato original do CPF

    let cpfExistente = beneficiarios.find(beneficiario => beneficiario.CPF !== cpfOriginal && beneficiario.CPF === cpf);

    if (cpfExistente) {
        cpfField.next(".text-danger").text("CPF já cadastrado").show(); // Mensagem de erro
        return;
    } else {
        cpfField.next(".text-danger").hide(); // Oculta a mensagem de erro
    }

    if (isEditing) {
        beneficiarios = beneficiarios.map(beneficiario => beneficiario.CPF === cpfOriginal ? { CPF: cpf, Nome: nome } : beneficiario);
    } else {
        beneficiarios.push({ CPF: cpf, Nome: nome });
    }

    // Atualizar a tabela
    renderBeneficiariosTable();

    // Resetar formulário e botões
    $('#formBeneficiario')[0].reset();
    $('#beneficiarioCPF').prop('readonly', false);
    $('#btnAddBeneficiario').text('Incluir').off('click').on('click', function () {
        let cpf = $('#beneficiarioCPF').val().trim();
        let nome = $('#beneficiarioNome').val();

        if (isEditing) {
            salvarEdicaoBeneficiario(cpf);
        } else {
            adicionarBeneficiario(cpf, nome);
        }
    });
    isEditing = false; // Resetar a variável de edição
}

function adicionarBeneficiario(cpf, nome) {
    let cpfField = $('#beneficiarioCPF');
    let cpfExistente = beneficiarios.find(beneficiario => beneficiario.CPF === cpf);

    if (cpfExistente) {
        cpfField.next(".text-danger").text("CPF já cadastrado").show(); // Mensagem de erro
        return;
    } else {
        cpfField.next(".text-danger").hide(); // Oculta a mensagem de erro
    }

    beneficiarios.push({ CPF: cpf, Nome: nome });

    // Atualizar a tabela
    renderBeneficiariosTable();

    // Resetar formulário e botões
    $('#formBeneficiario')[0].reset();
    $('#beneficiarioCPF').prop('readonly', false);
    $('#btnAddBeneficiario').text('Incluir').off('click').on('click', function () {
        let cpf = $('#beneficiarioCPF').val().trim();
        let nome = $('#beneficiarioNome').val();

        if (isEditing) {
            salvarEdicaoBeneficiario(cpf);
        } else {
            adicionarBeneficiario(cpf, nome);
        }
    });
}

function excluirBeneficiario(cpf) {
    beneficiarios = beneficiarios.filter(beneficiario => beneficiario.CPF !== cpf);

    // Atualizar a tabela
    renderBeneficiariosTable();
}


function ModalDialog(titulo, texto) {
    var random = Math.random().toString().replace('.', '');
    var texto = '<div id="' + random + '" class="modal fade">                                                               ' +
        '        <div class="modal-dialog">                                                                                 ' +
        '            <div class="modal-content">                                                                            ' +
        '                <div class="modal-header">                                                                         ' +
        '                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>         ' +
        '                    <h4 class="modal-title">' + titulo + '</h4>                                                    ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-body">                                                                           ' +
        '                    <p>' + texto + '</p>                                                                           ' +
        '                </div>                                                                                             ' +
        '                <div class="modal-footer">                                                                         ' +
        '                    <button type="button" class="btn btn-default" data-dismiss="modal">Fechar</button>             ' +
        '                                                                                                                   ' +
        '                </div>                                                                                             ' +
        '            </div><!-- /.modal-content -->                                                                         ' +
        '  </div><!-- /.modal-dialog -->                                                                                    ' +
        '</div> <!-- /.modal -->                                                                                        ';

    $('body').append(texto);
    $('#' + random).modal('show');
}
