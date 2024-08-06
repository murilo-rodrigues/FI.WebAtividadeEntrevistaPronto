CREATE PROCEDURE FI_SP_IncBeneficiario
    @IdCLIENTE BIGINT,
    @CPF VARCHAR(14),
    @Nome VARCHAR(255)
AS
BEGIN
    -- Verificar se o IdCLIENTE é válido
    IF @IdCLIENTE IS NULL OR @IdCLIENTE <= 0
    BEGIN
        RAISERROR('ID do Cliente inválido.', 16, 1);
        RETURN;
    END

    -- Inserir o beneficiário na tabela BENEFICIARIOS
    INSERT INTO BENEFICIARIOS (CPF, Nome, IdCLIENTE)
    VALUES (@CPF, @Nome, @IdCLIENTE);
END
